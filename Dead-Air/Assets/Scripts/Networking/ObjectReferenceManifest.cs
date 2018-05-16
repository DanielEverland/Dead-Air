using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMS;

public static class ObjectReferenceManifest {
    
    private static List<ObjectReferenceData> _objectReferenceData = new List<ObjectReferenceData>();

    public static void InitializeAsClient(IEnumerable<ModFile> mods, IDictionary<string, ushort> networkIDs)
    {
        Initialize(mods, x => ObjectReferenceData.CreateAsClient(x, networkIDs[x.ID]));
    }
    public static void InitializeAsServer(IEnumerable<ModFile> mods)
    {
        Initialize(mods, x => ObjectReferenceData.CreateAsServer(x));
    }
    private static void Initialize(IEnumerable<ModFile> mods, System.Func<ModFile.Entry, ObjectReferenceData> onCreate)
    {
        foreach (ModFile modfile in mods)
        {
            ObjectHandler.RegisterData(modfile);

            foreach (string id in modfile.IDs)
            {
                if (_objectReferenceData.Any(x => x.ObjectFileID == id) || !modfile.ShouldDeserialize(id))
                    continue;

                ModFile.Entry entry = modfile[id];
                _objectReferenceData.Add(onCreate(entry));
            }
        }
    }

    public static Dictionary<string, ushort> GetAllNetworkIDs()
    {
        Dictionary<string, ushort> networkIds = new Dictionary<string, ushort>();

        foreach (ObjectReferenceData objectReference in _objectReferenceData)
        {
            networkIds.Add(objectReference.ObjectFileID, objectReference.NetworkID);
        }

        return networkIds;
    }

    public static ushort GetNetworkID(Object obj)
    {
        return Get(obj).NetworkID;
    }
    public static ushort GetNetworkID(string objectKey)
    {
        return Get(objectKey).NetworkID;
    }
    public static Object GetObject(ushort networkID)
    {
        return Get(networkID).Object;
    }
    public static Object GetObject(string objectKey)
    {
        return Get(objectKey).Object;
    }
    public static string GetObjectKey(Object obj)
    {
        return Get(obj).ObjectFileID;
    }

    private static ObjectReferenceData Get(string objectKey)
    {
        return _objectReferenceData.FirstOrDefault(x => x.ObjectFileID == objectKey);
    }
    private static ObjectReferenceData Get(ushort networkID)
    {
        return _objectReferenceData.FirstOrDefault(x => x.NetworkID == networkID);
    }
    private static ObjectReferenceData Get(Object obj)
    {
        return _objectReferenceData.FirstOrDefault(x => x.Object == obj);
    }

    private class ObjectReferenceData
    {
        private static ushort _currentIndex = 0;

        private ObjectReferenceData() { }
        
        private static ObjectReferenceData Create(ModFile.Entry entry, ushort id)
        {
            ObjectReferenceData referenceData = new ObjectReferenceData();

            System.Type type = null;
            Object obj = null;

            Result result = MetaData.GetType(entry.Data, out type);
            result += Serializer.Deserialize(entry.Data, type, ref obj);

            if (!result.Succeeded)
                throw new System.ArgumentException("Object deserialization failed with message " + result.FormattedMessage);

            referenceData.Object = obj;
            referenceData.NetworkID = id;
            referenceData.ObjectFileID = entry.ID;

            return referenceData;
        }
        public static ObjectReferenceData CreateAsServer(ModFile.Entry entry)
        {
            ObjectReferenceData referenceData = Create(entry, _currentIndex);
            _currentIndex++;

            return referenceData;
        }
        public static ObjectReferenceData CreateAsClient(ModFile.Entry entry, ushort networkID)
        {
            return Create(entry, networkID);
        }

        public Object Object { get; private set; }
        public ushort NetworkID { get; private set; }
        public string ObjectFileID { get; private set; }
    }
}

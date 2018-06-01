using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles serialization and creation of profiles
/// </summary>
public static class ProfileManager {

	static ProfileManager()
    {
        Directories.EnsurePathExists(Directories.Profiles);
        LoadProfiles();

        Server.OnSave += SaveProfiles;
    }

    private static Dictionary<int, Profile> _profiles = new Dictionary<int, Profile>();

    public static Profile GetProfile(Peer peer)
    {
        int id = peer.ProfileID;

        if (!_profiles.ContainsKey(id))
        {
            CreateNewProfile(peer);
        }

        return _profiles[id];
    }
    private static Profile CreateNewProfile(Peer peer)
    {
        Profile profile = new Profile(peer);

        SaveProfile(profile);
        _profiles.Add(profile.ID, profile);

        return profile;
    }

    private static void LoadProfiles()
    {
        foreach (string filePath in Directory.GetFiles(Directories.Profiles))
        {
            try
            {
                byte[] data = File.ReadAllBytes(filePath);
                Profile profile = ByteConverter.Deserialize<Profile>(data);

                int id = profile.ID;

                if (id == default(int))
                    throw new System.ArgumentException();

                _profiles.Add(id, profile);
            }
            catch (System.Exception)
            {
                Debug.LogError("Issue deserializing profile " + filePath);
                throw;
            }
        }
    }
    private static void SaveProfiles()
    {
        foreach (Profile profile in _profiles.Values)
        {
            SaveProfile(profile);
        }
    }
    private static void SaveProfile(Profile profile)
    {
        try
        {
            int id = profile.ID;
            byte[] data = ByteConverter.Serialize(profile);
            string fullPath = $@"{Directories.Profiles}\{id}";

            File.WriteAllBytes(fullPath, data);
        }
        catch (System.Exception)
        {
            Debug.LogError("Issue serializing profile " + profile);
            throw;
        }
    }
}

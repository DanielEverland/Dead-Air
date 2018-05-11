using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public sealed class ModManifestPackage : NetworkPackage
{
    public override ushort ID { get { return (ushort)PackageIdentification.ModManifest; } }

    public ModManifestPackage(IEnumerable<Guid> guids)
    {
        AssignData(guids.ToArray());
    }
    public ModManifestPackage(Guid[] guids)
    {
        AssignData(guids);
    }

    private void AssignData(Guid[] guids)
    {
        Data = new byte[guids.Length * 16];

        for (int i = 0; i < guids.Length; i++)
        {
            int index = i * 16;
            byte[] serializedGUID = guids[i].ToByteArray();
            
            for (int j = 0; j < serializedGUID.Length; j++)
            {
                Data[index + j] = serializedGUID[j];
            }
        }
    }
    
    public static List<Guid> Process(byte[] data)
    {
        List<Guid> toReturn = new List<Guid>();

        for (int i = 0; i < data.Length / 16; i++)
        {
            int index = i * 16;
            byte[] guidData = new byte[16];

            for (int j = 0; j < 16; j++)
            {
                guidData[j] = data[index + j];
            }

            toReturn.Add(new Guid(guidData));
        }

        return toReturn;
    }
}

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ModDownloadRequest : NetworkPackage {

    public override ushort ID { get { return (ushort)PackageIdentification.ModDownloadRequest; } }

    public ModDownloadRequest(IEnumerable<Guid> guids)
    {
        AssignData(guids.ToArray());
    }
    public ModDownloadRequest(Guid[] guids)
    {
        AssignData(guids);
    }

    private void AssignData(Guid[] guids)
    {
        Data = guids.ToByteArray();
    }

    public static List<Guid> Process(byte[] data)
    {
        return Utility.ByteArrayToGUID(data);
    }
}

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UMS;

public class ModDownloadPackage : NetworkPackage
{
    public override ushort ID { get { return (ushort)PackageIdentification.ModDownload; } }

    public ModDownloadPackage(ModFile file)
    {
        Data = ByteConverter.Serialize(file);
    }

    public static ModFile Process(byte[] data)
    {
        return ByteConverter.Deserialize<ModFile>(data);
    }
}

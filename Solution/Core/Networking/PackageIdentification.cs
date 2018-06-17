namespace Networking
{
    public enum PackageIdentification : ushort
    {
        None = 0,

        ModManifest,
        ModDownloadRequest,
        ModDownload,

        ObjectIDManifest,
        RequestObjectIDManifest,
        ServerInformation,
        ServerPerformance,

        JoinflowCompleted,

        Instantiate,
    }
}
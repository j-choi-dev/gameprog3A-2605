using System;
using System.Collections.Generic;
namespace SampleEditorSDK.Domain
{

    [Serializable]
    public class AssetBundleBuildInfo
    {
        public string Platform;
        public string BuildTarget;
        public string Version;
    }

    [Serializable]
    public class AssetBundleAppInfo
    {
        public string Platform;
        public string BuildTarget;
        public string BuildDirectory;
        public string Version;
        public string GeneratedAt;
        public List<AssetBundleInfo> AppInfo = new List<AssetBundleInfo>();
    }

    [Serializable]
    public class AssetBundleInfo
    {
        public string AssetBundleName;
        public long FileSize;
        public uint CRC;
        public string Hash;
        public List<string> Dependencies = new List<string>();
        public string AssetBundlePath;
    }
}

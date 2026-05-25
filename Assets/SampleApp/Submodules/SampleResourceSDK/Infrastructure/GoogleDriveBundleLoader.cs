using Cysharp.Threading.Tasks;
using SampleResourceSDK.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace SampleResourceSDK.Infrastructure
{
    public class GoogleDriveBundleLoader : IBundleResourceDomain
    {
        private const string Prefix = ".ab";

        private readonly Dictionary<string, string> _driveFileIdByFileName;
        private readonly string _cacheRootPath;
        private readonly string _apiKey;
        private readonly Func<UniTask<string>> _accessTokenProvider;
        private readonly bool _useCache;

        public GoogleDriveBundleLoader( string apiKey = null,
            Func<UniTask<string>> accessTokenProvider = null,
            bool useCache = true)
        {
            _driveFileIdByFileName = new Dictionary<string, string>
                {
                    { "Character_001", "1NDmpD9OgZQ4zOB0ZprV5UKr6EVvMDkTV" },
                    { "Character_002", "1jf-OFWQ2GWiiNA-3TE2YPZahcFPVg-ri" },
                    { "Character_003", "1ApYFrmrgf3RbyWek5XYMGodkocv8I-cX" },
                    { "Character_004", "1Zk3liu54UhviEgRSNPIelZH7sfcNYzCr" },
                    { "Character_005", "1DcN2uyarz1i5JM_7n0RtjJlnV9cB-1mI" },
                    { "Character_006", "1qwtZtzsfrfAlUb7FcZASvqZ1P8o2Fpla" },
                };
            _apiKey = apiKey;
            _accessTokenProvider = accessTokenProvider;
            _useCache = useCache;

            _cacheRootPath = Path.Combine(UnityEngine.Application.persistentDataPath, "AssetBundles");

            if (!Directory.Exists(_cacheRootPath))
            {
                Directory.CreateDirectory(_cacheRootPath);
            }
        }

        public bool GetIsExist(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;
            if (_driveFileIdByFileName.ContainsKey(fileName))
                return true;

            var cachedPath = GetCachedBundlePath(fileName);
            return File.Exists(cachedPath);
        }

        public async UniTask<Object> LoadObject(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("LoadObject failed. fileName is null or empty.");
                return null;
            }

            var localBundlePath = GetCachedBundlePath(fileName);

            if (!_useCache || !File.Exists(localBundlePath))
            {
                var downloadSuccess = await DownloadBundle(fileName, localBundlePath);

                if (!downloadSuccess)
                    return null;
            }

            var bundle = await LoadAssetBundleFromFile(localBundlePath);

            if (bundle == null)
                return null;

            var loadedAsset = await bundle.LoadAssetAsync<Object>(fileName).ToUniTask();

            if (loadedAsset == null)
            {
                Debug.LogError($"Not Exist Asset in AssetBundle. assetName: {fileName}");
            }

            bundle.Unload(false);

            return loadedAsset;
        }

        private string GetCachedBundlePath(string fileName)
        {
            return Path.Combine(_cacheRootPath, $"{fileName}{Prefix}");
        }

        private async UniTask<bool> DownloadBundle(string fileName, string savePath)
        {
            if (!_driveFileIdByFileName.TryGetValue(fileName, out var driveFileId))
            {
                Debug.LogError($"Google Drive file id is not registered. fileName: {fileName}");
                return false;
            }

            var url = BuildDownloadUrl(driveFileId);

            using var request = UnityWebRequest.Get(url);
            request.downloadHandler = new DownloadHandlerBuffer();

            if (_accessTokenProvider != null)
            {
                var accessToken = await _accessTokenProvider.Invoke();

                if (!string.IsNullOrEmpty(accessToken))
                {
                    request.SetRequestHeader("Authorization", $"Bearer {accessToken}");
                }
            }

            await request.SendWebRequest().ToUniTask();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(
                    $"Google Drive AssetBundle download failed.\n" +
                    $"fileName: {fileName}\n" +
                    $"url: {url}\n" +
                    $"responseCode: {request.responseCode}\n" +
                    $"error: {request.error}"
                );

                return false;
            }

            var bytes = request.downloadHandler.data;

            if (bytes == null || bytes.Length == 0)
            {
                Debug.LogError($"Downloaded AssetBundle is empty. fileName: {fileName}");
                return false;
            }

            var directory = Path.GetDirectoryName(savePath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var tempPath = $"{savePath}.tmp";

            await UniTask.SwitchToThreadPool();

            try
            {
                File.WriteAllBytes(tempPath, bytes);

                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }

                File.Move(tempPath, savePath);
            }
            catch (Exception e)
            {
                Debug.LogException(e);

                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }

                await UniTask.SwitchToMainThread();
                return false;
            }

            await UniTask.SwitchToMainThread();

            Debug.Log($"Google Drive AssetBundle downloaded. fileName: {fileName}, path: {savePath}");

            return true;
        }

        private async UniTask<AssetBundle> LoadAssetBundleFromFile(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"Cached AssetBundle file does not exist. path: {path}");
                return null;
            }

            var request = AssetBundle.LoadFromFileAsync(path);
            var bundle = await request.ToUniTask();

            if (bundle == null)
            {
                Debug.LogError($"AssetBundle.LoadFromFileAsync failed. path: {path}");
                return null;
            }

            return bundle;
        }

        private string BuildDownloadUrl(string driveFileId)
        {
            var encodedFileId = UnityWebRequest.EscapeURL(driveFileId);
            var url = $"https://drive.google.com/file/d/{encodedFileId}?alt=media";
            if (!string.IsNullOrEmpty(_apiKey))
            {
                url += $"&key={UnityWebRequest.EscapeURL(_apiKey)}";
            }

            return url;
        }
    }
}
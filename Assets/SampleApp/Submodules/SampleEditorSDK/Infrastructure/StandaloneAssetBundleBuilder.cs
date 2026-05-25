using Cysharp.Threading.Tasks;
using SampleEditorSDK.Domain;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SampleCharacterSDK.Domain;
using System.IO;
using System;

namespace SampleEditorSDK.View
{
    public class StandaloneAssetBundleBuilder : IAssetBundleBuildDomain
    {
        private List<AssetBundleBuild> _buildMap = new List<AssetBundleBuild>();

        private AssetBundleManifest _manifest;
        private AssetBundleBuildInfo _buildInfo;
        private string _buildDirectory;

        private const string TargetFolder = "Assets/SampleApp/Submodules/SampleCharacterSDK/AssetBundles/";
        private const string InfoFileName = "Info.dat";
        private const string LegacyManifestInfoFileName = "BundleManifestInfo.txt";

        public async UniTask<bool> PreProcess(AssetBundleBuildInfo buildInfo)
        {
            var _guids = new List<string>();
            _buildInfo = buildInfo;
            try
            {
                _guids.Clear();
                _buildMap.Clear();

                _guids = AssetDatabase.FindAssets("t:Prefab", new[] { TargetFolder }).ToList();

                foreach (var guid in _guids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                    Debug.Log(assetPath);
                    if (prefab != null)
                    {
                        var targetComponent = prefab.GetComponent<ICharacter>();
                        if (targetComponent != null)
                        {
                            var build = new AssetBundleBuild();
                            build.assetBundleName = prefab.name + ".ab";
                            build.assetNames = new[] { assetPath };
                            _buildMap.Add(build);
                        }
                    }
                }

                if (_buildMap.Count == 0)
                {
                    Debug.LogError("Can Not Find Exist Any Component Assets");
                    return false;
                }
                Debug.Log($"AssetBundle PreProcess Successed");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"AssetBundle PreProcess Failed: {e.Message}");
                return false;
            }
        }

        public async UniTask<bool> BuildProcess()
        {
            try
            {
                var projectRoot = Directory.GetParent(UnityEngine.Application.dataPath).FullName;

                _buildDirectory = Path.Combine(projectRoot,
                "Builds",
                _buildInfo.Platform,
                _buildInfo.Version);

                if (Directory.Exists(_buildDirectory) == false)
                {
                    Directory.CreateDirectory(_buildDirectory);
                }

                _manifest = BuildPipeline.BuildAssetBundles(
                    _buildDirectory,
                    _buildMap.ToArray(),
                    BuildAssetBundleOptions.None,
                    EditorUserBuildSettings.activeBuildTarget
                );

                if (_manifest == null)
                {
                    Debug.LogError("Manifest Build Failed");
                    return false;
                }

                Debug.Log($"AssetBundle BuildProcess Successed: {_buildMap.Count} to `{_buildDirectory}`");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"AssetBundle BuildProcess Failed: {e.Message}");
                return false;
            }
        }

        public async UniTask<bool> PostProcess()
        {
            try
            {
                if (_manifest == null)
                {
                    Debug.LogError("AssetBundle PostProcess Failed: Manifest is null");
                    return false;
                }

                if (string.IsNullOrEmpty(_buildDirectory))
                {
                    Debug.LogError("AssetBundle PostProcess Failed: Build directory is null or empty");
                    return false;
                }

                var appInfo = new AssetBundleAppInfo
                {
                    Platform = _buildInfo.Platform,
                    BuildTarget = _buildInfo.BuildTarget,
                    BuildDirectory = NormalizePath(_buildDirectory),
                    Version = _buildInfo.Version,
                    GeneratedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                var builtBundles = _manifest.GetAllAssetBundles();
                foreach (string bundleName in builtBundles)
                {
                    var assetBundlePath = Path.Combine(_buildDirectory, bundleName);

                    if (File.Exists(assetBundlePath) == false)
                    {
                        Debug.LogError($"AssetBundle PostProcess Failed: Can not find asset bundle file. path: {assetBundlePath}");
                        return false;
                    }

                    if (BuildPipeline.GetCRCForAssetBundle(assetBundlePath, out uint crc) == false)
                    {
                        Debug.LogError($"AssetBundle PostProcess Failed: Can not calculate CRC. path: {assetBundlePath}");
                        return false;
                    }

                    var fileInfo = new FileInfo(assetBundlePath);
                    var info = new AssetBundleInfo
                    {
                        AssetBundleName = bundleName,
                        FileSize = fileInfo.Length,
                        CRC = crc,
                        Hash = _manifest.GetAssetBundleHash(bundleName).ToString(),
                        Dependencies = _manifest.GetAllDependencies(bundleName).ToList(),
                        AssetBundlePath = NormalizePath(assetBundlePath)
                    };

                    appInfo.AppInfo.Add(info);
                }

                var json = JsonUtility.ToJson(appInfo, true);
                var infoFilePath = Path.Combine(_buildDirectory, InfoFileName);
                File.WriteAllText(infoFilePath, json);
                CleanupUnnecessaryBuildArtifacts();

                AssetDatabase.Refresh();

                Debug.Log($"AssetBundle PostProcess Successed: {InfoFileName} created. path: {infoFilePath}");
                return await UniTask.FromResult(true);
            }
            catch (Exception e)
            {
                Debug.LogError($"AssetBundle PostProcess Failed: {e.Message}");
                return await UniTask.FromResult(false);
            }
        }

        private string NormalizePath(string path)
        {
            return path.Replace("\\", "/");
        }

        private void CleanupUnnecessaryBuildArtifacts()
        {
            var deletedFiles = new List<string>();
            var buildFolderName = Path.GetFileName(_buildDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

            DeleteFileIfExists(Path.Combine(_buildDirectory, buildFolderName), deletedFiles);
            DeleteFileIfExists(Path.Combine(_buildDirectory, buildFolderName + ".manifest"), deletedFiles);
            DeleteFileIfExists(Path.Combine(_buildDirectory, LegacyManifestInfoFileName), deletedFiles);

            foreach (var deletedFile in deletedFiles)
            {
                Debug.Log($"Deleted unnecessary AssetBundle artifact: {deletedFile}");
            }
        }

        private void DeleteFileIfExists(string filePath, List<string> deletedFiles)
        {
            if (File.Exists(filePath) == false)
            {
                return;
            }

            File.Delete(filePath);
            deletedFiles.Add(NormalizePath(filePath));
        }
    }
}

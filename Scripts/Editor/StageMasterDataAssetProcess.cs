using System.Linq;
using NUnit.Framework;
using Unity1week202403.Data;
using UnityEditor;

namespace Unity1week202403.Editor
{
    public class StageMasterDataAssetProcess
    {
        private const string FolderPath = "Assets/Application/ScriptableObjects/MasterData/Stage/";

        private const string DataStorePath =
            "Assets/Application/ScriptableObjects/MasterData/DataStoreSource/StageMasterDataStoreSource.asset";

        public void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var dataStore = AssetDatabase.LoadAssetAtPath<StageMasterDataStoreSource>(DataStorePath);
            Assert.IsNotNull(dataStore);

            var dataStoreSerializedObject = new SerializedObject(dataStore);
            dataStoreSerializedObject.Update();

            var targetImportedAssets = importedAssets
                .Where(path => path.StartsWith(FolderPath))
                .ToArray();

            var targetDeletedAssets = deletedAssets
                .Where(path => path.StartsWith(FolderPath))
                .ToArray();

            var targetMovedAssets = movedAssets
                .Where(path => path.StartsWith(FolderPath))
                .ToArray();

            var targetMovedFromAssetPaths = movedFromAssetPaths
                .Where(path => path.StartsWith(FolderPath))
                .ToArray();

            if (!targetImportedAssets
                    .Concat(targetDeletedAssets)
                    .Concat(targetMovedAssets)
                    .Concat(targetMovedFromAssetPaths)
                    .Any())
            {
                // 何も変更がない場合は何もしない
                return;
            }

            // 新規で追加されたアセットをデータストアに追加する
            var assets = AssetDatabase.FindAssets($"t:{nameof(StageMasterData)}", new[] { FolderPath })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<StageMasterData>)
                .ToArray();

            var monsterMasterDataArray = dataStoreSerializedObject.FindProperty("_data");
            monsterMasterDataArray.ClearArray();
            foreach (var masterData in assets)
            {
                monsterMasterDataArray.arraySize++;
                var arrayElement = monsterMasterDataArray.GetArrayElementAtIndex(monsterMasterDataArray.arraySize - 1);
                arrayElement.objectReferenceValue = masterData;
                arrayElement.serializedObject.ApplyModifiedProperties();
            }

            dataStore.Validate();
            dataStoreSerializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(dataStore);
            AssetDatabase.SaveAssets();
        }
    }
}
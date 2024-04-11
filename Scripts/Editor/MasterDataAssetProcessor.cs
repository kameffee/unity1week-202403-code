using UnityEditor;

namespace Unity1week202403.Editor
{
    public class MasterDataAssetProcessor : AssetPostprocessor
    {
        private static readonly MonsterMasterDataAssetProcess _monsterMasterDataAssetProcess = new();
        private static readonly StageMasterDataAssetProcess _stageMasterDataAssetProcess = new();

        public static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            _monsterMasterDataAssetProcess.OnPostprocessAllAssets(
                importedAssets,
                deletedAssets,
                movedAssets,
                movedFromAssetPaths);

            _stageMasterDataAssetProcess.OnPostprocessAllAssets(
                importedAssets,
                deletedAssets,
                movedAssets,
                movedFromAssetPaths);
        }
    }
}
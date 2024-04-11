using System.Linq;
using Unity1week202403.Data;
using UnityEditor;
using UnityEngine;

namespace Unity1week202403.Editor
{
    public class MonsterMasterDataAnalyseEditor : EditorWindow
    {
        private MonsterMasterDataStoreSource _dataStoreSource;
        private Vector2 _scrollPosition;

        [MenuItem("Tools/MonsterMasterDataAnalyse")]
        private static void Open()
        {
            var window = GetWindow<MonsterMasterDataAnalyseEditor>();
            window.titleContent = new GUIContent("MonsterMasterDataAnalyse");
            window.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("MonsterMasterDataStoreSourceを取得"))
            {
                var dataStoreSource = AssetDatabase.FindAssets("t:MonsterMasterDataStoreSource");
                if (dataStoreSource.Length == 0)
                {
                    Debug.LogError("MonsterMasterDataStoreSource is not found.");
                    return;
                }

                var dataStoreSourcePath = AssetDatabase.GUIDToAssetPath(dataStoreSource[0]);
                _dataStoreSource = AssetDatabase.LoadAssetAtPath<MonsterMasterDataStoreSource>(dataStoreSourcePath);
            }

            if (_dataStoreSource == null)
            {
                EditorGUILayout.LabelField("MonsterMasterDataStoreSource is not found.");
                return;
            }

            EditorGUILayout.LabelField("MonsterMasterData Count: " + _dataStoreSource.Data.Length);

            using (new GUILayout.HorizontalScope(GUILayout.Height(20)))
            {
                EditorGUILayout.LabelField("サムネ", GUILayout.Width(50));
                VerticalLine();
                EditorGUILayout.LabelField("ID", GUILayout.MinWidth(50));
                VerticalLine();
                EditorGUILayout.LabelField("名前", GUILayout.MinWidth(50));
                VerticalLine();
                EditorGUILayout.LabelField("コスト", GUILayout.MinWidth(50));
                VerticalLine();
                EditorGUILayout.LabelField("HP", GUILayout.MinWidth(50));
                VerticalLine();
                EditorGUILayout.LabelField("攻撃力", GUILayout.MinWidth(50));
                VerticalLine();
                EditorGUILayout.LabelField("攻撃範囲", GUILayout.MinWidth(50));
                VerticalLine();
                EditorGUILayout.LabelField("攻撃時間", GUILayout.MinWidth(50));
                VerticalLine();
                EditorGUILayout.LabelField("移動速度", GUILayout.MinWidth(50));
                VerticalLine();
                EditorGUILayout.LabelField("DPS", GUILayout.MinWidth(50));
                // スクロールバー分の幅を確保
                GUILayout.Space(13);
            }

            HorizontalLine();

            using (var scrollView = new GUILayout.ScrollViewScope(_scrollPosition, false, true))
            {
                foreach (var (data, index) in _dataStoreSource.Data.Select((data, index) => (data, i: index)))
                {
                    using (new GUILayout.HorizontalScope(GUILayout.Height(50)))
                    {
                        // サムネ
                        var thumbnail = data.Thumbnail;
                        var thumbnailRect = EditorGUILayout.GetControlRect(GUILayout.Width(50), GUILayout.Height(50));
                        EditorGUI.DrawPreviewTexture(thumbnailRect, thumbnail.texture);
                        VerticalLine();
                        EditorGUILayout.LabelField($"{data.Id.Value}", GUILayout.MinWidth(50));
                        VerticalLine();
                        EditorGUILayout.LabelField(data.Name, GUILayout.MinWidth(50));
                        VerticalLine();
                        EditorGUILayout.LabelField($"{data.Cost.Value}", GUILayout.MinWidth(50));
                        VerticalLine();
                        EditorGUILayout.LabelField($"{data.Parameter.Hp}", GUILayout.MinWidth(50));
                        VerticalLine();
                        EditorGUILayout.LabelField($"{data.Parameter.AttackPower}", GUILayout.MinWidth(50));
                        VerticalLine();
                        EditorGUILayout.LabelField($"{data.Parameter.AttackRange}", GUILayout.MinWidth(50));
                        VerticalLine();
                        EditorGUILayout.LabelField($"{data.Parameter.PreAttackTime + data.Parameter.PostAttackTime}", GUILayout.MinWidth(50));
                        VerticalLine();
                        EditorGUILayout.LabelField($"{data.Parameter.MoveSpeed}", GUILayout.MinWidth(50));
                        VerticalLine();
                        EditorGUILayout.LabelField(
                            $"{data.Parameter.AttackPower / (data.Parameter.PreAttackTime + data.Parameter.PostAttackTime)}",
                            GUILayout.MinWidth(50));
                    }

                    GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
                }

                _scrollPosition = scrollView.scrollPosition;
            }
        }

        private void HorizontalLine()
        {
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
        }

        private void VerticalLine()
        {
            GUILayout.Box("", GUILayout.Width(2), GUILayout.ExpandHeight(true));
        }
    }
}
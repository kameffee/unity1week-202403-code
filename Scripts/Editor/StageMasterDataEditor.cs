using Unity1week202403.Data;
using UnityEditor;

namespace Unity1week202403.Editor
{
    [CustomEditor(typeof(StageMasterData))]
    public class StageMasterDataEditor : UnityEditor.Editor
    {
        private SerializedProperty _stageMonsterListProperty;

        private void OnEnable()
        {
            _stageMonsterListProperty = serializedObject.FindProperty("_stageMonsterList");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var iterator = serializedObject.GetIterator();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                    EditorGUILayout.PropertyField(iterator, true);

                if (iterator.propertyPath == "_stageScene")
                {
                    // SceneObjectのプレビューを表示
                    var sceneObject = iterator.boxedValue as SceneObject;
                    if (sceneObject == null || sceneObject.IsEmpty)
                    {
                        EditorGUILayout.HelpBox("指定なしの場合はランダムで選定されます。", MessageType.Info);
                    }
                    else if (sceneObject is { IsEmpty: false })
                    {
                        // "Scenes/Stage" 以下にあるシーンのみを選択可能とする
                        var scenePath = $"Assets/Application/Scenes/Stage/{sceneObject.SceneName}.unity";
                        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                        if (sceneAsset == null)
                        {
                            EditorGUILayout.HelpBox("Stage/以下のシーンではありません。", MessageType.Error);
                        }
                    } 
                }
            }

            // Costの合計を描画
            var cost = 0;
            for (var i = 0; i < _stageMonsterListProperty.arraySize; i++)
            {
                var element = _stageMonsterListProperty.GetArrayElementAtIndex(i);
                var monsterData = element.FindPropertyRelative("_monsterMasterData").objectReferenceValue as MonsterMasterData;
                cost += monsterData.Cost.Value;
            }
            
            EditorGUILayout.LabelField("Total Cost", cost.ToString());
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
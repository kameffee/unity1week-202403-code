using Unity1week202403.Data;
using UnityEditor;
using UnityEngine;

namespace Unity1week202403.Editor
{
    [CustomEditor(typeof(MonsterMasterData))]
    public class MonsterMasterDataEditor : UnityEditor.Editor
    {
        private static bool _foldoutSkill;
        private static bool _foldoutBigMonster;
        private bool _isLockSkill = true;
        private bool _isLockBigMonster = true;

        private SerializedProperty _prefabProperty;
        private SerializedProperty _thumbnailProperty;

        private void OnEnable()
        {
            _prefabProperty = serializedObject.FindProperty("_prefab");
            _thumbnailProperty = serializedObject.FindProperty("_thumbnail");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var iterator = serializedObject.GetIterator();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                    EditorGUILayout.PropertyField(iterator, true);

                if (iterator.propertyPath == "_skillMasterData")
                {
                    // SkillMasterDataのプレビューを表示
                    DrawSkillDetail(serializedObject.FindProperty("_skillMasterData"));
                }

                if (iterator.propertyPath == "_bigMonsterMasterData")
                {
                    // BigMonsterMasterDataのプレビューを表示
                    DrawBigMonsterDetail(serializedObject.FindProperty("_bigMonsterMasterData"));
                }
            }

            // 横整列
            DrawPrefabPreview(_prefabProperty);
            DrawThumbnailPreview(_thumbnailProperty);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSkillDetail(SerializedProperty skillMasterData)
        {
            if (skillMasterData == null)
                return;

            _foldoutSkill = EditorGUILayout.Foldout(_foldoutSkill, "Skill Detail", true, EditorStyles.foldoutHeader);
            if (!_foldoutSkill) return;


            _isLockSkill = EditorGUILayout.ToggleLeft("Lock", _isLockSkill);
            using (new EditorGUI.DisabledGroupScope(_isLockSkill))
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.PropertyField(skillMasterData);
                if (skillMasterData.objectReferenceValue != null)
                {
                    var serializedObject = new SerializedObject(skillMasterData.objectReferenceValue);
                    using (new EditorGUI.IndentLevelScope())
                    {
                        var iterator = serializedObject.GetIterator();
                        for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
                        {
                            if (iterator.propertyPath == "m_Script") continue;

                            EditorGUILayout.PropertyField(iterator, true);
                        }
                    }

                    serializedObject.ApplyModifiedProperties();
                }
            }
        }

        private void DrawBigMonsterDetail(SerializedProperty bigMonsterMasterData)
        {
            if (bigMonsterMasterData == null)
                return;

            _foldoutBigMonster = EditorGUILayout.Foldout(_foldoutBigMonster, "Big Monster Detail", true,
                EditorStyles.foldoutHeader);

            if (!_foldoutBigMonster) return;

            _isLockBigMonster = EditorGUILayout.ToggleLeft("Lock", _isLockBigMonster);
            using (new EditorGUI.DisabledGroupScope(_isLockBigMonster))
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.PropertyField(bigMonsterMasterData);
                if (bigMonsterMasterData.objectReferenceValue != null)
                {
                    var serializedObject = new SerializedObject(bigMonsterMasterData.objectReferenceValue);
                    using (new EditorGUI.IndentLevelScope())
                    {
                        var iterator = serializedObject.GetIterator();
                        for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
                        {
                            if (iterator.propertyPath == "m_Script") continue;

                            EditorGUILayout.PropertyField(iterator, true);
                        }
                    }

                    serializedObject.ApplyModifiedProperties();
                }
            }
        }

        private void DrawPrefabPreview(SerializedProperty prefabProperty)
        {
            var prefab = prefabProperty.objectReferenceValue as GameObject;
            if (prefab == null) return;

            var previewTexture = AssetPreview.GetAssetPreview(prefab);
            if (previewTexture != null)
            {
                // ラベル表示
                GUILayout.Label("Prefab Preview", EditorStyles.boldLabel);
                GUILayout.Label(previewTexture);
            }
        }

        private void DrawThumbnailPreview(SerializedProperty thumbnailProperty)
        {
            var thumbnail = thumbnailProperty.objectReferenceValue as Sprite;
            if (thumbnail == null) return;

            var previewTexture = AssetPreview.GetAssetPreview(thumbnail);
            if (previewTexture != null)
            {
                GUILayout.Label("Thumbnail Preview", EditorStyles.boldLabel);
                GUILayout.Label(previewTexture);
            }
        }
    }
}
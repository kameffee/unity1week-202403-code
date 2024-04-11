using System;
using Unity1week202403.Data;
using UnityEditor;
using UnityEngine;

namespace Unity1week202403.Editor
{
    [CustomPropertyDrawer(typeof(SceneObject))]
    public class SceneObjectEditor : PropertyDrawer
    {
        private const string PropertyName = "_sceneName";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var sceneObj = GetSceneObject(property.FindPropertyRelative(PropertyName).stringValue);
            var newScene = EditorGUI.ObjectField(position, label, sceneObj, typeof(SceneAsset), false);
            if (newScene == null)
            {
                var prop = property.FindPropertyRelative(PropertyName);
                prop.stringValue = "";
            }
            else
            {
                if (newScene.name == property.FindPropertyRelative(PropertyName).stringValue) return;

                var scnObj = GetSceneObject(newScene.name);
                if (scnObj == null)
                {
                    Debug.LogWarning("The scene " + newScene.name +
                                     " cannot be used. To use this scene add it to the build settings for the project.");
                }
                else
                {
                    var prop = property.FindPropertyRelative(PropertyName);
                    prop.stringValue = newScene.name;
                }
            }
        }

        private SceneAsset GetSceneObject(string sceneObjectName)
        {
            if (string.IsNullOrEmpty(sceneObjectName))
                return null;

            for (var i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];
                if (scene.path.IndexOf(sceneObjectName, StringComparison.Ordinal) != -1)
                {
                    return AssetDatabase.LoadAssetAtPath(scene.path, typeof(SceneAsset)) as SceneAsset;
                }
            }

            Debug.Log("Scene [" + sceneObjectName +
                      "] cannot be used. Add this scene to the 'Scenes in the Build' in the build settings.");
            return null;
        }
    }
}
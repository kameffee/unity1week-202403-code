using System;
using System.Reflection;
using Unity1week202403.Data;
using UnityEditor;
using UnityEngine;

namespace Unity1week202403.Editor
{
    [CustomEditor(typeof(ButtonSePreset))]
    public class ButtonSePresetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var buttonSePreset = target as ButtonSePreset;

            using (new EditorGUILayout.HorizontalScope())
            {
                var hoverClipProperty = serializedObject.FindProperty("_hoverClip");
                EditorGUILayout.PropertyField(hoverClipProperty);
                using (new EditorGUI.DisabledGroupScope(hoverClipProperty.objectReferenceValue == null))
                {
                    if (GUILayout.Button("Play", GUILayout.Width(50)))
                    {
                        PlayClip(buttonSePreset.HoverClip);
                    }
                }
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                var clickClipProperty = serializedObject.FindProperty("_clickClip");
                EditorGUILayout.PropertyField(clickClipProperty);
                using (new EditorGUI.DisabledGroupScope(clickClipProperty.objectReferenceValue == null))
                {
                    if (GUILayout.Button("Play", GUILayout.Width(50)))
                    {
                        PlayClip(buttonSePreset.HoverClip);
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        // エディタ上でのサウンド再生.
        private void PlayClip(AudioClip clip)
        {
            if (clip == null) return;

            var audioUtil = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.AudioUtil");
            var playClipMethod = audioUtil.GetMethod(
                "PlayPreviewClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
                null
            );

            playClipMethod.Invoke(null, new object[] { clip, 0, false });
        }


        // エディタ上でのサウンドを停止する.
        private void StopClip(AudioClip clip)
        {
            if (clip == null) return;

            var unityEditorAssembly = typeof(AudioImporter).Assembly;
            var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            var method = audioUtilClass.GetMethod(
                "StopClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new Type[] { typeof(AudioClip) },
                null
            );

            method.Invoke(null, new object[] { clip });
        }
    }
}
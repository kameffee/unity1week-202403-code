using System;
using System.Reflection;
using Unity1week202403.Data;
using Unity1week202403.Presentation.UI;
using UnityEditor;
using UnityEngine;

namespace Unity1week202403.Editor
{
    [CustomEditor(typeof(ButtonSePlayer))]
    public class ButtonSePlayerEditor : UnityEditor.Editor
    {
        private static bool _isLocked = true;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // iteratorを使って、ButtonSePlayerのフィールドを描画する.
            var iterator = serializedObject.GetIterator();
            for (var enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                using var disable = new EditorGUI.DisabledGroupScope("m_Script" == iterator.propertyPath);
                EditorGUILayout.PropertyField(iterator, true);

                if (iterator.propertyPath == "_buttonSePreset")
                {
                    if (iterator.objectReferenceValue == null) continue;
                    var preset = new SerializedObject(iterator.objectReferenceValue);
                    PresetDrawer(preset);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private static void PresetDrawer(SerializedObject presetObject)
        {
            presetObject.Update();
            _isLocked = EditorGUILayout.ToggleLeft("Lock", _isLocked);

            using var indent = new EditorGUI.IndentLevelScope(1);

            var presetIterator = presetObject.GetIterator();
            for (var enterChildren = true; presetIterator.NextVisible(enterChildren); enterChildren = false)
            {
                if ("m_Script" == presetIterator.propertyPath) continue;
                if (presetIterator.propertyPath is "_clickClip" or "_hoverClip")
                {
                    using var horizontal = new EditorGUILayout.HorizontalScope();
                    using (new EditorGUI.DisabledGroupScope(_isLocked))
                    {
                        EditorGUILayout.PropertyField(presetIterator, true);
                    }
                    using (new EditorGUI.DisabledGroupScope(presetIterator.objectReferenceValue == null))
                    {
                        if (GUILayout.Button("Play", GUILayout.Width(50)))
                        {
                            var clip = presetIterator.objectReferenceValue as AudioClip;
                            PlayClip(clip);
                        }
                    }
                }
            }

            presetObject.ApplyModifiedProperties();
        }
        
        // エディタ上でのサウンド再生.
        private static void PlayClip(AudioClip clip)
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
    }
}
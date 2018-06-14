using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Components;
using UnityEditorInternal;

namespace Inspectors
{
    [CustomEditor(typeof(CommandLineEventHandler))]
    public class CommandLineEventHandlerInspector : Editor
    {
        private CommandLineEventHandlerReorderableList _commandList;
        private SerializedProperty _onMatchEvent;

        private void OnEnable()
        {
            _onMatchEvent = serializedObject.FindProperty("_onMatch");

            SerializedProperty property = serializedObject.FindProperty("_commands");
            _commandList = new CommandLineEventHandlerReorderableList(serializedObject, property);
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            _commandList.DoLayoutList();
            EditorGUILayout.PropertyField(_onMatchEvent);

            serializedObject.ApplyModifiedProperties();
        }

        private class CommandLineEventHandlerReorderableList : ReorderableList
        {
            public CommandLineEventHandlerReorderableList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
            {
                drawHeaderCallback += DrawHeader;
                drawElementCallback += DrawElement;
            }

            private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
            {
                SerializedProperty element = serializedProperty.GetArrayElementAtIndex(index);

                rect.height = EditorGUIUtility.singleLineHeight;
                rect.y++;

                EditorGUI.PropertyField(rect, element);
            }
            private void DrawHeader(Rect rect)
            {
                EditorGUI.LabelField(rect, "Commands");
            }
        }
    }    
}

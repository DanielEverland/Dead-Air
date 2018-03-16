using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameSettingsWindow : EditorWindow {

    [MenuItem("Window/Game Settings")]
    private static void ShowWindow()
    {
        GetWindow(typeof(GameSettingsWindow), false, "Settings", true);
    }

    private const float CATEGORY_WIDTH = 300;

    private Vector2 scrollPos;
    private int selected;

    private void OnGUI()
    {
        RenderCategories();
    }
    private void RenderCategories()
    {
        UtilityEditor.DrawSplit(new Vector2(CATEGORY_WIDTH - 2, 0), position.height);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(CATEGORY_WIDTH));
        EditorGUILayout.BeginVertical();

        DrawCategoryContent();

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }
    private void DrawCategoryContent()
    {
        GUIStyle normal = UtilityEditor.SelectionGridLabel.customStyles.First(x => x.name == "Default");
        GUIStyle active = UtilityEditor.SelectionGridLabel.customStyles.First(x => x.name == "Selected");
        
        for (int i = 0; i < 100; i++)
        {
            GUIStyle style = i == selected ? active : normal;

            if (GUILayout.Button("Test", style))
            {
                selected = i;
            }
        }
    }
}
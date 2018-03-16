using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class UtilityEditor {

    public const int BACKGROUND_WIDTH = 6;

    public static GUISkin BackgroundSkin
    {
        get
        {
            if (_backgroundSkin == null)
                _backgroundSkin = Load<GUISkin>(EDITOR_BACKGROUND_STYLE_NAME);

            return _backgroundSkin;
        }
    }
    private static GUISkin _backgroundSkin;

    public static GUISkin SelectionGridLabel
    {
        get
        {
            if (_selectionGridLabel == null)
                _selectionGridLabel = Load<GUISkin>(SELECTION_GRID_LABEL_STYLE_NAME);

            return _selectionGridLabel;
        }
    }
    private static GUISkin _selectionGridLabel;

    public static Texture2D ActiveBackground
    {
        get
        {
            if(_activeBackground == null)
            {
                _activeBackground = new Texture2D(1, 1);
                _activeBackground.SetPixel(0, 0, ACTIVE_LABEL_BACKGROUND_COLOR);
                _activeBackground.Apply();
            }

            return _activeBackground;
        }
    }
    private static Texture2D _activeBackground;

    private const string EDITOR_BACKGROUND_STYLE_NAME = "EditorBackgroundSkin";
    private const string SELECTION_GRID_LABEL_STYLE_NAME = "SelectionGridLabel";
    private static readonly Color ACTIVE_LABEL_BACKGROUND_COLOR = new Color32(62, 95, 150, 255);

    public static void DrawSplit(Vector2 position, float height)
    {
        Rect rect = new Rect(position.x, position.y - 2, BACKGROUND_WIDTH, height + 4);

        GUILayout.BeginArea(rect, BackgroundSkin.window);
        GUILayout.EndArea();

        EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeHorizontal);
    }
    private static T Load<T>(string name) where T : Object
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).ToString());

        foreach (string id in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(id);

            T obj = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if (obj.name == name)
                return obj;
        }

        return default(T);
    }
}

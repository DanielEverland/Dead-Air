using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class GameSettingsWindow : EditorWindow {

    [MenuItem("Window/Game Settings")]
    private static void ShowWindow()
    {
        GetWindow(typeof(GameSettingsWindow), false, "Settings", true);
    }

    private const float CATEGORY_WIDTH = 199;

    private static GameSettings _settingsInstance;
    private Vector2 scrollPos;
    private System.Type _selectedType;

    [DidReloadScripts()]
    private static void Initialize()
    {
        _settingsInstance = UtilityEditor.Load<GameSettings>("GameSettings");
        
        LinkedList<System.Type> settings = new LinkedList<System.Type>();
        
        foreach (System.Type type in Assembly.Load("Assembly-CSharp").GetTypes())
        {
            if(type.GetInterfaces().Contains(typeof(ISettings)))
            {
                if (typeof(ScriptableObject).IsAssignableFrom(type))
                {
                    settings.AddLast(type);
                }
            }
        }

        _settingsInstance.PollTypes(settings);
    }
    private void PollInit()
    {
        if (_settingsInstance == null)
            Initialize();
    }
    private void OnGUI()
    {
        PollInit();

        if (_settingsInstance.Settings.Count == 0)
            return;

        RenderCategories();
        RenderInspector();
    }
    private void RenderInspector()
    {
        ScriptableObject selectedObject = _settingsInstance.Settings[_selectedType];

        Editor editor = Editor.CreateEditor(selectedObject);

        float xStart = CATEGORY_WIDTH + UtilityEditor.BACKGROUND_WIDTH - 2;
        Rect inspectorRect = new Rect(xStart, -2, position.width - xStart, position.height);
        
        GUILayout.BeginArea(inspectorRect);
            editor.DrawHeader();
            editor.DrawDefaultInspector();
        GUILayout.EndArea();
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
        
        if(_selectedType == null)
        {
            AssignFirstType();
        }
        if (!_settingsInstance.Settings.ContainsKey(_selectedType))
        {
            AssignFirstType();
        }

        foreach (System.Type type in _settingsInstance.Settings.Keys)
        {
            GUIStyle style = _selectedType == type ? active : normal;

            if (GUILayout.Button(GetName(type), style))
            {
                _selectedType = type;
            }
        }
    }
    private string GetName(System.Type type)
    {
        string name = type.Name;

        if(name.EndsWith("Settings"))
        {
            return name.Substring(0, name.LastIndexOf("Settings"));
        }

        return name;
    }
    private void AssignFirstType()
    {
        _selectedType = _settingsInstance.Settings.Keys.ElementAt(0);
    }
}
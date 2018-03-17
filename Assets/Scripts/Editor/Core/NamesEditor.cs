using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Names))]
public class NamesEditor : Editor {

    public Names Target { get { return (Names)target; } }
    
    private Vector2 ScrollPosition
    {
        get
        {
            if (!_scrollPositions.ContainsKey(_container))
            {
                _scrollPositions.Add(_container, Vector2.zero);
            }

            return _scrollPositions[_container];
        }
        set
        {
            _scrollPositions[_container] = value;
        }
    }
    private bool IsEditing
    {
        get
        {
            return GUI.GetNameOfFocusedControl() != string.Empty;
        }
    }

    private static readonly HashSet<KeyCode> _dropKeys = new HashSet<KeyCode>()
    {
        KeyCode.Escape,
        KeyCode.KeypadEnter,
        KeyCode.Return,
    };

    private const float WINDOW_HEIGHT = 200;
    private const float HEADER_HEIGHT = 16;
    private const float SPACING = 20;
    private const float SCROLL_VIEW_ELEMENT_HEIGHT = 15;
    private const float SCOLL_VIEW_TOP_PADDING = 3;
    
    private Styles _styles;
    private Names.NameContainer _container;
    private Rect _windowRect;
    private Dictionary<Names.NameContainer, Vector2> _scrollPositions = new Dictionary<Names.NameContainer, Vector2>();
    private Names.NameContainer _selectedContainer;
    private Rect _selectedRect;

    public override void OnInspectorGUI()
    {
        if (_styles == null)
            _styles = new Styles();

        PollKeyEvents();

        foreach (Names.NameContainer container in Target.Containers)
        {
            DrawCollection(container);

            GUILayoutUtility.GetRect(0, SPACING);
        }
    }
    private void PollKeyEvents()
    {
        Event currentEvent = Event.current;

        if (currentEvent.type == EventType.KeyDown)
        {
            if (_dropKeys.Contains(currentEvent.keyCode))
            {
                Deselect();
            }
        }
    }
    private void PollInputEvents()
    {
        Event currentEvent = Event.current;
        Vector2 mousePosition = currentEvent.mousePosition;        
        
        if(currentEvent.type == EventType.MouseDown)
        {
            if(_selectedRect != Rect.zero && !_selectedRect.Contains(mousePosition))
            {
                Deselect();
            }            
        }
    }
    private void Deselect()
    {
        GUIUtility.keyboardControl = 0;

        Repaint();
    }
    private void DrawCollection(Names.NameContainer container)
    {
        _container = container;
        _windowRect = GUILayoutUtility.GetRect(Screen.width, WINDOW_HEIGHT, _styles.BackgroundStyle);
        
        DrawBackground();
        DrawWindowHeader();
        DrawScrollView();

        if(_selectedContainer == _container)
            PollInputEvents();
    }
    private void DrawScrollView()
    {
        Rect scrollRect = new Rect(_windowRect.x, _windowRect.y + HEADER_HEIGHT + SCOLL_VIEW_TOP_PADDING, _windowRect.width, _windowRect.height - HEADER_HEIGHT - SCOLL_VIEW_TOP_PADDING);        
        Rect viewRect = new Rect(0, 0, scrollRect.width, _container.Collection.Count * SCROLL_VIEW_ELEMENT_HEIGHT);

        if(!IsEditing)
        {
            _container.Collection.Sort();
        }

        ScrollPosition = GUI.BeginScrollView(scrollRect, ScrollPosition, viewRect);
        for (int i = 0; i < _container.Collection.Count; i++)
        {
            _container.Collection[i] = DrawElement(new Rect(0, SCROLL_VIEW_ELEMENT_HEIGHT * i, viewRect.width, SCROLL_VIEW_ELEMENT_HEIGHT), i);
        }
        GUI.EndScrollView();
    }
    private string DrawElement(Rect rect, int index)
    {
        string name = _container.Collection[index];

        GUI.SetNextControlName(string.Format("{0}: {1}", index, name));
        name = GUI.TextField(rect, name, _styles.ScrollViewElement);

        return name;
    }
    private void DrawWindowHeader()
    {
        Rect headerRect = new Rect(_windowRect.x + 1, _windowRect.y + 1, _windowRect.width - 2, HEADER_HEIGHT);
        

        //Background
        EditorGUI.LabelField(headerRect, GUIContent.none, _styles.ToolbarBackground);

        //Label
        EditorGUI.LabelField(headerRect, _container.NameType, _styles.ToolbarLabel);

        //Create Button
        GUIContent content = new GUIContent("Create");
        GUIStyle style = _styles.ToolbarButton;
        float buttonWidth = style.CalcSize(content).x;

        Rect buttonRect = new Rect(headerRect.width - buttonWidth / 2, headerRect.y, buttonWidth, headerRect.height);

        if(GUI.Button(buttonRect, content, style))
        {
            _container.Collection.Add("_NewName_");
            
            Deselect();
        }
    }
    private void DrawBackground()
    {
        EditorGUI.LabelField(_windowRect, GUIContent.none, _styles.BackgroundStyle);
    }

    private class Styles
    {
        public GUIStyle BackgroundStyle = new GUIStyle("AnimationKeyframeBackground");
        public GUIStyle ToolbarBackground = new GUIStyle("Toolbar");
        public GUIStyle ToolbarLabel = new GUIStyle("Toolbar");
        public GUIStyle ToolbarButton = new GUIStyle("toolbarbutton");
        public GUIStyle ScrollViewElement = new GUIStyle("Label");

        public Styles()
        {
            ToolbarLabel.alignment = TextAnchor.MiddleLeft;

            ScrollViewElement.contentOffset = new Vector2(5, 0);
            ScrollViewElement.alignment = TextAnchor.MiddleLeft;
            ScrollViewElement.clipping = TextClipping.Overflow;
        }
    }
}

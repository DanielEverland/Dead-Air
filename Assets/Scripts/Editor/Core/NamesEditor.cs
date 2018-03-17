﻿using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

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
    public string SearchQuery
    {
        get
        {
            if (!_searchQuery.ContainsKey(_container))
            {
                _searchQuery.Add(_container, "");
            }

            return _searchQuery[_container];
        }
        set
        {
            _searchQuery[_container] = value;
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
    private const int SEARCH_RESULTS_MAX = 50;
    
    private Styles _styles;
    private Names.NameContainer _container;
    private Rect _windowRect;
    private Dictionary<Names.NameContainer, Vector2> _scrollPositions = new Dictionary<Names.NameContainer, Vector2>();
    private Dictionary<Names.NameContainer, string> _searchQuery = new Dictionary<Names.NameContainer, string>();
    private Rect _selectedRect;

    public override void OnInspectorGUI()
    {
        if (_styles == null)
            _styles = new Styles();

        PollKeyEvents();

        if (Target.containers == null)
            Target.CreateContainer();

        if (Target.containers.Count == 0)
            Target.CreateContainer();

        GUI.changed = false;

        foreach (Names.NameContainer container in Target.containers)
        {
            DrawCollection(container);

            GUILayoutUtility.GetRect(0, SPACING);
        }

        if(GUI.changed)
        {
            EditorUtility.SetDirty(target);
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
    }
    private void DrawScrollView()
    {
        Rect scrollRect = new Rect(_windowRect.x, _windowRect.y + HEADER_HEIGHT + SCOLL_VIEW_TOP_PADDING, _windowRect.width, _windowRect.height - HEADER_HEIGHT - SCOLL_VIEW_TOP_PADDING);        
        
        if (SearchQuery.Length == 0)
            return;

        if(!IsEditing)
        {
            _container.Collection.Sort();
        }

        IEnumerable<string> searchResults = _container.Collection.Where(x => x.Contains(SearchQuery));

        if (searchResults.Count() > SEARCH_RESULTS_MAX)
            return;

        Rect viewRect = new Rect(0, 0, scrollRect.width - 50, searchResults.Count() * SCROLL_VIEW_ELEMENT_HEIGHT);

        List<SearchResult> toView = new List<SearchResult>(searchResults.Select(x =>
        {
            return new SearchResult(x, _container.Collection.IndexOf(x));
        }));
        
        ScrollPosition = GUI.BeginScrollView(scrollRect, ScrollPosition, viewRect);
        for (int i = toView.Count - 1; i >= 0; i--)
        {
            SearchResult currentResult = toView[i];

            _container.Collection[currentResult.index] = DrawElement(new Rect(0, SCROLL_VIEW_ELEMENT_HEIGHT * i, viewRect.width, SCROLL_VIEW_ELEMENT_HEIGHT), currentResult.index, currentResult.value);

            if (_container.Collection[i] == "")
                _container.Collection.RemoveAt(i);
        }
        GUI.EndScrollView();
    }
    private string DrawElement(Rect rect, int index, string name)
    {
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
        EditorGUI.LabelField(headerRect, string.Format("{0} ({1})", _container.NameType, _container.Collection.Count), _styles.ToolbarLabel);

        //Search
        Rect searchRect = new Rect(headerRect.width * (1f / 3f), headerRect.y + 2, headerRect.width / 3, EditorGUIUtility.singleLineHeight);
        
        SearchQuery = UtilityEditor.SearchField(searchRect, SearchQuery);

        //Create Button
        GUIContent content = new GUIContent("New");
        GUIStyle buttonStyle = _styles.ToolbarButton;
        float buttonWidth = buttonStyle.CalcSize(content).x;

        Rect buttonRect = new Rect(headerRect.width - buttonWidth, headerRect.y, buttonWidth, headerRect.height);

        if(GUI.Button(buttonRect, content, buttonStyle))
        {
            _container.Collection.Add("_NewName_");
            
            Deselect();
        }

        //Batch Add
        content = new GUIContent("Batch Add");
        buttonWidth = buttonStyle.CalcSize(content).x;
        buttonRect.x -= buttonWidth;
        buttonRect.width = buttonWidth;

        if (GUI.Button(buttonRect, content, buttonStyle))
        {
            NamesBatchAddWindow.Create(_container);

            Deselect();
        }
    }
    private void DrawBackground()
    {
        EditorGUI.LabelField(_windowRect, GUIContent.none, _styles.BackgroundStyle);
    }

    private struct SearchResult
    {
        public SearchResult(string value, int index)
        {
            this.value = value;
            this.index = index;
        }

        public string value;
        public int index;
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

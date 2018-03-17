using System.Collections.Generic;
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
    public int? SelectedIndex
    {
        get
        {
            if (!_selectedName.ContainsKey(_container))
            {
                _selectedName.Add(_container, null);
            }

            int? index = _selectedName[_container];

            if (!index.HasValue)
                return null;

            return index.Value;
        }
        set
        {
            _selectedName[_container] = value;
        }
    }
    public Rect? SelectedRect
    {
        get
        {
            if (!_selectedRects.ContainsKey(_container))
            {
                _selectedRects.Add(_container, null);
            }

            Rect? rect = _selectedRects[_container];

            if (!rect.HasValue)
                return null;

            return rect.Value;
        }
        set
        {
            _selectedRects[_container] = value;
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
    private const float SPACING = 10;
    private const float SCROLL_VIEW_ELEMENT_HEIGHT = 15;
    private const float SCOLL_VIEW_TOP_PADDING = 3;
    private const int SEARCH_RESULTS_MAX = 50;

    private static Vector4 _padding = new Vector4(10, 50, 10, 0);

    private Styles _styles;
    private Names.NameContainer _container;
    private Rect _windowRect;
    private Dictionary<Names.NameContainer, Vector2> _scrollPositions = new Dictionary<Names.NameContainer, Vector2>();
    private Dictionary<Names.NameContainer, string> _searchQuery = new Dictionary<Names.NameContainer, string>();
    private Dictionary<Names.NameContainer, int?> _selectedName = new Dictionary<Names.NameContainer, int?>();
    private Dictionary<Names.NameContainer, Rect?> _selectedRects = new Dictionary<Names.NameContainer, Rect?>();

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

        for (int i = 0; i < Target.containers.Count; i++)
        {
            _windowRect = new Rect(_padding.x, (WINDOW_HEIGHT + SPACING) * i + _padding.y, Screen.width - (_padding.x + _padding.z), WINDOW_HEIGHT);
            
            DrawCollection(Target.containers[i]);
        }

        if(GUI.changed)
        {
            EditorUtility.SetDirty(target);
            CheckForEmptyElements();
        }
    }
    private void CheckForEmptyElements()
    {
        foreach (Names.NameContainer container in Target.containers)
        {
            _container = container;

            List<int> indexesToRemove = new List<int>(container.Collection.Where(x => x == "").Select(x => container.Collection.IndexOf(x)));

            foreach (int index in indexesToRemove)
            {
                if (SelectedIndex == index)
                    continue;

                container.Collection.RemoveAt(index);
            }
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

                _searchQuery.Clear();
            }
        }
    }
    private void PollMouseEvents()
    {
        Event currentEvent = Event.current;
        Vector2 mousePos = currentEvent.mousePosition;

        if (SelectedRect == null)
            return;

        if(currentEvent.type == EventType.MouseDown)
        {
            if (!SelectedRect.Value.Contains(mousePos))
                Deselect();
        }
    }
    private void Deselect()
    {
        GUIUtility.keyboardControl = 0;
        SelectedIndex = null;
        SelectedRect = null;

        _selectedName.Clear();
        _selectedRects.Clear();

        Repaint();
    }
    private void Select(int index, Rect rect)
    {
        SelectedIndex = index;
        SelectedRect = rect;

        Repaint();
    }
    private void DrawCollection(Names.NameContainer container)
    {
        _container = container;
        
        DrawBackground();
        DrawWindowHeader();
        DrawScrollView();

        PollMouseEvents();
    }
    private void DrawScrollView()
    {
        Rect scrollRect = new Rect(_windowRect.x, _windowRect.y + HEADER_HEIGHT + SCOLL_VIEW_TOP_PADDING, _windowRect.width, _windowRect.height - HEADER_HEIGHT - SCOLL_VIEW_TOP_PADDING);        
        
        if (SearchQuery.Length == 0)
            return;
        
        IEnumerable<string> searchResults = _container.Collection.Where(x => x.Contains(SearchQuery, System.StringComparison.OrdinalIgnoreCase));

        if (searchResults.Count() > SEARCH_RESULTS_MAX)
            return;
        
        Rect viewRect = new Rect(0, 0, scrollRect.width - 50, searchResults.Count() * SCROLL_VIEW_ELEMENT_HEIGHT);

        List<SearchResult> toView = CreateSearchResult(searchResults);
        
        if (!IsEditing)
        {
            toView = toView.OrderBy(x => x.value).ToList();
        }

        if (SelectedIndex != null)
        {
            SearchResult selectedSearchResult = new SearchResult(_container.Collection[SelectedIndex.Value], SelectedIndex.Value);

            if (!toView.Contains(selectedSearchResult))
                toView.Insert(0, selectedSearchResult);
        }

        ScrollPosition = GUI.BeginScrollView(scrollRect, ScrollPosition, viewRect);
        for (int i = 0; i < toView.Count; i++)
        {
            SearchResult currentResult = toView[i];

            _container.Collection[currentResult.index] = DrawElement(new Rect(0, SCROLL_VIEW_ELEMENT_HEIGHT * i, viewRect.width, SCROLL_VIEW_ELEMENT_HEIGHT), currentResult.index, currentResult.value);
        }
        GUI.EndScrollView();
    }
    /// <summary>
    /// Creates a list of search results where every item has a corresponding index with the container collection
    /// </summary>
    private List<SearchResult> CreateSearchResult(IEnumerable<string> searchResults)
    {
        List<SearchResult> results = new List<SearchResult>();
        List<string> containerCollection = _container.Collection;
        Dictionary<string, List<int>> indexes = new Dictionary<string, List<int>>();

        foreach (string item in searchResults)
        {
            if (indexes.ContainsKey(item))
                continue;

            List<int> indexesOfString = Enumerable.Range(0, containerCollection.Count)
                .Where(x => containerCollection[x] == item).ToList();

            indexes.Add(item, indexesOfString);
        }

        foreach (string name in searchResults)
        {
            int index = indexes[name][0];
            indexes[name].RemoveAt(0);

            results.Add(new SearchResult(name, index));
        }
        
        return results;
    }
    private string DrawElement(Rect rect, int index, string name)
    {
        if(SelectedIndex == index)
        {
            name = GUI.TextField(rect, name, (SelectedIndex == index) ? _styles.SelectedElementBackground : _styles.ScrollViewElement);               
        }
        else if(GUI.Button(rect, name, _styles.ScrollViewElement))
        {
            Select(index, rect);
        }
        
        return name;
    }
    private void DrawWindowHeader()
    {
        Rect headerRect = new Rect(_windowRect.x + 1, _windowRect.y + 1, _windowRect.width - 2, HEADER_HEIGHT);
        
        //Background
        EditorGUI.LabelField(headerRect, GUIContent.none, _styles.ToolbarBackground);

        //Label
        EditorGUI.LabelField(headerRect, string.Format("{0} ({1})", _container.NameType, _container.Collection.Count), _styles.ToolbarLabel);
        
        //Delete Button
        GUIContent content = new GUIContent("Delete");
        GUIStyle buttonStyle = _styles.ToolbarButton;
        float buttonWidth = buttonStyle.CalcSize(content).x;

        Rect buttonRect = new Rect(headerRect.width - buttonWidth, headerRect.y, buttonWidth, headerRect.height);

        if(GUI.Button(buttonRect, content, SelectedIndex != null ? buttonStyle : _styles.DisabledToolbarButton))
        {
            Delete();
        }

        //Create Button
        content = new GUIContent("New");
        buttonWidth = buttonStyle.CalcSize(content).x;
        buttonRect.x -= buttonWidth;
        buttonRect.width = buttonWidth;

        if (GUI.Button(buttonRect, content, buttonStyle))
        {
            New();
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

        //Search
        float searchRectWidth = headerRect.width / 4;
        Rect searchRect = new Rect(buttonRect.x - (searchRectWidth + SPACING), headerRect.y + 2, searchRectWidth, EditorGUIUtility.singleLineHeight);

        SearchQuery = UtilityEditor.SearchField(searchRect, SearchQuery);
    }
    private void Delete()
    {
        if (SelectedIndex == null)
            return;

        _container.Collection.RemoveAt(SelectedIndex.Value);
        _searchQuery.Clear();

        Deselect();
    }
    private void New()
    {
        string newName = "_NewName_";
        _container.Collection.Add(newName);
        SearchQuery = newName;

        Deselect();
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
        public GUIStyle DisabledToolbarButton;
        public GUIStyle SelectedElementBackground;

        public Styles()
        {
            ToolbarLabel.alignment = TextAnchor.MiddleLeft;

            ScrollViewElement.contentOffset = new Vector2(5, 0);
            ScrollViewElement.alignment = TextAnchor.MiddleLeft;
            ScrollViewElement.clipping = TextClipping.Overflow;

            SelectedElementBackground = new GUIStyle(ScrollViewElement);
            SelectedElementBackground.normal.background = UtilityEditor.SelectionGridLabel.customStyles.First(x => x.name == "Selected").normal.background;
            SelectedElementBackground.normal.textColor = Color.white;

            DisabledToolbarButton = new GUIStyle(ToolbarButton);
            DisabledToolbarButton.normal.textColor = new Color32(100, 100, 100, 255);
        }
    }
}

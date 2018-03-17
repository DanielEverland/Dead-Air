using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

public class NamesBatchAddWindow : EditorWindow {

	public static void Create(Names.NameContainer container)
    {
        NamesBatchAddWindow window = GetWindow<NamesBatchAddWindow>(true, "Batch Add - " + container.NameType);
        window.minSize = MIN_SIZE;
        window._container = container;
        window.Show();
    }

    private const string DEFAULT_REGEX = @"\b[^\d\W]+\b";
    private const float SPACING = 10;
    private const float FOOTER_HEIGHT = 20;
    private const int MAX_STRING_SIZE = 7000;

    private static readonly Vector2 MIN_SIZE = new Vector2(400, 300);
    private static readonly Vector4 PADDING = new Vector4(5, 10, 5, 5);

    private bool MegaStringMode
    {
        get
        {
            if (_input == null)
                return false;

            return _input.Length > MAX_STRING_SIZE;
        }
    }
    
    private string _regex = DEFAULT_REGEX;
    private string _input;
    private Rect _regexRect;
    private Rect _textAreaRect;
    private Rect _footerRect;
    private Names.NameContainer _container;

    private void OnGUI()
    {
        _regexRect = new Rect(PADDING.x, PADDING.y, position.width - (PADDING.z + PADDING.x), EditorGUIUtility.singleLineHeight);
        _footerRect = new Rect(PADDING.x, position.height - (FOOTER_HEIGHT + PADDING.w), position.width - (PADDING.x + PADDING.z), FOOTER_HEIGHT);
        _textAreaRect = new Rect(PADDING.x, _regexRect.y + _regexRect.height + SPACING, _footerRect.width, position.height - (_footerRect.height + _regexRect.height + _regexRect.y + SPACING * 2 + PADDING.w));

        DrawRegex();
        DrawTextArea();
        DrawFooter();
    }
    private void DrawRegex()
    {
        GUIContent labelContent = new GUIContent("Regular Expression");
        float labelWidth = EditorStyles.label.CalcSize(labelContent).x;
                
        Rect labelRect = new Rect(_regexRect.x, _regexRect.y, labelWidth, EditorGUIUtility.singleLineHeight);
        Rect textRect = new Rect(_regexRect.x + labelRect.width + SPACING, _regexRect.y, _regexRect.width - labelRect.width - SPACING, EditorGUIUtility.singleLineHeight);

        EditorGUI.LabelField(labelRect, labelContent);
        _regex = EditorGUI.TextArea(textRect, _regex);
    }
    private void DrawTextArea()
    {
        if (MegaStringMode)
            return;

        _input = EditorGUI.TextArea(_textAreaRect, _input);

        if (MegaStringMode)
        {
            Debug.LogWarning("Too many characters to render");
        }
    }
    private void DrawFooter()
    {
        GUIContent applyRegexContent = new GUIContent("Apply");
        GUIContent commitContent = new GUIContent("Commit");

        GUIStyle style = EditorStyles.label;
        style.clipping = TextClipping.Overflow;

        float applyRegexWidth = style.CalcSize(applyRegexContent).x + 20;
        float commitWidth = style.CalcSize(commitContent).x + 20;

        Rect firstButton = new Rect(_footerRect.x, _footerRect.y, applyRegexWidth, EditorGUIUtility.singleLineHeight);
        Rect secondButton = new Rect(firstButton.x + firstButton.width + SPACING, firstButton.y, commitWidth, EditorGUIUtility.singleLineHeight);

        if(GUI.Button(firstButton, applyRegexContent))
        {
            Deselect();
            Apply();
        }

        if(GUI.Button(secondButton, commitContent))
        {
            Commit();
            Close();
        }
    }
    private void Commit()
    {
        string[] names = _input.Split('\n');

        if (MegaStringMode)
        {
            Debug.Log("Applying " + names.Length + " names");
        }

        _container.Apply(names);
    }
    private void Apply()
    {
        Regex regex = new Regex(_regex);
        string output = "";

        MatchCollection matchCollection = regex.Matches(_input);
        foreach (Match match in matchCollection)
        {
            string name = match.Value;
            name = name.ToLower();
            name = char.ToUpper(name[0]) + name.Substring(1);
            
            output += name + "\n";
        }

        if(MegaStringMode)
        {
            Debug.Log("Found " + matchCollection.Count + " matches");
        }
        
        _input = output;
    }
    private void Deselect()
    {
        GUIUtility.keyboardControl = 0;

        Repaint();
    }
}

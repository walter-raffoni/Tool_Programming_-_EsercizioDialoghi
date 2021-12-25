using UnityEditor;
using UnityEngine;

public class EditDialogueStringWindow : EditorWindow
{
    public string stringID;
    public string characterName;
    public Sprite characterSprite;
    public Color textColor;
    public float textSpeed;
    public string text;
    public bool confirm;

    private void OnGUI()
    {
        titleContent = new GUIContent("Edit dialogue line");

        GUILayout.Label("String ID");

        GUI.enabled = false;
        EditorGUILayout.TextField(stringID);
        GUI.enabled = true;

        GUILayout.Label("Character name");
        EditorGUILayout.TextField(characterName);
        GUILayout.Label("Character sprite");
        EditorGUILayout.ObjectField(characterSprite, typeof(Sprite), true);
        GUILayout.Label("Text color");
        EditorGUILayout.ColorField(textColor);
        GUILayout.Label("Text speed");
        EditorGUILayout.FloatField(textSpeed);
        GUILayout.Label("Text");
        GUIStyle style = new GUIStyle(EditorStyles.textArea);
        style.wordWrap = true;
        style.stretchHeight = true;
        text = EditorGUILayout.TextArea(text, style);

        GUILayout.Space(25);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("OK"))
        {
            confirm = true;
            Close();
        }

        if (GUILayout.Button("Cancel")) Close();
    }
}
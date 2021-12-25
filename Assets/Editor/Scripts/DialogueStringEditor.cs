using UnityEditor;
using UnityEngine;

public class DialogueStringEditor : EditorWindow
{
    public string stringID;
    public string characterName;
    public Sprite characterSprite;
    public Color textColor;
    public float textSpeed;
    public string text;
    private bool confirm;

    private void OnGUI()
    {
        titleContent = new GUIContent("New dialogue line");

        GUILayout.Label("String ID");
        stringID = EditorGUILayout.TextField(stringID);
        GUILayout.Label("Character name");
        characterName = EditorGUILayout.TextField(characterName);
        GUILayout.Label("Character sprite");
        characterSprite = (Sprite)EditorGUILayout.ObjectField(characterSprite, typeof(Sprite), true);
        GUILayout.Label("Text color");
        textColor = EditorGUILayout.ColorField(textColor);
        GUILayout.Label("Text speed");
        textSpeed = EditorGUILayout.FloatField(textSpeed);
        GUILayout.Label("Text");
        GUIStyle style = new GUIStyle(EditorStyles.textArea);
        style.wordWrap = true;
        style.stretchHeight = true;
        text = EditorGUILayout.TextArea(text, style);

        EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true)); //Serve per mantenere i pulsanti nella parte bassa della finestra
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("OK"))
        {
            confirm = true;
            Close();
        }

        if (GUILayout.Button("Cancel"))
        {
            stringID = null;
            Close();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void OnDestroy()
    {
        if (!confirm) stringID = null;
    }
}
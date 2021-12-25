using System.IO;
using UnityEditor;
using UnityEngine;

public class DialoguesEditor : EditorWindow
{
    [MenuItem("Tools/Dialogues Editor")]
    private static void OpenWindow() => GetWindow<DialoguesEditor>();

    private Vector2 scrollPosition;
    private string[] dialogues;
    private string[] dialogueLabels;
    private int selectedDialogueIndex;
    private string characterName;

    private void GetAllDialogues()
    {
        dialogues = AssetDatabase.FindAssets("t: Dialogue");
        dialogueLabels = new string[dialogues.Length];

        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogues[i] = AssetDatabase.GUIDToAssetPath(dialogues[i]);
            dialogueLabels[i] = Path.GetFileName(dialogues[i]);
        }
    }

    private void OnGUI()
    {
        titleContent = new GUIContent("Dialogues Editor");

        if (GUILayout.Button("New dialogue")) NewDialogue();

        GUILayout.Space(5);
        GUILayout.BeginHorizontal();

        GUILayout.Label("Search by character name");
        characterName = EditorGUILayout.TextField(characterName);

        GUILayout.EndHorizontal();
        GUILayout.Space(15);

        GetAllDialogues();

        if (dialogues.Length == 0)
        {
            EditorGUILayout.HelpBox("No dialogue lines found", MessageType.Error);
            return;
        }

        selectedDialogueIndex = EditorGUILayout.Popup("Dialogue", selectedDialogueIndex, dialogueLabels);

        GUILayout.Space(15);

        GUILayout.Label("Strings:");

        Dialogue dialogue = AssetDatabase.LoadAssetAtPath<Dialogue>(dialogues[selectedDialogueIndex]);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int i = 0; i < dialogue.strings.Count; i++)
        {
            if ((characterName == string.Empty) || (characterName == null)) //Quando si apre per la prima volta la finestra è null, se cancelli è empty, serve per non far buggare nulla
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("e", EditorStyles.miniButtonLeft, GUILayout.Width(25)))
                {
                    EditDialogueStringWindow editWindow = GetWindow<EditDialogueStringWindow>();

                    DialogueStringData(dialogue, i, editWindow);

                    if (editWindow.confirm)
                    {
                        dialogue.strings[i].text = editWindow.text;
                        EditorUtility.SetDirty(dialogue);
                    }
                }
                if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(25)))
                {
                    if (EditorUtility.DisplayDialog("Confirm", $"Do you really want to remove the string {dialogue.strings[i].stringID}?", "Yes", "No"))
                    {
                        RemoveString(dialogue.strings[i].stringID);
                        return;
                    }
                }
                EditorGUILayout.LabelField(dialogue.strings[i].stringID);
                EditorGUILayout.EndHorizontal();
            }
            if (characterName == dialogue.strings[i].characterName)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("e", EditorStyles.miniButtonLeft, GUILayout.Width(25)))
                {
                    EditDialogueStringWindow editWindow = GetWindow<EditDialogueStringWindow>();
                    DialogueStringData(dialogue, i, editWindow);

                    if (editWindow.confirm)
                    {
                        dialogue.strings[i].text = editWindow.text;
                        EditorUtility.SetDirty(dialogue);
                    }
                }
                if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(25)))
                {
                    if (EditorUtility.DisplayDialog("Confirm", $"Do you really want to remove the string {dialogue.strings[i].stringID}?", "Yes", "No"))
                    {
                        RemoveString(dialogue.strings[i].stringID);
                        return;
                    }
                }
                EditorGUILayout.LabelField(dialogue.strings[i].stringID);
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("New dialogue line")) NewDialogueLine(dialogue);
    }

    private static void DialogueStringData(Dialogue dialogue, int i, EditDialogueStringWindow editWindow)
    {
        editWindow.stringID = dialogue.strings[i].stringID;
        editWindow.characterName = dialogue.strings[i].characterName;
        editWindow.characterSprite = dialogue.strings[i].characterSprite;
        editWindow.textColor = dialogue.strings[i].textColor;
        editWindow.textSpeed = dialogue.strings[i].textSpeed;
        editWindow.text = dialogue.strings[i].text;

        editWindow.ShowModalUtility();
    }

    private void NewDialogueLine(Dialogue dialogue)
    {
        DialogueStringEditor stringEditor = GetWindow<DialogueStringEditor>();
        stringEditor.ShowModalUtility();

        if (!string.IsNullOrWhiteSpace(stringEditor.stringID))
        {
            string newStringIndex = stringEditor.stringID.ToUpper();//per rendere l'indice tutto maiuscolo
            string characterName = stringEditor.characterName;
            Sprite characterSprite = stringEditor.characterSprite;
            Color textColor = stringEditor.textColor;
            float textSpeed = stringEditor.textSpeed;
            string text = stringEditor.text;

            if (dialogue.strings.Exists(s => s.stringID == newStringIndex))
            {
                EditorUtility.DisplayDialog("Error", $"String {newStringIndex} already exists", "OK");
            }
            else
            {
                AddString(newStringIndex, characterName, characterSprite, textColor, textSpeed, text);
            }
        }
    }

    public void AddString(string stringIndex, string name, Sprite charSprite, Color textCol, float speed, string textt)
    {
        foreach (string dialoguePath in dialogues)
        {
            Dialogue dialogue = AssetDatabase.LoadAssetAtPath<Dialogue>(dialoguePath);

            Dialogue.TranslatedString newString = new Dialogue.TranslatedString()
            {
                stringID = stringIndex,
                characterName = name,
                characterSprite = charSprite,
                textColor = textCol,
                textSpeed = speed,
                text = textt
            };

            dialogue.strings.Add(newString);
            EditorUtility.SetDirty(dialogue);
        }
    }

    private void NewDialogue()
    {
        string path = EditorUtility.SaveFilePanelInProject("New dialogue", "Dialogue", "asset", "New dialogue path");

        if (!string.IsNullOrWhiteSpace(path))
        {
            Dialogue newDialogue = CreateInstance<Dialogue>();
            Dialogue currentDialogue = AssetDatabase.LoadAssetAtPath<Dialogue>(dialogues[selectedDialogueIndex]);

            AssetDatabase.CreateAsset(newDialogue, path);

            newDialogue.strings.AddRange(currentDialogue.strings);

            EditorUtility.SetDirty(newDialogue);
        }
    }

    private void RemoveString(string stringIndex)
    {
        foreach (string dialoguePath in dialogues)
        {
            Dialogue dialogue = AssetDatabase.LoadAssetAtPath<Dialogue>(dialoguePath);
            dialogue.strings.RemoveAll(s => s.stringID == stringIndex);

            EditorUtility.SetDirty(dialogue);
        }
    }
}
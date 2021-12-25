using TMPro;
using UnityEngine;

public class TranslatedMesh : MonoBehaviour
{
    public string stringIndex;
    private string lastStringIndex = string.Empty;

    public enum stringType { characterName, characterSprite, text };
    public stringType StringType;
    
    private void Update()
    {
        if (GameController.instance == null) return;

        if (stringIndex != lastStringIndex)
        {
            lastStringIndex = stringIndex;

            if (StringType == stringType.characterName)
            {
                TextMeshProUGUI textMesh = GetComponent<TextMeshProUGUI>();
                textMesh.text = GameController.instance.currentDialogue.GetString(stringIndex, stringType.characterName);
            }
            if (StringType == stringType.characterSprite)
            {
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                sr.sprite = GameController.instance.currentDialogue.GetSprite(stringIndex, stringType.characterSprite);
            }
            if (StringType == stringType.text)
            {
                TextMeshProUGUI textMesh = GetComponent<TextMeshProUGUI>();
                textMesh.text = GameController.instance.currentDialogue.GetString(stringIndex, stringType.text);
                textMesh.color = GameController.instance.currentDialogue.GetColor(stringIndex, stringType.text);
            }
        }
    }
}
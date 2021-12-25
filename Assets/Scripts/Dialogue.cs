using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue.asset", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    [Serializable]
    public class TranslatedString
    {
        public string stringID;
        public string characterName;
        public Sprite characterSprite;
        public Color textColor;
        public float textSpeed;
        public string text;
    }

    public List<TranslatedString> strings = new List<TranslatedString>();

    public string GetString(string stringID, TranslatedMesh.stringType stringType)
    {
        foreach (TranslatedString tString in strings)
        {
            if (tString.stringID == stringID)
            {
                if (stringType == TranslatedMesh.stringType.characterName) return tString.characterName;
                else if (stringType == TranslatedMesh.stringType.text) return tString.text;
            }
        }
        return $"{stringID} ??";
    }

    public Color GetColor(string stringIndex, TranslatedMesh.stringType stringType)
    {
        foreach (TranslatedString tString in strings)
        {
            if (tString.stringID == stringIndex)
            {
                if (stringType == TranslatedMesh.stringType.text) return tString.textColor;
            }
        }
        return Color.clear;
    }

    public Sprite GetSprite(string indiceStringa, TranslatedMesh.stringType tipoStringa)
    {
        foreach (TranslatedString tString in strings)
        {
            if (tString.stringID == indiceStringa)
            {
                if (tipoStringa == TranslatedMesh.stringType.characterSprite) return tString.characterSprite;
            }
        }
        return null;
    }
}
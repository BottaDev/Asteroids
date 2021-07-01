using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    public string textKey;
    private TextMeshProUGUI _textObject;

    private void OnEnable()
    {
        print(EventManager.Instance);
        EventManager.Instance.Subscribe("OnLanguageChange", GetText);
        GetText();
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe("OnLanguageChange", GetText);
    }

    public void GetText(params object[] emptyParams)
    {
        _textObject = GetComponent<TextMeshProUGUI>();

        string txt = LocalizationManager.Instance.GetText(textKey);
        if (txt != textKey) _textObject.text = txt;
        else _textObject.text = "Error: There is no value asociated to the key " + txt;
    }
}

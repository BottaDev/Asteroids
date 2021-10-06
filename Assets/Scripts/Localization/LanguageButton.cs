using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
    public Sprite spanish;
    public Sprite english;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        UpdateLanguageImage();   
    }

    public void UpdateLanguageImage()
    {
        if (LocalizationManager.language == SystemLanguage.Spanish) image.sprite = spanish;
        else image.sprite = english;
    }
}

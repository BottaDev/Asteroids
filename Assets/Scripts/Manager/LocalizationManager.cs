using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using MiniJSON;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public string rootDirectory = "/Resources/Localization";

    public Dictionary<SystemLanguage, Dictionary<string, string>> texts = new Dictionary<SystemLanguage, Dictionary<string, string>>();

    public static LocalizationManager ins { get; private set; }

    public static SystemLanguage language = SystemLanguage.English;

    public List<TextMeshProUGUI> textsInScene = new List<TextMeshProUGUI>();

    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
        else Destroy(this);
    }

    private void Start()
    {
        language = Application.systemLanguage;
        LoadTexts();
        SetTexts();
    }

    public void SwitchLanguage()
    {
        language = language == SystemLanguage.Spanish ? SystemLanguage.English : SystemLanguage.Spanish;
        SetTexts();
    }

    public void LoadTexts()
    {
        texts = new Dictionary<SystemLanguage, Dictionary<string, string>>();

        var allFiles = new List<string>();
        foreach (var file in Directory.GetFiles(Application.dataPath + rootDirectory, "*.json", SearchOption.AllDirectories))
        {
            allFiles.Add(file);
            //print(file + " Added");
        }


        var parsedData = new Dictionary<string,string>();
        foreach (var filePath in allFiles)
        {

            var fileName = Path.GetFileName(filePath);

            string newPath = Path.GetDirectoryName(filePath);

            var asset = Resources.Load<TextAsset>("Localization/" + new DirectoryInfo(newPath).Name + "/" + fileName.Replace(".json", ""));

            /*
            print("FilePath " + filePath);
            print("Name " + fileName);
            print("newPath " + newPath);
            print("asset " + asset);
            print("Localization/" + new DirectoryInfo(newPath).Name + "/" + fileName.Replace(".json", ""));
            */

            string data = asset.text;
            foreach (var item in (Dictionary<string, object>)Json.Deserialize(data))
            {
                parsedData.Add(item.Key, (string)item.Value);
            }     
        }

        texts.Add(language, parsedData);
    }


    public string GetText(string key)
    {
        if (!texts[language].ContainsKey(key))
        {
            Debug.LogError("Key " + key + " for language " + language + "not found");
            return key;
        }

        return texts[language][key];
    }

    public void SetTexts()
    {
        foreach (var item in textsInScene)
        {
            if (texts[language].ContainsKey(item.name))
            {
                item.text = texts[language][item.name];
            }
        }
    }
}


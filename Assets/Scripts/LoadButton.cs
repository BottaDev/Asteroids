using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
    private void Awake()
    {
        if (File.Exists(Application.dataPath + "/Resources/SaveGame/SaveState.dat"))
        {
            GetComponent<Button>().interactable = true;
        }
        else GetComponent<Button>().interactable = false;
    }
}

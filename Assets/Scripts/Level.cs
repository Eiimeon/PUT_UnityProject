using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level
{
    public int levelIndex = 0;
    public int choiceCounter = 0;
    public string[,] keys;
    public Emissary emissary;
    public List<string> built = new List<string>();
    public TextMeshProUGUI displayText = UI_Manager.Instance.emissaryText;

    public Level(string[,] _placeKeys, Emissary _emissaire)
    {
        keys = _placeKeys;
        emissary = _emissaire;
    }

    public void BeginEmissarySection(int emissaryIndex)
    {
        UI_Manager.Instance.SwitchMode(true);
        if (emissaryIndex < GM.Instance.emissaries.Count)
        {
            emissary = GM.Instance.emissaries[emissaryIndex];
            if (emissary.firstAppearance)
            {
                displayText.text = emissary.introTexts[0];
            }
            else
            {
                GM.Instance.SetSuccessState();
                switch (GM.Instance.successState)
                {
                    case "success":
                        emissary.endTexts = emissary.successTexts;
                        break;
                    case "specialSuccess":
                        emissary.endTexts = emissary.specialSuccessTexts;
                        break;
                    case "failure":
                        emissary.endTexts = emissary.failureTexts;
                        break;
                }
                displayText.text = emissary.endTexts[0];
            }
        }
        else
        {
            displayText.text = "c'est fini";
        }
    }

    public void EndEmissarySection()
    {
        emissary.speechCounter = 0;
        if (emissary.firstAppearance)
        {
            emissary.firstAppearance = false;
            UI_Manager.Instance.SwitchMode(false);
        }
        else
        {
            if (GM.Instance.successState == "failure")
            {
                emissary.firstAppearance = true;
            }
            else
            {
                GM.Instance.emissaryIndex++;
            }
            UI_Manager.Instance.SwitchMode(false);
            BeginEmissarySection(GM.Instance.emissaryIndex);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class Level
{
    public int levelIndex = 0;
    public int choiceCounter = -1;
    public string[,] keys;
    public Emissary emissary;
    public List<string> built = new List<string>();
    public TextMeshProUGUI displayText = UI_Manager.Instance.emissaryText;
    [SerializeField] public Place giftedPlace = null;
    public Transform startFocus;

    public Level(string[,] _placeKeys, Emissary _emissaire)
    {
        keys = _placeKeys;
        emissary = _emissaire;
    }

    public Level(int _index, string[,] _placeKeys, Emissary _emissaire)
    {
        levelIndex = _index;
        keys = _placeKeys;
        emissary = _emissaire;
    }

    public Level(int _index, string[,] _placeKeys, Emissary _emissaire, Transform _startFocus)
    {
        levelIndex = _index;
        keys = _placeKeys;
        emissary = _emissaire;
        startFocus = _startFocus;
    }

    public void SetPlace(Place _place)
    {
        giftedPlace = _place;
    }

    public void BeginEmissarySection(int emissaryIndex)
    {
        // Passe en mode UI �missaire
        UI_Manager.Instance.SwitchMode(true);
        // Remplace l'image de l'ancien �missaire par le nouveau
        UI_Manager.Instance.SetEmissary(emissary);
        MusicAndData_Manager.Instance.SetEmissary(emissary);

        // Changement de jauge
        for (int i = 0; i < GM.Instance.levels.Count; i++)
        {
            if (i == levelIndex)
            {
                UI_Manager.Instance.imperialGauges[i].gameObject.SetActive(true);
            }
            else
            {
                UI_Manager.Instance.imperialGauges[i].gameObject.SetActive(false);
            }
        }
        // Fade in 
        UI_Manager.Instance.UI_Emissary.GetComponent<CanvasGroup>().alpha = 0;
        //UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.FadeUI(UI_Manager.Instance.UI_Emissary.GetComponent<CanvasGroup>(), 1));


        if (emissaryIndex < GM.Instance.emissaries.Count)
        {
            emissary = GM.Instance.emissaries[emissaryIndex];
            // Si c'est la premi�re recontre, l'�missaire fait son discours d'introduction
            if (emissary.firstAppearance)
            {
                
                if (giftedPlace != null)
                {
                    //giftedPlace.building3D.gameObject.SetActive(true);
                    Debug.Log("should build");
                    GM.Instance.StartCoroutine(giftedPlace.BuildBuildingEmissary());
                }
                else
                {
                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.FadeUI(UI_Manager.Instance.UI_Emissary.GetComponent<CanvasGroup>(), 1));
                    MusicAndData_Manager.Instance.emissary.GetComponent<AudioSource>().Play();
                }
                displayText.text = emissary.introTexts[0];
            }
            // Sinon son texte d�pend du succes du joueur
            else
            {
                UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.FadeUI(UI_Manager.Instance.UI_Emissary.GetComponent<CanvasGroup>(), 1));
                MusicAndData_Manager.Instance.emissary.GetComponent<AudioSource>().Play();
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
            UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.FadeUI(UI_Manager.Instance.UI_Emissary.GetComponent<CanvasGroup>(), 0));
            Camera_Manager.Instance.GoToGlobalView();
            Place temp;
            foreach (string key in GM.Instance.deadKeys)
            {
                temp = GM.Instance.places[key];
                Buildings_Manager.Instance.StartCoroutine(Buildings_Manager.Instance.Build(temp.building3D));
                Buildings_Manager.Instance.StartCoroutine(Buildings_Manager.Instance.Build(temp.district));
            }
            UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.SpawnEndingButtonWithDelay(2));
        }
    }

    public void EndEmissarySection()
    {
        emissary.speechCounter = 0;
        if (emissary.firstAppearance)
        {
            emissary.firstAppearance = false;
            UI_Manager.Instance.SwitchMode(false);
            GM.Instance.MoveToNextChoices();
            GM.Instance.StartCoroutine(GM.Instance.CantActForSeconds(0.5f));
        }
        else
        {
            if (GM.Instance.successState == "failure") 
            {
                Debug.Log("should quit");
                SceneManager.LoadScene("S_Menu");
            }
            else
            {
                GM.Instance.emissaryIndex++;
            }
            if (levelIndex + 1 < GM.Instance.levels.Count)
            {
                GM.Instance.currLevel = GM.Instance.levels[levelIndex + 1];
                GM.Instance.StartCoroutine(FadeTransition(1));
                GM.Instance.StartCoroutine(GM.Instance.CantActForSeconds(0.5f));
            }
            else
            {
                UI_Manager.Instance.emissaryText.text = "c'est fini";
                GM.Instance.canAct = false;
                UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.FadeUI(UI_Manager.Instance.UI_Emissary.GetComponent<CanvasGroup>(), 0));
                Camera_Manager.Instance.GoToGlobalView();
                Place temp;
                for (int i = 0; i < GM.Instance.deadKeys.Count; i++)
                {
                    temp = GM.Instance.places[GM.Instance.deadKeys[i]];
                    if (i == 0)
                    {
                        temp.building3D.GetComponent<AudioSource>().Play();
                    }
                    Buildings_Manager.Instance.StartCoroutine(Buildings_Manager.Instance.Build(temp.building3D));
                    if (temp.district != null)
                    {
                        Buildings_Manager.Instance.StartCoroutine(Buildings_Manager.Instance.Build(temp.district));
                    }
                }
                /*foreach (string key in GM.Instance.deadKeys)
                {
                    Debug.Log(key);
                    temp = GM.Instance.places[key];
                    Buildings_Manager.Instance.StartCoroutine(Buildings_Manager.Instance.Build(temp.building3D));
                    Buildings_Manager.Instance.StartCoroutine(Buildings_Manager.Instance.Build(temp.district));
                }*/
                UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.SpawnEndingButtonWithDelay(2));
            }
        }
        
    }


    public IEnumerator FadeTransition(float duration)
    {
        GM.Instance.canAct = false;
        CanvasGroup blackPanel = UI_Manager.Instance.blackPanel.GetComponent<CanvasGroup>();
        float timer = 0f;
        //Color initialColor = blackPanel.GetComponent<Image>().color;
        while (timer < duration)
        {
            //Debug.Log(timer);
            timer += Time.deltaTime;
            blackPanel.alpha = Mathf.Lerp(0, 1, timer / duration);
            //blackPanel.GetComponent<Image>().color = Color.Lerp(blackPanel.GetComponent<Image>().color, Color.black, timer / duration);
            yield return new WaitForEndOfFrame();
        }
        UI_Manager.Instance.SwitchMode(false);
        GM.Instance.currLevel.BeginEmissarySection(GM.Instance.emissaryIndex);
        yield return new WaitForSeconds(1f);
        timer = 0;
        while (timer < duration)
        {
            //Debug.Log(timer);
            timer += Time.deltaTime;
            blackPanel.alpha = Mathf.Lerp(1, 0, timer / duration);
            //blackPanel.GetComponent<Image>().color = Color.Lerp(blackPanel.GetComponent<Image>().color, Color.black, timer / duration);
            yield return new WaitForEndOfFrame();
        }
        //yield return new WaitForSeconds(1);
        GM.Instance.canAct = true;
    }
}

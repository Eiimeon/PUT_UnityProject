using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class UI_Manager : MonoBehaviour
{


    // ----------------------------------------------------------
    //                         SINGLETON
    // ----------------------------------------------------------

    private static UI_Manager _instance;

    public static UI_Manager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    // ----------------------------------------------------------
    // ----------------------------------------------------------


    /***
     *                                              
     *    __ __ __ __ __ __ __ __ __ __ __ __ __ __ 
     *               __          __        ___  __  
     *    \  /  /\  |__) |  /\  |__) |    |__  /__` 
     *     \/  /~~\ |  \ | /~~\ |__) |___ |___ .__/ 
     *    __ __ __ __ __ __ __ __ __ __ __ __ __ __ 
     *                                              
     *                                              
     */

    #region VARIABLES
    public GameObject UI_Choice;
    public GameObject UI_Emissary;
    public GameObject UI_City;

    public GameObject[] allBuildings3D;

    public List<Emissary> emissaryList = new List<Emissary>(); // TODO check if useless

    public Sprite[] emissaries;
    public Sprite[] advisors;
    
    public Image blackPanel;
    public Image leftAdvisor;
    public Image rightAdvisor;
    public Image emissary;

    public TextMeshProUGUI displayedText;
    public TextMeshProUGUI emissaryText;
    
    

    public bool choiceMode = true;
    public bool cityMode = false;
    public bool emissaryMode = false;
    #endregion


    /***
     *                                              
     *    __ __ __ __ __ __ __ __ __ __ __ __ __ __ 
     *           ___ ___       __   __   ___  __    
     *     |\/| |__   |  |__| /  \ |  \ |__  /__`   
     *     |  | |___  |  |  | \__/ |__/ |___ .__/   
     *    __ __ __ __ __ __ __ __ __ __ __ __ __ __ 
     *                                              
     *                                              
     */

    #region METHODES
    public void Shadow(Image advisor, bool isShadowed)
    {
        if (isShadowed)
        {
            advisor.GetComponent<Image>().color = Color.grey;
            advisor.GetComponent<Image>().rectTransform.localScale = 1f * Vector3.one;
        }
        else
        {
            advisor.GetComponent<Image>().color = Color.white;
            advisor.GetComponent<Image>().rectTransform.localScale = 1.5f * Vector3.one;
        }
    }

    

    public void SwitchMode(bool emissary = false)
    {
        if (!emissaryMode && !emissary)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (choiceMode)
                {
                    choiceMode = false;
                    cityMode = true;
                }
                else if (cityMode)
                {
                    cityMode = false;
                    choiceMode = true;
                }
            }
        }
        else if (!emissaryMode && emissary)
        {
            cityMode = false;
            choiceMode = false;
            emissaryMode = true;
        }
        else if (emissaryMode && !emissary)
        {
            emissaryMode = false;
            cityMode = false;
            choiceMode = true;
        }
        else if ( emissaryMode && emissary ){
            Debug.Log("Normalement, ça n'arrive pas");
        }
        /*StartCoroutine(FadeUI(UI_Choice.GetComponent<CanvasGroup>(), BoolToInt(choiceMode)));
        StartCoroutine(FadeUI(UI_Emissary.GetComponent<CanvasGroup>(), BoolToInt(emissaryMode)));
        StartCoroutine(FadeUI(UI_City.GetComponent<CanvasGroup>(), BoolToInt(cityMode)));*/
        UI_Choice.SetActive(choiceMode);
        UI_Emissary.SetActive(emissaryMode);
        UI_City.SetActive(cityMode);

    }

    public void SetEmissary(Emissary _emissary)
    {
        //emissary.sprite = _emissary;
    }
    #endregion




    /***
     *                                                  
     *    __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __    
     *     __   __   __   __       ___         ___  __  
     *    /  ` /  \ |__) /  \ |  |  |  | |\ | |__  /__` 
     *    \__, \__/ |  \ \__/ \__/  |  | | \| |___ .__/ 
     *    __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __    
     *                                                  
     *                                                  
     */

    #region Coroutines

    public IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
    }
    public IEnumerator FadeTo(float targetAlpha, float duration, Action after = null, bool unfade = false)
    {
        float timer = 0f;
        Color initialColor = blackPanel.GetComponent<Image>().color;
        while (timer<duration) 
        {
            Debug.Log(timer);
            timer += Time.deltaTime;
            //blackPanel.GetComponent<Image>().color = Color.Lerp(initialColor, new Color(0, 0, 0, targetAlpha), timer / duration);
            blackPanel.GetComponent<Image>().color = Color.Lerp(blackPanel.GetComponent<Image>().color, new Color(0, 0, 0, targetAlpha), timer / duration);
            yield return null;
        }
        /*while (Mathf.Abs(blackPanel.GetComponent<Image>().color.a - targetAlpha)>0.005)
        {
            Debug.Log(timer);
            timer += Time.deltaTime;
            blackPanel.GetComponent<Image>().color = Color.Lerp(blackPanel.GetComponent<Image>().color, new Color(0, 0, 0, targetAlpha), 10 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }*/
        after?.Invoke();

        if (unfade)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(FadeTo(1 - targetAlpha, duration));
        }
    }
    public IEnumerator FadeUI(CanvasGroup canvasGroup, float targetAlpha, float fadeSpeed = 10)
    {
        while (Mathf.Abs(canvasGroup.alpha - targetAlpha)>0.05)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        canvasGroup.alpha = targetAlpha;
    }

    public IEnumerator IntroFade()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeUI(blackPanel.GetComponent<CanvasGroup>(), 0, 0.5f));
    }

    #endregion


    /***
     *                                                                            
     *    __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __                                
     *     __  ___       __  ___          __   __       ___  ___     ___ ___  __  
     *    /__`  |   /\  |__)  |     |  | |__) |  \  /\   |  |__     |__   |  /  ` 
     *    .__/  |  /~~\ |  \  |     \__/ |    |__/ /~~\  |  |___    |___  |  \__, 
     *    __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __                               
     *                                                                            
     *                                                                            
     */

    private void Start()
    {
        blackPanel.GetComponent<CanvasGroup>().alpha = 1;
        StartCoroutine(IntroFade());
    }
}

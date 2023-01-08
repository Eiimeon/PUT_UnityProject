using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Image[] imperialGauges;
    public Image peopleGauge;

    public TextMeshProUGUI displayedText;
    public TextMeshProUGUI emissaryText;

    public Image EndButton;
    
    // TODO Faire une vraie state machine
    public bool choiceMode = true;
    public bool cityMode = false;
    public bool emissaryMode = false;

    public bool isShadowing = false; // Conditionne ShadowCoroutine, empêche de lancer deux fois la coroutine simultanément ce qui bloque l'animation
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

    private void InitializeUI()
    {
        Debug.Log("Initialize UI");
        //Image panel = transform.Find("Fond Texte").GetComponent<Image>();
        //panel.GetComponent<Transform>

        //RectTransform choicePanel = UI_Choice.transform.Find("Fond Texte").GetComponent<RectTransform>();
        RectTransform emissaryPanel = UI_Emissary.transform.Find("Fond Texte").GetComponent<RectTransform>();

        Rect rect = new Rect(-Screen.width / 2, -Screen.height / 2, Screen.width, Screen.height);
        /*choicePanel.sizeDelta = new Vector2(rect.width, rect.height);
        choicePanel.position = new Vector2(rect.x, rect.y);
        emissaryPanel.sizeDelta = new Vector2(rect.width, rect.height);
        emissaryPanel.position = new Vector2(rect.x, rect.y);*/
        //emissaryPanel.rect = rect;

        UI_Emissary.transform.Find("Fond Texte").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,Screen.width);
        UI_Emissary.transform.Find("Fond Texte").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
        UI_Choice.transform.Find("Fond Texte").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        UI_Choice.transform.Find("Fond Texte").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);

        UI_Emissary.transform.Find("Texte").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        UI_Emissary.transform.Find("Texte").GetComponent<RectTransform>().anchoredPosition = new Vector2(0,Screen.height * -7 / 48);

        UI_Choice.transform.Find("Texte").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        UI_Choice.transform.Find("Texte").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Screen.height * -7 / 48);

        emissary.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Screen.height * -1 / 6);
        emissary.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        emissary.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.width);

        leftAdvisor.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width * -11 / 16, Screen.height * -1 / 6);
        leftAdvisor.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width / 4); // On divise par 4 pour compenser le scaling de Shadow
        leftAdvisor.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.width / 4);

        rightAdvisor.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width * 11 / 16, Screen.height * -1 / 6);
        rightAdvisor.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width / 4);
        rightAdvisor.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.width / 4);

        for (int i = 0; i < 4; i++)
        {
            imperialGauges[i].gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * 3 / 4);
            imperialGauges[i].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.width * 3 / 100);
        }
        peopleGauge.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * 3 / 4);
        peopleGauge.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.width * 3 / 100);

    }
    public void Shadow(Image advisor, bool isShadowed)
    {
        if (isShadowed)
        {
            advisor.GetComponent<Image>().color = Color.grey;
            advisor.GetComponent<Image>().rectTransform.localScale = 3f * Vector3.one;
        }
        else
        {
            advisor.GetComponent<Image>().color = Color.white;
            advisor.GetComponent<Image>().rectTransform.localScale = 4f * Vector3.one;
        }
    }
    // Indique si le conseiller est dans un état stable ou pas, permet d'éviter les problèmes quand on spamme le changement de choix
    public bool IsShadowing(Image _advisor)
    {
        return (!(_advisor.color == Color.gray && _advisor.GetComponent<Image>().rectTransform.localScale == 3 * Vector3.one)||
                !(_advisor.color == Color.white && _advisor.GetComponent<Image>().rectTransform.localScale == 4 * Vector3.one));
    }

    public IEnumerator ShadowCoroutine (Image advisor, bool isShadowed)
    {
        // Définition des propriétés ciblées selon si le conseiller doit être en retrait ou pas
        Color targetColor;
        Vector3 targetScale;
        if (isShadowed)
        {
            targetColor = Color.gray;
            targetScale = 3 * Vector3.one;
        }
        else
        {
            targetColor = Color.white;
            targetScale = 4 * Vector3.one;
        }
        // On autorise le shadowing uniquement si le conseiller n'est pas dans un état intermédiaire pour empêcher de softlock le conseiller dans deux lerp contradictoires en spammant les touches
        if (isShadowed && advisor.color == Color.white)
        {
            while ((advisor.color - targetColor).maxColorComponent > 5 || (advisor.GetComponent<Image>().rectTransform.localScale - targetScale).magnitude > 0.1)
            {
                isShadowing = true;
                advisor.color = Color.Lerp(advisor.color, targetColor, 10f * Time.deltaTime);
                advisor.GetComponent<Image>().rectTransform.localScale = Vector3.Lerp(advisor.GetComponent<Image>().rectTransform.localScale, targetScale, 10f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            advisor.color = targetColor;
            advisor.GetComponent<Image>().rectTransform.localScale = targetScale;
        }
        else if (!isShadowed && advisor.color == Color.gray)
        {
            while ((advisor.color - targetColor).maxColorComponent > 5 || (advisor.GetComponent<Image>().rectTransform.localScale - targetScale).magnitude > 0.1)
            {
                isShadowing = true;
                advisor.color = Color.Lerp(advisor.color, targetColor, 10f * Time.deltaTime);
                advisor.GetComponent<Image>().rectTransform.localScale = Vector3.Lerp(advisor.GetComponent<Image>().rectTransform.localScale, targetScale, 10f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            advisor.color = targetColor;
            advisor.GetComponent<Image>().rectTransform.localScale = targetScale;
        }
        isShadowing = false;
    }
    

    public float GetFillRatio(Image _gauge)
    {
        float fillRatio = _gauge.GetComponent<Scrollbar>().size;
        return fillRatio;
    }

    public float GetNewFillRatio(Image _gauge, float _additionalFillRatio)
    {
        return  Mathf.Clamp(_gauge.GetComponent<Scrollbar>().size + _additionalFillRatio, 0.0f, 1.0f);
    }
    public void SetFillRatio(Image _gauge, float _fillRatio)
    {
        _fillRatio = Mathf.Clamp(_fillRatio, 0.0f, 1.0f);
        _gauge.GetComponent<Scrollbar>().size = _fillRatio;
    }

    public void FillGauge(Image _gauge, float _additionalFillRatio)
    {
        SetFillRatio(_gauge, _gauge.GetComponent<Scrollbar>().size+_additionalFillRatio);
    }

    public IEnumerator FillGaugeCoroutine(Image _gauge, float _targetRatio)
    {
        //float targetRatio = Mathf.Clamp(_gauge.GetComponent<Scrollbar>().size + _additionalFillRatio, 0.0f, 1.0f);
        Debug.Log("gaugecoroutine");
        while (Mathf.Abs(_gauge.GetComponent<Scrollbar>().size - _targetRatio) > 0.01 )
        {
            
            _gauge.GetComponent<Scrollbar>().size = Mathf.Lerp(_gauge.GetComponent<Scrollbar>().size, _targetRatio, 5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        _gauge.GetComponent<Scrollbar>().size = _targetRatio;
    }

    // La state machine du bled
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
        UI_Choice.SetActive(choiceMode);
        UI_Emissary.SetActive(emissaryMode);
        UI_City.SetActive(cityMode);
    }

    public void SetEmissary(Emissary _emissary)
    {
        emissary.sprite = _emissary.sprite;
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

    public IEnumerator FinalFade()
    {
        blackPanel.gameObject.SetActive(true);
        StartCoroutine(FadeUI(blackPanel.GetComponent<CanvasGroup>(), 1, 0.5f));
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("S_Menu");
    }

    public void DoFinalFade()
    {
        Debug.Log("Clic sur le bouton de fin");
        StartCoroutine(FinalFade());
    }

    public IEnumerator SpawnEndingButtonWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EndButton.gameObject.SetActive(true);
        FadeUI(EndButton.GetComponent<CanvasGroup>(), 1, 3);
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
        InitializeUI();

        // Initialisation de la taille des jauges
        imperialGauges[0].GetComponent<Gauge>().SetLength(5);
        imperialGauges[1].GetComponent<Gauge>().SetLength(12);
        imperialGauges[2].GetComponent<Gauge>().SetLength(12);
        imperialGauges[3].GetComponent<Gauge>().SetLength(12);
        peopleGauge.GetComponent<Gauge>().SetLength(5);

        // Fade d'intro pour cacher que ça rame au lancement
        blackPanel.GetComponent<CanvasGroup>().alpha = 1;
        StartCoroutine(IntroFade());
    }
}

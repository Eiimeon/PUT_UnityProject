using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Menu : MonoBehaviour
{

    [SerializeField] private Image panel;
    private bool canLoadGame = true;

    public IEnumerator FadeUI(CanvasGroup canvasGroup, float targetAlpha, float fadeSpeed = 10) // Copie de la méthode de l'UI manager, qui n'existe pas au moment du menu parce que le projet il tient avec des bouts de scotch on va pas se mentir
    {
        while (Mathf.Abs(canvasGroup.alpha - targetAlpha) > 0.05)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        canvasGroup.alpha = targetAlpha;
    }

    // Les onclick des deux boutons 
    public void ButtonPlay_FR()
    {
        MusicAndData_Manager.Instance.isFrench = true;
        panel.gameObject.SetActive(true);
        StartCoroutine(FadeUI(panel.GetComponent<CanvasGroup>(),1,3)); // On black out l'écran pour faire passer le gros freeze pour un temps de chargement
        //SceneManager.LoadScene("S_City_New", LoadSceneMode.Additive);
    }
    public void ButtonPlay_EN()
    {
        MusicAndData_Manager.Instance.isFrench = false;
        panel.gameObject.SetActive(true);
        StartCoroutine(FadeUI(panel.GetComponent<CanvasGroup>(), 1,3)); // On black out l'écran pour faire passer le gros freeze pour un temps de chargement
        //SceneManager.LoadScene("S_City_New", LoadSceneMode.Additive);
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadSceneAsync("S_MusicAndData", LoadSceneMode.Additive);
        //SceneManager.SetActiveScene(SceneManager.GetActiveScene());
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("S_Menu"));
    }

    // Update is called once per frame
    void Update()
    {
        if (panel.GetComponent<CanvasGroup>().alpha > 0.999f && canLoadGame)
        {
            canLoadGame = false;
            SceneManager.LoadScene("S_City_New", LoadSceneMode.Additive);
        }
    }
}

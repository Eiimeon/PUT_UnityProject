using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public void ButtonPlay_FR()
    {
        MusicAndData_Manager.Instance.isFrench = true;
        SceneManager.LoadScene("S_City_New", LoadSceneMode.Additive);
    }
    public void ButtonPlay_EN()
    {
        MusicAndData_Manager.Instance.isFrench = false;

        SceneManager.LoadScene("S_City_New", LoadSceneMode.Additive);
    }

    public void BackToMainMenu()
    {
        Debug.Log("Back to main menu");
        SceneManager.LoadScene("S_Menu");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public void ButtonPlay()
    {
        SceneManager.LoadScene("S_City_New");
    }
}

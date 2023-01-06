using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LostPeople : MonoBehaviour
{
    private bool canAct = false;

    private IEnumerator WaitBeforeCanActForSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        canAct = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitBeforeCanActForSeconds(1));   
    }

    // Update is called once per frame
    void Update()
    {
        if (canAct && Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("S_Menu");
        }
    }
}

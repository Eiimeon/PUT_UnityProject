using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings_Manager : MonoBehaviour
{

    // ----------------------------------------------------------
    //                         SINGLETON
    // ----------------------------------------------------------

    private static Buildings_Manager _instance;

    public static Buildings_Manager Instance { get { return _instance; } }


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

    public Transform[] buildingsTransform;

    // ----------------------------------------------------------
    // ----------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        //buildingsTransform = GetComponentsInChildren<Transform>();
        GM.Instance.buildingsTransforms = buildingsTransform;
        foreach (Transform t in buildingsTransform)
        {
            if (t.name != "Portes") //Les portes refusent de réaparaître au début du jeu, alors je les fais juste pas disparaitre (?°?°??? ???
            {
                t.gameObject.SetActive(false);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

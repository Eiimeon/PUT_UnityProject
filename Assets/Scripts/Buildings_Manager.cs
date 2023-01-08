using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Transform[] districtTransforms;

    [SerializeField] AudioClip buildingSound;

    // ----------------------------------------------------------
    // ----------------------------------------------------------

    public IEnumerator Build(Transform building) // Fait sortir un building de terre, étonnament, utile uniquement pour la final
    {
        building.gameObject.SetActive(true);
        Vector3 initialBuildingPos = building.transform.position;
        building.position -= 5 * Vector3.up;
        while ((building.transform.position - initialBuildingPos).magnitude > 0.1)
        {
            building.position = Vector3.Lerp(building.position, initialBuildingPos, 0.7f * Time.deltaTime);
            //Camera_Manager.Instance.cam.transform.position = targetPos + 0.5f * UnityEngine.Random.insideUnitSphere;
            Handheld.Vibrate();
            yield return new WaitForEndOfFrame();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //buildingsTransform = GetComponentsInChildren<Transform>();
        GM.Instance.buildingsTransforms = buildingsTransform;
        GM.Instance.districtTransforms = districtTransforms;
        foreach (Transform t in buildingsTransform)
        {
            if ( true /*t.name != "Portes"*/) //Les portes refusent de réaparaître au début du jeu, alors je les fais juste pas disparaitre (?°?°??? ???
            {
                t.gameObject.SetActive(false);
                AudioSource temp = t.AddComponent<AudioSource>();
                temp.clip = buildingSound;
                temp.loop = true;
                temp.playOnAwake = false;
                temp.volume = 0.75f;
                //temp.spatialBlend = 1; //tentative de spatialisation
                
            }
        }
        foreach (Transform t in districtTransforms)
        {
            t.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

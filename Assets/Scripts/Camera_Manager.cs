using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    // ----------------------------------------------------------
    //                         SINGLETON
    // ----------------------------------------------------------

    private static Camera_Manager _instance;

    public static Camera_Manager Instance { get { return _instance; } }


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

    public float speed = 1;
    public float zoomSpeed = 1;

    public Camera cam;

    private float baseFOV;

    private Vector3 standardOffSet;
    private Vector3 targetPos;

    [SerializeField] private float altitude;

    public float GetAltitude() // Pour les portes, où GetTargetPos ne fonctionne pas 
    {
        return altitude;
    }


    public Vector3 GetTargetPosition(Transform building) // TODO delete, probablement obsolette
    {
        Vector3 targetPos = Vector3.zero;
        targetPos.x = building.position.x + standardOffSet.x;
        targetPos.y = altitude;
        targetPos.z = building.position.z + standardOffSet.z;
        return targetPos;
    }

    public void SetTargetPosition(Transform building)
    {
        targetPos = Vector3.zero;
        targetPos.x = building.position.x + standardOffSet.x;
        targetPos.y = altitude;
        targetPos.z = building.position.z + standardOffSet.z;
    }

    public bool IsOnTarget()
    {
        return ((cam.transform.position - targetPos).magnitude > 0.1);
    }

    public IEnumerator PosAndFOVLerp(Vector3 targetPos, float targetFOV)
    {
        while ((cam.transform.position - targetPos).magnitude > 0.1 || Mathf.Abs(cam.fieldOfView - targetFOV) > 0.1f)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, 1 * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, 1 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        baseFOV = cam.fieldOfView;
        altitude = cam.transform.position.y;
        standardOffSet.y = cam.transform.position.y;
        standardOffSet.x = cam.transform.position.x - Buildings_Manager.Instance.buildingsTransform[0].position.x;
        standardOffSet.z = cam.transform.position.z - Buildings_Manager.Instance.buildingsTransform[0].position.z;

        cam.transform.position = GetTargetPosition(Buildings_Manager.Instance.buildingsTransform[0]);
    }

    

    // Update is called once per frame
    void Update()
    {
        // Contrôles du city mode (non implémenté ?) TODO
        if (Input.GetKey(KeyCode.D))
        {
            transform.position -= speed * Vector3.right * Time.deltaTime * cam.fieldOfView /60;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += speed * Vector3.right * Time.deltaTime * cam.fieldOfView / 60;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            transform.position -= speed * Vector3.forward * Time.deltaTime * cam.fieldOfView / 60;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += speed * Vector3.forward * Time.deltaTime * cam.fieldOfView / 60;
        }
        if (Input.GetKey(KeyCode.I))
        {
            cam.fieldOfView -= zoomSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.O))
        {
            cam.fieldOfView += zoomSpeed * Time.deltaTime;
        }
        // A toutes les frames on lerp vers la target pos qui pointe sur un batiment en construction
        //cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, 1 * Time.deltaTime);
    }
}

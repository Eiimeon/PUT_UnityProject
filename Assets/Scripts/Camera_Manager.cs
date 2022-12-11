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

    Vector3 standardOffSet;
    float altitude;

    // Start is called before the first frame update
    void Start()
    {
        altitude = cam.transform.position.y;
        standardOffSet.y = cam.transform.position.y;
        standardOffSet.x = cam.transform.position.x - Buildings_Manager.Instance.buildingsTransform[0].position.x;
        standardOffSet.z = cam.transform.position.z - Buildings_Manager.Instance.buildingsTransform[0].position.z;

        cam.transform.position = GetTargetPosition(Buildings_Manager.Instance.buildingsTransform[0]);
    }

    public Vector3 GetTargetPosition(Transform building)
    {
        Vector3 targetPos = Vector3.zero;
        targetPos.x = building.position.x + standardOffSet.x;
        targetPos.y = altitude;
        targetPos.z = building.position.z + standardOffSet.z;
        return targetPos;
    }

    // Update is called once per frame
    void Update()
    {
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
    }
}

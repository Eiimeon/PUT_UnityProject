using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovePC : MonoBehaviour
{

    public float speed = 1;
    public float zoomSpeed = 1;

    public Camera Camera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position -= speed * Vector3.right * Time.deltaTime * Camera.fieldOfView /60;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += speed * Vector3.right * Time.deltaTime * Camera.fieldOfView / 60;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= speed * Vector3.forward * Time.deltaTime * Camera.fieldOfView / 60;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            transform.position += speed * Vector3.forward * Time.deltaTime * Camera.fieldOfView / 60;
        }
        if (Input.GetKey(KeyCode.I))
        {
            Camera.fieldOfView -= zoomSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.O))
        {
            Camera.fieldOfView += zoomSpeed * Time.deltaTime;
        }
    }
}

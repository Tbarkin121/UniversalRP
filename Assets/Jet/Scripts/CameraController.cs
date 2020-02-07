using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float zoomSpeed = 4f;
    public float minZoom = -15f;
    public float maxZoom = 15f;
    public float yawSpeed = 100f;
    private float currentZoom = 1f;
    private float currentYaw = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        if(Input.GetKey(KeyCode.E))
        {
            transform.Rotate(new Vector3(0f, 0f, 1f));
        }
        if(Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0f, 0f, -1f));
        }
        
    }
    void LateUpdate()
    {
        transform.position = target.position - offset - Vector3.forward* currentZoom;
        
        // transform.RotateAround(target.position, Vector3.forward, currentYaw);
    }

}

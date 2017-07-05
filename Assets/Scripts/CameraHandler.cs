using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour {

    public float forwardMin = 0;
    public float forwardMax = 0;

    public float rightMin = 0;
    public float rightMax = 0;

    public float verticalMin = 0;
    public float verticalMax = 0;
    public float verticalForwardDelta = 0;

    public float cameraMinAngle = 10;
    public float cameraMaxAngle = 60;

    public float cameraSpeed = 1;
    public float zoomSpeed = 0.1f;

    Vector3 lastMousePos = Vector3.zero;

    public float zoomExtent = 0;


	// Use this for initialization
	void Start () {
		
	}
	
	void LateUpdate () {
        if (Input.GetMouseButtonDown(0))
            lastMousePos = Input.mousePosition;
        else if (Input.GetMouseButton(0))
        {
            Vector3 mousePosDelta = Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;
            Vector3 dragPosDelta = ((Vector3.ProjectOnPlane(transform.forward, Vector3.up) * mousePosDelta.y) + (Vector3.ProjectOnPlane(transform.right, Vector3.up) * mousePosDelta.x));
            Vector3 newCameraPos = transform.position - (dragPosDelta * cameraSpeed * Time.deltaTime);

            Vector3 newCameraPosClamped = new Vector3(Mathf.Clamp(newCameraPos.x, rightMin, rightMax), newCameraPos.y, Mathf.Clamp(newCameraPos.z, forwardMin, forwardMax));

            transform.position = newCameraPosClamped;
        }

        zoomExtent += Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
        zoomExtent = Mathf.Clamp01(zoomExtent);


        float cameraAngle = ((cameraMaxAngle - cameraMinAngle) * (transform.position.z - forwardMin) / (forwardMax - forwardMin)) + cameraMinAngle;
        float zoomedCameraAngle = Mathf.Lerp(cameraMaxAngle, cameraAngle, zoomExtent);

        transform.eulerAngles = new Vector3(zoomedCameraAngle, transform.eulerAngles.y, transform.eulerAngles.z);
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(verticalMax, verticalMin + (verticalForwardDelta * (transform.position.z - forwardMin) / (forwardMax - forwardMin)), zoomExtent), transform.position.z);

    }
}

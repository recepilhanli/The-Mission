using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoom : MonoBehaviour
{
    private Vector3 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;

    // Update is called once per frame
    [System.NonSerialized]
    public static bool isMoving;
    private Vector3 camPos;
    private float camsize;

    private Camera cam;

    private void Awake()
    {
        cam = gameObject.GetComponent<Camera>();
        camPos = gameObject.transform.position;
        camsize = cam.orthographicSize;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
    #if UNITY_ANDROID || UNITY_IOS
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.touchCount == 2)
        {
            
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;



            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            
            zoom(difference * 0.01f);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += direction;
        }


#endif

        zoom(Input.GetAxis("Mouse ScrollWheel"));

#if UNITY_ANDROID || UNITY_IOS
        if (camPos != gameObject.transform.position || camsize != cam.orthographicSize || Input.touchCount >= 2)
        {
            isMoving = true;
        }
        else isMoving = false;
#endif

        camPos = gameObject.transform.position;
        camsize = cam.orthographicSize;

    }

    void zoom(float increment)
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }
}
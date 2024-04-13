using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraMovement : MonoBehaviour
{
    private Camera cam;

    public float scrollSpeed = 500f;
    public int maxDistance = 200;
    public int minDistance = 50;

    private Vector3 dragOrigin;

    void Start()
    {
        cam = GetComponent<Camera>();
        //Ограничиваю частоту кадров до 30
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        //canTakeCoin = true;
    }

    // Update is called once per frame
    void Update()
    {
        //прокручивание колесика
        if (Input.GetAxis("Mouse ScrollWheel") > 0.1 && transform.position.y > minDistance)
        {
            transform.position += transform.forward * Time.deltaTime * scrollSpeed;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < -0.1 && transform.position.y < maxDistance)
        {
            transform.position -= transform.forward * Time.deltaTime * scrollSpeed;
        }

        //нажатие на мышку
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mp = Input.mousePosition;
            mp.z = transform.position.y;
            dragOrigin = cam.ScreenToWorldPoint(mp);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 mp = Input.mousePosition;
            mp.z = transform.position.y;
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(mp);
            transform.position += difference;
        }
    }
}

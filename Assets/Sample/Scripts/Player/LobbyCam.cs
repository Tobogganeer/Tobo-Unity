using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobo.Util;

public class LobbyCam : MonoBehaviour
{
    public static LobbyCam instance;
    private void Awake()
    {
        instance = this;
        startPos = transform.position;
        startRot = transform.rotation;
    }

    public GameObject cam;

    Vector3 startPos;
    Quaternion startRot;
    Transform mainCam;

    bool to;

    const float TravelSpeed = 3f;
    float time = 0;

    public static void Enable(Transform cam)
    {
        if (instance == null) return;

        instance.cam.SetActive(true);
        instance.to = false;
        instance.time = 0;
        if (cam != null)
        {
            instance.transform.position = cam.position;
            instance.transform.rotation = cam.rotation; // Cam is destroyed after this call
        }
    }

    public static void Disable(Transform newCam)
    {
        //instance.cam.SetActive(false);
        instance.time = 0;
        instance.mainCam = newCam;

        instance.to = true;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > 1f)
            time += Time.deltaTime * 3f; // MORE ACCEL HEHEHEHA

        if (to) // Go to new cam
        {
            if (time > 1f)
                time += Time.deltaTime / transform.position.Dist(mainCam.position);
            transform.position = Vector3.Lerp(transform.position, mainCam.position, Time.deltaTime * TravelSpeed * time);
            transform.rotation = Quaternion.Slerp(transform.rotation, mainCam.rotation, Time.deltaTime * TravelSpeed * time);
            bool close = transform.position.Dist(mainCam.position) < 0.05f;
            bool closeAngle = Quaternion.Angle(transform.rotation, mainCam.rotation) < 3f;
            bool longTime = time > 5f; // Better way to do this with proper lerps but idc for now
            if ((close && closeAngle) || longTime)
            {
                cam.SetActive(false);
            }
        }
        else // Return to main
        {
            transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * TravelSpeed * time);
            transform.rotation = Quaternion.Slerp(transform.rotation, startRot, Time.deltaTime * TravelSpeed * time);
        }
    }
}

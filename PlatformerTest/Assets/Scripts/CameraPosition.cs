using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private Transform mainCamera;
    [SerializeField]
    private Transform cornerB;
    [SerializeField]
    private Transform cornerT;

    private Vector3 cornerBVec;
    private Vector3 cornerTVec;
    private float z;

    [SerializeField]
    private Vector3 offset;

    private Vector3 cameraPosition;

    public static bool cameraTransition;

    private float transitionStart;
    private Vector3 targetPosition;
    private static Vector3 oldPosition;
    private static bool firstCamera = true;
    private static float counterGeneral;
    private float counter;
    public static float transitionLength;

    void Start()
    {
        z = mainCamera.position.z;
        float tmp = Mathf.Min(cornerB.position.x, cornerT.position.x);
        cornerTVec.x = Mathf.Max(cornerB.position.x, cornerT.position.x);
        cornerBVec.x = tmp;
        tmp = Mathf.Min(cornerB.position.y, cornerT.position.y);
        cornerTVec.y = Mathf.Max(cornerB.position.y, cornerT.position.y);
        cornerBVec.y = tmp;

        if (firstCamera)
        {
            cameraPosition = player.transform.position;
            cameraPosition.x = Mathf.Max(Mathf.Min(cameraPosition.x, cornerTVec.x), cornerBVec.x);
            cameraPosition.y = Mathf.Max(Mathf.Min(cameraPosition.y, cornerTVec.y), cornerBVec.y);
            cameraPosition.z = z;
            mainCamera.position = cameraPosition + offset;

            firstCamera = false;
        }
        else
        {
            cameraTransition = true;
            transitionStart = Time.time;

            targetPosition = player.transform.position;
            targetPosition.x = Mathf.Max(Mathf.Min(targetPosition.x, cornerTVec.x), cornerBVec.x);
            targetPosition.y = Mathf.Max(Mathf.Min(targetPosition.y, cornerTVec.y), cornerBVec.y);
            targetPosition.z = z;

            targetPosition += offset;
        }

        counter = counterGeneral;
    }

    void Update()
    {
        if (counter != counterGeneral)
        {
            counter = counterGeneral;
            Start();
            return;
        }
        
        if (Time.time > transitionStart + transitionLength)
        {
            cameraTransition = false;
            mainCamera.position = targetPosition;
        }

        if (cameraTransition)
        {
            mainCamera.position = oldPosition * (transitionStart + transitionLength - Time.time) / transitionLength +
                                  targetPosition * (Time.time - transitionStart) / transitionLength;
            return;
        }

        cameraPosition = player.transform.position;
        cameraPosition.x = Mathf.Max(Mathf.Min(cameraPosition.x, cornerTVec.x), cornerBVec.x);
        cameraPosition.y = Mathf.Max(Mathf.Min(cameraPosition.y, cornerTVec.y), cornerBVec.y);
        cameraPosition.z = z;
        
        mainCamera.position = cameraPosition + offset;
        oldPosition = mainCamera.position;

        counterGeneral++;
        counter = counterGeneral;
    }
}
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

    void Start()
    {
        z = mainCamera.position.z;
        float tmp = Mathf.Min(cornerB.position.x, cornerT.position.x);
        cornerTVec.x = Mathf.Max(cornerB.position.x, cornerT.position.x);
        cornerBVec.x = tmp;
        tmp = Mathf.Min(cornerB.position.y, cornerT.position.y);
        cornerTVec.y = Mathf.Max(cornerB.position.y, cornerT.position.y);
        cornerBVec.y = tmp;
    }

    void Update()
    {
        cameraPosition = player.transform.position;
        cameraPosition.x = Mathf.Max(Mathf.Min(cameraPosition.x, cornerTVec.x), cornerBVec.x);
        cameraPosition.y = Mathf.Max(Mathf.Min(cameraPosition.y, cornerTVec.y), cornerBVec.y);
        cameraPosition.z = z;
        mainCamera.position = cameraPosition + offset;
    }
}
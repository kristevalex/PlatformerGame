using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalConstaints : MonoBehaviour
{
    [SerializeField]
    float sceneTrinsitionsLength;

    void Start()
    {
        CameraPosition.transitionLength = sceneTrinsitionsLength;
    }
}

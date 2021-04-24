using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalConstaints : MonoBehaviour
{
    [SerializeField]
    float sceneTrinsitionsLength;
    [SerializeField]
    float jumpOrbCooldown;

    void Start()
    {
        CameraPosition.transitionLength = sceneTrinsitionsLength;
        JumpOrb.orbCooldown = jumpOrbCooldown;
    }
}

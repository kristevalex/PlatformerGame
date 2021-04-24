using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOrb : MonoBehaviour
{
    public static JumpOrb instance = null;
    public static bool readyToJump = true;

    SpriteRenderer orbRenderer;
    float cooldownStartTime;
    public static float orbCooldown;


    private void Start()
    {
        orbRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            instance = this;
            readyToJump = orbRenderer.enabled;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
            instance = null;
    }

    public void CooldownStart()
    {
        orbRenderer.enabled = false;
        cooldownStartTime = Time.time;
        instance = null;
        readyToJump = false;
    }

    private void Update()
    {
        if (!orbRenderer.enabled)
            if (Time.time > cooldownStartTime + orbCooldown)
                orbRenderer.enabled = true;

        if (instance == this)
            readyToJump = orbRenderer.enabled;  
    }
}

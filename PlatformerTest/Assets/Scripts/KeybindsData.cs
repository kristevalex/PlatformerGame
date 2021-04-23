using System;
using UnityEngine;

[Serializable]
public class KeyBindsData
{
    public string right, left, up, down, jump, dash, timelapse;

    public KeyBindsData()
    {
        right     = KeyCode.RightArrow.ToString();
        left      = KeyCode.LeftArrow.ToString();
        up        = KeyCode.UpArrow.ToString();
        down      = KeyCode.DownArrow.ToString();
        jump      = KeyCode.Z.ToString();
        dash      = KeyCode.C.ToString();
        timelapse = KeyCode.X.ToString();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBinds : MonoBehaviour
{
    public static Dictionary<string, KeyCode> keyBinds;

    [SerializeField]
    Text right, left, up, down, jump, dash, timelapse;

    GameObject currentKey;

    void Start()
    {
        Storage.LoadKeyBinds();
        UpdateAllKeys();
    }

    private void OnGUI()
    {
        if (currentKey != null)
        {
            Event e = Event.current;

            if (e.isKey)
            {
                keyBinds[currentKey.tag] = e.keyCode;
                currentKey.GetComponent<Text>().text = e.keyCode.ToString();
                currentKey = null;
                Storage.SaveKeyBinds();
            }
        }
    }

    public void ChangeKey (GameObject clicked)
    {
        currentKey = clicked;
    }

    public void ResetToDefault()
    {
        Storage.ResetKeyBinds();
        UpdateAllKeys();
    }

    private void UpdateAllKeys()
    {
        right.text     = keyBinds["Right"].ToString();
        left.text      = keyBinds["Left"].ToString();
        up.text        = keyBinds["Up"].ToString();
        down.text      = keyBinds["Down"].ToString();
        jump.text      = keyBinds["Jump"].ToString();
        dash.text      = keyBinds["Dash"].ToString();
        timelapse.text = keyBinds["Timelapse"].ToString();
    }
}

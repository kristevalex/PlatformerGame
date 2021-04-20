using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    GameObject EscapeMenuCanvas;

    public static bool gamePaused;

    private void Start()
    {
        gamePaused = EscapeMenuCanvas.activeSelf;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !EscapeMenuCanvas.activeSelf;
            EscapeMenuCanvas.SetActive(gamePaused);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text text;
    private static int frames;
    public static bool active = true;


    void Start()
    {
        active = true;
        text = GetComponent<Text>();
    }

    void FixedUpdate()
    {
        if (text)
        {
            if (!active)
                return;

            ++frames;

            text.text = "" + frames / 50 + "." + (frames / 5) % 10 + (frames * 2) % 10;
        }
    }
        
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            active = false;
        }
    }

    static public void Reset()
    {
        frames = 0;
        active = true;
    }
}
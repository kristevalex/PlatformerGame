using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneExitInfo : MonoBehaviour
{
    [SerializeField]
    GameObject[] turnOn;
    [SerializeField]
    GameObject[] turnOff;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            foreach (GameObject element in turnOn)
                element.SetActive(true);

            foreach (GameObject element in turnOff)
                element.SetActive(false);
        }
    }
}

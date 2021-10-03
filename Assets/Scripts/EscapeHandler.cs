using UnityEngine;

public class EscapeHandler : MonoBehaviour
{
    float lastEscapeKeyTime = 0;
    float threshold = 0.75f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.unscaledTime - lastEscapeKeyTime < threshold)
            {
                Debug.Log("Quitting...");
                Application.Quit();
            }
            else
            {
                lastEscapeKeyTime = Time.unscaledTime;
            }
        }
    }
}

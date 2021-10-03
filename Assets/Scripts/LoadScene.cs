using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public int buildIndex;

    public void LoadNow()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);
    }
}

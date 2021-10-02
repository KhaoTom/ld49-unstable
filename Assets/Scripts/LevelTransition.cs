using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class LevelTransition : MonoBehaviour
{
    public int nextLevelBuildIndex = 0;
    public Text resultText;
    public void DoLevelTransition(Score score)
    {
        Time.timeScale = 0;
        resultText.text = $"RESULT\n\nWorlds destroyed: {score.instability:0.00} million\n\nHits landed: {score.hits}";
        GetComponent<PlayableDirector>().Play();
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelBuildIndex);
    }
}

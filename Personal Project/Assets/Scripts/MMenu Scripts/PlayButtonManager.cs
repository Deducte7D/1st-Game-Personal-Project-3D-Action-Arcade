using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButtonManager : MonoBehaviour
{
    public Button playButton;

    void Start()
    {
        // Clear old listeners (optional)
        playButton.onClick.RemoveAllListeners();

        // Add new listener
        //waveResetButton.onClick.AddListener(TaskOnClick);
        //playButton.onClick.AddListener(() => spawnManager.CallLevelUpdateStatsIncrement());

    }

    // call OnClick event
    public void PlayGame()
    {
        SceneManager.LoadScene("My Game");
        Time.timeScale = 1f; // unfreeze
    }

}

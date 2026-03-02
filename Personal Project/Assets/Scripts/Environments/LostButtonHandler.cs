using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LostButtonHandler : MonoBehaviour
{
    //public Button resartButton;
    public SpawnManagerV2 spawnManager;

    // used this one instead
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        // Ensure button remains interactable
        //playButton.interactable = true;
        spawnManager.ResetWaveCount();
        spawnManager.Resets();
    }

    // not using
    public void Restart()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
        // work for built game, in editor no
    }

    
}

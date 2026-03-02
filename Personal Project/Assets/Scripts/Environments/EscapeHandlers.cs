using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class EscapeHandlers : MonoBehaviour
{
    public GameObject confirmationPanel; // assign Inspector
    public PlayerController playerController;

    private void Start()
    {
        confirmationPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool newState = !confirmationPanel.activeSelf;
            
            // basically if inactive, convert to true to display and vice versa
            confirmationPanel.SetActive(newState);

            // pause if active, resume if inactive
            Time.timeScale = newState? 0f : 1f;

            if (newState)
                OnKinematic();
            else
                OffKinematic();
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        // Ensure button remains interactable
        //playButton.interactable = true;
    }

    public void QuitGame()
    {
        Application.Quit();
        // work for built game, in editor no
    }

    public void Resume()
    {
        confirmationPanel.SetActive(false);
        Time.timeScale = 1f;
        OffKinematic();
    }

    private IEnumerator OnKinematic()
    {
        playerController.PlayerRbKinematicToggleTrue();

        yield return null;
    }

    private IEnumerator OffKinematic()
    {
        playerController.PlayerRbKinematicToggleFalse();

        yield return null;
    }

}

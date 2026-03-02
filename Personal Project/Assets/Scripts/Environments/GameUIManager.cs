using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;

    public TextMeshProUGUI winPointsText;
    public TextMeshProUGUI winNextLevelText;
    public TextMeshProUGUI losePointsText;
    public TextMeshProUGUI loseLevelText;

    public int currentPoints;
    public int currentLevel;

    public float displayDelay = 2.5f;

    public ScoringSys scoreManager;
    public LevelUpdater levelUpdater;

    private void Update()
    {
        if (scoreManager == null)
        {
            scoreManager = FindFirstObjectByType<ScoringSys>();
        }
    }
    //public void WinGame()
    //{
    //    RecordCurrent_LevelPoints();
    //    winPanel.SetActive(true);
    //    //winPointsText.text = "Current Point: " + currentPoints;
    //    //winNextLevelText.text = "To Next Level: " + (currentLevel + 1);
    //    winPointsText.text = "" + currentPoints;
    //    winNextLevelText.text = "" + (currentLevel + 1);
    //    Time.timeScale = 0f;
    //}

    public void WinGame()
    {
        StartCoroutine(DelayedWinPanel());
    }

    private IEnumerator DelayedWinPanel()
    {
        RecordCurrent_LevelPoints();

        yield return new WaitForSeconds(1.5f); // adjust to match animation length

        winPanel.SetActive(true);
        //winPointsText.text = "Current Point: " + currentPoints;
        //winNextLevelText.text = "To Next Level: " + (currentLevel + 1);
        winPointsText.text = "" + currentPoints;
        winNextLevelText.text = "" + (currentLevel + 1);
        Time.timeScale = 0f;
    }

    //public void LoseGame()
    //{
    //    new WaitForSeconds(displayDelay);
    //    RecordCurrent_LevelPoints();
    //    losePanel.SetActive(true);
    //    losePointsText.text = "" + currentPoints;
    //    loseLevelText.text = "" + currentLevel;
    //    Time.timeScale = 0f;
    //}

    public void LoseGame()
    {
        StartCoroutine(DelayedLosePanel());
    }

    private IEnumerator DelayedLosePanel()
    {
        RecordCurrent_LevelPoints();

        yield return new WaitForSeconds(1.5f); // adjust to match animation length

        losePanel.SetActive(true);
        losePointsText.text = "" + currentPoints;
        loseLevelText.text = "" + currentLevel;
        Time.timeScale = 0f;
    }


    // still in planning
    public void GoNextLevel()
    {
        Time.timeScale = 1f; // time scale reset normal
        //SceneManager.LoadScene("Level" + (currentLevel + 1));
        HidePanels();
    }

    // save for main menu scene
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // copy values for display 
    public void RecordCurrent_LevelPoints()
    {
        currentPoints = (int)scoreManager.totalScore;
        currentLevel = levelUpdater.currentLevel;

        Debug.Log("Original variable value: " + scoreManager.totalScore);
        Debug.Log("Current variable value: " + currentPoints);
    }

    public void HidePanels()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }
    
}

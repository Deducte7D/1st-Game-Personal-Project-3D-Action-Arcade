using UnityEngine;
using UnityEngine.UI;

public class NextLevelButtonHandler : MonoBehaviour
{
    public Button waveResetButton;
    public SpawnManagerV2 spawnManager;
    public GameUIManager gameUIManager;
    public DistanceCounter distanceCounter;

    void Start()
    {
        // Clear old listeners (optional)
        waveResetButton.onClick.RemoveAllListeners();

        // Add new listener
        //waveResetButton.onClick.AddListener(TaskOnClick);
        waveResetButton.onClick.AddListener(() => spawnManager.CallLevelUpdateStatsIncrement());
        waveResetButton.onClick.AddListener(() => spawnManager.ResetWaveCount());
        waveResetButton.onClick.AddListener(() => spawnManager.Resets());
        waveResetButton.onClick.AddListener(() => distanceCounter.NewLevel());
        waveResetButton.onClick.AddListener(() => gameUIManager.GoNextLevel());
        
    }

    //public void NextLevel()
    //{

    //}

    //void TaskOnClick()
    //{
    //    Debug.Log("New Level Reset !");

    //}
}

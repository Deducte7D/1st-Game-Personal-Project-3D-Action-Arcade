using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DistanceCounter : MonoBehaviour
{
    public float distanceTravelled;   // Current distance
    public float levelDistance; // End level after this distance
    public float initLevelDistance;

    public bool levelEnded = false;

    private MoveLeftBG moveLeftScript;
    public ScoringSys scoreManager;

    public TextMeshProUGUI distanceText;

    void Start()
    {
        moveLeftScript = FindFirstObjectByType<MoveLeftBG>();
        //levelDistance = 250f;
        initLevelDistance = levelDistance;

        
    }

    void Update()
    {
        if (scoreManager == null)
        {
            scoreManager = FindFirstObjectByType<ScoringSys>();
        }

        if (!levelEnded)
        {
            // Increase distance based on movement speed
            distanceTravelled += moveLeftScript.GetSpeed() * Time.deltaTime * 0.5f;

            distanceText.text = "Distance: " + distanceTravelled.ToString("F2");

            // Check if we reached end
            if (distanceTravelled >= levelDistance)
            {
                EndLevel();

            }
        }

    }
    
    public void EndLevel()
    {
        levelEnded = true;

        // Stop background and ground movement
        moveLeftScript.StopMovement();

        // Stop all other MoveLeftBG scripts (if multiple)
        foreach (MoveLeftBG mover in Object.FindObjectsByType<MoveLeftBG>(FindObjectsSortMode.None))
        {
            mover.StopMovement();
        }

        // Optional: Trigger end UI or animations here
        Debug.Log("Make the Shot");
    }

    // call on NewLevelManager aka SpawnManagerV2
    public void NewLevel()
    {
        levelEnded = false;

        // Reenable background and ground movement
        moveLeftScript.ReEnableMovement();

        // Reenable all other MoveLeftBG scripts (if multiple)
        foreach (MoveLeftBG mover in Object.FindObjectsByType<MoveLeftBG>(FindObjectsSortMode.None))
        {
            mover.ReEnableMovement();
        }

    }

    public void CounterIncrement()
    {
        levelDistance += 25f; // originally 50 but too long
    }

    public void CounterToRecord()
    {
        levelEnded = true;
        ScoringSys.Instance.AddScore(distanceTravelled);
        //scoreManager.AddScore(distanceTravelled);

        Debug.Log("Distance Travel value : " + distanceTravelled);
        Debug.Log("ScoreManager reference: " + scoreManager);
    }

    public void ResetLevelStatus()
    {
        distanceTravelled = 0;
        levelDistance = initLevelDistance; // get the original preset level distance to reach
        levelEnded = false;
    }

}

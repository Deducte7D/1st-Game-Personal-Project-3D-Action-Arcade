using TMPro;
using UnityEngine;

public class DistanceCounter : MonoBehaviour
{
    public float distanceTravelled;   // Current distance
    public float levelDistance = 500f; // End level after this distance
    public bool levelEnded = false;

    private MoveLeftBG moveLeftScript;

    public TextMeshProUGUI distanceText;

    void Start()
    {
        moveLeftScript = FindFirstObjectByType<MoveLeftBG>();
    }

    void Update()
    {
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

    
    void EndLevel()
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
        Debug.Log("Level Complete!");
    }
}

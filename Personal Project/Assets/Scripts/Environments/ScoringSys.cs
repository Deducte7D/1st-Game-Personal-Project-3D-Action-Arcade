using UnityEngine;

public class ScoringSys : MonoBehaviour
{

    public static ScoringSys Instance; // instance is just variable name for this class
    public float totalScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //private void Start()
    //{
    //    LoadScore();
    //}


    // accumulate score final scores
    public void AddScore(float score)
    {
        LoadScore();
        totalScore += score;
        SaveScore();

        Debug.Log("Total Score: " + totalScore);
    }

    public void GameOverResetScore()
    {
        totalScore = 0;
        SaveScore();
    }


    public void ResetScore()
    {
        totalScore = 0;
        //SaveScore(); do not save score coz it will be 0
    }

    public void SaveScore()
    {
        PlayerPrefs.SetFloat("Highscore", totalScore);
        PlayerPrefs.Save(); // forces all changes made to it to be written to disk immediately.
    }

    // to load last score saved in memory
    public float LoadScore()
    {
        totalScore = PlayerPrefs.GetFloat("Highscore", 0);

        Debug.Log("Total Score: " + totalScore);
        
        return totalScore;
    }
    
}

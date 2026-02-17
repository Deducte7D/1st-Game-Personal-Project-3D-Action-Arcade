using UnityEngine;
using System.Collections;

public class SlowMoController : MonoBehaviour
{
    public static SlowMoController Instance;

    private float startFixedDeltaTime;

    void Awake() => Instance = this;

    private void Start()
    {
        startFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void StartSlowMotion(float duration)
    {
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = Time.fixedDeltaTime * 0.5f;
        StartCoroutine(EndSlowMotion(duration));
    }

    private IEnumerator EndSlowMotion(float duration)
    {
        yield return new WaitForSecondsRealtime(duration); // basically after duration reset timescale
        Time.timeScale = 1f;
        Time.fixedDeltaTime = startFixedDeltaTime;
    }
}

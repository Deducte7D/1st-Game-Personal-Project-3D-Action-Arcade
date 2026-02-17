using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageFlashUI : MonoBehaviour
{
    public Image flashImage;
    public float flashDuration = 0.25f;

    private Coroutine flashRoutine;

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashEffect());
    }

    private IEnumerator FlashEffect()
    {
        // Turn red visible
        flashImage.color = new Color(1, 0, 0, 0.6f);

        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.deltaTime;

            float alpha = Mathf.Lerp(0.6f, 0f, t / flashDuration);
            flashImage.color = new Color(1, 0, 0, alpha);

            yield return null;
        }

        flashImage.color = new Color(1, 0, 0, 0f);
        flashRoutine = null;
    }
}

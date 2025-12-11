using System.Collections;
using UnityEngine;
using TMPro;

public class StartupTextFader : MonoBehaviour
{
    [Header("Text to control (TextMeshPro)")]
    public TextMeshProUGUI startupText;

    [Header("Timing (seconds)")]
    [Tooltip("How long the text stays fully visible before fading.")]
    public float displaySeconds = 2f;
    [Tooltip("How long the fade-out takes.")]
    public float fadeSeconds = 1.5f;

    [Header("Options")]
    [Tooltip("Start fading automatically when the scene starts.")]
    public bool startOnAwake = true;
    [Tooltip("Optional: initial text to show (overrides inspector text if not empty).")]
    public string initialText = "";

    // cached color
    private Color originalColor;

    private void Awake()
    {
        if (startupText == null)
        {
            Debug.LogError("StartupTextFader: startupText is not assigned.", this);
            enabled = false;
            return;
        }

        originalColor = startupText.color;
        // Ensure text is visible at start
        SetTextAlpha(1f);

        if (!string.IsNullOrEmpty(initialText))
            startupText.text = initialText;
    }

    private void Start()
    {
        if (startOnAwake)
            StartCoroutine(ShowThenFadeRoutine());
    }

    /// <summary>
    /// Public method to trigger the sequence from other scripts.
    /// </summary>
    public void Trigger()
    {
        StopAllCoroutines();
        SetTextAlpha(1f);
        StartCoroutine(ShowThenFadeRoutine());
    }

    private IEnumerator ShowThenFadeRoutine()
    {
        // Wait while fully visible
        yield return new WaitForSeconds(displaySeconds);

        // Fade out over fadeSeconds
        if (fadeSeconds <= 0f)
        {
            SetTextAlpha(0f);
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < fadeSeconds)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeSeconds);
            // t from 0 -> 1, alpha should go 1 -> 0
            float a = Mathf.Lerp(1f, 0f, t);
            SetTextAlpha(a);
            yield return null;
        }

        SetTextAlpha(0f);
    }

    private void SetTextAlpha(float a)
    {
        Color c = originalColor;
        c.a = a;
        startupText.color = c;
    }
}

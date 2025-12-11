using System.Collections;
using UnityEngine;
using TMPro;

public class CubeInteraction : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject instructionUI;               // Your existing popup object
    public TextMeshProUGUI collisionText;          // OPTIONAL: Text that appears when player collides

    [Header("Collision Text Settings")]
    public string textToShow = "Press K to progress";
    public float displaySeconds = 2f;              // How long text stays fully visible
    public float fadeSeconds = 1.5f;               // Fade-out duration
    public bool enableCollisionText = false;       // Toggle: do you want the text to appear on collision?

    private Color originalColor;
    private bool colorCached = false;

    private void Start()
    {
        if (instructionUI != null)
            instructionUI.SetActive(false);

        // Hide collision text at start
        if (collisionText != null)
        {
            Color c = collisionText.color;
            c.a = 0f;
            collisionText.color = c;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If collision text is enabled and assigned, show + fade
        if (enableCollisionText && collisionText != null)
        {
            collisionText.text = textToShow;
            StartCoroutine(ShowThenFade());
        }

        // If you want the instruction UI to show up, keep this part as-is
        if (instructionUI != null)
            instructionUI.SetActive(true);
    }

    public void TriggerDisappearance()
    {
        if (instructionUI != null)
            instructionUI.SetActive(false);

        gameObject.SetActive(false);
    }

    private IEnumerator ShowThenFade()
    {
        if (!colorCached)
        {
            originalColor = collisionText.color;
            colorCached = true;
        }

        // Reset alpha to full
        SetAlpha(1f);

        // Stay fully visible
        yield return new WaitForSeconds(displaySeconds);

        // Fade out
        float elapsed = 0f;
        while (elapsed < fadeSeconds)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeSeconds);
            float a = Mathf.Lerp(1f, 0f, t);
            SetAlpha(a);
            yield return null;
        }

        SetAlpha(0f);
    }

    private void SetAlpha(float a)
    {
        Color c = collisionText.color;
        c.a = a;
        collisionText.color = c;
    }
}

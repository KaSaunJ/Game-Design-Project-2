using UnityEngine;
using UnityEngine.UI; // Remove if using TextMeshPro

public class CubeInteraction : MonoBehaviour
{
    [Header("Reference to the Player")]
    public Transform player;

    [Header("UI Instruction")]
    public GameObject instructionUI;
    // You can use a Text, TMP, or any UI object. Just enable/disable it.

    private bool playerIsClose = false;

    private void Start()
    {
        if (instructionUI != null)
            instructionUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            playerIsClose = true;
            if (instructionUI != null)
                instructionUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            playerIsClose = false;
            if (instructionUI != null)
                instructionUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerIsClose && Input.GetKeyDown(KeyCode.K))
        {
            // Hide instructions
            if (instructionUI != null)
                instructionUI.SetActive(false);

            // Make cube disappear
            gameObject.SetActive(false);
        }
    }
}

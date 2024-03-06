using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI infoText; // Reference to the UI Text element

    private void Start()
    {
        // Ensure the Text component is assigned
        if (infoText == null)
        {
            Debug.LogError("Text component not assigned in ObjectInfoDisplay script.");
            enabled = false; // Disable the script to prevent errors
        }

        // Initially, hide the info text
        infoText.gameObject.SetActive(false);
    }

    private void OnMouseOver()
    {
        // Display information when the mouse is over the object
        infoText.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        // Hide the information when the mouse exits the object
        infoText.gameObject.SetActive(false);
    }
}

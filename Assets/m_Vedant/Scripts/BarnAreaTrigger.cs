using UnityEngine;

public class BarnAreaTrigger : MonoBehaviour
{
    public GameObject barnInventoryPanel;  // Assign the BarnPanel (inventory UI)
    public GameObject promptText;          // Assign UI Text like “Press E to open barn inventory”

    private bool isPlayerInside = false;

    void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            bool isOpen = barnInventoryPanel.activeSelf;
            barnInventoryPanel.SetActive(!isOpen);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            promptText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            promptText.SetActive(false);
            barnInventoryPanel.SetActive(false); // force-close if player exits
        }
    }
}

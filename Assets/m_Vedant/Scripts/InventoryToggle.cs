using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject playerInventoryPanel;
    public InventoryUI inventoryUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isOpen = playerInventoryPanel.activeSelf;
            playerInventoryPanel.SetActive(!isOpen);

            if (!isOpen)
            {
                inventoryUI.RefreshUI();
            }
        }
    }
}

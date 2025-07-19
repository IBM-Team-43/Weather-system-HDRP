using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public GameObject barnInventoryPanel;
    public GameObject playerInventoryPanel;

    void Update()
    {
        bool isAnyInventoryOpen = barnInventoryPanel.activeSelf || playerInventoryPanel.activeSelf;

        if (isAnyInventoryOpen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

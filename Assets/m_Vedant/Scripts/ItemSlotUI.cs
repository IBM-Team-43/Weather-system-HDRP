using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;

    private InventoryItem item;
    private bool isFromBarn;

    public void Initialize(InventoryItem newItem, bool fromBarn)
    {
        item = newItem;
        isFromBarn = fromBarn;
        iconImage.sprite = item.icon;
        nameText.text = item.itemName;
    }

    public void OnClick()
    {
        if (item == null) return;

        bool moved = false;

        if (isFromBarn)
        {
            moved = PlayerInventory.AddItem(item);
            if (moved) BarnInventory.RemoveItem(item);
        }
        else
        {
            BarnInventory.AddItem(item);
            PlayerInventory.RemoveItem(item);
            moved = true;
        }

        if (moved)
        {
            FindObjectOfType<InventoryUI>()?.RefreshUI();
        }
        else
        {
            Debug.Log("Couldn't move item — possibly full inventory.");
            // Optionally: show warning in UI
        }
    }

}

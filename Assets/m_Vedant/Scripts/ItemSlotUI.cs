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
        if (PlayerStorage.Instance.items.ContainsKey(item.itemType))
        {
            PlayerStorage.Instance.RemoveItem(item);
            BarnStorage.Instance.AddItem(item);
        }
        else
        {
            BarnStorage.Instance.RemoveItem(item);
            PlayerStorage.Instance.AddItem(item);
        }
        Destroy(gameObject);
    }

}

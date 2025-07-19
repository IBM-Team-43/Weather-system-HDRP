using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public GameObject itemSlotPrefab;
    public Transform barnGridParent;
    public Transform playerGridParent;

    void OnEnable()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        RefreshGrid(barnGridParent, BarnInventory.GetAllItems(), true);
        RefreshGrid(playerGridParent, PlayerInventory.GetAllItems(), false);
    }

    private void RefreshGrid(Transform parent, List<InventoryItem> items, bool isFromBarn)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        foreach (InventoryItem item in items)
        {
            GameObject slot = Instantiate(itemSlotPrefab, parent);
            var slotUI = slot.GetComponent<ItemSlotUI>();
            slotUI.Initialize(item, isFromBarn);
        }
    }
}

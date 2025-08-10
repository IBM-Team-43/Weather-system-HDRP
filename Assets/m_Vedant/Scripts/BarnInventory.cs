/*
using System.Collections.Generic;
using UnityEngine;

public static class BarnInventory
{
    private static List<InventoryItem> items = new List<InventoryItem>();

    public static void AddItem(InventoryItem newItem)
    {
        items.Add(newItem);
        Debug.Log($"{newItem.itemName} added to BARN inventory.");
    }

    public static void RemoveItem(InventoryItem item)
    {
        if (items.Contains(item))
            items.Remove(item);
    }

    public static List<InventoryItem> GetAllItems()
    {
        return new List<InventoryItem>(items);
    }

    public static void ClearInventory()
    {
        items.Clear();
        Debug.Log("Barn inventory cleared.");
    }
}
*/

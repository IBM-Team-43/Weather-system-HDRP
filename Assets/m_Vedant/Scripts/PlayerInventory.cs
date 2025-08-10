/*
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInventory
{
    private static List<InventoryItem> playerItems = new ();
    private static int maxCapacity = 10;

    public static bool AddItem(InventoryItem item)
    {
        if (playerItems.Count >= maxCapacity)
        {
            Debug.LogWarning("Player inventory is full!");
            return false;
        }

        playerItems.Add(item);
        Debug.Log($"{item.itemName} added to PLAYER inventory.");
        return true;
    }

    public static void RemoveItem(InventoryItem item)
    {
        if (playerItems.Contains(item))
        {
            playerItems.Remove(item);
            Debug.Log($"{item.itemName} removed from PLAYER inventory.");
        }
    }

    public static List<InventoryItem> GetAllItems()
    {
        return  new(playerItems);
    }

    public static void ClearInventory()
    {
        playerItems.Clear();
        Debug.Log("Player inventory cleared.");
    }

    public static int GetMaxCapacity() => maxCapacity;
    public static int GetCurrentCount() => playerItems.Count;
}
*/

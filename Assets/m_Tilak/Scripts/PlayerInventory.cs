using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class PlayerInventory : MonoBehaviour
{
    public int gold = 100;
    public List<Item> items = new List<Item>();
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI itemsText;

    void Start()
    {
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateUI();
    }

    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    public void AddItem(Item item)
    {
        if (item != null)
        {
            items.Add(item);
            UpdateUI();
        }
    }

    public void AddItem(string itemName)
    {
        Item newItem = new Item();
        newItem.name = itemName;
        newItem.price = 10;
        AddItem(newItem);
    }

    public bool RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            UpdateUI();
            return true;
        }
        return false;
    }

    public bool RemoveItem(string itemName)
    {
        Item itemToRemove = items.FirstOrDefault(item => item.name == itemName);
        if (itemToRemove != null)
        {
            return RemoveItem(itemToRemove);
        }
        return false;
    }

    public List<Item> GetSellableItems()
    {
        return new List<Item>(items);
    }

    public int GetGold()
    {
        return gold;
    }

    public bool HasItem(string itemName)
    {
        return items.Any(item => item.name == itemName);
    }

    public int GetItemCount(string itemName)
    {
        return items.Count(item => item.name == itemName);
    }

    void UpdateUI()
    {
        if (goldText != null)
            goldText.text = "Gold: " + gold;

        if (itemsText != null)
        {
            if (items.Count > 0)
            {
                var itemNames = items.Select(item => item.name).ToArray();
                itemsText.text = "Items: " + string.Join(", ", itemNames);
            }
            else
            {
                itemsText.text = "Items: None";
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class BarnStorage : MonoBehaviour
{
    public ItemSlotUI ItemSlotUI;
    public GameObject barnInventoryPanel;
    public List<InventoryItem> prestored;
    public Dictionary<InventoryItem.ItemType,InventoryItem> items = new ();
    public static BarnStorage Instance;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }

        foreach (var item in prestored)
        {
            AddItem(item);
        }
    }
    public void AddItem(InventoryItem item)
    {
        if (item != null && items.TryAdd(item.itemType, item))
        {
            var ui=Instantiate(ItemSlotUI, barnInventoryPanel.transform);
            ui.Initialize(item,false);
            Debug.Log($"Added item: {item.itemName}");
        }
        
    }
    public void RemoveItem(InventoryItem item)
    {
        if (item != null && items.ContainsKey(item.itemType))
        {
            items.Remove(item.itemType);
            Debug.Log($"Removed item: {item.itemName}");
        }
        
    }
}

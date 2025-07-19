using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite icon;

    public enum ItemType
    {
        Tool,
        Seed,
        WaterCan
        // Add more types as needed
    }
}

using UnityEngine;

public class BarnTest : MonoBehaviour
{
    public InventoryItem hoe;
    public InventoryItem tomatoSeed;

    void Start()
    {
        BarnInventory.ClearInventory();
        PlayerInventory.ClearInventory();

        BarnInventory.AddItem(hoe);
        BarnInventory.AddItem(tomatoSeed);
    }
}
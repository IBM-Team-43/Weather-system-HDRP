using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public int price; 
    
    public int GetSellPrice()
    {
        return Mathf.Max(1, price / 2); 
    }
}
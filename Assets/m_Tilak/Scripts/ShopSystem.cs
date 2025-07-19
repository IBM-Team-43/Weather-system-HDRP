using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopSystem : MonoBehaviour
{
    [Header("UI References")]
    public Canvas shopCanvas;
    public GameObject shopPanel;
    public GameObject buyMenuPanel;
    public GameObject sellMenuPanel;
    public TextMeshProUGUI shopPromptText;
    public TextMeshProUGUI goldDisplayText;
    public GameObject itemButtonPrefab;

    [Header("Shop Items")]
    public List<Item> shopItems = new List<Item>();

    [Header("Player References")]
    public PlayerInventory playerInventory;

    private bool isPlayerInZone = false;

    void Start()
    {
        if (shopCanvas != null)
            shopCanvas.gameObject.SetActive(false);

        if (playerInventory == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerInventory = player.GetComponent<PlayerInventory>();
        }

        UpdateGoldDisplay();
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            OpenShop();
        }

        if (shopCanvas != null && shopCanvas.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;

            if (shopCanvas != null)
                shopCanvas.gameObject.SetActive(true);

            if (shopPromptText != null)
            {
                shopPromptText.gameObject.SetActive(true);
                shopPromptText.text = "Press E to open shop";
            }

            if (shopPanel != null)
                shopPanel.SetActive(false);
            if (buyMenuPanel != null)
                buyMenuPanel.SetActive(false);
            if (sellMenuPanel != null)
                sellMenuPanel.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;

            if (shopCanvas != null)
                shopCanvas.gameObject.SetActive(false);
        }
    }

    public void OpenShop()
    {
        if (shopPanel != null)
        {
            if (shopPromptText != null)
                shopPromptText.gameObject.SetActive(false);
            if (buyMenuPanel != null)
                buyMenuPanel.SetActive(false);
            if (sellMenuPanel != null)
                sellMenuPanel.SetActive(false);

            shopPanel.SetActive(true);
            UpdateGoldDisplay();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void CloseShop()
    {
        if (shopCanvas != null)
            shopCanvas.gameObject.SetActive(false);

        ClearBuyMenu();
        ClearSellMenu();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenBuyMenu()
    {
        if (shopPanel != null && buyMenuPanel != null)
        {
            shopPanel.SetActive(false);
            if (sellMenuPanel != null)
                sellMenuPanel.SetActive(false);
            if (shopPromptText != null)
                shopPromptText.gameObject.SetActive(false);

            buyMenuPanel.SetActive(true);
            UpdateGoldDisplay();
            PopulateBuyMenu();
        }
    }

    public void CloseBuyMenu()
    {
        if (shopPanel != null && buyMenuPanel != null)
        {
            ClearBuyMenu();
            buyMenuPanel.SetActive(false);
            if (shopPromptText != null)
                shopPromptText.gameObject.SetActive(false);

            shopPanel.SetActive(true);
        }
    }

    public void OpenSellMenu()
    {
        if (shopPanel != null && sellMenuPanel != null)
        {
            shopPanel.SetActive(false);
            if (buyMenuPanel != null)
                buyMenuPanel.SetActive(false);
            if (shopPromptText != null)
                shopPromptText.gameObject.SetActive(false);

            sellMenuPanel.SetActive(true);
            UpdateGoldDisplay();
            PopulateSellMenu();
        }
    }

    public void CloseSellMenu()
    {
        if (shopPanel != null && sellMenuPanel != null)
        {
            ClearSellMenu();
            sellMenuPanel.SetActive(false);
            if (shopPromptText != null)
                shopPromptText.gameObject.SetActive(false);

            shopPanel.SetActive(true);
        }
    }

    public void BuyItem(Item item)
    {
        if (playerInventory != null && item != null)
        {
            if (playerInventory.SpendGold(item.price))
            {
                Item purchasedItem = new Item();
                purchasedItem.name = item.name;
                purchasedItem.price = item.price;
                playerInventory.AddItem(purchasedItem);
                UpdateGoldDisplay();
            }
        }
    }

    public void SellItem(Item item)
    {
        if (playerInventory != null && item != null)
        {
            int sellPrice = item.GetSellPrice();

            if (playerInventory.RemoveItem(item))
            {
                playerInventory.AddGold(sellPrice);
                UpdateGoldDisplay();
                PopulateSellMenu();
            }
        }
    }

    private void PopulateBuyMenu()
    {
        if (buyMenuPanel == null || itemButtonPrefab == null) return;

        ClearBuyMenu();
        ConfigureMenuLayout(buyMenuPanel);

        foreach (Item item in shopItems)
        {
            if (item != null)
            {
                GameObject button = Instantiate(itemButtonPrefab, buyMenuPanel.transform);
                button.name = "DynamicBuyButton_" + item.name;
                SetupButtonSize(button);

                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = $"{item.name}\nBuy: {item.price} Gold";
                    buttonText.fontSize = 14;
                    buttonText.alignment = TextAlignmentOptions.Center;
                }

                Button buttonComponent = button.GetComponent<Button>();
                if (buttonComponent != null)
                {
                    Item itemToBuy = item;
                    buttonComponent.onClick.AddListener(() => BuyItem(itemToBuy));
                }
            }
        }
    }

    private void PopulateSellMenu()
    {
        if (sellMenuPanel == null || itemButtonPrefab == null || playerInventory == null) return;

        List<Item> sellableItems = playerInventory.GetSellableItems();
        ClearSellMenu();
        ConfigureMenuLayout(sellMenuPanel);

        if (sellableItems.Count == 0)
        {
            GameObject noItemsButton = Instantiate(itemButtonPrefab, sellMenuPanel.transform);
            noItemsButton.name = "NoItemsMessage";
            SetupButtonSize(noItemsButton);

            TextMeshProUGUI buttonText = noItemsButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "No items to sell";
                buttonText.fontSize = 14;
                buttonText.alignment = TextAlignmentOptions.Center;
            }

            Button buttonComponent = noItemsButton.GetComponent<Button>();
            if (buttonComponent != null)
                buttonComponent.interactable = false;

            return;
        }

        foreach (Item item in sellableItems)
        {
            if (item != null)
            {
                GameObject button = Instantiate(itemButtonPrefab, sellMenuPanel.transform);
                button.name = "DynamicSellButton_" + item.name;
                SetupButtonSize(button);

                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = $"{item.name}\nSell: {item.GetSellPrice()} Gold";
                    buttonText.fontSize = 14;
                    buttonText.alignment = TextAlignmentOptions.Center;
                }

                Button buttonComponent = button.GetComponent<Button>();
                if (buttonComponent != null)
                {
                    Item itemToSell = item;
                    buttonComponent.onClick.AddListener(() => SellItem(itemToSell));
                }
            }
        }
    }

    private void SetupButtonSize(GameObject button)
    {
        RectTransform buttonRect = button.GetComponent<RectTransform>();
        if (buttonRect != null)
        {
            buttonRect.localScale = Vector3.one;
            buttonRect.sizeDelta = new Vector2(180f, 40f);
            buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
            buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
            buttonRect.pivot = new Vector2(0.5f, 0.5f);
        }

        LayoutElement layoutElement = button.GetComponent<LayoutElement>();
        if (layoutElement == null)
        {
            layoutElement = button.AddComponent<LayoutElement>();
        }

        layoutElement.preferredWidth = 180f;
        layoutElement.preferredHeight = 40f;
        layoutElement.flexibleWidth = 0f;
        layoutElement.flexibleHeight = 0f;
    }

    private void ConfigureMenuLayout(GameObject menuPanel)
    {
        if (menuPanel == null) return;

        VerticalLayoutGroup layoutGroup = menuPanel.GetComponent<VerticalLayoutGroup>();
        if (layoutGroup == null)
        {
            layoutGroup = menuPanel.AddComponent<VerticalLayoutGroup>();
        }

        layoutGroup.childControlWidth = false;
        layoutGroup.childControlHeight = false;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.spacing = 10f;
        layoutGroup.padding = new RectOffset(10, 10, 10, 10);
        layoutGroup.childAlignment = TextAnchor.MiddleCenter;
    }

    private void UpdateGoldDisplay()
    {
        if (goldDisplayText != null && playerInventory != null)
        {
            goldDisplayText.text = $"Gold: {playerInventory.GetGold()}";
        }
    }

    private void ClearBuyMenu()
    {
        if (buyMenuPanel == null) return;

        foreach (Transform child in buyMenuPanel.transform)
        {
            if (child.name.StartsWith("DynamicBuyButton_"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void ClearSellMenu()
    {
        if (sellMenuPanel == null) return;

        foreach (Transform child in sellMenuPanel.transform)
        {
            if (child.name.StartsWith("DynamicSellButton_") || child.name == "NoItemsMessage")
            {
                Destroy(child.gameObject);
            }
        }
    }

    [ContextMenu("Add Sample Items")]
    private void AddSampleItems()
    {
        shopItems.Clear();

        Item healthPotion = new Item { name = "Health Potion", price = 10 };
        Item magicSword = new Item { name = "Magic Sword", price = 50 };
        Item shield = new Item { name = "Shield", price = 30 };
        Item manaPotion = new Item { name = "Mana Potion", price = 8 };

        shopItems.Add(healthPotion);
        shopItems.Add(magicSword);
        shopItems.Add(shield);
        shopItems.Add(manaPotion);
    }
}

using UnityEngine;
using TMPro; 
using UnityEngine.UI;

public class PlantActivator : MonoBehaviour
{
    private bool cropReadyToHarvest = false;

    [Header("Harvest Prompt")]
    public TextMeshProUGUI harvestText;

    [Header("Common")]
    public GameObject seedlingPrefab;
    public TextMeshProUGUI promptText;
    public AudioClip plantSound;

    [Header("Seed UI Reference")]
    public Image heldSeedUI;
    public TextMeshProUGUI cropInfoText;

    [Header("Tomato Prefabs")]
    public GameObject midTomatoPrefab;
    public GameObject fullTomatoPrefab;

    [Header("Corn Prefabs")]
    public GameObject midCornPrefab;
    public GameObject fullCornPrefab;

    public float growthTime1 = 5f;
    public float growthTime2 = 5f;

    private AudioSource audioSource;
    private bool playerInRange = false;
    private GameObject currentPlant;

    private enum CropType { None, Tomato, Corn }
    private CropType currentCrop = CropType.None;

    void Start()
    {
        harvestText.enabled = false;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (playerInRange && currentPlant == null)
        {
            if (heldSeedUI.sprite != null && heldSeedUI.enabled)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    string spriteName = heldSeedUI.sprite.name.ToLower();

                    if (spriteName.Contains("tomato"))
                    {
                        currentCrop = CropType.Tomato;
                        PlantSeed();
                    }
                    else if (spriteName.Contains("corn"))
                    {
                        currentCrop = CropType.Corn;
                        PlantSeed();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                ClearHeldSeed();
            }
        }

        if (playerInRange && cropReadyToHarvest && IsFullCropReady())
        {
            harvestText.text = "Press [R] to harvest your crop";
            harvestText.enabled = true;

            if (Input.GetKeyDown(KeyCode.R))
            {
                var items = PlayerInventory.GetAllItems();
                foreach (var item in items)
                {
                    if (item.itemType == InventoryItem.ItemType.Tool)
                    {
                        HarvestCrop();
                        break;
                    }
                }
                
            }
        }
    }

    void ClearHeldSeed()
    {
        heldSeedUI.sprite = null;
        heldSeedUI.enabled = false;
        cropInfoText.text = "";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (currentPlant == null)
            {
                if (heldSeedUI != null && heldSeedUI.sprite != null && heldSeedUI.enabled)
                {
                    string spriteName = heldSeedUI.sprite.name.ToLower();

                    if (spriteName.Contains("tomato"))
                        promptText.text = "Press [E] to plant Tomato";
                    else if (spriteName.Contains("corn"))
                        promptText.text = "Press [E] to plant Corn";
                    else
                        promptText.text = "Unknown crop";

                    cropInfoText.text = "Press [X] to choose another crop";
                }
                else
                {
                    promptText.text = "Pick a crop from the sack";
                    cropInfoText.text = "";
                }

                promptText.enabled = true;
            }

            // If crop already grown when player enters
            if (cropReadyToHarvest && IsFullCropReady())
            {
                var items = PlayerInventory.GetAllItems();
                foreach (var item in items)
                {
                    if (item.itemType == InventoryItem.ItemType.Tool)
                    {
                        ShowHarvestPrompt();
                        break;
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            promptText.enabled = false;
            cropInfoText.text = "";
            harvestText.enabled = false;
        }
    }

    void PlantSeed()
    {
        promptText.enabled = false;
        harvestText.enabled = false;

        if (plantSound != null)
            audioSource.PlayOneShot(plantSound);

        currentPlant = Instantiate(seedlingPrefab, transform.position, Quaternion.identity);
        StartCoroutine(GrowCrop());
    }

    System.Collections.IEnumerator GrowCrop()
    {
        yield return new WaitForSeconds(growthTime1);

        if (currentPlant != null)
            Destroy(currentPlant);

        switch (currentCrop)
        {
            case CropType.Tomato:
                currentPlant = Instantiate(midTomatoPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                break;
            case CropType.Corn:
                currentPlant = Instantiate(midCornPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                break;
        }

        yield return new WaitForSeconds(growthTime2);

        if (currentPlant != null)
            Destroy(currentPlant);

        switch (currentCrop)
        {
            case CropType.Tomato:
                currentPlant = Instantiate(fullTomatoPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                currentPlant.name = "FullTomato";
                break;
            case CropType.Corn:
                currentPlant = Instantiate(fullCornPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                currentPlant.name = "FullCorn";
                break;
        }

        cropReadyToHarvest = true;

        if (playerInRange)
        {
            var items = PlayerInventory.GetAllItems();
            foreach (var item in items)
            {
                if (item.itemType == InventoryItem.ItemType.Tool)
                {
                    ShowHarvestPrompt();
                    break;
                }
            }
            
        }
            
    }

    void HarvestCrop()
    {
        if (currentPlant != null)
        {
            Destroy(currentPlant);
            currentPlant = null;
        }

        cropReadyToHarvest = false;
        currentCrop = CropType.None;

        harvestText.enabled = false;
        promptText.text = "Crop harvested!";
        promptText.enabled = true;

        Invoke(nameof(HidePrompt), 2f);
    }

    void HidePrompt()
    {
        promptText.enabled = false;
    }

    bool IsFullCropReady()
    {
        return (currentCrop == CropType.Tomato && currentPlant?.name.Contains("FullTomato") == true) ||
               (currentCrop == CropType.Corn && currentPlant?.name.Contains("FullCorn") == true);
    }

    void ShowHarvestPrompt()
    {
        harvestText.text = "Press [R] to harvest your crop";
        harvestText.enabled = true;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace m_Devansh._Scripts.CropSystem
{
    public class EquippedCrops : MonoBehaviour
    {
        public StarterAssetsInputs inputs;
        [Header("Crop UI")] public Image equippedSeedUI;
        public TextMeshProUGUI plantText;

        HashSet<PlantArea> plantAreas = new();
        HashSet<PlantArea> harvestAreas = new();
        public Crop currentCrop;
        public int currentCropIndex = -1;
        public HashSet<Crop> crops = new();
        public List<Crop> cropsList = new();

        private void OnEnable()
        {
            inputs.OnInteract += PlantCrops;
            inputs.OnSwitch += SwitchCrop;
            inputs.OnHarvest += HarvestCrop;
        }
        private void OnDisable()
        {
            inputs.OnSwitch -= SwitchCrop;
            inputs.OnInteract -= AddCrop;
            inputs.OnHarvest -= HarvestCrop;
        }
       
        public SeedCollector seedCollector;
        private void SwitchCrop()
        {
            currentCropIndex = (currentCropIndex + 1) % cropsList.Count;
            EquipCrop(cropsList[currentCropIndex]);
        }
        private void EquipCrop(Crop crop)
        {
            currentCrop = crop;
            equippedSeedUI.sprite = currentCrop.cropIcon;
            equippedSeedUI.gameObject.SetActive(true);
        }
        public void AddCrop()
        {
            if (seedCollector)
            {
                if(crops.Add(seedCollector.crop))
                {
                    cropsList.Add(seedCollector.crop);
                    currentCropIndex++;
                }
                EquipCrop(seedCollector.crop);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlantArea plantArea))
            {
                switch (plantArea.state)
                {
                    case PlantArea.CropState.Empty:
                        if (!currentCrop) break;
                        plantAreas.Add(plantArea);
                        PromptPlanting();
                        break;
                    case PlantArea.CropState.ReadyToHarvest:
                        if(PlayerStorage.Instance.items.ContainsKey(InventoryItem.ItemType.Tool))
                            harvestAreas.Add(plantArea);
                        PromptHarvest();
                        break;
                }
            }
            else if (other.TryGetComponent(out SeedCollector seedCollector))
            {
                this.seedCollector = seedCollector;
                inputs.OnInteract += AddCrop;
                plantText.gameObject.SetActive(true);
                plantText.text = seedCollector.promptTextStr;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlantArea plantArea))
            {
                
                plantAreas.Remove(plantArea);
                harvestAreas.Remove(plantArea);
                PromptPlanting();
            }
            else if (other.TryGetComponent(out SeedCollector seedCollector))
            {
                inputs.OnInteract -= AddCrop;
                this.seedCollector = null;
                plantText.gameObject.SetActive(false);
            }
        }
        private void PromptPlanting()
        {
            
            if (plantAreas.Count > 0)
            {
                plantText.text = currentCrop.plantDescription;
                plantText.gameObject.SetActive(true);
            }
            else
            {
                plantText.gameObject.SetActive(false);
            }
        }
        private void PromptHarvest()
        {
            plantText.text = "Press X to Harvest";
            if (harvestAreas.Count > 0)
            {
                plantText.gameObject.SetActive(true);
            }
            else
            {
                plantText.gameObject.SetActive(false);
            }

        }
        private void HarvestCrop()
        {
            foreach (var harvestArea in harvestAreas)
            {
                if (harvestArea.state == PlantArea.CropState.ReadyToHarvest)
                {
                    harvestArea.Harvest();
                }
                PromptHarvest();
            }
            harvestAreas.Clear();
            
        }
        private void PlantCrops()
        {
            foreach (var plantArea in plantAreas)
            {
                plantArea.Plant(currentCrop);
                
                plantText.gameObject.SetActive(false);
            }
            plantAreas.Clear();
        }

    }
}

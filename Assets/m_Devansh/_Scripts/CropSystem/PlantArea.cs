using System;
using System.Collections;
using UnityEngine;

namespace m_Devansh._Scripts.CropSystem
{
    public class PlantArea : MonoBehaviour
    {
        public CropState state = CropState.Empty;
        public int stage;
        public GameObject cropGO;
        public Crop crop;
        public float growthMultiplier = 1f; // Multiplier for growth speed
        public AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public void Plant(Crop currentCrop)
        {
            crop= currentCrop;
            state= CropState.Seedling;
            stage = crop.currStage;
            cropGO = Instantiate(currentCrop.cropStagesPrefabs[stage],transform.position, Quaternion.identity, transform);
            audioSource.clip = crop.plantSound;
            audioSource.Play(); 
            StartCoroutine(GrowCrop());
        }
        private IEnumerator GrowCrop()
        {
            while (stage < crop.cropStagesPrefabs.Length - 1)
            {
                float timer = 0f;
                float targetTime = crop.growthTime;

                while (timer < targetTime)
                {
                    // Increase timer based on multiplier and deltaTime
                    timer += Time.deltaTime * growthMultiplier;
                    yield return null;
                }

                // Move to next stage
                stage++;
                UpdateCropStage();
                if(stage == crop.cropStagesPrefabs.Length - 1) break;
            }

            state = CropState.ReadyToHarvest;
        }
        private void UpdateCropStage()
        {
            if (cropGO != null)
                Destroy(cropGO);

            cropGO = Instantiate(crop.cropStagesPrefabs[stage], transform.position, Quaternion.identity, transform);

            switch (stage)
            {
                case 1:
                    state = CropState.MidGrowth;
                    break;
                case 2:
                    state = CropState.ReadyToHarvest;
                    break;
            }
        }

        public void Harvest()
        {
            Destroy(cropGO);
            crop= null;
            state= CropState.Empty;
            stage = 0;
        }
        public enum CropState
        {
            Empty,
            Seedling,
            MidGrowth,
            FullGrowth,
            ReadyToHarvest
        }
    }
}

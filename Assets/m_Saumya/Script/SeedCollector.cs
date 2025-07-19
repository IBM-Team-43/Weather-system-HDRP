using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SeedCollector : MonoBehaviour
{
    public string seedName;
    public Sprite seedIcon;

    public TextMeshProUGUI promptText; 
    public Image heldSeedUI;

    private bool playerInRange = false;

    private void Start()
    {
        promptText.gameObject.SetActive(false);
        heldSeedUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            CollectSeed();  
        }
    }

private void CollectSeed()
{
    heldSeedUI.sprite = seedIcon;
    
    heldSeedUI.enabled = true;
    heldSeedUI.gameObject.SetActive(true);

    promptText.gameObject.SetActive(false); 
    Debug.Log($"Picked {seedName} seed!");
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            promptText.text = $"Press [E] to pick {seedName}";
            promptText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            promptText.gameObject.SetActive(false);
        }
    }
}

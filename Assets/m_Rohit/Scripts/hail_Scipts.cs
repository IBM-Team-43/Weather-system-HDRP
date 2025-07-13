using UnityEngine;

public class hail_Scripts : MonoBehaviour
{
    public GameObject hailstormEffect; // Assign the Hailstorm GameObject in Inspector

    void Update()
    {
        // Turn ON with R key
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (hailstormEffect != null)
            {
                hailstormEffect.SetActive(true);
            }
        }

        // Turn OFF with S key
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (hailstormEffect != null)
            {
                hailstormEffect.SetActive(false);
            }
        }
    }
    
    
}

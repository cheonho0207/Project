using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEffect4 : MonoBehaviour
{

    public GameObject sparkEffectPrefab;

    private GameObject sparkEffectInstance;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the spark effect at the position of the object
        if (sparkEffectPrefab != null)
        {
            sparkEffectInstance = Instantiate(sparkEffectPrefab, transform.position, Quaternion.Euler(189, 90, 0));
            sparkEffectInstance.SetActive(false); // Initially deactivate the effect
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sparkEffectInstance != null)
        {
            if (!sparkEffectInstance.activeSelf)
            {
                // Activate the spark effect
                sparkEffectInstance.SetActive(true);
            }

            // Optionally, you can update the position of the spark effect if needed
            sparkEffectInstance.transform.position = transform.position;
        }
    }
}

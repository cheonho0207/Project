using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Motion2_2 : MonoBehaviour
{
    public GameObject sparkEffect;
    public GameObject sparkEffect2;

    private Animation playerAnimation;
    private bool sparkEffectTriggered = false;

    void Start()
    {
      
    }

    void Update()
    {
        
    
    }


    public void TriggerSparkEffects()
    {
        
        // sparkEffect1 �� sparkEffect2�� �ߵ�
        if (sparkEffect != null)
        {
            Instantiate(sparkEffect, transform.position, transform.rotation);
        }

        if (sparkEffect2 != null)
        {
            Instantiate(sparkEffect2, transform.position, transform.rotation);
        }
    }
}

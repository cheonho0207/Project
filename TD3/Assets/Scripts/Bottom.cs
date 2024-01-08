using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottom : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            Application.LoadLevel(0);
        }
    }
}

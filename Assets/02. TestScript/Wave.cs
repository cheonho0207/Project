using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject[] enemyPrefabs;
    private int currentPrefabIndex = 0;

    public int count;
    public float rate;

    public void SpawnNextEnemy()
    {
        if (currentPrefabIndex < enemyPrefabs.Length) 
        {
            //Instantiate(enemyPrefabs[currentPrefabIndex], trans);
        }
    }
}

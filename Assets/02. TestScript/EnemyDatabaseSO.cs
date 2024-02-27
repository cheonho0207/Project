using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyDatabaseSO : ScriptableObject
{
    public List<EnemyData> enemysData;
}

[Serializable]
public class EnemyData
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int ID { get; private set; }

    [field: SerializeField]
    public float HP { get; private set; }

    [field: SerializeField]
    public float Damage { get; private set; }

    [field: SerializeField]
    public float MoveSpeed { get; private set; }

    [field: SerializeField]
    public GameObject Prefab { get; private set; }
}
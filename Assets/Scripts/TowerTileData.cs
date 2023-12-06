using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName ="Tower", menuName ="Test123/asd", order =int.MinValue)]
public class TowerTileData : ScriptableObject
{
    public int ID; //tower select nomber
    public string Name; //tower name
    public string Description; //tower description
    public Vector2Int Size; //tower setting size
    public GameObject Prefab;
}
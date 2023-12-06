using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField]
    private List<TowerTileData> m_TowerList = new List<TowerTileData>();
    public List<TowerTileData> Towers => m_TowerList;


}

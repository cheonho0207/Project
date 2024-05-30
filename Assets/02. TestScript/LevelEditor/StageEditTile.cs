using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEditTile : MonoBehaviour
{
    StageLevelManager.StageCell cell;


    public void SetCell(StageLevelManager.StageCell cell)
    {
        this.cell = cell;
    }

    public StageLevelManager.StageCell GetCell()
    {
        return cell;
    }

    public void EditType(StageLevelManager.StageCellType type, GameObject typeObj)
    {
        if (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        cell.type = type;
        if (typeObj != null)
        {
            GameObject obj = Instantiate(typeObj, transform);
        }
    }

}

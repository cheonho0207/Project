using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    public float speed = 5f;

    public void Move(List<Vector3> destinations)
    {
        var executeOnMoveFinishes = new List<System.Action>();

        for (var i = 0; i < destinations.Count; i++)
        {
            var a = i;
            executeOnMoveFinishes.Add(() =>
            {
                var moveParams = iTween.Hash("easetype", "linear", "speed", speed, "position", destinations[a],
                    "oncomplete", "moveComplete", "oncompletetarget", gameObject, "oncompleteparams", executeOnMoveFinishes);
                iTween.MoveTo(gameObject, moveParams);
            });
        }

        moveComplete(executeOnMoveFinishes);
    }

    void moveComplete(List<System.Action> actions)
    {
        if (actions.Count > 0)
        {
            actions[0]();
            actions.RemoveAt(0);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    [SerializeField] private TargetObject[] _targets;
    [SerializeField] private float _distanceAllow;

    private void Awake()
    {
        DraggableObject.OnFinishedDrag += DraggableObjectFinishedDrag;
    }

    ///Check if it near to the allow distance to one of the posible targets availables
    private void DraggableObjectFinishedDrag(DraggableObject obj)
    {
        TargetObject target;
        float distance;
        Vector3 objPos, targetPos;

        objPos = obj.transform.position;

        for (int i = 0; i < _targets.Length; i++)
        {
            target = _targets[i];

            targetPos = target.transform.position;

            if (target.Available)
            {
                distance = Mathf.Sqrt((Mathf.Pow(objPos.x - targetPos.x, 2) + Mathf.Pow(objPos.y - targetPos.y, 2)));

                if (distance <= _distanceAllow)
                {
                    target.Available = false;

                    obj.transform.position = targetPos;

                    obj.CanDrag = false;

                    return;
                }
            }
        }

        obj.BackOriginalPosition();
    }

    private void OnDestroy()
    {
        DraggableObject.OnFinishedDrag -= DraggableObjectFinishedDrag;
    }
}

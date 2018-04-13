using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    public static System.Action<GameController.Screen> OnNext;
    public static System.Action<bool> OnResult;

    [SerializeField] private TargetObject[] _targets;
    [SerializeField] private DraggableObject[] _dragables;
    [SerializeField] private float _distanceAllow;
    [SerializeField] private TextMeshProUGUI _textResult;
    [SerializeField] private float _delayToShowResult = 1;

    int result;
    int firstOperand;
    int secondOperand;
    int incorrectOperand;

    #region Private methods
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

                    CheckFinishOperation();

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

    private void GenerateOperation()
    {
        //Get first operand
        firstOperand = Random.Range(0, 10);

        //Get second operand
        secondOperand = Random.Range(0, 10);

        //Get result of the operation based on two operands
        result = firstOperand * secondOperand;

        Debug.Log("first operand: " + firstOperand + ", second: " + secondOperand + ", result: " + result);
    }

    private void LoadDragables()
    {
        int index = -1;
        DraggableObject obj;
        bool next = false;

        List<DraggableObject> dragablesToLoad = new List<DraggableObject>();

        //Make a copy of dragables to assign content
        for (int i = 0; i < _dragables.Length; i++)
        {
            _dragables[i].SetContentValue(-1);
            dragablesToLoad.Add(_dragables[i]);
        }

        incorrectOperand = -1;

        while (incorrectOperand == firstOperand || incorrectOperand == secondOperand || incorrectOperand == -1)
        {
            incorrectOperand = Random.Range(0, 10);
        }

        int[] operands = { firstOperand, secondOperand, incorrectOperand};
        int operandIndex = 0;

        while (dragablesToLoad.Count > 0)
        {
            next = false;

            while (!next)
            {
                index = Random.Range(0, dragablesToLoad.Count);

                obj = dragablesToLoad[index];

                if (obj.ContentValue == -1)
                {
                    obj.SetContentValue(operands[operandIndex]);

                    Debug.Log("Operand " + index + ", value: " + operands[operandIndex]);

                    operandIndex++;
                    next = true;
                    dragablesToLoad.Remove(obj);
                }
            }
        }
    }

    private void LoadResult()
    {
        _textResult.text = string.Format("{00:0}", result);
    }

    private void CheckFinishOperation()
    {
        int amount = 0;
        int currentResult = 1;
        for (int i = 0; i < _dragables.Length; i++)
        {
            if (!_dragables[i].CanDrag)
            {
                currentResult = currentResult * _dragables[i].ContentValue;

                amount++;

                if (amount == 2)
                {
                    CheckResult(currentResult);
                    return;
                }
            }
        }
    }

    private void CheckResult(int res)
    {
        string sResult = "<color=";

        if (result == res)
            sResult += "green";
        else
            sResult += "red";

        sResult += ">RESULT</color>";

        Debug.Log(sResult);

        OnResult(result == res);

        StartCoroutine(LoadScreenResult());
    }

    private IEnumerator LoadScreenResult()
    {
        yield return new WaitForSeconds(_delayToShowResult);

        OnNext(GameController.Screen.RESULT);
    }
    #endregion

    #region Public methods
    public void InitSession()
    {
        GenerateOperation();

        LoadDragables();

        LoadResult();
    }

   
    #endregion
}

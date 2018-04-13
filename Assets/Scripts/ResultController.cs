using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textResult;

    public void SetResult(bool result)
    {
        if (result)
            _textResult.text = "GANASTE!";
        else
            _textResult.text = "PERDISTE!";
    }
}
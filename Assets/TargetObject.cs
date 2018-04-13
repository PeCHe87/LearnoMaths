using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetObject : MonoBehaviour
{
    [SerializeField] private bool available = true;
    [SerializeField] private Image _imgBackground;
    [SerializeField] private int _correctNumber;
    [SerializeField] private bool _answer = false;

    public bool Available { get { return available; } set { available = value; } }
    public int CorrectNumber { get { return _correctNumber; } }
    public bool Answer { get { return _answer; } set { _answer = value; } }

    public void SetAnswer(Color col, int val)
    {
        _imgBackground.color = col;
        _correctNumber = val;
    }

    public void Init()
    {
        available = true;
        _correctNumber = -1;
        _answer = false;
    }
}

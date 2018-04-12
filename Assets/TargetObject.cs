using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour
{
    [SerializeField] private bool available = true;

    public bool Available { get { return available; } set { available = value; } }
}

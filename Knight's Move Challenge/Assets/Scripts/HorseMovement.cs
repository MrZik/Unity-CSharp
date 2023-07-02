using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseMovement : MonoBehaviour
{
    [field: SerializeField] public int CurrentIndex { get; private set; }

    public void UpdateIndex(int index)
    {
        CurrentIndex = index;
    }
}

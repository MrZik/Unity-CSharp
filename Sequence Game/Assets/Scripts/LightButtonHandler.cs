using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightButtonHandler : MonoBehaviour
{
    [SerializeField] private LightButton[] lightBtns;

    internal LightButton GetLightBtnAtIndex(int index)
    {
        return lightBtns[index];
    }

    internal void InitializeButtons()
    {
        for (int i = 0; i < lightBtns.Length; i++)
        {
            lightBtns[i].SetIndex(i);
        }
    }
}

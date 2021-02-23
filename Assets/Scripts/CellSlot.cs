using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSlot : MonoBehaviour
{
    [SerializeField]
    GameObject Top;

    [SerializeField]
    GameObject Bottom;

    [SerializeField]
    GameObject Left;

    [SerializeField]
    GameObject Right;

    [SerializeField]
    GameObject RedDot;


    public void DrawCell(bool top, bool left, bool bottom, bool right)
    {
        if (top)
        {
            Top.SetActive(false);
        }

        if (right)
        {
            Right.SetActive(false);
        }

        if (bottom)
        {
            Bottom.SetActive(false);
        }

        if (left)
        {
            Left.SetActive(false);
        }
    }

    public void toggleGuide(bool turnOn)
    {

        RedDot.SetActive(turnOn);
    }

}

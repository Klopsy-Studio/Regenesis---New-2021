using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public void SetHand()
    {
        GameCursor.instance.SetHandCursor();
    }

    public void SetRegular()
    {
        GameCursor.instance.SetRegularCursor();
    }
}

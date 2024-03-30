using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCursorFunctions : MonoBehaviour
{
    public void SetRegularCursor()
    {
        GameCursor.instance.SetRegularCursor();
    }

    public void SetHandCursor()
    {
        GameCursor.instance.SetHandCursor();
    }
}

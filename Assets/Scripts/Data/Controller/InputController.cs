using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputController : MonoBehaviour
{
    public static event EventHandler<InfoEventArgs<Point>> moveEvent;
    public static event EventHandler<InfoEventArgs<KeyCode>> selectEvent;
    public static event EventHandler<InfoEventArgs<KeyCode>> escapeEvent;
    public static event EventHandler<InfoEventArgs<KeyCode>> mouseConfirmEvent;
    public static event EventHandler<InfoEventArgs<KeyCode>> mouseCancelEvent;
    public static event EventHandler<InfoEventArgs<Point>> mouseSelectEvent;

    Repeater _hor = new Repeater("Horizontal");
    Repeater _ver = new Repeater("Vertical");
    //string[] _buttons = new string[] { "Fire1", "Fire2", "Fire3" };
    KeyCode mouseConfirmKey = KeyCode.Mouse0;
    KeyCode mouseCancelKey = KeyCode.Mouse1;
    KeyCode acceptKey = KeyCode.Space;
    KeyCode escapeKey = KeyCode.Escape;

    void Update()
    {
        //Debug.Log(Input.GetAxisRaw("Horizontal"));

        int x = _hor.Update();
        int y = _ver.Update();

     

        if (x != 0 || y != 0)
        {
            if (moveEvent != null)
                moveEvent(this, new InfoEventArgs<Point>(new Point(x, y)));
        }


        if (Input.GetAxis("Mouse X")!=0 || Input.GetAxis("Mouse Y") != 0)
        {
            if (mouseSelectEvent != null)
                mouseSelectEvent(this, new InfoEventArgs<Point>(new Point(x, y)));
        }

        //for (int i = 0; i < 3; i++)
        //{
        //    if (Input.GetButtonUp(_buttons[i]))
        //    {
        //        if(fireEvent != null)
        //        {
        //            fireEvent(this, new InfoEventArgs<int>(i));
        //        }
        //    }
        //}

        if (Input.GetKeyUp(acceptKey))
        {
            if (selectEvent != null)
            {
                selectEvent(this, new InfoEventArgs<KeyCode>());
            }
        }


        if (Input.GetKeyUp(escapeKey))
        {
            if(escapeEvent != null)
            {
                escapeEvent(this, new InfoEventArgs<KeyCode>());
            }
        }

        if (Input.GetKeyUp(mouseConfirmKey))
        {
            if (mouseConfirmEvent != null)
            {
                mouseConfirmEvent(this, new InfoEventArgs<KeyCode>());
            }
        }

        if (Input.GetKeyUp(mouseCancelKey))
        {
            if (mouseCancelEvent != null)
            {
                mouseCancelEvent(this, new InfoEventArgs<KeyCode>());
            }
        }

    }

   

    


}


class Repeater
{
    const float threshold = 0.5f; //Determine the amount of pause to wait between the initial press of the button and the point at which the input will begin repeating
    const float rate = 0.25f; //Determine the speed that the input will repeat
    float _next;
    bool _hold;
    string _axis;

    public Repeater (string axisName)
    {
        _axis = axisName;
    }

    public int Update()
    {
        int retValue = 0;
        int value = Mathf.RoundToInt(Input.GetAxisRaw(_axis));

        if(value != 0)
        {
            if (Time.time > _next)
            {
                retValue = value;
                _next = Time.time + (_hold ? rate : threshold);
                _hold = true;
            }
        }
        else
        {
            _hold = false;
            _next = 0;
        }

        return retValue;
    }
}

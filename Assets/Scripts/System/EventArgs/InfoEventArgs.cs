using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InfoEventArgs<T> : EventArgs //this class can hold any TYPE we want    
{
    public T info;

    public InfoEventArgs()
    {
        info = default(T);
    }

    public InfoEventArgs(T info)
    {
        this.info = info;
    }

}

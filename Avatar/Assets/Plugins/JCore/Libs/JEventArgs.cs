using UnityEngine;
using System.Collections;
using System;

public class StringEventArgs : EventArgs
{
    public string data;

    public StringEventArgs(string d)
    {
        data = d;
    }
}

public class FloatEventArgs : EventArgs
{
    public float data;

    public FloatEventArgs(float d)
    {
        data = d;
    }
}

using UnityEngine;
using System.Collections;

/// <summary>
/// A class to easily integrate new UI debug menus in the application
/// </summary>
public abstract class JDebuggerMenu
{
    //This flag is set by DebugUI
    public bool visible = false;

    public string title;

    public JDebuggerMenu(string name)
    {
        title = name;
    }

    /// <summary>
    /// Method called to display a menu section inside the vertical bar
    /// </summary>
    public abstract void Display();


    /// <summary>
    /// Method called after the vertical menu has been rendered
    /// This can be used for additional menu elements that are not inside the vertical bar
    /// </summary>
    public virtual void LateDisplay() { }
}
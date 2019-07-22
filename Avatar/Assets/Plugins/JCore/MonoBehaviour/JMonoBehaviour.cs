using UnityEngine;
using System.Collections;

/// <summary>
/// Base class containing useful helper methods for all mono behaviours
/// </summary>
public class JMonoBehaviour : MonoBehaviour
{
    /// <summary>
    /// Includes component type in logs to make it easier to spot the log source
    /// </summary>
    /// <param name="message"></param>
    protected void JLog(string message)
    {
        Debug.Log("[" + this.GetType().ToString() + "] " + message);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SC_AvatarSwitch : MonoBehaviour
{
    public GameObject[] avatars;
    public int defaultAvatar; // collision
    public int currentAvatar;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            currentAvatar = 0;
            SetAvatar(0);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            currentAvatar = 1;
            SetAvatar(1);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            currentAvatar = 2;
            SetAvatar(2);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            currentAvatar = 3;
            SetAvatar(3);
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            currentAvatar = 4;
            SetAvatar(4);
        }
    }


    /// <summary>
    /// Set current avatar
    /// </summary>
    /// <param name="keyRef"></param>
    public void SetAvatar(int keyRef)
    {
        if (avatars != null)
        {
            for (int i = 0; i < avatars.Length; i++)
            {
                if (i == keyRef)
                    avatars[i].SetActive(true);

                else
                    avatars[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Set the default avatar in Avatar settings (UI)
    /// </summary>
    /// <param name="toggleIdx"></param>
    public void SetDefaultAvatar(int toggleIdx)
    {
        if (avatars != null)
            defaultAvatar = toggleIdx;
    }
}

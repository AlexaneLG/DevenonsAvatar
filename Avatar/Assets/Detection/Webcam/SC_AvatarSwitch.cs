using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SC_AvatarSwitch : MonoBehaviour
{
    public GameObject[] avatars;
    public int defaultAvatar; // collision
    public int currentAvatar;
    public int avatarIndex = 0;

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

    public void SwitchAvatar()
    {
        ++avatarIndex;
        if (avatarIndex > avatars.Length-1)
        {
            avatarIndex = 0;
        }
        currentAvatar = avatarIndex;
        SetAvatar(avatarIndex);
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
                if (i == keyRef) {
                    if (i == 0) // cameraPlane
                    {
                        avatars[i].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                    }
                    else
                    {
                        avatars[i].SetActive(true);
                    }
                }
                else {
                    if (i == 0) // cameraPlane
                    {
                        avatars[i].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                    } else
                    {
                        avatars[i].SetActive(false);
                    }
                }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextualUIController : MonoBehaviour
{

    public DataManager dataManager;

    private bool _canDisplay;

    public bool CanDisplay
    {
        get
        {
            return _canDisplay;
        }

        set
        {
            _canDisplay = value;
        }
    }

    private void Awake()
    {
        // Disable all childs
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }


    }

    // Use this for initialization
    void Start()
    {
        // Get Data Manager
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();

        _canDisplay = false;

    }

    // Update is called once per frame
    void Update()
    {
        // Warning : The contextual layer is based on the order of the scene's scenario items
        if (_canDisplay)
        {
            switch (dataManager.CurrentScenarioItemIdx)
            {
                case 0:
                    // Start_Avatar : Environnement_Avatar
                    //DisplayLayer(0, true);
                    break;
                case 1:
                    // ProjectUser_Avatar
                    DisplayLayer(0, false);
                    DisplayLayer(1, true);
                    break;
                case 2:
                    // TakeOff_Avatar
                    DisplayLayer(1, false);
                    DisplayLayer(2, true);
                    break;
                case 3:
                    // Dive_Avatar
                    DisplayLayer(2, false);
                    DisplayLayer(3, true);
                    break;
                case 4:
                    // FreeFly_Avatar_1

                    DisplayLayer(4, true);
                    break;
                case 5:
                    // Canyon_Avatar
                    DisplayLayer(3, false);
                    DisplayLayer(4, false);
                    DisplayLayer(5, true);
                    break;
                case 6:
                    // FreeFly_Avatar_2
                    DisplayLayer(5, false);
                    break;
                case 7:
                    // Storm_Avatar
                    DisplayLayer(6, true);
                    break;
                case 8:
                    // FreeFly_Avatar_3
                    DisplayLayer(6, false);
                    break;
                case 9:
                    // Freeze_Avatar ; not currently enabled in scenario
                    break;
                case 10:
                    // End_Avatar
                    DisplayLayer(7, true);
                    break;
                default:
                    break;
            }
            _canDisplay = false;
        }

    }

    private void DisplayLayer(int index, bool isDisplayed)
    {
        transform.GetChild(index).gameObject.SetActive(isDisplayed);
    }

}

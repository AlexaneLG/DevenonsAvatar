using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLayer : MonoBehaviour {

    //public GameObject collisionTexts;

    private int _collisionNumber;
    private bool _canDisplay; // display a layer once
    private bool _isDisplaying; // layer currently displayed

	// Use this for initialization
	void Start () {
        _collisionNumber = 0;
        _canDisplay = true;
        _isDisplaying = false;

        /*if (collisionTexts == null)
        {
            collisionTexts = GameObject.FindGameObjectWithTag("CollisionTexts");
        }*/

        // Hide all texts
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (_canDisplay && _isDisplaying == false && _collisionNumber <= 3 && _collisionNumber > 0)
        {
            Debug.Log("Collision n°" + _collisionNumber);
            switch (_collisionNumber)
            {
                case 1:
                    StartCoroutine(DisplayText(0));
                    break;
                case 2:
                    StartCoroutine(DisplayText(2));
                    break;
                case 3:
                    StartCoroutine(DisplayText(3));
                    break;
                default:
                    break;
            }
        }
        
	}

    public void DisplayCollisionLayer()
    {
        if (!_isDisplaying)
        {
            ++_collisionNumber;
            _canDisplay = true;
        }
    }

    IEnumerator DisplayText(int textIndex)
    {
        _canDisplay = false;
        _isDisplaying = true;

        SetActiveText(textIndex, true);
        yield return new WaitForSeconds(4);
        SetActiveText(textIndex, false);

        if (textIndex == 0)
        {
            SetActiveText(1, true);
            yield return new WaitForSeconds(4);
            SetActiveText(1, false);
        }

        _isDisplaying = false;
    }

    private void SetActiveText(int index, bool display)
    {
        transform.GetChild(index).gameObject.SetActive(display);
    }
}

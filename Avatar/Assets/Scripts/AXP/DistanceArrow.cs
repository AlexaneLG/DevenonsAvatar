using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceArrow : MonoBehaviour
{
    public GameObject arrowObj;
    public float distance;
    public Transform leftHit;
    public Transform rightHit;
    public GameObject arrowLeft;
    public GameObject arrowRight;
    public GameObject[] dots;
    public Transform positionReference;

    public void Init()
    {
        arrowObj = Resources.Load("arrow") as GameObject;
        arrowLeft = Instantiate(arrowObj);
        arrowRight = Instantiate(arrowObj);
        arrowLeft.name = "ArrowLeft";
        arrowRight.name = "ArrowRight";
        int nDots = 5;
        dots = new GameObject[nDots];
        for (int i = 0; i < nDots; ++i)
        {
            dots[i] = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        }
    }

    public void SetDistance(float d)
    {
        distance = d;
    }

    public void SetLeftHit(Transform leftH)
    {
        leftHit = leftH;
    }

    public void SetRightHit(Transform rightH)
    {
        rightHit = rightH;
    }

    public void SetPositionReference(Transform t)
    {
        positionReference = t;
    }

    public void SetArrowsPosition()
    {
        arrowRight.transform.position = rightHit.position;
        arrowLeft.transform.position = leftHit.position;
    }

    public void SetDotsPosition()
    {
        float deltaD = distance / dots.Length;
        Debug.Log("Distance : " + distance);
        for (int i = 0; i < dots.Length; ++i)
        {
            dots[i].transform.position = rightHit.position + rightHit.forward * deltaD * (i + 1);
        }
    }
}

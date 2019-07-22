using UnityEngine;
using System.Collections;

public class SpaceController_EXT : AvatarFlightController
{

    public Transform leftHand, rightHand, hipCenter;

    [HideInInspector]
    public Vector3 hipToRightHandVector;
    public Vector3 hipToLeftHandVector;

    private Rigidbody myRigidbody;
    private Transform myTransform;

    private bool isRaisingRightArm = false;
    private bool isRaisingLeftArm = false;
    private bool isRaisingBothArms = false;

    public GameObject astronaute;
    public AudioSource propulseurSound;

    public GameObject propLeftUp;
    public GameObject propLeftBottom;
    public GameObject propRightUp;
    public GameObject propRightBottom;

    public Transform propulsor;
    public Transform spinRef;

    public float forwardForce = 5f;
    public float rotationForce = 0.08f;

    public float maxVel = 200f;
    public float maxAngVel = 5f;

    private float myMaxAngularVelocity;

    public override void Start()
    {
        base.Start();
        myRigidbody = GetComponent<Rigidbody>();
        myTransform = transform;

        // Coupe les propulseurs
        propRightUp.SetActive(false);
        propRightBottom.SetActive(false);
        propLeftUp.SetActive(false);
        propLeftBottom.SetActive(false);
    }

    public void Update()
    {
        if (KinectManager.Instance != null)
        {
            if (KinectManager.Instance.GetUsersCount() > 0)
            {
                // Positions des articulations
                Vector3 rightHandPos = rightHand.position;
                Vector3 leftHandPos = leftHand.position;
                Vector3 hipPos = hipCenter.position;


                hipToRightHandVector = rightHandPos - hipPos;
                hipToLeftHandVector = leftHandPos - hipPos;
                hipToLeftHandVector.Normalize();
                hipToRightHandVector.Normalize();


                // Calcul de la hauteur des bras
                if (hipToLeftHandVector.y > 0.3f)
                {
                    propLeftUp.SetActive(true);
                    propLeftBottom.SetActive(true);

                    isRaisingLeftArm = true;
                }
                else
                {
                    propLeftUp.SetActive(false);
                    propLeftBottom.SetActive(false);

                    isRaisingLeftArm = false;
                }

                if (hipToRightHandVector.y > 0.3f)
                {
                    propRightUp.SetActive(true);
                    propRightBottom.SetActive(true);

                    isRaisingRightArm = true;
                }
                else
                {
                    propRightUp.SetActive(false);
                    propRightBottom.SetActive(false);

                    isRaisingRightArm = false;
                }

                if (isRaisingLeftArm == true && isRaisingRightArm == true)
                {
                    propRightUp.SetActive(true);
                    propRightBottom.SetActive(true);
                    propLeftUp.SetActive(true);
                    propLeftBottom.SetActive(true);

                    isRaisingBothArms = true;
                }

                // Play sound if needed

                if ((isRaisingLeftArm || isRaisingRightArm) && propulseurSound.isPlaying == false)
                    propulseurSound.Play();

                if (!isRaisingLeftArm && !isRaisingRightArm && propulseurSound.isPlaying == true)
                    propulseurSound.Stop();



                // Max angular velocity
                myMaxAngularVelocity = myRigidbody.maxAngularVelocity;
                myMaxAngularVelocity = maxAngVel;
                myRigidbody.maxAngularVelocity = myMaxAngularVelocity;

                // Max velocity
                if (GetComponent<Rigidbody>().velocity.sqrMagnitude > maxVel)
                {
                    GetComponent<Rigidbody>().velocity *= 0.99f;
                }

                // Add Force sur le personnage en fonction de quel bras est levé
                if (isRaisingLeftArm)
                {
                    myRigidbody.AddRelativeTorque(Vector3.up * rotationForce, ForceMode.Acceleration);
                }

                if (isRaisingRightArm)
                {
                    myRigidbody.AddRelativeTorque(Vector3.up * rotationForce * -1, ForceMode.Acceleration);
                }

                if (isRaisingBothArms)
                {
                    myRigidbody.AddForce(myRigidbody.transform.forward * forwardForce, ForceMode.Acceleration);
                }
            }
        }
        propulsor.position = spinRef.position;
        propulsor.rotation = spinRef.rotation;
    }

    public void stopPropulsorSound()
    {
        if (propulseurSound.isPlaying == true)
            propulseurSound.Stop();
    }

    public void adjustForwardForce(float newForwardForce)
    {
        forwardForce = newForwardForce;
    }

    public void adjustRotationForce(float newRotationForce)
    {
        rotationForce = newRotationForce;
    }
}

using UnityEngine;
using System.Collections;

public class WiiController : MonoBehaviour
{

    public WiiBBSocket wiiBBSocket;

    public float verticalSensitivity = 0.01f;
    public float horizontalSensitivity = 0.01f;

    public Transform pointManHead;

    public Transform board;

    public static bool enableMovement;

    public float baseSpeed;
    public float speedMultiplier;

    public float rotateSpeedMultiplier;
    public float maxRotationSpeed;
    float boardRotation;

    public static int directionSwitch = 1;

    private float currentHorizontalRotationSpeed;
    private float currentVerticalRotationSpeed;

    private float heightVariator = 1;

    private bool enableCalDistance = false;
    private bool enableCalStart = true;

    public float maxAltitude = 300f;
    public float minAltitude = 100f;

    private bool userCalibration = false;
    private float initialHeadPosition;

    float perc = 0;

    void Start()
    {
        boardRotation = board.localEulerAngles.z;
    }

    void Update()
    {
        ConstrainAltitude();

        if (KinectManager.Instance != null)
        {
            if (KinectManager.Instance.GetUsersCount() > 0 && FreeFly_Wii.enableFreeFly)
            {
                if (!userCalibration && pointManHead.position.y != 0)
                {
                    initialHeadPosition = pointManHead.position.y;
                    userCalibration = true;
                }

                if (heightVariator < 0.95f && !enableMovement)
                {
                    enableMovement = true;
                }
            }

            if (initialHeadPosition > 0)
            {
                heightVariator = pointManHead.position.y / initialHeadPosition;
            }

            if (heightVariator > 1)
                heightVariator = 1;
        }

        if (enableMovement)
        {


            if (pointManHead == null)
                transform.Translate(Vector3.forward * baseSpeed * 0.1F * Time.deltaTime);

            transform.Translate(Vector3.forward * ((1 + (1 - heightVariator) * speedMultiplier) * baseSpeed) * Time.deltaTime);

            float xcp = wiiBBSocket.X_CP;
            float ycp = wiiBBSocket.Y_CP;


            if (currentVerticalRotationSpeed < maxRotationSpeed)
                currentVerticalRotationSpeed += 0.4f * rotateSpeedMultiplier * Time.deltaTime;

            if (currentHorizontalRotationSpeed < maxRotationSpeed)
                currentHorizontalRotationSpeed += 0.8f * rotateSpeedMultiplier * Time.deltaTime;


            transform.Rotate(verticalSensitivity * xcp * Vector3.right * currentVerticalRotationSpeed * Time.deltaTime);

            transform.Rotate(horizontalSensitivity * ycp * Vector3.down * currentHorizontalRotationSpeed * Time.deltaTime * directionSwitch, Space.World);

            if (board.localEulerAngles.z < 190)
                boardRotation += 0.2f;

            if (board.localEulerAngles.z > 170)
                boardRotation -= 0.2f;

            board.localEulerAngles = new Vector3(0, 0, boardRotation);


            if (transform.localEulerAngles.x <= 290 && transform.localEulerAngles.x > 180)
                transform.localEulerAngles = new Vector3(290, transform.localEulerAngles.y, transform.localEulerAngles.z);

            else if (transform.localEulerAngles.x >= 70 && transform.localEulerAngles.x < 180)
                transform.localEulerAngles = new Vector3(70, transform.localEulerAngles.y, transform.localEulerAngles.z);

            /* 
                        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
                        {
                            if (currentVerticalRotationSpeed < maxRotationSpeed)
                                currentVerticalRotationSpeed += 0.4f * rotateSpeedMultiplier * Time.deltaTime;

                            if (currentHorizontalRotationSpeed < maxRotationSpeed)
                                currentHorizontalRotationSpeed += 0.8f * rotateSpeedMultiplier * Time.deltaTime;


                            if (Input.GetKey(KeyCode.UpArrow))
                                transform.Rotate(Vector3.right * currentVerticalRotationSpeed * Time.deltaTime);    

                            else if (Input.GetKey(KeyCode.DownArrow))
                                transform.Rotate(Vector3.left * currentVerticalRotationSpeed * Time.deltaTime);


                            if (Input.GetKey(KeyCode.LeftArrow))
                            {
                                transform.Rotate(Vector3.down * currentHorizontalRotationSpeed * Time.deltaTime * directionSwitch, Space.World);

                                if (board.localEulerAngles.z < 190)
                                    boardRotation += 0.2f;

                                board.localEulerAngles = new Vector3(0, 0, boardRotation);
                            }

                            else if (Input.GetKey(KeyCode.RightArrow))
                            {
                                transform.Rotate(Vector3.up * currentHorizontalRotationSpeed * Time.deltaTime * directionSwitch, Space.World);

                                if (board.localEulerAngles.z > 170)
                                    boardRotation -= 0.2f;

                                board.localEulerAngles = new Vector3(0, 0, boardRotation);
                            }

                            if (transform.localEulerAngles.x <= 290 && transform.localEulerAngles.x > 180)
                                transform.localEulerAngles = new Vector3(290, transform.localEulerAngles.y, transform.localEulerAngles.z);

                            else  if (transform.localEulerAngles.x >= 70 && transform.localEulerAngles.x < 180)
                                transform.localEulerAngles = new Vector3(70, transform.localEulerAngles.y, transform.localEulerAngles.z);
                        }
                        else
                        {
                            currentVerticalRotationSpeed = 0;
                            currentHorizontalRotationSpeed = 0;

                            if (board.localEulerAngles.z < 180)
                            {
                                boardRotation += 0.5f;

                                if (board.localEulerAngles.z > 179)
                                    boardRotation = 180;
                            }

                            else if (board.localEulerAngles.z > 180)
                            {
                                boardRotation -= 0.5f;

                                if (board.localEulerAngles.z < 181)
                                    boardRotation = 180;
                            }

                            board.localEulerAngles = new Vector3(0, 0, boardRotation);
                        }
                        */
        }
    }

    void ConstrainAltitude()
    {
        if (transform.localPosition.y > maxAltitude)
            transform.localPosition = new Vector3(transform.localPosition.x, maxAltitude, transform.localPosition.z);

        else if (transform.localPosition.y <= minAltitude)
            transform.localPosition = new Vector3(transform.localPosition.x, minAltitude, transform.localPosition.z);
    }

    public void AdjustBaseSpeed(float newBaseSpeed)
    {
        baseSpeed = newBaseSpeed;
    }

    public void AdjustSpeedMultiplier(float newSpeedMultiplier)
    {
        speedMultiplier = newSpeedMultiplier;
    }
}
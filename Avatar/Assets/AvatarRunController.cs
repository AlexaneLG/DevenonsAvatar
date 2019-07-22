using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;

public class AvatarRunController : MonoBehaviour
{

    // Use this for initialization
    public bool autorun = false;

    [Range(0f, 10f)]
    public float autorunSpeed = 4;

    [Range(0f, 10f)]
    public float speed = 0.0f;

    [Range(1f, 10f)]
    public float maxSpeed = 5;


    [Range(0.1f, 5f)]
    public float sideChangeSpeed = 1f;

    [Range(0.1f, 0.9999f)]
    public float speedDecay = 0.9875f;


    [Range(0.25f, 10f)]
    public float minspeed = 0.75f;

    [Range(0.1f, 1f)]
    public float maxDelay = 0.75f;

    [Range(0f, 5f)]
    public float min_mean_cp = 0.5f;

    public Transform avatar;

    public bool followTerrain = true;

    public float m_GroundCheckDistance = 0.25f;

    splineMove _splineMove;
    public LayerMask layerMask = 0;

    public WiiBBSocket wii;

    public TeaSocket tea;

    AvatarPathManager _pathManager;

    float[] last_cps = new float[10];
    int cp_index = 0;

    float last_mean_cp = 0;

    public float angleCorrection = -180;
    public float debugCorrectedAngle;

    public AvatarPathManager.Turn debugTurn;

    float _delay = 0;

    private bool runStarted = false;

    float mean_cp
    {
        get
        {
            float sum = 0;
            int count = last_cps.Length;
            for (int i = 0; i < count; i++)
            {
                sum += last_cps[i];
            }
            return sum / count;
        }
    }

    void Start()
    {
        _splineMove = GetComponent<splineMove>();
        _pathManager = GetComponent<AvatarPathManager>();
        _pathManager.turn = AvatarPathManager.Turn.Forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_pathManager.randomTurn)
        {
            float a = tea.motion1.localRotation.eulerAngles.y + angleCorrection;

            debugCorrectedAngle = a;

            if (a < -5.0f)
            {
                _pathManager.turn = AvatarPathManager.Turn.Left;
            }
            else
            if (a > 5.0f)
            {
                _pathManager.turn = AvatarPathManager.Turn.Right;
            }
            else
            {
                _pathManager.turn = AvatarPathManager.Turn.Forward;
            }
            debugTurn = _pathManager.turn;
        }

        if (autorun)
        {
            _splineMove.ChangeSpeed(autorunSpeed);
            return;
        }

        last_cps[cp_index] = wii.X_CP;
        cp_index = (cp_index + 1) % last_cps.Length;


        float mean = mean_cp;
        //Debug.Log("Mean cp = " + mean);

        if (last_mean_cp * mean < 0 && Mathf.Abs(mean) > min_mean_cp)
        {
            runStarted = true;
            // change side occured
            speed += sideChangeSpeed;
            speed = Mathf.Min(speed, maxSpeed);

            //Debug.Log("Side Change");
            _delay = 0;
        }
        else
        {
            if (runStarted)
            {
                _delay += Time.deltaTime;
                speed = speedDecay * speed;
                speed = Mathf.Max(speed, minspeed);
                /*
                    if (_delay > maxDelay)
                    {
                        speed = minspeed;
                    }
                */
            }
        }

        last_mean_cp = mean;
        _splineMove.ChangeSpeed(speed);


    }
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    /// </summary>
    void LateUpdate()
    {
        if (followTerrain)
        {

            Vector3 startPosition = avatar.position + (Vector3.up * 1f);
            Vector3 ray = Vector3.down * m_GroundCheckDistance;

            RaycastHit hitInfo;

            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if (Physics.Raycast(startPosition, ray, out hitInfo, m_GroundCheckDistance, layerMask))
            {
                var p = avatar.position;
                p.y = hitInfo.point.y;
                avatar.position = p;
#if UNITY_EDITOR
                // helper to visualise the ground check ray in the scene view
                Debug.DrawRay(startPosition, ray, Color.green);
#endif
            }
            else
            {
#if UNITY_EDITOR
                // helper to visualise the ground check ray in the scene view
                Debug.DrawRay(startPosition, ray, Color.red);
#endif
            }
        }
    }
}

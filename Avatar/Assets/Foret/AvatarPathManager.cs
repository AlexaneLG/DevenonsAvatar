using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SWS;
public class AvatarPathManager : MonoBehaviour
{

    public enum Turn
    {
        Left,
        Right,
        Forward,

    };

    PathManager _startPath = null;
    splineMove _splineMove = null;

    public bool randomTurn = false;

    public Turn turn = Turn.Left;

    public Start_Foret start_Foret;

    // Use this for initialization
    void Start()
    {

        var c = GetComponent<CharacterControllerBasedOnAxis>();
        c.StartScenario();

        _splineMove = GetComponent<splineMove>();
        _startPath = _splineMove.pathContainer;

        _splineMove.onMoveDone = () =>
        {
            var pm = _splineMove.pathContainer;
            var connected = pm.GetComponent<ConnectedPath>();

            if (connected == null)
            {
                //endOfPath();
            }
            else
            {
                _splineMove.moveToPath = true;
                PathManager nextPath = null;

                if (randomTurn)
                {
                    turn = (Turn)Random.Range(0, connected.front ? 3 : 2);
                }

                switch (turn)
                {
                    case Turn.Left:
                        nextPath = connected.left;
                        break;
                    case Turn.Right:
                        nextPath = connected.right;
                        break;
                    case Turn.Forward:
                        nextPath = connected.front;
                        break;
                }

                if (nextPath == null)
                {
                    switch (turn)
                    {
                        case Turn.Left:
                            nextPath = connected.front;
                            break;
                        case Turn.Right:
                            nextPath = connected.front;
                            break;
                        case Turn.Forward:
                            nextPath = connected.left ? connected.left : connected.right;
                            break;
                    }
                }

                Debug.Log("Turn : " + turn);
                Debug.Log("Next path : " + nextPath);
                _splineMove.SetPath(nextPath);

                if (nextPath.GetComponent<ConnectedPath>() == null)
                {
                    int count = _splineMove.events.Count;
                    _splineMove.events[1].AddListener(() =>
                    {
                        endOfPath();
                    });
                }
            }
        };

    }

    void endOfPath()
    {
        Debug.Log("endOfPath");

        start_Foret.ActivateNextScenarioItem();
    }

    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        _splineMove.Stop();
    }

}

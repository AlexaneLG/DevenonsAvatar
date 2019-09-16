using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class CharacterControllerBasedOnAxis : MonoBehaviour
{
    //public PropertyAxis speed;
    public Transform self;
    public float maxBascule;

    public float speed = 300;
    public float startAltitude;
    public float altitude = 100f;

    //public float taperFactor = 0.15f;

    public float armRegularRotationSpeed = 5f;
    public float armSlowRotationSpeed = 2f;

    public LayerMask layerMask;
    public TerrainGenerator terrain;

    [Range(-1f, 1f)]
    public float direction = 0.0f;
    public float verticalDirection = 0.0f;
    public float flightMaxAngle = 25f;

    public float worldLimits = 5000f;

    public Transform avatar;

    public Transform pointManHead, pointManRightAnkle, pointManLeftAnkle;
    public Transform[] avatars3D;
    public float[] avatarScaleFactor;

    public ScenarioItem[] ScenarioItems;
    public static int currentScenarioItem = -1;

    public bool scenarioRunning
    {
        get
        {
            return currentScenarioItem >= 0;
        }
    }

    public List<Transform> objectsToReplace = new List<Transform>();

    public static CharacterControllerBasedOnAxis instance;

    void Awake()
    {
        instance = this;

        currentScenarioItem = -1;

        startAltitude = transform.position.y;

        foreach (var item in ScenarioItems)
        {
            item.gameObject.SetActive(false);
        }

    }

    void Start()
    {

    }

    public void TriggerDataManager()
    {
        if (GameObject.FindGameObjectWithTag("DataManager") != null)
        {
            GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>().SetScenarioItemOnChange(ScenarioItems[currentScenarioItem], currentScenarioItem);
        }
    }

    public void StartScenario()
    {
        currentScenarioItem = 0;
        ScenarioItems[currentScenarioItem].gameObject.SetActive(true);

        TriggerDataManager();
    }

    public void ActivateNextScenarioItem()
    {
        if (currentScenarioItem < ScenarioItems.Length)
        {
            //Debug.Log("ID Scenario item : " + currentScenarioItem);
            ScenarioItems[currentScenarioItem].gameObject.SetActive(false);
            currentScenarioItem += 1;

            if (ScenarioItems.Length > currentScenarioItem)
            {
                if (ScenarioItems[currentScenarioItem].EnabledInScenario)
                {
                    ScenarioItems[currentScenarioItem].gameObject.SetActive(true);
                    TriggerDataManager();
                }
                else
                {
                    ActivateNextScenarioItem();
                }
            }
            else
            {
                // End of Scenario 

                SensorRecorderManager.instance.endRecording();
            }
        }
    }

    void ConstraintToWordLimits()
    {
        Transform t = this.transform;
        Vector3 p = t.localPosition;

        float dx = 0f, dz = 0f;


        if (p.x > worldLimits)
        {
            p.x -= worldLimits;
            dx = -worldLimits;
        }

        if (p.x < -worldLimits)
        {
            p.x += worldLimits;
            dx = worldLimits;

        }

        if (p.z > worldLimits)
        {
            p.z -= worldLimits;
            dz = -worldLimits;
        }

        if (p.z < -worldLimits)
        {
            p.z += worldLimits;
            dz = worldLimits;
        }

        t.localPosition = p;

        if (dx != 0f || dz != 0f)
        {
            foreach (var item in objectsToReplace)
            {
                p = item.position;
                p.x += dx;
                p.z += dz;
                item.position = p;
            }

            terrain.ReplaceTiles(dx, dz);
        }
    }

    void Update()
    {
        ConstraintToWordLimits();
    }

    public void AdjustAvatarScale()
    {
        Vector3 pointManBase = Vector3.Lerp(pointManRightAnkle.localPosition, pointManLeftAnkle.localPosition, 0.5f);

        for (int i = 0; i < avatars3D.Length; i++)
        {
            float avatarHeight = Mathf.Clamp((Vector3.Distance(pointManBase, pointManHead.localPosition)) * avatarScaleFactor[i], 0.5f, 2f);
            avatars3D[i].localScale = new Vector3(avatarHeight, avatarHeight, avatarHeight);
            Debug.Log("i : " + i + ", name : " + avatars3D[i].gameObject.name + ", avatarHeight : " + avatarHeight + ", avatars3D[i].localScale : " + avatars3D[i].localScale);
        }
    }


    #region GUI Functions

    public void AdjustMaxBascule(float newMaxBascule)
    {
        maxBascule = newMaxBascule;
    }

    public void AdjustArmRegularRotationSpeed(float newArmRegularRotationSpeed)
    {
        armRegularRotationSpeed = newArmRegularRotationSpeed;
    }

    public void AdjustArmSlowRotationSpeed(float newArmSlowRotationSpeed)
    {
        armSlowRotationSpeed = newArmSlowRotationSpeed;
    }

    public void AdjustFlightMaxAngle(float newFlightMaxAngle)
    {
        flightMaxAngle = newFlightMaxAngle;
    }

    public void AdjustAltitude(float newAltitude)
    {
        altitude = newAltitude;
    }

    public void AdjustSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    #endregion
}

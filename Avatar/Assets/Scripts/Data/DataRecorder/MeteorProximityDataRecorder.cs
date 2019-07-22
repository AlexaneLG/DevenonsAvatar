using UnityEngine;
using System.Collections;

public class MeteorProximityDataRecorder : MonoBehaviour {

    public MeteorProximityDataCollection dataCollection = new MeteorProximityDataCollection();

    public Transform characterTransform;
    public GameObject stormScenarioItem;
    public GameObject[] meteorsTab;

    private float meteorDistance_1, meteorDistance_2, meteorDistance_3 = -1;
    private int meteorIndex = 0;

    void FixedUpdate()
    {
        if (SensorRecorderManager.startRecordingSensorData)
        {
            if (characterTransform != null && stormScenarioItem.activeInHierarchy == true)
            {
                meteorsTab = GameObject.FindGameObjectsWithTag("Meteor");

                for (int i = 0; i < meteorsTab.Length; i++)
                {
                    float meteorDistance = Vector3.Distance(characterTransform.position, meteorsTab[i].transform.position);
                    meteorDistance = Mathf.Round(meteorDistance * 100f) / 100f;

                    if(i == 0)
                    {
                        dataCollection.meteorProximity_1.AddRecordedValue(meteorDistance);
                    }
                    else if (i == 1)
                    {
                        dataCollection.meteorProximity_2.AddRecordedValue(meteorDistance);
                    }
                    else if (i == 2)
                    {
                        dataCollection.meteorProximity_3.AddRecordedValue(meteorDistance);
                    }
                }
            }
            else
            {
                dataCollection.meteorProximity_1.AddRecordedValue(-1);
                dataCollection.meteorProximity_2.AddRecordedValue(-1);
                dataCollection.meteorProximity_3.AddRecordedValue(-1);
            }
        }
    }
}

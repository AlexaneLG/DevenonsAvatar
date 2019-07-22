using UnityEngine;
using System.Collections;

public class ScenarioItemDataCollection : SensorDataCollection
{
    public SensorGeneric<int> ScenarioItem = new SensorGeneric<int>("Scenario");


    public ScenarioItemDataCollection()
        : base("Scenario")
    {
        AddSensor(ScenarioItem);
    }
}



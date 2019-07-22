using System.Collections;
using System.Collections.Generic;



public class EHealthSensorDataCollection : SensorDataCollection {

    public SensorGeneric<float> ECG = new SensorGeneric<float>("ECG");
    public SensorGeneric<float> airFlow = new SensorGeneric<float>("AirFlow");
    public SensorGeneric<float> skinVoltage = new SensorGeneric<float>("Skin Voltage");
    public SensorGeneric<float> raw_ECG = new SensorGeneric<float>("raw_ECG");
    public SensorGeneric<float> BPM = new SensorGeneric<float>("BPM");
    public SensorGeneric<float> IBI = new SensorGeneric<float>("IBI");

	public EHealthSensorDataCollection() :  base("EHealth")
	{
        AddSensor(ECG);
        AddSensor(airFlow);
        AddSensor(skinVoltage);
        AddSensor(raw_ECG);
        AddSensor(BPM);
        AddSensor(IBI);
	}
}

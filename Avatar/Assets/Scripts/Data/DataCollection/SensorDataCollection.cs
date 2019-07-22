using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;




public class SensorDataCollection {
	public string name;

	protected List<Sensor> _values = new List<Sensor>();

	public SensorDataCollection(string collectionName)
    {
		name = collectionName;
	}

	public void AddSensor(Sensor s)
	{
		_values.Add(s);
	}


	public void ClearRecordings()
	{
		foreach(Sensor k in _values)
		{
			k.ClearRecordings();
		}
	}

    public void DumpRecordings(StreamWriter writer)
    {
        foreach (Sensor k in _values)
        {
            k.DumpRecordings(writer);
        }
    }

    public void DumpColumnName(StreamWriter writer)
    {
        foreach (Sensor k in _values)
        {
            k.DumpColumnName(writer);
        }
    }

    public void DumpNextValue(StreamWriter writer)
    {
        foreach (Sensor k in _values)
        {
            k.DumpNextRecording(writer);
        }
    }

}

public class Sensor {
    public string sensorName;
	
    public Sensor(string name)
    {
        sensorName = name;
    }

    virtual public void ClearRecordings()
    {
        throw (new System.Exception("ClearRecordings virtual should be overiden"));
        
    }

    virtual public void DumpRecordings(StreamWriter writer)
    {
        throw (new System.Exception("DumpRecordings virtual should be overiden"));
    }

    virtual public void DumpColumnName(StreamWriter writer)
    {
        throw (new System.Exception("DumpRecordings virtual should be overiden"));
    }

    virtual public void DumpNextRecording(StreamWriter writer)
    {
        throw (new System.Exception("DumpRecordings virtual should be overiden"));
    }


};



public class SensorGeneric<T> : Sensor {
	public List<T> values = new List<T>();

    public int maxDataIndex;
	public T lastValue;
	public float lastUpdateTime;

    public string defaultValue = "0.0";

    int dump_recordingIndex = 0; 

    public SensorGeneric(string name) : base(name)
    {
		
	}

	public void AddRecordedValue(T value){
		lastValue = value;
		values.Add(value);
	}

    public void AddRecordedValues256(T[] vals)
    {
       for(int i = 0;i<vals.Length;i++)
       {
           values.Add(vals[i]);
       }
       lastValue = vals[vals.Length-1];
		
    }

    public void AddRecordedValues128(T[] vals)
    {
        for (int i = 0; i < vals.Length; i++)
        {
            // Add 2 times the value to be @ 256Hz
            values.Add(vals[i]);
            values.Add(vals[i]);
        }
        lastValue = vals[vals.Length - 1];

    }


    public void AddRecordedValues64(T[] vals)
    {
        for (int i = 0; i < vals.Length; i++)
        {
            // Add 8 times the value to be @ 256Hz
            values.Add(vals[i]);
            values.Add(vals[i]);
            values.Add(vals[i]);
            values.Add(vals[i]);
        }
        lastValue = vals[vals.Length - 1];

    }

    public void AddRecordedValues32(T[] vals)
    {
        for (int i = 0; i < vals.Length; i++)
        {
            // Add 8 times the value to be @ 256Hz
            values.Add(vals[i]);
            values.Add(vals[i]);
            values.Add(vals[i]);
            values.Add(vals[i]);
            values.Add(vals[i]);
            values.Add(vals[i]);
            values.Add(vals[i]);
            values.Add(vals[i]);

        }
        lastValue = vals[vals.Length - 1];

    }


	override public void ClearRecordings()
	{
		values.Clear();
        dump_recordingIndex = 0;

    }

    override public void DumpRecordings(StreamWriter writer)
    {
        writer.Write(sensorName);
        foreach (var item in values)
        {
            writer.Write(",");
            writer.Write(item.ToString());
        }
        writer.Write(writer.NewLine);
    }

    override public void DumpColumnName(StreamWriter writer)
    {
        writer.Write(sensorName + ";");
    }

    override public void DumpNextRecording(StreamWriter writer)
    {
        if(dump_recordingIndex < values.Count)
        {
            writer.Write(values[dump_recordingIndex].ToString() +";");
        }
        else
        {           
            WriteDefaultValue(writer);
        }

        dump_recordingIndex++;

    }

    public virtual void WriteDefaultValue(StreamWriter writer)
    {
        writer.Write(defaultValue + ";");
    }

}

public static class SensorGenericExtension
{

    public static void AddRecordedValues64(this SensorGeneric<Vector3> obj, float[][] vals)
    {
        Vector3 v = Vector3.zero;
        Quaternion q = Quaternion.identity;

        for (int i = 0; i < vals[0].Length; i++)
        {
            float x = vals[0][i];
            float y = vals[1][i];
            float z = vals[2][i];
            float w = vals[3][i];


            q = new Quaternion(w,-x,-z,y);
            v = q.eulerAngles;

            // Add 2 times the value to be @ 256Hz
            obj.values.Add(v);
            obj.values.Add(v);
            obj.values.Add(v);
            obj.values.Add(v);
        }
        obj.lastValue = v;

    }
}


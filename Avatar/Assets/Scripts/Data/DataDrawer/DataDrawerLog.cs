using UnityEngine;
using System.Collections;
using Vectrosity;

public class DataDrawerLog : DataDrawerBase {
    
    //public int logBase = 10;

    private int frame = 0;
	
	// Update is called once per frame
	void Update () 
    {
        Vector2[] pts = _line.points2;

        int idx = frame % resolution;
		float v = Mathf.Log10(dataSource.lastValue-dataOffset*10);
		float pv = offsetY * Screen.height + scale * v;
		pts[idx].y = pv;
        
		_line.Draw();
        frame++;
	}
}

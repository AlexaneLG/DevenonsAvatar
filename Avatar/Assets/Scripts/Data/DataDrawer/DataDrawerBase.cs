using UnityEngine;
using System.Collections;
using Vectrosity;

public class DataDrawerBase : MonoBehaviour {

	protected VectorLine _line;
	public SensorGeneric<float> dataSource;

    public Color color;
    public float scale = 1.0f;
    public float length = 0.65f;

    public float offsetX = 0f;
    public float offsetY = 0f;
    public float dataOffset = 0f;

    Vector2[] pts;
    protected float X_PtMin, X_PtMax;


    //[Range(1f,60f)]
    public static float visualizationDuration = 10f;
       
	public Material lineMaterial;
    public Material verticalLineMaterial;
    public Material colisionLineMaterial;

    protected int resolutionPerSecond = 128;
    protected int resolution = 1280;

	// Use this for initialization
	virtual public void Start () {

        resolution = (int)(visualizationDuration * resolutionPerSecond);

        Vector2[] pts = new Vector2[resolution];

		for(int i = 0; i < resolution; i++)
        {
            pts[i].x = offsetX * Screen.width + i * (- Mathf.Abs((0.5f * Screen.width - 2 * offsetX * Screen.width) / resolution * length));
            pts[i].y = offsetY * Screen.height;

            if (i == 0)
                X_PtMin = pts[i].x;

            if (i == resolution -1)
                X_PtMax = pts[i].x;
		}

        if(dataSource != null)
            _line = new VectorLine(dataSource.sensorName, pts, color, lineMaterial, 1, LineType.Continuous);
    }

    public void adjustScale(float newScale)
    {
        scale = newScale;
    }

    public void adjustYOffset(float newYOffset)
    {
        offsetY = newYOffset;
    }
}

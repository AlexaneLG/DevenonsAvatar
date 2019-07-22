using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vectrosity;

public class DataDrawerReplay : DataDrawerBase {

    public int idxOffset;

    public GameObject rectPrefab;
    private GameObject dataRect;
    private float verticalRectScale, horizontalMaxRectScale;
    public float verticalRectScaleAdjust = 1f;

    private Vector3 dataCenterScreenPosition;

    public CSVExtractor Extractor;

    Vector2[] verticalReferencePoints = new Vector2[2];
    private Color verticalReferenceLineColor = Color.white;

    Vector2[] collisionEventPoints;
    private Color collisionEventLineColor = Color.magenta;
    VectorLine _Cline;
    public CollisionEventDataReplay collisionEnventDataReplay;

    void Start()
    {
        resolutionPerSecond = 50;

        base.Start();

        DrawVerticalReferenceLine();
        InitialiseCollisionEventLine();

        SetInitialeRectPositionAndScale();
    }

    void Update()
    {
        if (dataSource != null)
        {
            SetDataRectScale();

            DrawCollisionEventLine();

            // Dessein de la coubre physiologique 

            Vector2[] pts = _line.points2;

            for (int i = resolution - 1; i >= 0; i--)
            {
                if (((TimeDataReplay.dataTimeIdx + idxOffset) - i) >= 0 && (TimeDataReplay.dataTimeIdx + idxOffset) - i < dataSource.maxDataIndex)
                {
                    float v_3 = dataSource.values[(TimeDataReplay.dataTimeIdx + idxOffset) - i] - dataOffset;
                    float pv_3 = offsetY * Screen.height + scale * v_3;

                    pts[i].y = pv_3;
                }

                else
                {
                    float v_2 = 0;
                    float pv_2 = offsetY * Screen.height + scale * v_2;

                    pts[i].y = pv_2;
                }
            }
            _line.Draw();
        }
    }

    public void DrawVerticalReferenceLine()
    {
        idxOffset = resolution / 2;

        verticalReferencePoints[0].x = offsetX * Screen.width + (idxOffset) * (-Mathf.Abs((0.5f * Screen.width - 2 * offsetX * Screen.width) / resolution * length));
        verticalReferencePoints[0].y = Screen.height / 1.8f;

        verticalReferencePoints[1].x = offsetX * Screen.width + (idxOffset) * (-Mathf.Abs((0.5f * Screen.width - 2 * offsetX * Screen.width) / resolution * length)); ;
        verticalReferencePoints[1].y = 0;

        VectorLine _VRLine = new VectorLine("verticalReferenceLine", verticalReferencePoints, verticalReferenceLineColor, verticalLineMaterial, 3, LineType.Continuous);
        _VRLine.Draw();
    }

    public void InitialiseCollisionEventLine()
    {
        Vector2[] collisionEventPoints = new Vector2[resolution];

        for (int i = 0; i < resolution; i++)
        {
            collisionEventPoints[i].x = offsetX * Screen.width + i * (-Mathf.Abs((0.5f * Screen.width - 2 * offsetX * Screen.width) / resolution * length));
            collisionEventPoints[i].y = -10;
        }
        _Cline = new VectorLine(collisionEnventDataReplay.collisionEvent.sensorName, collisionEventPoints, collisionEventLineColor, colisionLineMaterial, 3, LineType.Continuous);
    }

    public void DrawCollisionEventLine()
    {
        Vector2[] CollisionEventPoints = _Cline.points2;

        for (int i = resolution - 1; i >= 0; i--)
        {
            if (((TimeDataReplay.dataTimeIdx + idxOffset) - i) >= 0 && (TimeDataReplay.dataTimeIdx + idxOffset) - i < collisionEnventDataReplay.collisionEvent.maxDataIndex)
            {
                if (collisionEnventDataReplay.collisionEvent.values[(TimeDataReplay.dataTimeIdx + idxOffset) - i] - dataOffset == 1)
                    CollisionEventPoints[i].y = Screen.height / 1.8f;

                else
                    CollisionEventPoints[i].y = -10;
            }
            else
                CollisionEventPoints[i].y = -10;
        }
        _Cline.Draw();
    }

    public void SetInitialeRectPositionAndScale()
    {
        GameObject vectorCam_GO = GameObject.Find("VectorCam");
        Camera vectorCam = vectorCam_GO.GetComponent<Camera>();

        vectorCam.orthographic = true;
        vectorCam.orthographicSize = (vectorCam.nearClipPlane + vectorCam.farClipPlane) / 2;

        float dataRectZPosition = (vectorCam.nearClipPlane + vectorCam.farClipPlane) / 2 + 1;
        float dataRectYPosition = Screen.height * offsetY;

        dataCenterScreenPosition = new Vector3(verticalReferencePoints[0].x, dataRectYPosition, dataRectZPosition);
        Vector3 worldPosition = vectorCam.ScreenToWorldPoint(dataCenterScreenPosition);

        dataRect = Instantiate(rectPrefab, worldPosition, Quaternion.identity) as GameObject;

        horizontalMaxRectScale = X_PtMax - X_PtMin;
        verticalRectScale = (dataRect.transform.localScale.y * Screen.height * 0.2f) * verticalRectScaleAdjust;

        vectorCam.farClipPlane = dataRectZPosition;
    }

    public void SetDataRectScale()
    {
        float widhtDataRect = (horizontalMaxRectScale * Extractor.extractDuration) / visualizationDuration;

        if(Extractor.extractDuration >= visualizationDuration)
        {
            widhtDataRect = horizontalMaxRectScale;
            dataRect.transform.localScale = new Vector3(widhtDataRect, verticalRectScale, dataRect.transform.localScale.z);
        }

        else
        {
            dataRect.transform.localScale = new Vector3(widhtDataRect, verticalRectScale, dataRect.transform.localScale.z);
        }
    }
}

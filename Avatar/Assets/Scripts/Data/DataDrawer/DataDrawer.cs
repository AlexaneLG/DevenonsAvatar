using UnityEngine;
using System.Collections;
using Vectrosity;

public class DataDrawer : DataDrawerBase
{

    float[] values128 = new float[2 * 14];
    float[] values32 = new float[8 * 14];

    public bool isSensorActive = true;

    public void UpdateWithValues256(float[] values)
    {
        Vector2[] pts = _line.points2;
        int vl = values.Length;

        for (int i = resolution - 1; i >= values.Length; i--)
        {
            pts[i].y = pts[i - vl].y;
        }

        int idx = values.Length - 1;
        foreach (var f in values)
        {
            float v = (float)f - dataOffset;
            float pv = offsetY * Screen.height + scale * v;
            pts[idx].y = pv;
            idx--;
        }
    }


    public void UpdateWithValues128(float[] values)
    {


        for (int i = 0; i < values.Length; i++)
        {
            values128[2 * i + 0] = values[i];
            values128[2 * i + 1] = values[i];
        }

        UpdateWithValues256(values128);

    }

    public void UpdateWithValues32(float[] values)
    {


        for (int i = 0; i < values.Length; i++)
        {
            values32[8 * i + 0] = values[i];
            values32[8 * i + 1] = values[i];
            values32[8 * i + 2] = values[i];
            values32[8 * i + 3] = values[i];
            values32[8 * i + 4] = values[i];
            values32[8 * i + 5] = values[i];
            values32[8 * i + 6] = values[i];
            values32[8 * i + 7] = values[i];

        }

        UpdateWithValues256(values32);

    }

    void Update()
    {
        if (isSensorActive)
        {
            _line.Draw();
        }
    }
}

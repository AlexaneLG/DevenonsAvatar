using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Xml;
using System;

/// <summary>
/// This class parses strings and converts to Unity types
/// This parsing is consistent with .ToString() methods on Unity type, performing the exact opposite operation
/// </summary>
public static class JTypeParsing
{
    public static int ParseInt(string s)
    {
        int r = 0;
        if (int.TryParse(s, out r))
            return r;
        else
            return 0;
    }

    public static float ParseFloat(string s)
    {
        float r = 0;
        if (float.TryParse(s, out r))
            return r;
        else
            return 0;
    }

    public static Color ParseColor(string s)
    {
        string newS = s.Replace("(", "").Replace(")", "");
        string[] vals = newS.Split(',');
        if (vals.Length != 4) return Color.white;
        if (Mathf.Max(Mathf.Max(float.Parse(vals[0]), float.Parse(vals[1])), Mathf.Max(float.Parse(vals[2]), float.Parse(vals[3]))) <= 1)
            return new Color(float.Parse(vals[0]), float.Parse(vals[1]), float.Parse(vals[2]), float.Parse(vals[3]));
        else
            return new Color(float.Parse(vals[0]) / 255f, float.Parse(vals[1]) / 255f, float.Parse(vals[2]) / 255f, float.Parse(vals[3]) / 255f);
    }

    public static Vector2 ParseVector2(string s)
    {
        string newS = s.Replace("(", "").Replace(")", "");
        string[] vals = newS.Split(',');
        if (vals.Length != 2) return Vector2.zero;
        Vector2 v = new Vector2(float.Parse(vals[0]), float.Parse(vals[1]));
        return v;
    }

    public static Vector3 ParseVector3(string s)
    {
        string newS = s.Replace("(", "").Replace(")", "");
        string[] vals = newS.Split(',');
        if (vals.Length != 3) return Vector3.zero;
        Vector3 v = new Vector3(float.Parse(vals[0]), float.Parse(vals[1]), float.Parse(vals[2]));
        return v;
    }

    public static Quaternion ParseQuaternion(string s)
    {
        string newS = s.Replace("(", "").Replace(")", "");
        string[] vals = newS.Split(',');
        if (vals.Length != 4) return Quaternion.identity;
        Quaternion q = new Quaternion(float.Parse(vals[0]), float.Parse(vals[1]), float.Parse(vals[2]), float.Parse(vals[3]));
        return q;
    }

    public static bool ParseBool(string s)
    {
        return (s.ToLower().IndexOf("true") >= 0);
    }

    //array types
    public static string[] ParseStringArray(string s)
    {
        return DecodeArray(s);
    }

    public static int[] ParseIntArray(string s)
    {
        string[] tmp = DecodeArray(s);
        int[] vars = new int[tmp.Length];
        for (int i = 0; i < tmp.Length; i++)
            vars[i] = ParseInt(tmp[i]);
        return vars;
    }

    public static float[] ParseFloatArray(string s)
    {
        string[] tmp = DecodeArray(s);
        float[] vars = new float[tmp.Length];
        for (int i = 0; i < tmp.Length; i++)
            vars[i] = ParseFloat(tmp[i]);
        return vars;
    }

    public static bool[] ParseBoolArray(string s)
    {
        string[] tmp = DecodeArray(s);
        bool[] vars = new bool[tmp.Length];
        for (int i = 0; i < tmp.Length; i++)
            vars[i] = ParseBool(tmp[i]);
        return vars;
    }

    public static object[] ParseEnumArray(System.Type t, string s)
    {
        string[] tmp = DecodeArray(s);
        object[] vars = new object[tmp.Length];
        for (int i = 0; i < tmp.Length; i++)
        {
            FieldInfo f = t.GetField(tmp[i]);
            int intValue = (int)f.GetValue(t);
            vars[i] = Enum.ToObject(t, intValue);
        }
        return vars;
    }

    public static Color[] ParseColorArray(string s)
    {
        string[] tmp = DecodeArray(s);
        Color[] vars = new Color[tmp.Length];
        for (int i = 0; i < tmp.Length; i++)
            vars[i] = ParseColor(tmp[i]);
        return vars;
    }

    public static Vector2[] ParseVector2Array(string s)
    {
        string[] tmp = DecodeArray(s);
        Vector2[] vars = new Vector2[tmp.Length];
        for (int i = 0; i < tmp.Length; i++)
            vars[i] = ParseVector2(tmp[i]);
        return vars;
    }

    public static Vector3[] ParseVector3Array(string s)
    {
        string[] tmp = DecodeArray(s);
        Vector3[] vars = new Vector3[tmp.Length];
        for (int i = 0; i < tmp.Length; i++)
            vars[i] = ParseVector3(tmp[i]);
        return vars;
    }

    public static Quaternion[] ParseQuaternionArray(string s)
    {
        string[] tmp = DecodeArray(s);
        Quaternion[] vars = new Quaternion[tmp.Length];
        for (int i = 0; i < tmp.Length; i++)
            vars[i] = ParseQuaternion(tmp[i]);
        return vars;
    }

    static string[] DecodeArray(string s)
    {
        XmlDocument xml = new XmlDocument();
        xml.LoadXml("<xml>" + s + "</xml>");
        XmlNode root = xml.FirstChild;
        string[] vals = new string[root.ChildNodes.Count];
        int index = 0;
        foreach (XmlNode node in root.ChildNodes)
        {
            vals[index++] = node.InnerText;
        }
        return vals;
    }
}

using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Xml;
using System;

public class JSettings
{
    #region PUBLIC METHODS
    /// <summary>
    /// Provide an xml settings string and a key, let the script find gameObjects, components and assign field or properties
    /// </summary>
    /// <param name="iXmlString">An Xml string with all the required settings</param>
    /// <param name="iSettingsKey">a key to filter the Xml proper settings node</param>
    public static void ProcessXmlSettings(string iXmlString, string iSettingsKey)
    {
        XmlDocument xml = new XmlDocument();
        try
        {
            xml.LoadXml(iXmlString);
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading XML settings. Expecting <xml><settings key=\"default\">... error=" + e.ToString());
        }
        XmlNode root = xml.LastChild;

        //find settings section
        XmlNode settings = JXmlUtils.GetXmlNode(root, "settings,key=" + iSettingsKey);

        if (settings == null)
            Debug.Log("can't find settings section with key=" + iSettingsKey + " in settings xml");
        else
        {
            string goName;
            string propName;

            foreach (XmlNode goNode in settings.ChildNodes)
            {
                if (goNode.Name == "gameObject")
                {
                    if (!JXmlUtils.HasAttribute(goNode, "name", out goName))
                        Debug.Log("gameObject node missing 'name' attribute");
                    else
                    {
                        foreach (XmlNode propNode in goNode.ChildNodes)
                        {
                            if (propNode.Name == "property")
                            {

                                if (!JXmlUtils.HasAttribute(propNode, "name", out propName))
                                    Debug.Log(propName + ": property node missing 'name' attribute");
                                else
                                {
                                    string[] s = propName.Split('.');
                                    if (s.Length == 2)
                                    {
                                        //Debug.Log("settings: " + goName + "." + s[0] + "." + s[1] + "=" + propNode.InnerText);
                                        SetFieldOrPropertyByName(goName, s[0], s[1], propNode.InnerXml);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    if (goNode.Name!="#comment")
                        Debug.Log("unknown xml node: " + goNode.Name);
            }
        }
        //SetFieldOrPropertyByName(go.name, compName, propName, propValue);
    }

    /* removed, there are cleaner ways to serialize components
    public static string SerializeComponent(Component iComp)
    {
        //for now we do this the hard way, clean up when possible
        string xml = "<component name='" + iComp.GetType().ToString() + "'>";

        FieldInfo[] fields = iComp.GetType().GetFields();
        foreach (FieldInfo f in fields)
            xml += "<field name='" + f.Name + "' type='" + f.GetValue(iComp).GetType().ToString() + "'>" + f.GetValue(iComp).ToString() + "</field>";
        xml += "</component>";
        return xml;
    }

    public static bool DeserializeComponent(Component iComp, string xmlString)
    {
        if (xmlString == "")
            return false;
        if (!iComp)
            return false;

        XmlDocument xml = new XmlDocument();
        xml.LoadXml(xmlString);

        XmlNode root = xml.LastChild;
        string name = "";
        bool success = true;
        foreach (XmlNode node in root.ChildNodes)
            if (JXmlUtils.HasAttribute(node, "name", out name))
                success = success && SetFieldOrPropertyByName(iComp, name, node.InnerXml);
        return success;
    }
     */


    /// <summary>
    /// Set a field or property on a gameObject by name
    /// </summary>
    /// <param name="iGameObject"></param>
    /// <param name="iComp"></param>
    /// <param name="iProp"></param>
    /// <param name="iValue"></param>
    /// <returns></returns>
    public static bool SetFieldOrPropertyByName(string iGameObject, string iComp, string iProp, string iValue)
    {
        GameObject go = GameObject.Find(iGameObject);
        if (!go)
        {
            Debug.LogError("gameObject " + iGameObject + " not found, can't set properties");
            return false;
        }
        Component[] comps;
        if (iComp != "")
        {
            Component comp = GetComponentByName(go, iComp);
            if (!comp)
            {
                Debug.Log("gameObject " + iGameObject + " doesn't have component: " + iComp);
                return false;
            }
            comps = new Component[1] { comp };
        }
        else
            comps = go.GetComponents(typeof(Component));


        foreach (Component c in comps)
        {
            //try to find property first, then field if needed
            if (SetFieldOrPropertyByName(c, iProp, iValue))
                return true;
        }
        if (comps.Length == 1)
        {
            Debug.LogWarning("Can't apply following setting: " + iGameObject + "." + iComp + "." + iProp);
            //DebugListProperties(comps[0]);
            //DebugListFields(comps[0]);
        }
        return false;
    }

    /// <summary>
    /// Set a field or property on a gameObject by name
    /// </summary>
    /// <param name="iComp"></param>
    /// <param name="iProp"></param>
    /// <param name="iValue"></param>
    /// <returns></returns>
    public static bool SetFieldOrPropertyByName(Component iComp, string iProp, string iValue)
    {
        return SetField(iComp, iProp, iValue) || SetProperty(iComp, iProp, iValue);
    }

    /// <summary>
    /// Get field or property on a gameObject by name
    /// </summary>
    /// <param name="iGameObject"></param>
    /// <param name="iComp"></param>
    /// <param name="iProp"></param>
    /// <returns></returns>
    public static string GetFieldOrPropertyByName(string iGameObject, string iComp, string iProp)
    {
        Component comp = GetComponentByName(iGameObject, iComp);
        return GetFieldOrPropertyByName(comp, iProp);
    }

    /// <summary>
    /// Get field or property on a gameObject by name 
    /// </summary>
    /// <param name="iComp"></param>
    /// <param name="iProp"></param>
    /// <returns></returns>
    public static string GetFieldOrPropertyByName(Component iComp, string iProp)
    {
        string value = "";
        if (iComp)
            if (!GetProperty(iComp, iProp, out value))
                GetField(iComp, iProp, out value);
        return value;
    }

    /// <summary>
    /// Find a gameObject's component by name
    /// </summary>
    /// <param name="iGameObject"></param>
    /// <param name="iComp"></param>
    /// <returns></returns>
    public static Component GetComponentByName(string iGameObject, string iComp)
    {
        GameObject go = GameObject.Find(iGameObject);
        if (!go)
        {
            Debug.Log("gameObject " + iGameObject + " not found, can't set properties");
            return null;
        }
        return GetComponentByName(go, iComp);
    }

    /// <summary>
    /// Find a gameObject's component by name
    /// </summary>
    /// <param name="iGo"></param>
    /// <param name="iCompName"></param>
    /// <returns></returns>
    public static Component GetComponentByName(GameObject iGo, string iCompName)
    {
        //Debug.Log("gameObject="+iGo+", setting "+iCompName);
        Component[] comps = iGo.GetComponents(typeof(Component));
        Component comp = null;
        foreach (Component c in comps)
        {
            if (c.GetType().ToString() == iCompName || c.GetType().ToString().Replace("UnityEngine.", "") == iCompName)
            {
                comp = c;
                break;
            }
        }
        return comp;
    }

    #endregion

    #region FIELDS AND PROPERTIES GET/SET

    static void DebugListComponents(GameObject iGo)
    {
        Component[] comps = iGo.GetComponents(typeof(Component));
        string tmp = "comp list for " + iGo.name + " is: ";
        foreach (Component c in comps)
            tmp += (c.GetType().ToString() + ", ");
        Debug.Log(tmp);
    }

    static void DebugListProperties(Component iComp)
    {
        PropertyInfo[] props = iComp.GetType().GetProperties();
        string tmp = "prop list is: ";
        foreach (PropertyInfo p in props)
            tmp += (p.Name + ", ");
        Debug.Log(tmp);
    }

    static void DebugListFields(Component iComp)
    {
        FieldInfo[] fields = iComp.GetType().GetFields();
        string tmp = "field list is: ";
        foreach (FieldInfo f in fields)
            tmp += (f.Name + ", ");
        Debug.Log(tmp);
    }

    static PropertyInfo GetPropertyByName(string iPropName, Component iComp)
    {
        PropertyInfo[] props = iComp.GetType().GetProperties();
        foreach (PropertyInfo p in props)
            if (p.Name.ToLower() == iPropName.ToLower())
                return p;
        return null;
    }

    static FieldInfo GetFieldByName(string iFieldName, Component iComp)
    {
        FieldInfo[] fields = iComp.GetType().GetFields();
        foreach (FieldInfo f in fields)
            if (f.Name.ToLower() == iFieldName.ToLower())
                return f;
        return null;
    }

    static bool GetProperty(Component iComp, string iPropName, out string value)
    {
        value = "";
        if (iComp == null)
            return false;
        else
        {
            PropertyInfo prop = GetPropertyByName(iPropName, iComp);
            if (prop != null)
            {
                value = prop.GetValue(iComp, null).ToString();
                return true;
            }
        }
        return false;
    }

    static bool SetProperty(Component iComp, string iPropName, string value)
    {
        if (iComp == null)
            return false;
        else
        {
            PropertyInfo prop = GetPropertyByName(iPropName, iComp);
            if (prop != null)
            {

                object val = prop.GetValue(iComp, null);
                val = ReadType(value, val.GetType());
                if (val != null)
                {
                    prop.SetValue(iComp, val, null);
                    return true;
                }
                else
                    return false;
            }
        }
        return false;
    }

    static bool GetField(Component iComp, string iFieldName, out string value)
    {
        value = "";
        if (iComp == null)
            return false;
        else
        {
            FieldInfo field = GetFieldByName(iFieldName, iComp);
            if (field != null)
            {
                value = field.GetValue(iComp).ToString();
                return true;
            }
        }
        return false;
    }

    static bool SetField(Component iComp, string iFieldName, string value)
    {
        if (iComp == null)
            return false;
        else
        {
            FieldInfo field = GetFieldByName(iFieldName, iComp);
            if (field != null)
            {
                object val = field.GetValue(iComp);
                Type t = val.GetType();
                val = ReadType(value, t);
                if (val != null)
                {
                    if (val.GetType() == typeof(System.Object[]) && t.IsArray && t.GetElementType().IsEnum)
                    {
                        //go through the object array and set enums one by one
                        //not sure how to do that :(
                        Debug.Log("enum arrays not supported yet...");
                    }
                    else
                        field.SetValue(iComp, val);
                    return true;
                }
                else
                    return false;
            }
        }
        return false;
    }

    #endregion

    #region TYPE PARSING
    static System.Object ReadType(string value, System.Type t)
    {
        if (!t.IsArray)
        {
            if (t == typeof(float))
                return JTypeParsing.ParseFloat(value);
            else if (t == typeof(int))
                return JTypeParsing.ParseInt(value);
            else if (t.IsEnum)
            {
                //get the enum field based on value
                FieldInfo f = t.GetField(value);
                //convert to int
                if (f != null)
                {
                    int intValue = (int)f.GetValue(t);
                    return Enum.ToObject(t, intValue);
                }
                else
                    return null;
            }
            else if (t == typeof(bool))
                return JTypeParsing.ParseBool(value);
            else if (t == typeof(string))
                return value;
            else if (t == typeof(Color))
                return JTypeParsing.ParseColor(value);
            else if (t == typeof(Vector2))
                return JTypeParsing.ParseVector2(value);
            else if (t == typeof(Vector3))
                return JTypeParsing.ParseVector3(value);
            else if (t == typeof(Quaternion))
                return JTypeParsing.ParseQuaternion(value);
            /*
            else if (t == typeof(Component))
                return ReadEZRef(k);
            else if (t == typeof(GameObject))
                return ReadEZRef(k);
             */
        }
        else
        {
            if (t == typeof(int[]))
                return JTypeParsing.ParseIntArray(value);
            else if (t == typeof(float[]))
                return JTypeParsing.ParseFloatArray(value);
            else if (t == typeof(string[]))
                return JTypeParsing.ParseStringArray(value);
            else if (t.GetElementType().IsEnum)
                return JTypeParsing.ParseEnumArray(t.GetElementType(), value);
            else if (t == typeof(bool[]))
                return JTypeParsing.ParseBoolArray(value);
            else if (t == typeof(Color[]))
                return JTypeParsing.ParseColorArray(value);
            else if (t == typeof(Vector2[]))
                return JTypeParsing.ParseVector2Array(value);
            else if (t == typeof(Vector3[]))
                return JTypeParsing.ParseVector3Array(value);
            else if (t == typeof(Quaternion[]))
                return JTypeParsing.ParseQuaternionArray(value);
        }
        return null;
    }

    /*
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
        if (Mathf.Max(Mathf.Max(float.Parse(vals[0]),float.Parse(vals[1])),Mathf.Max(float.Parse(vals[2]),float.Parse(vals[3])))<=1)
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
    //int float string
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
    */
    #endregion
}

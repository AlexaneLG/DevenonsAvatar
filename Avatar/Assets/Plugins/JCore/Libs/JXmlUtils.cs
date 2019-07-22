using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Xml;

/// <summary>
/// This is a helper class for parsing XML, serializing and deserializing objects
/// </summary>
public static class JXmlUtils
{
    #region Serialization helpers

    public static bool DeserializeFromLocalFile<T>(out T deserializedObject, string localFilePath)
    {
        deserializedObject = default(T);
        try
        {
            XmlTextReader reader = new XmlTextReader(new StreamReader(localFilePath));
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            deserializedObject = (T)serializer.Deserialize(reader);
            reader.Close();
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Document load exception: " + e.ToString());
            return false;
        }
    }

    public static bool SerializeToLocalFile<T>(T objectToSerialize, string localFilePath)
    {
        try
        {
            TextWriter writer = new StreamWriter(localFilePath);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            serializer.Serialize(writer, objectToSerialize, ns);
            writer.Close();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Document save exception: " + e.ToString());
            return false;
        }
        return true;
    }

    public static string ToXmlString<T>(T objectToSerialize)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            serializer.Serialize(writer, objectToSerialize, ns);
            writer.Close();
            return sb.ToString();
        }
        catch (System.Exception e)
        {
            Debug.LogError("ToXmlString exception: " + e.ToString());
            return objectToSerialize.ToString();
        }
    }

    #endregion

    #region Some XML helpers functions (in case manual XML reading is used)

    /// <summary>
    /// Look for an attribute on the xml node
    /// </summary>
    /// <param name="iNode">XML node to search</param>
    /// <param name="iName">attribute name</param>
    /// <param name="oValue">attribute value if found</param>
    /// <returns>true if attribute found, false if not</returns>
    public static bool HasAttribute(XmlNode iNode, string iName, out string oValue)
    {
        oValue = "";
        foreach (XmlAttribute attr in iNode.Attributes)
            if (attr.Name == iName)
            {
                oValue = attr.Value;
                return true;
            }
        return false;
    }

    /// <summary>
    ///  this method performs a search in an xml node
    /// </summary>
    /// <param name="iNode">parent node to search</param>
    /// <param name="iQuery">query to perform, ex: root/item1,key1=value1,key2=value2/item2/item3</param>
    /// <param name="oValue">return string value</param>
    /// <returns>true if found, false if not</returns>
    public static bool GetXmlValue(XmlNode iNode, string iQuery, out string oValue)
    {
        oValue = "";
        XmlNode node = iNode;
        string[] p = iQuery.Split('/');
        bool found = false;
        string tmp = "";

        for (int i = 0; i < p.Length; i++)
        {
            //consider the next item to find, with it's key attributes
            string[] s1 = p[i].Split(',');
            found = false;
            foreach (XmlNode n in node.ChildNodes)
                if (n.Name == s1[0])
                {
                    //let's see if we need to check key attributes
                    found = true;
                    for (int j=1;j<s1.Length;j++)
                    {
                        string[] s2=s1[j].Split('=');
                        if (s2.Length!=2 || !HasAttribute(n, s2[0], out tmp) || s2[1]!=tmp)
                            found = false;
                        if (!found)
                            break;
                    }

                    if (found)
                    {
                        node = n;
                        break;
                    }
                }
            if (!found)
                return false;
        }
        if (found)
        {
            if (node.FirstChild != null)
                oValue = node.FirstChild.Value;
            //else
            //    Debug.Log("value is empty for query: " + iQuery + " on node " + iNode.InnerText);
        }
        return found;
    }

    /// <summary>
    ///  this method performs a search in an xml node
    /// </summary>
    /// <param name="iNode">parent node to search</param>
    /// <param name="iQuery">query to perform, ex: root/item1,key1=value1,key2=value2/item2/item3</param>
    /// <returns>an xml node if found</returns>
    public static XmlNode GetXmlNode(XmlNode iNode, string iQuery)
    {
        string p=iQuery;
        if (p.IndexOf('/') > 0)
            p = p.Substring(0, p.IndexOf('/'));

        bool found = false;
        string tmp = "";


        //consider the next item to find, with it's key attributes
        string[] s1 = p.Split(',');
        found = false;

        //go through nodes to find one that works
        foreach (XmlNode node in iNode.ChildNodes)
            if (node.Name == s1[0])
            {
                //let's see if we need to check key attributes
                found = true;
                for (int j = 1; j < s1.Length; j++)
                {
                    string[] s2 = s1[j].Split('=');
                    if (s2.Length != 2 || !HasAttribute(node, s2[0], out tmp) || s2[1] != tmp)
                        found = false;
                    if (!found)
                        break;
                }

                if (found)
                {
                    //let's recurse, submitting the sub query
                    if (p == iQuery)
                        return node;
                    else
                    {
                        int s=iQuery.IndexOf('/');
                        string subQuery = iQuery.Substring(s + 1, iQuery.Length - s - 1);
                        XmlNode foundNode = GetXmlNode(node, subQuery);
                        if (foundNode != null)
                            return foundNode;
                    }
                }
            }
        return null;
    }

    #endregion
}

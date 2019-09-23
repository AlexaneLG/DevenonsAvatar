using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Xml;
using System.IO;

public class SetTimeline : MonoBehaviour
{

    public string LoadingFileName = "timeline";

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadFromXml()
    {
        string filepath = Application.dataPath + @"/StreamingAssets/" + LoadingFileName + ".xml";
        XmlDocument xmlDoc = new XmlDocument();

        string tmpText = "";

        if (File.Exists(filepath))
        {
            xmlDoc.Load(filepath);
            XmlNodeList scenarioItemsList = xmlDoc.GetElementsByTagName("scenarioitem");
            foreach (XmlNode scenarioitem in scenarioItemsList)
            {
                
				XmlNodeList itemcontent = scenarioitem.ChildNodes;

                foreach (XmlNode attribut in itemcontent)
                {
                    if (attribut.Name == "texts")
                    {
                        tmpText = ; //float.Parse(attribut.InnerText); 
                    }
                }
				
            }
        }
        else
        {
            Debug.Log("Incorrect file name");
            return;
        }

    }
}

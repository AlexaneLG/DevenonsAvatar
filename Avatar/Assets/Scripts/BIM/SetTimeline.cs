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

        if (File.Exists(filepath))
        {
            xmlDoc.Load(filepath);
            XmlNodeList scenarioItemsList = xmlDoc.GetElementsByTagName("scenarioitem");
            foreach (XmlNode scenarioitem in scenarioItemsList)
            {
                /*
				XmlNodeList transformcontent = transformInfo.ChildNodes;

                foreach (XmlNode transformItens in transformcontent)
                {
                    if (transformItens.Name == "cameraDistance")
                    {
                        cameraDistance = float.Parse(transformItens.InnerText);
                        distance_Slider.value = cameraDistance;
                    }
                    if (transformItens.Name == "cameraAltitude")
                    {
                        cameraAltitude = float.Parse(transformItens.InnerText); 
                        altitude_Slider.value = cameraAltitude;
                    }
                }
				*/
            }
        }
        else
        {
            Debug.Log("Incorrect file name");
            return;
        }

    }
}

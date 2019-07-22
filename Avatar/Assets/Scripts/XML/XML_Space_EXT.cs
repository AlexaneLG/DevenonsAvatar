using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;


public class XML_Space_EXT : MonoBehaviour
{
    private string SavingFileName, LoadingFileName;
    public string defaultSettingsName;

    public Slider distance_Slider, altitude_Slider, forwardForce_Space_EXT_Slider, rotationForce_Space_EXT_Slider, start_Space_EXT_Slider, freeFly_Space_EXT_Slider, end_Space_EXT_Slider;

    public Transform TPSTransform;
    
    private float cameraDistance = 0;
    private float cameraAltitude = 0;

    public Transform scenarioItems;

    private Start_Space_EXT start_Space_EXT;
    private FreeFly_Space_EXT freeFly_Space_EXT;
    private End_Space_EXT end_Space_EXT;

    private SpaceController_EXT spaceController_EXT;
     
    void Awake()
    {
        Transform lTemp;

        lTemp = scenarioItems.Find("Start_Space_EXT");
        start_Space_EXT = lTemp.GetComponent<Start_Space_EXT>();

        lTemp = scenarioItems.Find("FreeFly_Space_EXT");
        freeFly_Space_EXT = lTemp.GetComponent<FreeFly_Space_EXT>();

        lTemp = scenarioItems.Find("End_Space_EXT");
        end_Space_EXT = lTemp.GetComponent<End_Space_EXT>();

        spaceController_EXT = CharacterControllerBasedOnAxis.instance.GetComponent<SpaceController_EXT>();
    }

    void Start()
    {
        if(defaultSettingsName != null)
        {
            LoadingFileName = defaultSettingsName;
            LoadFromXml();
        }
    }
	
	public void WriteToXml()
	{
        string filepath = Application.dataPath + @"/StreamingAssets/" + SavingFileName + ".xml";
        Debug.Log(filepath);

       // if (File.Exists(filepath))

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml("<settings></settings>");

        XmlElement elmRoot = xmlDoc.DocumentElement;

            XmlElement elmNew = xmlDoc.CreateElement("Camera_settings");

                XmlElement cameraDistance = xmlDoc.CreateElement("cameraDistance");
                cameraDistance.InnerText = TPSTransform.localPosition.z.ToString();

                XmlElement cameraAltitude = xmlDoc.CreateElement("cameraAltitude");
                cameraAltitude.InnerText = TPSTransform.localPosition.y.ToString();

                elmNew.AppendChild(cameraDistance);
                elmNew.AppendChild(cameraAltitude);

                elmRoot.AppendChild(elmNew);
        
                XmlElement elmNew_2 = xmlDoc.CreateElement("Main_settings");

                XmlElement forwardForce_Space_EXT = xmlDoc.CreateElement("forwardForce_Space_EXT");
                forwardForce_Space_EXT.InnerText = spaceController_EXT.forwardForce.ToString();

                XmlElement rotationForce_Space_EXT = xmlDoc.CreateElement("rotationForce_Space_EXT");
                rotationForce_Space_EXT.InnerText = spaceController_EXT.rotationForce.ToString();
 
                elmNew_2.AppendChild(forwardForce_Space_EXT);
                elmNew_2.AppendChild(rotationForce_Space_EXT);

                elmRoot.AppendChild(elmNew_2);
        
            XmlElement elmNew_3 = xmlDoc.CreateElement("Scenario_settings");

                XmlElement start_Space_EXT_Duration = xmlDoc.CreateElement("start_Space_EXT_Duration");
                start_Space_EXT_Duration.InnerText = start_Space_EXT.duration.ToString();

                XmlElement freeFly_Space_EXT_Duration = xmlDoc.CreateElement("freeFly_Space_EXT_Duration");
                freeFly_Space_EXT_Duration.InnerText = freeFly_Space_EXT.duration.ToString();

                XmlElement end_Space_EXT_Duration = xmlDoc.CreateElement("end_Space_EXT_Duration");
                end_Space_EXT_Duration.InnerText = end_Space_EXT.duration.ToString();

                elmNew_3.AppendChild(start_Space_EXT_Duration);
                elmNew_3.AppendChild(freeFly_Space_EXT_Duration);
                elmNew_3.AppendChild(end_Space_EXT_Duration);

                elmRoot.AppendChild(elmNew_3);

        xmlDoc.Save(filepath);   
    }
	
	public void LoadFromXml()
	{
        string filepath = Application.dataPath + @"/StreamingAssets/" + LoadingFileName + ".xml";
		XmlDocument xmlDoc = new XmlDocument();

        if (File.Exists(filepath))
        {
            xmlDoc.Load(filepath);

            XmlNodeList transformList = xmlDoc.GetElementsByTagName("Camera_settings");

            foreach (XmlNode transformInfo in transformList)
            {
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
            }

            XmlNodeList transformList_2 = xmlDoc.GetElementsByTagName("Main_settings");

            foreach (XmlNode transformInfo in transformList_2)
            {
                XmlNodeList transformcontent = transformInfo.ChildNodes;

                foreach (XmlNode transformItens in transformcontent)
                {
                    if (transformItens.Name == "forwardForce_Space_EXT")
                    {
                        spaceController_EXT.forwardForce = float.Parse(transformItens.InnerText);
                        forwardForce_Space_EXT_Slider.value = spaceController_EXT.forwardForce;
                    }
                    if (transformItens.Name == "rotationForce_Space_EXT")
                    {
                        spaceController_EXT.rotationForce = float.Parse(transformItens.InnerText);
                        rotationForce_Space_EXT_Slider.value = spaceController_EXT.rotationForce;
                    }
                }
            }
            
            XmlNodeList transformList_3 = xmlDoc.GetElementsByTagName("Scenario_settings");

            foreach (XmlNode transformInfo in transformList_3)
            {
                XmlNodeList transformcontent = transformInfo.ChildNodes;

                foreach (XmlNode transformItens in transformcontent)
                {
                    if (transformItens.Name == "start_Space_EXT_Duration")
                    {
                        start_Space_EXT.duration = float.Parse(transformItens.InnerText);
                        start_Space_EXT_Slider.value = start_Space_EXT.duration;
                    }
                        if (transformItens.Name == "freeFly_Space_EXT_Duration")
                    {
                        freeFly_Space_EXT.duration = float.Parse(transformItens.InnerText);
                        freeFly_Space_EXT_Slider.value = freeFly_Space_EXT.duration;
                    }
                    if (transformItens.Name == "end_Space_EXT_Duration")
                    {
                        end_Space_EXT.duration = float.Parse(transformItens.InnerText);
                        end_Space_EXT_Slider.value = end_Space_EXT.duration;
                    }
                }
            }
        }
        else
        {
            Debug.Log("Incorrect name");
            return;
        }
            
        TPSTransform.localPosition = new Vector3(TPSTransform.localPosition.x, cameraAltitude, cameraDistance); // Apply the values.        
	}
    
    public void SaveAs(string newSavingfileName)
    {
        SavingFileName = newSavingfileName;
    }

    public void LoadFile(string newloadingFileName)
    {
        LoadingFileName = newloadingFileName;
    }
}
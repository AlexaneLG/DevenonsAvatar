using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;


public class XML_Space_INT : MonoBehaviour
{
    private string SavingFileName, LoadingFileName;
    public string defaultSettingsName;

    public Slider distance_Slider, altitude_Slider, baseSpeed_Space_INT_Slider, flexionSpeed_SPACE_INT_Slider, start_Space_INT_Slider, projectUser_Space_INT_Slider, navigation_Space_INT_Slider;

    public Transform TPSTransform;

    private float cameraDistance = 0;
    private float cameraAltitude = 0;

    public Transform scenarioItems;

    private Start_Space_INT start_Space_INT;
    private ProjectUser_Space_INT projectUser_Space_INT;
    private Navigation_Space_INT navigation_Space_INT;

    private SpaceController spaceController;

    void Awake()
    {
        Transform lTemp;

        if (defaultSettingsName.CompareTo("SPACEINT2") == 0)
        {
            lTemp = scenarioItems.Find("Navigation_Space_INT2");
            navigation_Space_INT = lTemp.GetComponent<Navigation_Space_INT2>();

        }
        else
        {
            lTemp = scenarioItems.Find("Navigation_Space_INT");
            navigation_Space_INT = lTemp.GetComponent<Navigation_Space_INT>();

        }

        lTemp = scenarioItems.Find("Start_Space_INT");
        start_Space_INT = lTemp.GetComponent<Start_Space_INT>();

        lTemp = scenarioItems.Find("ProjectUser_Space_INT");
        projectUser_Space_INT = lTemp.GetComponent<ProjectUser_Space_INT>();

        var ccba = CharacterControllerBasedOnAxis.instance;
        spaceController = ccba.GetComponent<SpaceController>();
    }

    void Start()
    {
        if (defaultSettingsName != null)
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

        XmlElement baseSpeed_Space_INT = xmlDoc.CreateElement("baseSpeed_Space_INT");
        baseSpeed_Space_INT.InnerText = navigation_Space_INT.baseSpeed.ToString();

        XmlElement flexionSpeed_Space_INT = xmlDoc.CreateElement("flexionSpeed_Space_INT");
        flexionSpeed_Space_INT.InnerText = spaceController.flexionSpeed.ToString();

        elmNew_2.AppendChild(baseSpeed_Space_INT);
        elmNew_2.AppendChild(flexionSpeed_Space_INT);

        elmRoot.AppendChild(elmNew_2);

        XmlElement elmNew_3 = xmlDoc.CreateElement("Scenario_settings");

        XmlElement start_Space_INT_Duration = xmlDoc.CreateElement("start_Space_INT_Duration");
        start_Space_INT_Duration.InnerText = start_Space_INT.duration.ToString();

        XmlElement projectUser_Space_INT_Duration = xmlDoc.CreateElement("projectUser_Space_INT_Duration");
        projectUser_Space_INT_Duration.InnerText = projectUser_Space_INT.duration.ToString();

        XmlElement navigation_Space_INT_Duration = xmlDoc.CreateElement("navigation_Space_INT_Duration");
        navigation_Space_INT_Duration.InnerText = navigation_Space_INT.duration.ToString();

        elmNew_3.AppendChild(start_Space_INT_Duration);
        elmNew_3.AppendChild(projectUser_Space_INT_Duration);
        elmNew_3.AppendChild(navigation_Space_INT_Duration);

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
                    if (transformItens.Name == "baseSpeed_Space_INT")
                    {
                        navigation_Space_INT.baseSpeed = float.Parse(transformItens.InnerText);
                        baseSpeed_Space_INT_Slider.value = navigation_Space_INT.baseSpeed;
                    }
                    if (transformItens.Name == "flexionSpeed_Space_INT")
                    {
                        spaceController.flexionSpeed = float.Parse(transformItens.InnerText);
                        flexionSpeed_SPACE_INT_Slider.value = spaceController.flexionSpeed;
                    }
                }
            }

            XmlNodeList transformList_3 = xmlDoc.GetElementsByTagName("Scenario_settings");

            foreach (XmlNode transformInfo in transformList_3)
            {
                XmlNodeList transformcontent = transformInfo.ChildNodes;

                foreach (XmlNode transformItens in transformcontent)
                {
                    if (transformItens.Name == "start_Space_INT_Duration")
                    {
                        start_Space_INT.duration = float.Parse(transformItens.InnerText);
                        start_Space_INT_Slider.value = start_Space_INT.duration;
                    }
                    if (transformItens.Name == "projectUser_Space_INT_Duration")
                    {
                        projectUser_Space_INT.duration = float.Parse(transformItens.InnerText);
                        projectUser_Space_INT_Slider.value = projectUser_Space_INT.duration;
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
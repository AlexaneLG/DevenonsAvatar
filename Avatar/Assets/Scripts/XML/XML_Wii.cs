using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;


public class XML_Wii : MonoBehaviour
{
    private string SavingFileName, LoadingFileName;
    public string defaultSettingsName;

    public Slider distance_Slider, altitude_Slider, baseSpeed_Wii_Slider, speedMultiplier_Slider, start_Wii_Slider, projectUser_Wii_Slider, freeFly_Wii_Slider, gates_Wii_Slider, end_Wii_Slider;
    public Slider verticalSensitivity_Wii_Slider, horizontalSensitivity_Wii_Slider;
    public Transform TPSTransform;

    private float cameraDistance = 0;
    private float cameraAltitude = 0;

    public Transform scenarioItems;

    private Start_Wii start_Wii;
    private ProjectUser_Wii projectUser_Wii;
    private FreeFly_Wii freeFly_Wii;
    private Gates_Wii gates_Wii;
    private End_Wii end_Wii;

    private WiiController wiiController;

    void Awake()
    {
        Transform lTemp;

        lTemp = scenarioItems.Find("Start_Wii");
        start_Wii = lTemp.GetComponent<Start_Wii>();

        lTemp = scenarioItems.Find("ProjectUser_Wii");
        projectUser_Wii = lTemp.GetComponent<ProjectUser_Wii>();

        lTemp = scenarioItems.Find("FreeFly_Wii");
        freeFly_Wii = lTemp.GetComponent<FreeFly_Wii>();

        lTemp = scenarioItems.Find("Gates_Wii");
        gates_Wii = lTemp.GetComponent<Gates_Wii>();

        lTemp = scenarioItems.Find("End_Wii");
        end_Wii = lTemp.GetComponent<End_Wii>();

        wiiController = CharacterControllerBasedOnAxis.instance.GetComponent<WiiController>();
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

        XmlElement baseSpeed_Wii = xmlDoc.CreateElement("baseSpeed_Wii");
        baseSpeed_Wii.InnerText = wiiController.baseSpeed.ToString();

        XmlElement speedMultiplier_Wii = xmlDoc.CreateElement("speedMultiplier_Wii");
        speedMultiplier_Wii.InnerText = wiiController.speedMultiplier.ToString();

        elmNew_2.AppendChild(baseSpeed_Wii);
        elmNew_2.AppendChild(speedMultiplier_Wii);

        elmRoot.AppendChild(elmNew_2);

        XmlElement elmNew_3 = xmlDoc.CreateElement("Scenario_settings");

        XmlElement start_Wii_Duration = xmlDoc.CreateElement("start_Wii_Duration");
        start_Wii_Duration.InnerText = start_Wii.duration.ToString();

        XmlElement projectUser_Wii_Duration = xmlDoc.CreateElement("projectUser_Wii_Duration");
        projectUser_Wii_Duration.InnerText = projectUser_Wii.duration.ToString();

        XmlElement freeFly_Wii_Duration = xmlDoc.CreateElement("freeFly_Wii_Duration");
        freeFly_Wii_Duration.InnerText = freeFly_Wii.duration.ToString();

        XmlElement gates_Wii_Duration = xmlDoc.CreateElement("gates_Wii_Duration");
        gates_Wii_Duration.InnerText = gates_Wii.duration.ToString();

        XmlElement end_Wii_Duration = xmlDoc.CreateElement("end_Wii_Duration");
        end_Wii_Duration.InnerText = end_Wii.duration.ToString();

        elmNew_3.AppendChild(start_Wii_Duration);
        elmNew_3.AppendChild(projectUser_Wii_Duration);
        elmNew_3.AppendChild(freeFly_Wii_Duration);
        elmNew_3.AppendChild(gates_Wii_Duration);
        elmNew_3.AppendChild(end_Wii_Duration);

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
                    if (transformItens.Name == "baseSpeed_Wii")
                    {
                        wiiController.baseSpeed = float.Parse(transformItens.InnerText);
                        baseSpeed_Wii_Slider.value = wiiController.baseSpeed;
                    }

                    if (transformItens.Name == "speedMultiplier_Wii")
                    {
                        wiiController.speedMultiplier = float.Parse(transformItens.InnerText);
                        speedMultiplier_Slider.value = wiiController.speedMultiplier;
                    }

                    if (transformItens.Name == "horizontalSensitivity")
                    {
                        wiiController.horizontalSensitivity = float.Parse(transformItens.InnerText);
                        horizontalSensitivity_Wii_Slider.value = wiiController.horizontalSensitivity;
                    }

                    if (transformItens.Name == "verticalSensitivity")
                    {
                        wiiController.verticalSensitivity = float.Parse(transformItens.InnerText);
                        verticalSensitivity_Wii_Slider.value = wiiController.verticalSensitivity;
                    }
                }
            }

            XmlNodeList transformList_3 = xmlDoc.GetElementsByTagName("Scenario_settings");

            foreach (XmlNode transformInfo in transformList_3)
            {
                XmlNodeList transformcontent = transformInfo.ChildNodes;

                foreach (XmlNode transformItens in transformcontent)
                {
                    if (transformItens.Name == "start_Wii_Duration")
                    {
                        start_Wii.duration = float.Parse(transformItens.InnerText);
                        start_Wii_Slider.value = start_Wii.duration;
                    }
                    if (transformItens.Name == "projectUser_Wii_Duration")
                    {
                        projectUser_Wii.duration = float.Parse(transformItens.InnerText);
                        projectUser_Wii_Slider.value = projectUser_Wii.duration;
                    }
                    if (transformItens.Name == "freeFly_Wii_Duration")
                    {
                        freeFly_Wii.duration = float.Parse(transformItens.InnerText);
                        freeFly_Wii_Slider.value = freeFly_Wii.duration;
                    }
                    if (transformItens.Name == "gates_Wii_Duration")
                    {
                        gates_Wii.duration = float.Parse(transformItens.InnerText);
                        gates_Wii_Slider.value = gates_Wii.duration;
                    }
                    if (transformItens.Name == "end_Wii_Duration")
                    {
                        end_Wii.duration = float.Parse(transformItens.InnerText);
                        end_Wii_Slider.value = end_Wii.duration;
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
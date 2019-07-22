using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;


public class XML_Avatar : MonoBehaviour
{
    private string SavingFileName, LoadingFileName;
    public string defaultSettingsName;

    public Slider distance_Slider, altitude_Slider, maxBascule_Slider, vitesseDeRotationModeLibre_Slider, vitesseDeRotationModeCanyon_Slider, angleDeRotationMax_Slider,
                  altitudeModeLibre_Slider, vitesseDeVol_Slider, start_Avatar_Slider, projectUser_Avatar_Slider, takeOff_Avatar_Slider, dive_Avatar_Slider, freeFly_Avatar_1_Slider,
                  canyon_Avatar_Slider, freeFly_Avatar_2_Slider, storm_Avatar_Slider, freeFly_Avatar_3_Slider, freeze_Avatar_Slider, end_Avatar_Slider, altitudeFactor_Slider, dimensionMeteores_Slider;

    public Transform TPSTransform;
    
    private float cameraDistance = 0;
    private float cameraAltitude = 0;

    public Transform scenarioItems;

    private Start_Avatar start_Avatar;
    private ProjectUser_Avatar projectUser_Avatar;
    private TakeOff_Avatar takeOff_Avatar;
    private Dive_Avatar dive_Avatar;
    private FreeFly_Avatar freeFly_Avatar_1, freeFly_Avatar_2, freeFly_Avatar_3;
    private Canyon_Avatar canyon_Avatar;
    private Storm_Avatar storm_Avatar;
    private Freeze_Avatar freeze_Avatar;
    private End_Avatar end_Avatar;

    private ArmController armController;
     
    void Awake()
    {
        Transform lTemp;

        lTemp = scenarioItems.Find("Start_Avatar");
        start_Avatar = lTemp.GetComponent<Start_Avatar>();
        
        lTemp = scenarioItems.Find("ProjectUser_Avatar");
        projectUser_Avatar = lTemp.GetComponent<ProjectUser_Avatar>();

        lTemp = scenarioItems.Find("TakeOff_Avatar");
        takeOff_Avatar = lTemp.GetComponent<TakeOff_Avatar>();
   
        lTemp = scenarioItems.Find("Dive_Avatar");
        dive_Avatar = lTemp.GetComponent<Dive_Avatar>();     
  
        lTemp = scenarioItems.Find("FreeFly_Avatar_1");
        freeFly_Avatar_1 = lTemp.GetComponent<FreeFly_Avatar>();     

        lTemp = scenarioItems.Find("Canyon_Avatar");
        canyon_Avatar = lTemp.GetComponent<Canyon_Avatar>();        
      
        lTemp = scenarioItems.Find("FreeFly_Avatar_2");
        freeFly_Avatar_2 = lTemp.GetComponent<FreeFly_Avatar>();
         
        lTemp = scenarioItems.Find("Storm_Avatar");
        storm_Avatar = lTemp.GetComponent<Storm_Avatar>();
            
        lTemp = scenarioItems.Find("FreeFly_Avatar_3");
        freeFly_Avatar_3 = lTemp.GetComponent<FreeFly_Avatar>();

        lTemp = scenarioItems.Find("Freeze_Avatar");
        freeze_Avatar = lTemp.GetComponent<Freeze_Avatar>();

        lTemp = scenarioItems.Find("End_Avatar");
        end_Avatar = lTemp.GetComponent<End_Avatar>();
        
        armController = CharacterControllerBasedOnAxis.instance.GetComponent<ArmController>();
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

                XmlElement maxBascule = xmlDoc.CreateElement("Bascule");
                maxBascule.InnerText = CharacterControllerBasedOnAxis.instance.maxBascule.ToString();

                XmlElement armRegularRotationSpeed = xmlDoc.CreateElement("Vitesse_rotation_mode_libre");
                armRegularRotationSpeed.InnerText = CharacterControllerBasedOnAxis.instance.armRegularRotationSpeed.ToString();
        
                XmlElement armSlowRotationSpeed = xmlDoc.CreateElement("Vitesse_rotation_mode_canyon");
                armSlowRotationSpeed.InnerText = CharacterControllerBasedOnAxis.instance.armSlowRotationSpeed.ToString();

                XmlElement flightMaxAngle = xmlDoc.CreateElement("Angle_de_rotation_max");
                flightMaxAngle.InnerText = CharacterControllerBasedOnAxis.instance.flightMaxAngle.ToString();

                XmlElement altitude = xmlDoc.CreateElement("Altitude_mode_libre");
                altitude.InnerText = CharacterControllerBasedOnAxis.instance.altitude.ToString();
        
                XmlElement vitesseDeVol = xmlDoc.CreateElement("Vitesse_de_vol");
                vitesseDeVol.InnerText = CharacterControllerBasedOnAxis.instance.speed.ToString();

                XmlElement facteurDAltitude = xmlDoc.CreateElement("Facteur_d_altitude");
                facteurDAltitude.InnerText = armController.altitudeFactor.ToString();

                XmlElement dimensionMeteores = xmlDoc.CreateElement("Dimension_meteores");
                dimensionMeteores.InnerText = storm_Avatar.meteorSize.ToString();
    
                elmNew_2.AppendChild(maxBascule);
                elmNew_2.AppendChild(armRegularRotationSpeed);
                elmNew_2.AppendChild(armSlowRotationSpeed);
                elmNew_2.AppendChild(flightMaxAngle);
                elmNew_2.AppendChild(altitude);
                elmNew_2.AppendChild(vitesseDeVol);
                elmNew_2.AppendChild(facteurDAltitude);
                elmNew_2.AppendChild(dimensionMeteores);

                elmRoot.AppendChild(elmNew_2);
        
            XmlElement elmNew_3 = xmlDoc.CreateElement("Scenario_settings");

                XmlElement start_Avatar_Duration = xmlDoc.CreateElement("start_Avatar_Duration");
                start_Avatar_Duration.InnerText = start_Avatar.duration.ToString();

                XmlElement projectUser_Avatar_Duration = xmlDoc.CreateElement("projectUser_Avatar_Duration");
                projectUser_Avatar_Duration.InnerText = projectUser_Avatar.duration.ToString();

                XmlElement takeOff_Avatar_Duration = xmlDoc.CreateElement("takeOff_Avatar_Duration");
                takeOff_Avatar_Duration.InnerText = takeOff_Avatar.duration.ToString();

                XmlElement dive_Avatar_Duration = xmlDoc.CreateElement("dive_Avatar_Duration");
                dive_Avatar_Duration.InnerText = dive_Avatar.duration.ToString();

                XmlElement freeFly_Avatar_1_Duration = xmlDoc.CreateElement("freeFly_Avatar_1_Duration");
                freeFly_Avatar_1_Duration.InnerText = freeFly_Avatar_1.duration.ToString();

                XmlElement Canyon_Avatar_Duration = xmlDoc.CreateElement("Canyon_Avatar_Duration");
                Canyon_Avatar_Duration.InnerText = canyon_Avatar.duration.ToString();

                XmlElement freeFly_Avatar_2_Duration = xmlDoc.CreateElement("freeFly_Avatar_2_Duration");
                freeFly_Avatar_2_Duration.InnerText = freeFly_Avatar_2.duration.ToString();

                XmlElement storm_Avatar_Duration = xmlDoc.CreateElement("storm_Avatar_Duration");
                storm_Avatar_Duration.InnerText = storm_Avatar.duration.ToString();

                XmlElement freeFly_Avatar_3_Duration = xmlDoc.CreateElement("freeFly_Avatar_3_Duration");
                freeFly_Avatar_3_Duration.InnerText = freeFly_Avatar_3.duration.ToString();

                XmlElement freeze_Avatar_Duration = xmlDoc.CreateElement("freeze_Avatar_Duration");
                freeze_Avatar_Duration.InnerText = freeze_Avatar.duration.ToString();

                XmlElement end_Avatar_Duration = xmlDoc.CreateElement("end_Avatar_Duration");
                end_Avatar_Duration.InnerText = end_Avatar.duration.ToString();

                elmNew_3.AppendChild(start_Avatar_Duration);
                elmNew_3.AppendChild(projectUser_Avatar_Duration);
                elmNew_3.AppendChild(takeOff_Avatar_Duration);
                elmNew_3.AppendChild(dive_Avatar_Duration);
                elmNew_3.AppendChild(freeFly_Avatar_1_Duration);
                elmNew_3.AppendChild(Canyon_Avatar_Duration);
                elmNew_3.AppendChild(freeFly_Avatar_2_Duration);
                elmNew_3.AppendChild(storm_Avatar_Duration);
                elmNew_3.AppendChild(freeFly_Avatar_3_Duration);
                elmNew_3.AppendChild(freeze_Avatar_Duration);
                elmNew_3.AppendChild(end_Avatar_Duration);

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
                    if (transformItens.Name == "Bascule")
                    {
                        CharacterControllerBasedOnAxis.instance.maxBascule = float.Parse(transformItens.InnerText);
                        maxBascule_Slider.value = CharacterControllerBasedOnAxis.instance.maxBascule;
                    }
                    if (transformItens.Name == "Vitesse_rotation_mode_libre")
                    {
                        CharacterControllerBasedOnAxis.instance.armRegularRotationSpeed = float.Parse(transformItens.InnerText);
                        vitesseDeRotationModeLibre_Slider.value = CharacterControllerBasedOnAxis.instance.armRegularRotationSpeed;
                    }
                    if (transformItens.Name == "Vitesse_rotation_mode_canyon")
                    {
                        CharacterControllerBasedOnAxis.instance.armSlowRotationSpeed = float.Parse(transformItens.InnerText);
                        vitesseDeRotationModeCanyon_Slider.value = CharacterControllerBasedOnAxis.instance.armSlowRotationSpeed;
                    }
                    if (transformItens.Name == "Angle_de_rotation_max")
                    {
                        CharacterControllerBasedOnAxis.instance.flightMaxAngle = float.Parse(transformItens.InnerText);
                        angleDeRotationMax_Slider.value = CharacterControllerBasedOnAxis.instance.flightMaxAngle;
                    }
                    if (transformItens.Name == "Altitude_mode_libre")
                    {
                        CharacterControllerBasedOnAxis.instance.altitude = float.Parse(transformItens.InnerText);
                        altitudeModeLibre_Slider.value = CharacterControllerBasedOnAxis.instance.altitude;
                    }
                    if (transformItens.Name == "Vitesse_de_vol")
                    {
                        CharacterControllerBasedOnAxis.instance.speed = float.Parse(transformItens.InnerText);
                        vitesseDeVol_Slider.value = CharacterControllerBasedOnAxis.instance.speed;
                    }
                    if (transformItens.Name == "Facteur_d_altitude")
                    {
                        armController.altitudeFactor = float.Parse(transformItens.InnerText);
                        altitudeFactor_Slider.value = armController.altitudeFactor;
                    }
                    if (transformItens.Name == "Dimension_meteores")
                    {
                        storm_Avatar.meteorSize = float.Parse(transformItens.InnerText);
                        dimensionMeteores_Slider.value = storm_Avatar.meteorSize;
                    }
                }
            }
            
            XmlNodeList transformList_3 = xmlDoc.GetElementsByTagName("Scenario_settings");

            foreach (XmlNode transformInfo in transformList_3)
            {
                XmlNodeList transformcontent = transformInfo.ChildNodes;

                foreach (XmlNode transformItens in transformcontent)
                {
                    if (transformItens.Name == "start_Avatar_Duration")
                    {
                        start_Avatar.duration = float.Parse(transformItens.InnerText);
                        start_Avatar_Slider.value = start_Avatar.duration;
                    }
                    if (transformItens.Name == "projectUser_Avatar_Duration")
                    {
                        projectUser_Avatar.duration = float.Parse(transformItens.InnerText);
                        projectUser_Avatar_Slider.value = projectUser_Avatar.duration;
                    }
                    if (transformItens.Name == "takeOff_Avatar_Duration")
                    {
                        takeOff_Avatar.duration = float.Parse(transformItens.InnerText);
                        takeOff_Avatar_Slider.value = takeOff_Avatar.duration;
                    }
                    if (transformItens.Name == "dive_Avatar_Duration")
                    {
                        dive_Avatar.duration = float.Parse(transformItens.InnerText);
                        dive_Avatar_Slider.value = dive_Avatar.duration;
                    }
                    if (transformItens.Name == "freeFly_Avatar_1_Duration")
                    {
                        freeFly_Avatar_1.duration = float.Parse(transformItens.InnerText);
                        freeFly_Avatar_1_Slider.value = freeFly_Avatar_1.duration;
                    }
                    if (transformItens.Name == "Canyon_Avatar_Duration")
                    {
                        canyon_Avatar.duration = float.Parse(transformItens.InnerText);
                        canyon_Avatar_Slider.value = canyon_Avatar.duration;
                    }
                    if (transformItens.Name == "freeFly_Avatar_2_Duration")
                    {
                        freeFly_Avatar_2.duration = float.Parse(transformItens.InnerText);
                        freeFly_Avatar_2_Slider.value = freeFly_Avatar_2.duration;
                    }
                    if (transformItens.Name == "storm_Avatar_Duration")
                    {
                        storm_Avatar.duration = float.Parse(transformItens.InnerText);
                        storm_Avatar_Slider.value = storm_Avatar.duration;
                    }
                    if (transformItens.Name == "freeFly_Avatar_3_Duration")
                    {
                        freeFly_Avatar_3.duration = float.Parse(transformItens.InnerText);
                        freeFly_Avatar_3_Slider.value = freeFly_Avatar_3.duration;
                    }
                    if (transformItens.Name == "freeze_Avatar_Duration")
                    {
                        freeze_Avatar.duration = float.Parse(transformItens.InnerText);
                        freeze_Avatar_Slider.value = freeze_Avatar.duration;
                    }
                    if (transformItens.Name == "end_Avatar_Duration")
                    {
                        end_Avatar.duration = float.Parse(transformItens.InnerText);
                        end_Avatar_Slider.value = end_Avatar.duration;
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
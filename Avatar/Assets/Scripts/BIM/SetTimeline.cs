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
    public GameObject sliderInfo;
    public Transform Screen;

    // Use this for initialization
    void Start()
    {
        sliderInfo = Resources.Load("Slider-info") as GameObject;
        LoadFromXml();
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
            Debug.Log("Open XML file " + LoadingFileName);
            xmlDoc.Load(filepath);
            // xmlDoc.GetElementsByTagName("scenarioitem");
            XmlNodeList contentlist = xmlDoc.LastChild.ChildNodes; // 2 nodes : scenarioitems & infos
            XmlNodeList scenarioitems, infos;
            Debug.Log("Nom item 0 : " + contentlist.Item(0).Name);
            Debug.Log("Nom item 1 : " + contentlist.Item(1).Name);
            foreach (XmlNode node in contentlist)
            {
                if (node.Name == "scenarioitems")
                {
                    scenarioitems = node.ChildNodes;
                }
                else if (node.Name == "infos")
                {
                    infos = node.ChildNodes;

                    foreach (XmlNode info in infos)
                    {
                        GameObject tmpSlider;
                        foreach (XmlNode attribut in info.ChildNodes)
                        {
                            tmpSlider = Instantiate(sliderInfo, Screen);
                            if (attribut.Name == "info_label")
                            {
                            }
                            else if (attribut.Name == "info_timestamp")
                            {
                                tmpSlider.GetComponent<Slider>().value = float.Parse(attribut.InnerText);
                                Debug.Log("xml values : " + attribut.InnerText);
                            }
                            else if (attribut.Name == "text")
                            {
                            }

                        }


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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Xml;
using System.IO;
using UnityEngine.Video;

public class SetTimeline : MonoBehaviour
{

    public string LoadingFileName = "timeline";

    public GameObject sliderInfo;
    public GameObject labelInfo;
    public GameObject textBlockLeft;
    public GameObject textBlockTop;

    public Transform Screen;
    public VideoClip videoClip;
    private float _videoLenght;

    public List<InfoStructure> infosList;

    // Use this for initialization
    void Start()
    {
        sliderInfo = Resources.Load("Slider-info") as GameObject;
        labelInfo = Resources.Load("Label-info") as GameObject;
        textBlockLeft = Resources.Load("TextBlock-left") as GameObject;
        textBlockTop = Resources.Load("TextBlock-top") as GameObject;

        infosList = new List<InfoStructure>();

        _videoLenght = (float)videoClip.length;

        LoadFromXml();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayLabel(GameObject area)
    {
        foreach (InfoStructure info in infosList)
        {
            if (info.sliderRect == area)
            {
                info.labelRect.SetActive(true);
                info.textRect.SetActive(true);
                return;
            }
        }
    }

    public void HideLabel(GameObject area)
    {
        foreach (InfoStructure info in infosList)
        {
            if (info.sliderRect == area)
            {
                info.labelRect.SetActive(false);
                info.textRect.SetActive(false);
                return;
            }
        }
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
                        InfoStructure tmpInfo = new InfoStructure();
                        tmpInfo.labelRect = Instantiate(labelInfo, Screen);
                        tmpInfo.sliderRect = Instantiate(sliderInfo, Screen);

                        foreach (XmlNode attribut in info.ChildNodes)
                        {
                            if (attribut.Name == "info_label")
                            {
                                tmpInfo.labelRect.transform.GetChild(0).GetComponent<Text>().text = attribut.InnerText;
                            }
                            else if (attribut.Name == "info_timestamp")
                            {
                                tmpInfo.sliderRect.GetComponent<Slider>().maxValue = _videoLenght;
                                tmpInfo.sliderRect.GetComponent<Slider>().value = float.Parse(attribut.InnerText);
                                Debug.Log("xml values : " + attribut.InnerText);
                                tmpInfo.labelRect.transform.position = new Vector3(tmpInfo.sliderRect.GetComponent<Slider>().handleRect.position.x, tmpInfo.labelRect.transform.position.y, tmpInfo.labelRect.transform.position.z);
                            }
                            else if (attribut.Name == "text")
                            {
                                if (attribut.InnerText.Length < 400) // max characters
                                {
                                    tmpInfo.textRect = Instantiate(textBlockTop, Screen);
                                }
                                else
                                {
                                    tmpInfo.textRect = Instantiate(textBlockLeft, Screen);
                                }
                                tmpInfo.textRect.transform.GetChild(0).GetComponent<Text>().text = attribut.InnerText;
                            }
                        }
                        infosList.Add(tmpInfo);
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

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
    public GameObject sliderScenario;
    public GameObject labelInfo;
    public GameObject labelItem;
    public GameObject textBlockLeft;
    public GameObject textBlockTop;

    public Transform Screen;
    public VideoClip videoClip;
    private float _videoLenght;
    public VideoPlayer videoPlayer;

    public List<InfoStructure> infosList;

    public InfoStructure currentInfo;
    public InfoStructure lastInfo;

    public GameObject infoButton;
    public GameObject cameraButton;

    private bool isDisplaying = false;

    public ChangeTimeScale timeScaler;

    // Use this for initialization
    void Start()
    {
        sliderInfo = Resources.Load("Slider-info") as GameObject;
        sliderScenario = Resources.Load("Slider-scenario") as GameObject;
        labelInfo = Resources.Load("Label-info") as GameObject;
        labelItem = Resources.Load("Label-item") as GameObject;
        textBlockLeft = Resources.Load("TextBlock-left") as GameObject;
        textBlockTop = Resources.Load("TextBlock-top") as GameObject;

        infosList = new List<InfoStructure>();

        _videoLenght = (float)videoClip.length;

        LoadFromXml();
    }

    // Update is called once per frame
    void Update()
    {
        // Manage buttons and text blocks display
        if (videoPlayer != null)
        {
            infoButton.SetActive(false);
            cameraButton.SetActive(true);

            foreach (InfoStructure info in infosList)
            {
                if (videoPlayer.time >= info.timestamp && videoPlayer.time <= info.timestamp + 3)
                {
                    infoButton.SetActive(true); // display button
                    cameraButton.SetActive(false);

                    // Automatic mode
                    currentInfo = info;
                    if (!isDisplaying && currentInfo != lastInfo)
                    {
                        StartCoroutine(DispayInfoText(currentInfo));
                    }
                    return;
                }
            }
        }
    }

    public IEnumerator DispayInfoText(InfoStructure currentInfo)
    {
        lastInfo = currentInfo;
        isDisplaying = true;
        currentInfo.textRect.SetActive(true);
        timeScaler.pauseGame();

        yield return new WaitForSecondsRealtime(20f);

        timeScaler.playGame();
        currentInfo.textRect.SetActive(false);
        isDisplaying = false;
    }

    public void DisplayLabel(GameObject area)
    {
        foreach (InfoStructure info in infosList)
        {
            if (info.sliderRect == area)
            {
                info.labelRect.SetActive(true);
                //info.textRect.SetActive(true);
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
                //info.textRect.SetActive(false);
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

            XmlNodeList contentlist = xmlDoc.LastChild.ChildNodes; // 2 nodes : scenarioitems & infos
            XmlNodeList scenarioitems, infos;

            foreach (XmlNode node in contentlist)
            {
                if (node.Name == "scenarioitems")
                {
                    scenarioitems = node.ChildNodes;

                    foreach (XmlNode item in scenarioitems)
                    {
                        InfoStructure tmpInfo = new InfoStructure();
                        tmpInfo.labelRect = Instantiate(labelItem, Screen);
                        tmpInfo.sliderRect = Instantiate(sliderScenario, Screen);

                        foreach (XmlNode attribut in item.ChildNodes)
                        {
                            if (attribut.Name == "label")
                            {
                                tmpInfo.labelRect.transform.GetChild(0).GetComponent<Text>().text = attribut.InnerText;
                            }
                            else if (attribut.Name == "scenarioitem_timestamp")
                            {
                                tmpInfo.sliderRect.GetComponent<Slider>().maxValue = _videoLenght;
                                tmpInfo.sliderRect.GetComponent<Slider>().value = float.Parse(attribut.InnerText);
                                tmpInfo.labelRect.transform.position = new Vector3(tmpInfo.sliderRect.GetComponent<Slider>().handleRect.position.x, tmpInfo.labelRect.transform.position.y, tmpInfo.labelRect.transform.position.z);
                            }
                        }
                        infosList.Add(tmpInfo);
                    }
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
                                tmpInfo.labelRect.transform.position = new Vector3(tmpInfo.sliderRect.GetComponent<Slider>().handleRect.position.x, tmpInfo.labelRect.transform.position.y, tmpInfo.labelRect.transform.position.z);
                                tmpInfo.timestamp = float.Parse(attribut.InnerText);
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

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
    //public GameObject exitInfoButton;

    public GameObject playButton;
    public GameObject pauseButton;

    private bool isDisplaying = false;

    public ChangeTimeScale timeScaler;

    private GameObject tmpCurrentItem;

    public bool enableAutomaticMode = false;
    private bool _exitInfo = false;
    

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

        tmpCurrentItem = new GameObject();

        //exitInfoButton.SetActive(false);

        LoadFromXml();
    }

    // Update is called once per frame
    void Update()
    {
        // Manage buttons and text blocks display
        if (videoPlayer != null)
        {
            if (enableAutomaticMode)
            {
                infoButton.SetActive(false);
            }


            foreach (InfoStructure info in infosList)
            {
                if (info.type == "info")
                {
                    if (videoPlayer.time >= info.timestamp && videoPlayer.time <= info.timestamp + 3)
                    {

                        currentInfo = info;
                        // Automatic mode
                        if (enableAutomaticMode)
                        {
                            infoButton.SetActive(true); // display button
                            cameraButton.SetActive(false);

                            if (!isDisplaying && currentInfo != lastInfo)
                            {
                                StartCoroutine(DispayInfoText(currentInfo));
                            }
                            return;
                        }
                    }
                }
                else if (info.type == "item")
                {
                    //info.sliderRect.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(false);
                    if (videoPlayer.time >= info.timestamp)
                    {
                        //tmpCurrentItem = info.sliderRect.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
                        info.sliderRect.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = Color.white;
                        //Debug.Log("Couleur circle : " + info.sliderRect.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<RawImage>().color);
                    }
                }
            }
            //tmpCurrentItem.SetActive(true);
        }
    }

    public void DisplayCurrentInfo()
    {
        if (currentInfo != null)
        {
            pauseButton.SetActive(false);
            playButton.SetActive(true);
            cameraButton.SetActive(false);
            StartCoroutine(DispayInfoText(currentInfo));
        }
    }

    public void ExitInfo()
    {
        _exitInfo = true;
        //exitInfoButton.SetActive(false);
    }

    public void playBIM()
    {
        if (_exitInfo == false)
        {
            _exitInfo = true;
        }
        playButton.SetActive(false);
        pauseButton.SetActive(true);
        timeScaler.playGame();
    }

    public void pauseBIM()
    {
        playButton.SetActive(true);
        pauseButton.SetActive(false);
        timeScaler.pauseGame();
    }

    public IEnumerator DispayInfoText(InfoStructure currentInfo)
    {
        //exitInfoButton.SetActive(true);
        lastInfo = currentInfo;
        isDisplaying = true;
        currentInfo.textRect.SetActive(true);
        timeScaler.pauseGame();

        if (enableAutomaticMode)
        {
            yield return new WaitForSecondsRealtime(20f);
        }
        else
        {
            while (!_exitInfo)
            {
                yield return null;
            }
        }

        timeScaler.playGame();
        currentInfo.textRect.SetActive(false);
        isDisplaying = false;

        cameraButton.SetActive(true);
        _exitInfo = false;
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
                                tmpInfo.timestamp = float.Parse(attribut.InnerText);
                                tmpInfo.sliderRect.GetComponent<Slider>().maxValue = _videoLenght;
                                tmpInfo.sliderRect.GetComponent<Slider>().value = float.Parse(attribut.InnerText);
                                tmpInfo.labelRect.transform.position = new Vector3(tmpInfo.sliderRect.GetComponent<Slider>().handleRect.position.x, tmpInfo.labelRect.transform.position.y, tmpInfo.labelRect.transform.position.z);
                            }
                        }
                        tmpInfo.type = "item";
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
                        tmpInfo.type = "info";
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

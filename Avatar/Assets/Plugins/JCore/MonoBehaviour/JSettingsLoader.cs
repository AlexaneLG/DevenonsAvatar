using UnityEngine;
using System.Collections;
using System;
using System.IO;

/// <summary>
/// This class loads a settings file at start, and uses Reflection to modify public fields and properties of Uniy GameObjects
/// </summary>
public class JSettingsLoader : JMonoBehaviour
{

    #region SINGLETON
    private static JSettingsLoader onlyInstance = null;

    public static JSettingsLoader Instance
    {
        get
        {
            if (onlyInstance == null)
            {
                onlyInstance = FindObjectOfType(typeof(JSettingsLoader)) as JSettingsLoader;
                if (onlyInstance == null)
                    Debug.LogError("Could not locate a SettingsLoader object. You have to have exactly one SettingsLoader in the scene.");
            }
            return onlyInstance;
        }
    }

    #endregion

    #region Fields

    //public for in editor setting
    public string settings = "settings.xml";
    public bool UseInEditorOverride = true;
    public string inEditorOverride = "settings.xml";

    private string _xmlSettings = "";
    private string _path;

    #endregion

    public event EventHandler OnSettingsLoaded;

    void Awake()
    {

        /*
#if !UNITY_STANDALONE_WIN
        settings = PlayerPrefs.GetString("SettingsPath", settings);
        DebugUI.Instance.RegisterUI(new ModifySettingsPath("Settings Path"));
#endif
         */
        //determine settings full path
        string appPath = Application.streamingAssetsPath;

        //remove last folder (in editor, it is "assets", in EXE, it is "Name_data")
        //appPath = appPath.Substring(0, appPath.LastIndexOf("/"));

        if (UseInEditorOverride && Application.isEditor)
        {
            settings = inEditorOverride;
            if (inEditorOverride.StartsWith("file://"))
                _path = inEditorOverride;
            else
                _path = appPath + "/" + inEditorOverride;
        }
        else
        {
            if (settings.StartsWith("http:"))
                _path = settings;
            else
            {
                if (Application.isWebPlayer)
                    _path = Application.absoluteURL.Substring(0, _path.LastIndexOf("/") + 1) + "/" + settings;
                else
                {
                    if (settings.StartsWith("file://"))
                        _path = settings;
                    else
                        _path = appPath + "/" + settings;
                }
            }
        }

        if (settings == "")
        {
            //do nothing, just raise event

            if (OnSettingsLoaded != null)
                OnSettingsLoaded(this, new EventArgs());
            return;
        }
		//prevent browser cache
		if (_path.StartsWith("http://"))
			_path+="?v="+UnityEngine.Random.value;
			
        JLog("Loading settings from: " + _path);

        if (_path.StartsWith("http://"))
            StartCoroutine(DownloadSettings());
        else
        {
            if (_path.StartsWith("file:"))
                _path=_path.Replace("file://","");
            if (File.Exists(_path))
            {
                string txt = File.ReadAllText(_path);
                HandleSettings(txt);
            }
            else
                Debug.LogError("settings file not found: " + _path);
        }
    }

    IEnumerator DownloadSettings()
    {
        WWW www = new WWW(_path);
        yield return www;
        if (www.error != null)
        {
            JLog("Error trying to load settings file at " + _path + " : " + www.error);
        }
        else
            HandleSettings(www.text);
    }

    public void Process(string key)
    {
        JSettings.ProcessXmlSettings(_xmlSettings, key);
    }

    public void SaveSettingsPath()
    {
        PlayerPrefs.SetString("SettingsPath", settings);
    }

    void HandleSettings(string iTxt)
    {
        _xmlSettings = iTxt;
        JSettings.ProcessXmlSettings(_xmlSettings, "default");
        if (OnSettingsLoaded != null)
            OnSettingsLoaded(this, new EventArgs());
    }

    class ModifySettingsPath : JDebuggerMenu
    {
        public ModifySettingsPath(string name)
            : base(name)
        {
        }

        public override void Display()
        {
            JSettingsLoader.Instance.settings = GUILayout.TextField(JSettingsLoader.Instance.settings);
            if (GUILayout.Button("Save"))
            {
                JSettingsLoader.Instance.SaveSettingsPath();
                visible = false;
            }
        }
    }
}

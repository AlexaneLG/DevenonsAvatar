using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Debugging tools including:
/// - on screen console
/// - logs and exception logs, with write to disk capability
/// - basic framework for creating
/// </summary>
public class JDebugger : MonoBehaviour
{
    #region Fields

    public bool showExceptionsOnScreen = false; //log exceptions in console at runtime and bring up the on screen console
    public bool enableConsoleLog = true; //enable logging history
    public bool showDebugButton = true;
    public bool showFPS = false;
    public string logFilePath = "errorLog.txt";
    public GUIStyle backgroundStyle;


    public bool ShowLogHistory
    {
        get { return _showLogHistory; }
        set {
            _showLogHistory = value;
            if (_showLogHistory) _inspectedLogEntry = null;
        }
    }
    private bool _showLogHistory = false;

    private List<LogEntry> _entries = new List<LogEntry>();
    private LogEntry _inspectedLogEntry;

    private bool _showErrorsOnly = false;
    private FPS _fpsCounter;

    private bool _showDebugUI = false;
    public int debugMenuWidth = 200;

    const int DEBUG_MINIMIZED_HEIGHT = 40;
    private float _uiScaler = 1;

    private Texture2D _background;

    private List<JDebuggerMenu> _debugMenus = new List<JDebuggerMenu>();
    private string _logDisplayFilter = "";

    #endregion

    #region SINGLETON
    private static JDebugger _onlyInstance = null;

    static JDebugger Instance
    {
        get
        {
            if (_onlyInstance == null)
                _onlyInstance = FindObjectOfType(typeof(JDebugger)) as JDebugger;
            return _onlyInstance;
        }
    }
    #endregion

    #region UnityLoop

    void Awake()
    {
        Application.RegisterLogCallback(OnDebugLog);
        _background = new Texture2D(1, 1);
        _background.SetPixel(0,0,new Color(0f,0f,0f,0.5f));
        _background.Apply();
        backgroundStyle.normal.background = _background;
    }

    void Update()
    {
        //if pressing tab + D, show/hide log
        if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.Tab))
            _showLogHistory = !_showLogHistory;

        //fps
        if (showFPS)
        {
            if (_fpsCounter == null)
                _fpsCounter = new FPS();
            _fpsCounter.Update();
        }
        else
            if (_fpsCounter != null)
                _fpsCounter = null;
    }

    void OnGUI()
    {
        if (enableConsoleLog && (showDebugButton || _fpsCounter != null))
        {
            GUILayout.BeginArea(new Rect(0, 0, debugMenuWidth, DEBUG_MINIMIZED_HEIGHT), "", backgroundStyle);
            GUILayout.BeginHorizontal();
            if (showDebugButton)
            {
                _showLogHistory = GUILayout.Toggle(_showLogHistory, "Log");
                _showDebugUI = GUILayout.Toggle(_showDebugUI, "Debug");
            }
            if (_fpsCounter != null)
                GUILayout.Label("FPS: " + _fpsCounter.CurrentFPS);
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        // display log history
        if (_showLogHistory)
            DisplayLogHistory();
        else
            if (_showDebugUI)
                DisplayDebugUI();
    }
    #endregion

    #region Log Entries

    class LogEntry
    {
        public string condition;
        public string stacktrace;
        public LogType type;

        public LogEntry(string iCondition, string iStacktrace, LogType iType)
        {
            condition = iCondition;
            stacktrace = iStacktrace;
            type = iType;
        }
    }

    void OnDebugLog(string condition, string stacktrace, LogType type)
    {
        _entries.Add(new LogEntry(condition, stacktrace, type));
        if (enableConsoleLog && showExceptionsOnScreen && (type==LogType.Error || type==LogType.Assert || type==LogType.Exception))
        {
            _showErrorsOnly = true;
            _showLogHistory = true;
        }
    }

    void ClearLog()
    {
        _entries = new List<LogEntry>();
    }

    void DisplayLogHistory()
    {
        Rect textArea = new Rect(DebugScrollViewRect().xMin, DebugScrollViewRect().yMin, DebugScrollViewRect().width - 30, (10 + GUI.skin.GetStyle("label").CalcHeight(new GUIContent("bla"), 200)) * _entries.Count);
        GUILayout.BeginArea(DebugWindowRect(), "", backgroundStyle);
        _debugViewScrollVector = GUI.BeginScrollView(DebugScrollViewRect(), _debugViewScrollVector, textArea);

        foreach (LogEntry entry in _entries)
        {
            if (_showErrorsOnly && entry.type != LogType.Error && entry.type != LogType.Exception && entry.type != LogType.Assert)
                continue;

            //use filter if available
            if (_logDisplayFilter != "" && !entry.condition.ToLower().Contains(_logDisplayFilter))
                continue;

            if (GUILayout.Button(entry.condition, "label"))
            {
                if (_inspectedLogEntry == entry)
                    _inspectedLogEntry = null;
                else
                    _inspectedLogEntry = entry;
            }
            if (_inspectedLogEntry == entry)
            {
                GUILayout.Label(entry.type + " --->  " + entry.stacktrace);
            }
        }

        GUI.EndScrollView();

        //bottom tools

        GUILayout.BeginArea(DebugWindowToolbarRect());
        GUILayout.BeginHorizontal();

        GUILayout.Label("Filter:", GUILayout.Width(40));
        _logDisplayFilter = GUILayout.TextField(_logDisplayFilter, GUILayout.Width(100));
        _showErrorsOnly = GUILayout.Toggle(_showErrorsOnly, "Errors only", GUILayout.Width(100));


        if (GUILayout.Button("Dump To File", GUILayout.Width(90)))
        {
            if (!logFilePath.Contains(":"))
                logFilePath = Application.dataPath + "/" + logFilePath;
            try
            {
                string text = "";
                foreach (LogEntry entry in _entries)
                    text += entry.condition + "\n";
                File.WriteAllText(logFilePath, text);
            }
            catch
            {
            }
        }
        logFilePath = GUILayout.TextField(logFilePath);

        if (GUILayout.Button("Clear", GUILayout.Width(45)))
            ClearLog();

        if (GUILayout.Button("Close", GUILayout.Width(45)))
            _showLogHistory = false;
        GUILayout.EndHorizontal();
        GUILayout.EndArea();


        GUILayout.EndArea();
    }

    #endregion

    #region FPS Counter

    class FPS
    {
        float[] _frameTime = new float[100];
        int _frameIndex = 0;

        public FPS()
        {
            
        }

        public void Update()
        {
            _frameTime[_frameIndex++] = Time.deltaTime;
            _frameIndex = _frameIndex % 100;
        }

        public int CurrentFPS
        {
            get
            {
                float f = 0;
                for (int i = 0; i < _frameTime.Length; i++)
                    f += _frameTime[i];
                return Mathf.FloorToInt(_frameTime.Length / f);
            }
        }

    }

    #endregion

    #region UI helpers

    Vector2 _debugViewScrollVector = Vector2.zero;

    Rect DebugWindowRect()
    {
        return new Rect(Screen.width / 10, Screen.height / 10, (int)(Screen.width * 0.8f), (int)(Screen.height * .8f));
    }

    Rect DebugWindowToolbarRect()
    {
        Rect win = DebugWindowRect();
        return new Rect(2, win.height -25, win.width - 4, 23);
    }

    Rect DebugScrollViewRect()
    {
        Rect win = DebugWindowRect();
        return new Rect(2, 2, win.width - 25, win.height - 30);
    }

    #endregion

    #region DebugMenus

    public static void RegisterMenu(JDebuggerMenu menu)
    {
        if (Instance != null)
            _onlyInstance._debugMenus.Add(menu);
    }

    public static void UnregisterMenu(JDebuggerMenu menu)
    {
        if (Instance != null)
            _onlyInstance._debugMenus.Remove(menu);
    }

    void DisplayDebugUI()
    {

/* code for upscaling on ipad?
#if !UNITY_STANDALONE_WIN
        //scaler
        GUI.skin.button.fixedHeight = 20 * UIScaler;
        GUI.skin.toggle.fixedHeight = 20 * UIScaler;
        GUI.skin.horizontalSlider.fixedHeight = 20 * UIScaler;
        GUI.skin.horizontalSliderThumb.fixedHeight = 20 * UIScaler;
#endif
 */

        //display our vertical list
        if (_showDebugUI)
        {
            GUILayout.BeginArea(new Rect(0, DEBUG_MINIMIZED_HEIGHT, debugMenuWidth * _uiScaler, Screen.height - DEBUG_MINIMIZED_HEIGHT), backgroundStyle);

            foreach (JDebuggerMenu m in _debugMenus)
            {
                m.visible = GUILayout.Toggle(m.visible, m.title);
                if (m.visible)
                {
                    //indent and display content
                    GUILayout.BeginHorizontal();

                    GUILayout.Label("", GUILayout.Width(20 * _uiScaler));
                    GUILayout.BeginVertical();
                    m.Display();

                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.EndArea();

            foreach (JDebuggerMenu m in _debugMenus)
                m.LateDisplay();
        }
    }
    
    #endregion
}


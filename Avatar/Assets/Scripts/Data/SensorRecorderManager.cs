#define USE_BLACKMAGIC

using UnityEngine;
using System.Collections;

public class SensorRecorderManager : MonoBehaviour
{
    public string subjectName = "";

    private bool _recordingOn = false;
    public int frameIndex = 0;

    public static bool startRecordingSensorData
    {
        get
        {
            return SensorRecorderManager.instance.isRecording;
        }
    }

    public TimeDataRecorder timeRecorder;
    public XBeeSerialPort eHealthRecorder;
    public KinectSensorRecorder kinectRecorder;
    public ScenarioItemDataRecorder scenarioItemRecorder;
    public EmpaticaSocket empatica;
    public TeaSocket tea;
    public OrionArduino orion;
    public WiiBBSocket wiiBB;
    public AltitudeDataRecorder altitudeDataRecorder;
    public CollisionEventDataRecorder collisionEventDataRecorder;
    public CanyonProximityDataRecorder canyonProximityDataRecorder;
    public MeteorProximityDataRecorder meteorProximityDataRecorder;

#if USE_BLACKMAGIC
    private BlackMagic blackMagic;
#else
    public AVProMovieCaptureBase videoRecorder;
#endif

    private bool movieCapture = true;

    public System.DateTime startTime;

    public static SensorRecorderManager instance;
    private string baseFolder;
    private string basePath;

    void Awake()
    {
        //startRecordingSensorData = false;
        instance = this;
        blackMagic = GetComponent<BlackMagic>();
    }

    void OnDisable()
    {
        //if (isRecording)
        //{
        //    endRecording();
        //}
    }

    public bool isRecording
    {
        get
        {
            return _recordingOn;
        }
    }

    public void startRecording()
    {
        if (!isRecording)
        {
            frameIndex = 0;
            startTime = System.DateTime.Now;

            string prefix = System.DateTime.Now.ToString().Replace('/', '-');
            prefix = prefix.Replace(':', '-');
            prefix = prefix.Replace(' ', '_');

            if (subjectName.Length == 0)
            {
                subjectName = "Avatar";
            }

            baseFolder = System.IO.Path.Combine(Application.persistentDataPath, subjectName + "-" + prefix);
            System.IO.Directory.CreateDirectory(baseFolder);

            basePath = System.IO.Path.Combine(baseFolder, "Video.ts");
            Debug.Log(basePath);

            _recordingOn = true;

#if USE_BLACKMAGIC

            if (movieCapture)
            {
                blackMagic.SetFileDestination(basePath);
                blackMagic.SetRecord(true);
            }

#else
            videoRecorder._outputFolderPath = baseFilename;
            if(movieCapture)
                videoRecorder.StartCapture();
#endif

            if (timeRecorder != null) timeRecorder.dataCollection.ClearRecordings();
            if (eHealthRecorder != null) eHealthRecorder.dataCollection.ClearRecordings();
            if (kinectRecorder != null) kinectRecorder.dataCollection.ClearRecordings();
            if (scenarioItemRecorder != null) scenarioItemRecorder.dataCollection.ClearRecordings();
            if (empatica != null) empatica.dataCollection.ClearRecordings();
            if (tea != null) tea.dataCollection.ClearRecordings();
            if (orion != null) orion.dataCollection.ClearRecordings();
            if (wiiBB != null) wiiBB.dataCollection.ClearRecordings();
            if (altitudeDataRecorder != null) altitudeDataRecorder.dataCollection.ClearRecordings();
            if (collisionEventDataRecorder != null) collisionEventDataRecorder.dataCollection.ClearRecordings();
            if (canyonProximityDataRecorder != null) canyonProximityDataRecorder.dataCollection.ClearRecordings();
            if (meteorProximityDataRecorder != null) meteorProximityDataRecorder.dataCollection.ClearRecordings();

            if (timeRecorder != null) timeRecorder.StartRecording();
            if (tea != null) tea.StartRecording();
        }
    }

    System.Action _onCaptureComplete = null;

    public void endRecording(System.Action onCaptureComplete = null)
    {
        if (_recordingOn)
        {
            _recordingOn = false;
            _onCaptureComplete = onCaptureComplete;
#if USE_BLACKMAGIC
            if (movieCapture)
            {
                blackMagic.SetRecord(false);
            }
#else
            videoRecorder.StopCapture();
#endif
            Dump();
            DumpColumns();


            if (tea)
            {
                tea.StopRecording();
            }


        }

    }

    public void OnCaptureComplete()
    {
        if (_onCaptureComplete != null)
        {
            _onCaptureComplete();
        }
    }

    void Update()
    {
        frameIndex++;
    }

    public void Dump()
    {

        var fileName = System.IO.Path.Combine(baseFolder, "dump.csv");
        System.IO.StreamWriter stream = new System.IO.StreamWriter(fileName);

        if (timeRecorder != null) timeRecorder.dataCollection.DumpRecordings(stream);
        if (scenarioItemRecorder != null) scenarioItemRecorder.dataCollection.DumpRecordings(stream);
        //  if (eHealthRecorder != null) eHealthRecorder.dataCollection.DumpRecordings(stream);
        //  if (empatica != null) empatica.dataCollection.DumpRecordings(stream);
        if (tea != null) tea.dataCollection.DumpRecordings(stream);
        //  if (orion != null) orion.dataCollection.DumpRecordings(stream);
        if (wiiBB != null) wiiBB.dataCollection.DumpRecordings(stream);
        if (kinectRecorder != null) kinectRecorder.dataCollection.DumpRecordings(stream);
        if (altitudeDataRecorder != null) altitudeDataRecorder.dataCollection.DumpRecordings(stream);
        if (collisionEventDataRecorder != null) collisionEventDataRecorder.dataCollection.DumpRecordings(stream);
        if (canyonProximityDataRecorder != null) canyonProximityDataRecorder.dataCollection.DumpRecordings(stream);
        if (meteorProximityDataRecorder != null) meteorProximityDataRecorder.dataCollection.DumpRecordings(stream);

        stream.Close();

        Debug.Log(fileName);
    }

    public void DumpColumns()
    {

        var fileName = System.IO.Path.Combine(baseFolder, "dump_columns.csv");
        System.IO.StreamWriter stream = new System.IO.StreamWriter(fileName);



        if (timeRecorder != null) timeRecorder.dataCollection.DumpColumnName(stream);
        if (scenarioItemRecorder != null) scenarioItemRecorder.dataCollection.DumpColumnName(stream);
        //  if (eHealthRecorder != null) eHealthRecorder.dataCollection.DumpColumnName(stream);
        //  if (empatica != null) empatica.dataCollection.DumpColumnName(stream);
        if (tea != null) tea.dataCollection.DumpColumnName(stream);
        //  if (orion != null) orion.dataCollection.DumpColumnName(stream);
        if (wiiBB != null) wiiBB.dataCollection.DumpColumnName(stream);
        if (kinectRecorder != null) kinectRecorder.dataCollection.DumpColumnName(stream);
        if (altitudeDataRecorder != null) altitudeDataRecorder.dataCollection.DumpColumnName(stream);
        if (collisionEventDataRecorder != null) collisionEventDataRecorder.dataCollection.DumpColumnName(stream);
        if (canyonProximityDataRecorder != null) canyonProximityDataRecorder.dataCollection.DumpColumnName(stream);
        if (meteorProximityDataRecorder != null) meteorProximityDataRecorder.dataCollection.DumpColumnName(stream);

        stream.WriteLine();

        foreach (var item in timeRecorder.dataCollection.TimeData.values)
        {
            if (timeRecorder != null) timeRecorder.dataCollection.DumpNextValue(stream);
            if (scenarioItemRecorder != null) scenarioItemRecorder.dataCollection.DumpNextValue(stream);
            //  if (eHealthRecorder != null) eHealthRecorder.dataCollection.DumpNextValue(stream);
            //  if (empatica != null) empatica.dataCollection.DumpNextValue(stream);
            if (tea != null) tea.dataCollection.DumpNextValue(stream);
            //  if (orion != null) orion.dataCollection.DumpNextValue(stream);
            if (wiiBB != null) wiiBB.dataCollection.DumpNextValue(stream);
            if (kinectRecorder != null) kinectRecorder.dataCollection.DumpNextValue(stream);
            if (altitudeDataRecorder != null) altitudeDataRecorder.dataCollection.DumpNextValue(stream);
            if (collisionEventDataRecorder != null) collisionEventDataRecorder.dataCollection.DumpNextValue(stream);
            if (canyonProximityDataRecorder != null) canyonProximityDataRecorder.dataCollection.DumpNextValue(stream);
            if (meteorProximityDataRecorder != null) meteorProximityDataRecorder.dataCollection.DumpNextValue(stream);

            stream.WriteLine();
        }


        stream.Close();

        Debug.Log(fileName);
    }

    public void enableMovieCapture(bool switchEnableMovieCapture)
    {
        movieCapture = switchEnableMovieCapture;
    }

    public void enableStartRecordingSensorData()
    {
//        Debug.LogError("enableStartRecordingSensorData");
        //startRecordingSensorData = !startRecordingSensorData;
    }

    public void SaveUserName(string newSubject)
    {
        subjectName = newSubject;
    }
}

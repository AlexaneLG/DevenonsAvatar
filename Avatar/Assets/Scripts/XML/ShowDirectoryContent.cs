using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class ShowDirectoryContent : MonoBehaviour
{

    ///Variables for listing the directory contents    
    //A string that holds the directory path  
    public string directoryPath;
    //A List of strings that holds the file names with their respective extensions  
    private List<string> fileNames;
    //A string that stores the full file path  
    private string fullFilePath;

    //A string that stores the selected file or an error message  
    private string outputMessage = "";
    ////  

    ///Variables for displaying the directory contents  
    private string dirOutputString = "";

    //The initial position of the scroll view  
    private Vector2 scrollPosition = Vector2.zero;
    ////  

    ///Variables for the file number index input  
    private string fileNumberString = "0";

    // Use this for initialization  
    void Start()
    {
        //Append the '@' verbatim to the directory path string  
        this.directoryPath = Application.dataPath + @"/StreamingAssets/";

        try
        {
            //Get the path of all files inside the directory and save them on a List  
            this.fileNames = new List<string>(Directory.GetFiles(this.directoryPath));

            //For each string in the fileNames List   
            for (int i = 0; i < this.fileNames.Count; i++)
            {
                //Remove the file path, leaving only the file name and extension  
                this.fileNames[i] = Path.GetFileName(this.fileNames[i]);
                //Append each file name to the outputString at a new line  
                this.dirOutputString += i.ToString("D5") + "\t-\t" + this.fileNames[i] + "\n";
            }
        }
        //Catch any of the following exceptions and store the error message at the outputMessage string  
        catch (System.UnauthorizedAccessException UAEx)
        {
            this.outputMessage = "ERROR: " + UAEx.Message;
        }
        catch (System.IO.PathTooLongException PathEx)
        {
            this.outputMessage = "ERROR: " + PathEx.Message;
        }
        catch (System.IO.DirectoryNotFoundException DirNfEx)
        {
            this.outputMessage = "ERROR: " + DirNfEx.Message;
        }
        catch (System.ArgumentException aEX)
        {
            this.outputMessage = "ERROR: " + aEX.Message;
        }
    }

    void OnGUI()
    {
        //If the outputMessage string contains the expression "ERRROR: "  
        if (outputMessage.Contains("ERROR: "))
        {
            //Display an error message  
            GUI.Label(new Rect(25, Screen.height - 50, Screen.width, 100), this.outputMessage);
            //Force an early out return of the OnGUI() method. No code below this line will get executed.  
            return;
        }

        //Display the directory path and the number of files listed  
        GUI.Label(new Rect(25, 25, Screen.width, 100), "Directory: " + this.directoryPath + "   Files found: " + this.fileNames.Count);

        //Begin GUILayout  
        GUILayout.BeginArea(new Rect(25, 50, Screen.width - 50, Screen.height - 100));
        //Create a scroll view.  
        this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, GUILayout.Width(Screen.width - 50), GUILayout.Height(Screen.height - 100));
        //Display all of the file names on a TextArea  
        GUILayout.TextArea(this.dirOutputString);
        //End of the scroll view  
        GUILayout.EndScrollView();
        //End of the GUI Layout  
        GUILayout.EndArea();

        //Display a simple label that asks for a file number as input  
        GUI.Label(new Rect(25, Screen.height - 40, 250, 30), "Input a file number:");

        //Assign the value of the fileNumberString with the value typed on the TextField  
        this.fileNumberString = GUI.TextField(new Rect(140, Screen.height - 40, 60, 30), this.fileNumberString, 5);

        //If the "Select File!" button has been pressed.  
        if (GUI.Button(new Rect(210, Screen.height - 40, 150, 30), "Select file!"))
        {
            //Convert the fileNumberString into an Interger  
            int fileIndex = int.Parse(this.fileNumberString);

            //If the number that has been passed as an input is out of range  
            if (fileIndex >= this.fileNames.Count)
            {
                //Display an error message  
                this.outputMessage = "Index out of range. Please enter a number between 0 and " + (this.fileNames.Count - 1).ToString();
                //Force an early out return of the OnGUI() method. No code below this line will get executed.  
                return;
            }

            //Concatenate the directory path with the file name  
            this.fullFilePath = this.directoryPath + @"\" + this.fileNames[fileIndex];

            //If the file exists  
            if (File.Exists(this.fullFilePath))
            {
                //Save its full path, name and extension on the outputMessage string  
                this.outputMessage = "Selected file:  " + this.fullFilePath;
            }
            else
            {
                //The file must have been removed since the last time the directory had been listed. Display an error message.  
                this.outputMessage = "ERROR: FILE NOT FOUND!";
            }
        }

        //Display the selected file path or an error message at the bottom of the screen  
        GUI.Label(new Rect(380, Screen.height - 40, Screen.width, 300), this.outputMessage);
    }
}  

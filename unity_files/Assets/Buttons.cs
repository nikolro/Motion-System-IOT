using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using TMPro;
using UnityEngine.Video;
using System.Linq;
using System.IO;

/*  
    This file resposnsible of creating the object for each button and assign a function for its functionality 
*/ 

public class Buttons : MonoBehaviour
{
    public bool play_clicked=false;

    public bool isAnimationStarted = false;
    public string[] lines;
    public string[] first_line;

    public bool videoStarted = false;
    public VideoPlayer videoPlayer;

    public Stickman stickman;

    private TMP_InputField VideoStartTimeInputField;
    private TMP_InputField StickmanStartTimeInputField;    

    public float videostartTime=0f;
    public float stickmanstartTime=0f;

    public bool FirstTimeStickman=true;
    public bool FirstTimeVideo=true;


    // Start is called before the first frame update
    void Start()
    {   
        Screen.fullScreen = true;
        stickman = GameObject.Find("Stickman").GetComponent<Stickman>();    
        GameObject canvasGameObject = GameObject.Find("ButtonsCanvas");

        // fields to manually add delay in case the video and data isn't synchronyzed
        Transform VideoStartTimeInputFieldTransform = canvasGameObject.transform.Find("VideoStartTimeInputField");
        VideoStartTimeInputField = VideoStartTimeInputFieldTransform.GetComponent<TMP_InputField>();
        Transform StickmanStartTimeInputFieldTransform = canvasGameObject.transform.Find("StickmanStartTimeInputField");
        StickmanStartTimeInputField = StickmanStartTimeInputFieldTransform.GetComponent<TMP_InputField>();

        // playFunction handeling the PLAY button functionality : plays the video and avatar
        Transform play_button_transform = canvasGameObject.transform.Find("Play");
        Button play_button = play_button_transform.GetComponent<Button>();
        play_button.onClick.AddListener(playFunction);
        void playFunction()
        {
            play_clicked=true;
            if (!string.IsNullOrEmpty(VideoStartTimeInputField.text) && float.TryParse(VideoStartTimeInputField.text, out videostartTime))
            {
                if(FirstTimeVideo==true)
                {
                    videoPlayer.time = videostartTime;
                    FirstTimeVideo=false;
                }
            }
            if (!string.IsNullOrEmpty(StickmanStartTimeInputField.text) && float.TryParse(StickmanStartTimeInputField.text, out stickmanstartTime))
            {
                if(FirstTimeStickman==true)
                {
                    stickman.currentIndex = (int)stickmanstartTime;
                    FirstTimeStickman=false;
                }
            }
        }

        // PauseFunction handeling the PAUSE button functionality: pause the video and avatar
        Transform pause_button_transform = canvasGameObject.transform.Find("Pause");
        Button pause_button = pause_button_transform.GetComponent<Button>();
        pause_button.onClick.AddListener(PauseFunction);
        void PauseFunction()
        {
            videoPlayer.Pause();
            play_clicked=false;
        }

        // QuitFunction handeling the QUIT button functionality: quits the app
        Transform quit_button_transform = canvasGameObject.transform.Find("Quit");
        Button quit_button = quit_button_transform.GetComponent<Button>();
        quit_button.onClick.AddListener(QuitFunction);
        void QuitFunction()
        {
            Application.Quit();
        }

        // RestartFunction handeling the RESTART button functionality: resets the video and avatar to the begining
        Transform Restart_button_transform = canvasGameObject.transform.Find("Restart");
        Button Restart_button = Restart_button_transform.GetComponent<Button>();
        Restart_button.onClick.AddListener(RestartFunction);
        void RestartFunction()
        {
            FirstTimeVideo=true;
            FirstTimeStickman=true;
            play_clicked=true;
            if (!string.IsNullOrEmpty(VideoStartTimeInputField.text) && float.TryParse(VideoStartTimeInputField.text, out videostartTime))
            {
                if(FirstTimeVideo==true)
                {
                videoPlayer.time = videostartTime;
                FirstTimeVideo=false;
                }
            }
            else
            {
                videoPlayer.time = 0.0f;
            }
            if (!string.IsNullOrEmpty(StickmanStartTimeInputField.text) && float.TryParse(StickmanStartTimeInputField.text, out stickmanstartTime))
            {
                if(FirstTimeStickman==true)
                {
                stickman.currentIndex = (int)stickmanstartTime;
                FirstTimeStickman=false;
                }
            }
            else
            {
                stickman.currentIndex = 1;
            }
        }

        // creating the search window to choose a 'mp4' file for the video

        Transform selectVideo_button_transform = canvasGameObject.transform.Find("SelectVideo");
        GameObject videoObject = GameObject.Find("VideoPlayer");
        videoPlayer = videoObject.GetComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        videoPlayer.targetCamera = videoObject.transform.Find("VideoCamera").GetComponent<Camera>();
        Button selectVideo_button = selectVideo_button_transform.GetComponent<Button>();
        selectVideo_button.onClick.AddListener(OpenFileDialog);
        void OpenFileDialog()
        {
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Select video", "", "mp4", false);
            if (paths.Length > 0)
            {
                string videoPath = paths[0];
                videoPlayer.url = videoPath;
                videoStarted=true;
                videoPlayer.Prepare();
                videoPlayer.prepareCompleted += OnVideoPrepared;
                }
            }
            void OnVideoPrepared(VideoPlayer source)
            {
                source.prepareCompleted -= OnVideoPrepared;
                source.Play();
                source.Pause();
            }

        // creating the search window to choose a 'csv' file for the avatar and surround data

        Transform selectFile_button_transform = canvasGameObject.transform.Find("SelectCSVFile");
        Button selectFile_button = selectFile_button_transform.GetComponent<Button>();
        selectFile_button.onClick.AddListener(OpenFileDialog1);
        void OpenFileDialog1()
        {
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Select CSV File", "", "csv", false);
            if (paths.Length > 0)
            {
                string selectedFilePath = paths[0];
                lines = File.ReadAllLines(selectedFilePath); 
                first_line = lines[0].Split(',');
                isAnimationStarted=true;
                stickman.UpdateCoordinates(stickman.currentIndex);
                stickman.UpdateData(stickman.currentIndex);
            }

        }


        // creating a 'dropdown' button to choose playspeed of video and data

        TMP_Dropdown dropdown = canvasGameObject.transform.Find("Dropdown").GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        void OnDropdownValueChanged(int index)
        {
            // Get the selected option text
            string selectedOption = dropdown.options[index].text;

            // Perform actions based on the selected option
            switch (selectedOption)
            {
                case "x0.25":
                    videoPlayer.playbackSpeed = 0.25f;
                    Application.targetFrameRate = Mathf.RoundToInt(15);
                    QualitySettings.vSyncCount = 0;
                    break;
                case "x0.5":
                    videoPlayer.playbackSpeed = 0.5f;
                    Application.targetFrameRate = Mathf.RoundToInt(30);
                    QualitySettings.vSyncCount = 0;
                    break;
                case "Normal":
                    videoPlayer.playbackSpeed = 1f;
                    Application.targetFrameRate = Mathf.RoundToInt(60);
                    QualitySettings.vSyncCount = 0;
                    break;
            }
        }

    }
}

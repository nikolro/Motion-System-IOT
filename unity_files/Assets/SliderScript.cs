using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


/* 
    This file responsible to add and control the time slider under the video
*/
public class SliderScript : MonoBehaviour
{
    [SerializeField] public Slider videoSlider; 
    [SerializeField] public VideoPlayer videoPlayer; 
    [SerializeField] public Text timeText; 

    public Stickman stickman;
    public Buttons buttons;

    void Start()
    {
        stickman = GameObject.Find("Stickman").GetComponent<Stickman>();  
        buttons= GameObject.Find("Buttons").GetComponent<Buttons>();  
        // creating the object for the slider
        GameObject slidersCanvasObject = GameObject.Find("SlidersCanvas");
        Transform videoSliderTransform = slidersCanvasObject.transform.Find("VideoSlider");
        videoSlider = videoSliderTransform.GetComponent<Slider>();
        GameObject timeTextObject = GameObject.Find("TimeText");
        timeText = timeTextObject.GetComponent<Text>();
        videoPlayer=buttons.videoPlayer;
        videoSlider.onValueChanged.AddListener(OnSliderValueChanged);  
    }

    // updates the indicator on the slider according to the time passed since the video start
    void Update()
    {
        timeText.text = FormatTime(videoPlayer.time) + " / " + FormatTime(videoPlayer.length);
        if (videoPlayer.isPlaying)
        {
            videoSlider.onValueChanged.RemoveListener(OnSliderValueChanged); 
            videoSlider.value=(float)videoPlayer.frame/(float)videoPlayer.frameCount;
            videoSlider.onValueChanged.AddListener(OnSliderValueChanged); 
        }
    }

    // reset the video and data according to the slider indicator's location 'on click'
    void OnSliderValueChanged(float value)
    {
        videoPlayer.Pause();
        buttons.play_clicked=false;
        int targetTime =(int)(value * videoPlayer.length);
        stickman.currentIndex=targetTime*60+(int)buttons.stickmanstartTime;
        videoPlayer.time = targetTime+(int)buttons.videostartTime;
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;
    }
            void OnVideoPrepared(VideoPlayer source)
        {
            source.prepareCompleted -= OnVideoPrepared;
            source.Play();
            source.Pause();
            buttons.play_clicked=true;
        }

    string FormatTime(double time)
    {
        int minutes = Mathf.FloorToInt((float)time / 60);
        int seconds = Mathf.FloorToInt((float)time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

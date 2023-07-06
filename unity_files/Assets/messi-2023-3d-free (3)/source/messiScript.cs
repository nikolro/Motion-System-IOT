using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using SFB;
using TMPro;
using UnityEngine.Video;


public class messiScript : MonoBehaviour
{
    public Buttons buttons;

    Transform messi;
    Transform pelvis;
    Transform hip_r ;
    Transform knee_r;
    Transform ankle_r;
    Transform hip_l;
    Transform knee_l;
    Transform ankle_l;
    Transform lumbar;
    Transform lumbar1;
    Transform lumbar2;
    Transform shoulder_r;
    Transform arm_r;
    Transform elbow_r;
    Transform shoulder_l;
    Transform arm_l;
    Transform elbow_l;

    string[] lines;
    string[] first_line; 

    bool file_chosen=false;

    int currentIndex;

    private void Start()
    {
        buttons = GameObject.Find("Buttons").GetComponent<Buttons>();

        messi = GameObject.Find("messi").transform;
        pelvis = messi.transform.Find("Hips");
        hip_r = pelvis.transform.Find("RightUpLeg");
        knee_r = hip_r.transform.Find("RightLeg");
        ankle_r = knee_r.transform.Find("RightFoot");
        hip_l = pelvis.transform.Find("LeftUpLeg");
        knee_l = hip_l.transform.Find("LeftLeg");
        ankle_l = knee_l.transform.Find("LeftFoot");
        lumbar = pelvis.transform.Find("Spine");
        lumbar1 = lumbar.transform.Find("Spine1");
        lumbar2 = lumbar1.transform.Find("Spine2");
        shoulder_r=lumbar2.transform.Find("RightShoulder");
        arm_r=shoulder_r.transform.Find("RightArm");
        elbow_r=arm_r.transform.Find("RightForeArm");
        shoulder_l=lumbar2.transform.Find("LeftShoulder");
        arm_l=shoulder_l.transform.Find("LeftArm");
        elbow_l=arm_l.transform.Find("LeftForeArm");
        
        Application.targetFrameRate = Mathf.RoundToInt(1.0f / 0.0167f);

        GameObject canvasGameObject = GameObject.Find("ButtonsCanvas");
        Transform selectFile_button_transform = canvasGameObject.transform.Find("SelectMotFile");
        Button selectFile_button = selectFile_button_transform.GetComponent<Button>();
        selectFile_button.onClick.AddListener(OpenFileDialog1);
        void OpenFileDialog1()
        {
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Select Mot File", "", "mot", false);
            if (paths.Length > 0)
            {
                string selectedFilePath = paths[0];
                lines = File.ReadAllLines(selectedFilePath); 
                first_line = lines[0].Split('\t');
                file_chosen=true;
            }

        }
        currentIndex = 7; 

    } 

        private void Update()
        {  
            if(file_chosen==true)
            {
                    if (currentIndex < lines.Length-1)
                    {
                        string[] lineData = lines[currentIndex].Split('\t');

                        float time = float.Parse(lineData[0]);

                        float pelvisTilt = float.Parse(lineData[1])* Mathf.Rad2Deg;
                        float pelvisList = float.Parse(lineData[2])* Mathf.Rad2Deg;
                        float pelvisRotation = float.Parse(lineData[3])* Mathf.Rad2Deg;
                        float pelvisTx = float.Parse(lineData[4]);
                        float pelvisTy = float.Parse(lineData[5]);
                        float pelvisTz = float.Parse(lineData[6]);
                        //pelvis.localPosition=new Vector3(pelvisTx, pelvisTy, pelvisTz);
                        pelvis.localRotation =Quaternion.Euler(pelvisTilt, pelvisList, pelvisRotation);
                         /*

                        float hipFlexionR = float.Parse(lineData[7])* Mathf.Rad2Deg;
                        float hipAdductionR = float.Parse(lineData[8])* Mathf.Rad2Deg;
                        float hiplocalRotationR = float.Parse(lineData[9])* Mathf.Rad2Deg;
                        Vector3 hip_r_currentPosition = hip_r.localRotation.eulerAngles;
                        hip_r.localRotation = Quaternion.Euler(hipFlexionR, hipAdductionR,hiplocalRotationR); 
                       

                        float kneeAngleR = float.Parse(lineData[10]);
                        float kneeAngleRBeta = float.Parse(lineData[11]);
                        Vector3 knee_r_currentPosition = knee_r.localRotation.eulerAngles;
                        knee_r.localRotation = Quaternion.Euler(knee_r_currentPosition.x, knee_r_currentPosition.y, kneeAngleR);
                        
                        float ankleAngleR = float.Parse(lineData[12]);
                        float subtalarAngleR = float.Parse(lineData[13]);
                        float mtpAngleR = float.Parse(lineData[14]);
                        Vector3 ankle_r_currentPosition = ankle_r.localRotation.eulerAngles;
                        ankle_r.localRotation = Quaternion.Euler(subtalarAngleR, ankle_r_currentPosition.y, ankleAngleR);

                        float hipFlexionL = float.Parse(lineData[15]);
                        float hipAdductionL = float.Parse(lineData[16]);
                        float hiplocalRotationL = float.Parse(lineData[17]);
                        hip_l.localRotation = Quaternion.Euler(hipAdductionL, hiplocalRotationL, hipFlexionL); 

                        float kneeAngleL = float.Parse(lineData[18]);
                        float kneeAngleLBeta = float.Parse(lineData[19]);
                        Vector3 knee_l_currentPosition = knee_l.transform.localRotation.eulerAngles;
                        knee_l.localRotation = Quaternion.Euler(knee_l_currentPosition.x, knee_l_currentPosition.y, kneeAngleL);


                        float ankleAngleL = float.Parse(lineData[20]);
                        float subtalarAngleL = float.Parse(lineData[21]);
                        float mtpAngleL = float.Parse(lineData[22]);
                        Vector3 ankle_l_currentPosition = ankle_l.localRotation.eulerAngles;
                        ankle_l.localRotation = Quaternion.Euler(subtalarAngleL, ankle_l_currentPosition.y, ankleAngleL);

                        float lumbarExtension = float.Parse(lineData[23]);
                        float lumbarBending = float.Parse(lineData[24]);
                        float lumbarlocalRotation = float.Parse(lineData[25]);
                        lumbar.localRotation = Quaternion.Euler(lumbarExtension, lumbarBending, lumbarlocalRotation);

                        float armFlexR = float.Parse(lineData[26]);
                        float armAddR = float.Parse(lineData[27]);
                        float armRotR = float.Parse(lineData[28]);
                        shoulder_r.localRotation = Quaternion.Euler(armAddR, armRotR, armFlexR);

                        float elbowFlexR = float.Parse(lineData[29]);
                        float proSupR = float.Parse(lineData[30]);
                        float wristFlexR = float.Parse(lineData[31]);
                        float wristDevR = float.Parse(lineData[32]);
                        Vector3 elbow_r_currentPosition = elbow_r.localRotation.eulerAngles;
                        elbow_r.localRotation = Quaternion.Euler(elbow_r_currentPosition.x, elbow_r_currentPosition.y, elbowFlexR);

                        float armFlexL = float.Parse(lineData[33]);
                        float armAddL = float.Parse(lineData[34]);
                        float armRotL = float.Parse(lineData[35]);
                        shoulder_l.localRotation = Quaternion.Euler(armAddL, armRotL, armFlexL);

                        float elbowFlexL = float.Parse(lineData[36]);
                        float proSupL = float.Parse(lineData[37]);
                        float wristFlexL = float.Parse(lineData[38]);
                        float wristDevL = float.Parse(lineData[39]);
                        Vector3 elbow_l_currentPosition = elbow_l.localRotation.eulerAngles;
                        elbow_l.localRotation = Quaternion.Euler(elbow_l_currentPosition.x, elbow_l_currentPosition.y, elbowFlexL);
                        */
 
                         currentIndex++;
                        }
            }
        }
}
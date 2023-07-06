using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using SFB;
using TMPro;
using UnityEngine.Video;


public class Stickman : MonoBehaviour
{
    public Buttons buttons;
    public Camera camera;

    // a dictionary mapping joint names to game objects
    List<GameObject> joints = new List<GameObject>();
    Dictionary<string, GameObject> balls = new Dictionary<string, GameObject>();

        GameObject stickman;
        GameObject P_mtp_toes_r ;
        GameObject P_subt_calc_r ;
        GameObject P_Ankle_tal_r ;
        GameObject P_KneeJ_r ;
        GameObject P_HipJ_r ;
        //GameObject P_Pelvis_C;
        GameObject P_Torso ;
        GameObject P_neck ;
        GameObject P_sho_r ;
        GameObject P_elb_r ;
        GameObject P_hand_r ;
        GameObject P_sho_l ;
        GameObject P_elb_l ;
        GameObject P_hand_l;       
        GameObject P_HipJ_l ;
        GameObject P_KneeJ_l ;
        GameObject P_Ankle_tal_l ;
        GameObject P_subt_calc_l;
        GameObject P_mtp_toes_l;
        //GameObject Pc_total;   

    // the line renderer component used to draw the stickman's limbs
    private LineRenderer lineRenderer_lower_body;
    private LineRenderer lineRenderer_upper_body;
    private LineRenderer lineRenderer_middle;

    LineRenderer LineRenderer;

    bool isAnimationStarted;
    string[] lines;
    string[] first_line;

    VideoPlayer videoPlayer;
    bool videoStarted;

    public Color Color = Color.red;

    public int currentIndex;

    private void Start()
    {
        buttons = GameObject.Find("Buttons").GetComponent<Buttons>();

        stickman = GameObject.Find("Stickman");

        P_mtp_toes_r = new GameObject("P_mtp_toes_r");
        P_subt_calc_r = new GameObject("P_subt_calc_r");
        P_Ankle_tal_r = new GameObject("P_Ankle_tal_r");
        P_KneeJ_r = new GameObject("P_KneeJ_r");
        P_HipJ_r = new GameObject("P_HipJ_r");
        //P_Pelvis_C = new GameObject("P_Pelvis_C");
        P_Torso = new GameObject("P_Torso");
        P_neck = new GameObject("P_neck");
        P_sho_r = new GameObject("P_sho_r");
        P_elb_r = new GameObject("P_elb_r");
        P_hand_r = new GameObject("P_hand_r");
        P_sho_l = new GameObject("P_sho_l");
        P_elb_l = new GameObject("P_elb_l");
        P_hand_l = new GameObject("P_hand_l");        
        P_HipJ_l = new GameObject("P_HipJ_l");
        P_KneeJ_l = new GameObject("P_KneeJ_l");
        P_Ankle_tal_l = new GameObject("P_Ankle_tal_l");
        P_subt_calc_l = new GameObject("P_subt_calc_l");
        P_mtp_toes_l = new GameObject("P_mtp_toes_l");
        //this is not a joint
        //Pc_total = new GameObject("Pc_total");

        // populate the joints dictionary
        joints.Add(P_mtp_toes_r);
        joints.Add(P_subt_calc_r);
        joints.Add(P_Ankle_tal_r);
        joints.Add(P_KneeJ_r);
        joints.Add(P_HipJ_r);
        //joints.Add(P_Pelvis_C);
        joints.Add(P_Torso);
        joints.Add(P_neck);
        joints.Add(P_sho_r);
        joints.Add(P_elb_r);
        joints.Add(P_hand_r);
        joints.Add(P_sho_l);
        joints.Add(P_elb_l);
        joints.Add(P_hand_l);
        joints.Add(P_HipJ_l);
        joints.Add(P_KneeJ_l);
        joints.Add(P_Ankle_tal_l);
        joints.Add(P_subt_calc_l);
        joints.Add(P_mtp_toes_l);

        foreach (GameObject joint in joints)
        {
        joint.transform.parent = stickman.transform;
        }
        
        // create the line renderer component and set its parameters
        lineRenderer_lower_body = P_mtp_toes_r.AddComponent<LineRenderer>();
        // Set the width of the line renderer points
        lineRenderer_lower_body.startWidth = 0.02f;
        lineRenderer_lower_body.endWidth = 0.02f;
        // Set the material used by the line renderer
        lineRenderer_lower_body.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer_lower_body.material.color = Color.blue;
        lineRenderer_lower_body.positionCount = 11;

         // create the line renderer component and set its parameters
        lineRenderer_upper_body = P_neck.AddComponent<LineRenderer>();
        // Set the width of the line renderer points
        lineRenderer_upper_body.startWidth = 0.02f;
        lineRenderer_upper_body.endWidth = 0.02f;
        // Set the material used by the line renderer
        lineRenderer_upper_body.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer_upper_body.material.color = Color.blue;
        lineRenderer_upper_body.positionCount = 7;

        // create the line renderer component and set its parameters
        lineRenderer_middle = P_Torso.AddComponent<LineRenderer>();
        // Set the width of the line renderer points
        lineRenderer_middle.startWidth = 0.02f;
        lineRenderer_middle.endWidth = 0.02f;
        // Set the material used by the line renderer
        lineRenderer_middle.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer_middle.material.color = Color.blue;
        lineRenderer_middle.positionCount = 2;

        GameObject all_balls = new GameObject("all_balls");
        foreach(GameObject joint in joints)
        {
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.name = joint.name + "_Sphere";
        ball.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
        balls.Add(joint.name, ball);
        }
        foreach (GameObject ball in balls.Values)
        {
        ball.transform.parent = all_balls.transform;
        }

        GameObject xAxis = new GameObject("Axis");
        LineRenderer = xAxis.AddComponent<LineRenderer>();
        LineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        LineRenderer.startColor = Color;
        LineRenderer.endColor = Color;
        LineRenderer.startWidth = 0.02f;
        LineRenderer.endWidth = 0.02f;

        Application.targetFrameRate = Mathf.RoundToInt(1.0f / 0.0167f);
        QualitySettings.vSyncCount = 0;

        currentIndex = 1; 

    }

    private void Update()
    {  
        if (buttons.play_clicked==true)
        {
                if (currentIndex < lines.Length-1)
                {
                    if (!buttons.videoPlayer.isPlaying)
                        buttons.videoPlayer.Play();
                    UpdateCoordinates(currentIndex);
                    UpdateData(currentIndex);    
                }
        }
    }


    public void UpdateCoordinates(int Index)
    {
        Vector3 pc_total_position=new Vector3(0, 0, 0);
        lines = buttons.lines;
        first_line = buttons.first_line;
        string[] values = lines[Index].Split(',');
                for (int i = 1; i <=61; i=i+3)
                {
                    if(i!=16)
                    {
                    float x = float.Parse(values[i]);
                    float y = float.Parse(values[i+1]);
                    float z = float.Parse(values[i+2]);
                    string jointName = first_line[i];
                    
                    
                    float max_acc=0.1f;
                    float min_acc=0.02f;
                    if(i==58)
                    {
                    pc_total_position=new Vector3(x, y, z);
                    LineRenderer.SetPosition(0, pc_total_position);
                    }
                    else if(i==61)
                    {
                        float acc_size =Mathf.Sqrt(x*x + y*y + z*z);
                        if(acc_size < min_acc){
                            if(acc_size < 0.05f){ // check VAL *
                                acc_size = 0.0f;
                            } else {
                                acc_size = min_acc;
                            }
                        } else if(acc_size > max_acc){
                            acc_size = max_acc;
                        }
                        float norm_x = x*acc_size;
                        float norm_y = y*acc_size;
                        float norm_z = z*acc_size;
                        
                        //Vector3 acc_total_position=new Vector3(x/20.0f,y/20.0f,z/20.0f);
                        //Vector3 acc_total_position=new Vector3(norm_x,norm_y,norm_z);
                        //acc_total_position=SetAccPositon(acc_total_position);
                        Vector3 acc_total_position = new Vector3((norm_x)+pc_total_position.x, (norm_y)+pc_total_position.y, (norm_z)+pc_total_position.z);
                        LineRenderer.SetPosition(1, acc_total_position);
                    }
                    else
                    {
                    foreach (GameObject joint in joints)
                    {
                        if (joint.name == jointName)
                        {
                            joint.transform.position = new Vector3(x, y, z);
                            break;
                        }
                    }
                    balls[jointName].transform.position =new Vector3(x, y, z);
                    }
                    }
                    
                }

                lineRenderer_lower_body.SetPosition(0,P_mtp_toes_r.transform.position);
                lineRenderer_lower_body.SetPosition(1, P_subt_calc_r.transform.position);
                lineRenderer_lower_body.SetPosition(2, P_Ankle_tal_r.transform.position);
                lineRenderer_lower_body.SetPosition(3, P_KneeJ_r.transform.position);
                lineRenderer_lower_body.SetPosition(4, P_HipJ_r.transform.position);
                lineRenderer_lower_body.SetPosition(5, P_Torso.transform.position);
                lineRenderer_lower_body.SetPosition(6, P_HipJ_l.transform.position);
                lineRenderer_lower_body.SetPosition(7, P_KneeJ_l.transform.position);
                lineRenderer_lower_body.SetPosition(8, P_Ankle_tal_l.transform.position);
                lineRenderer_lower_body.SetPosition(9, P_subt_calc_l.transform.position);
                lineRenderer_lower_body.SetPosition(10, P_mtp_toes_l.transform.position);

                lineRenderer_upper_body.SetPosition(0,P_hand_l.transform.position);
                lineRenderer_upper_body.SetPosition(1, P_elb_l.transform.position);
                lineRenderer_upper_body.SetPosition(2, P_sho_l.transform.position);
                lineRenderer_upper_body.SetPosition(3, P_neck.transform.position);
                lineRenderer_upper_body.SetPosition(4, P_sho_r.transform.position);
                lineRenderer_upper_body.SetPosition(5, P_elb_r.transform.position);
                lineRenderer_upper_body.SetPosition(6, P_hand_r.transform.position);

                lineRenderer_middle.SetPosition(0, P_neck.transform.position);
                lineRenderer_middle.SetPosition(1, P_Torso.transform.position);
    }

    public void UpdateData(int Index)
    {
        string[] values = lines[Index].Split(',');

        //surferdata
        GameObject surferDataCanvas = GameObject.Find("SurferDataCanvas");
        Transform SurferData = surferDataCanvas.transform.Find("SurferData");
        Transform pc_total= SurferData.transform.Find("Pc_total");
        pc_total.transform.Find("x-value").GetComponentInChildren<Text>().text = float.Parse(values[58]).ToString();
        pc_total.transform.Find("y-value").GetComponentInChildren<Text>().text = float.Parse(values[59]).ToString();
        pc_total.transform.Find("z-value").GetComponentInChildren<Text>().text = float.Parse(values[60]).ToString();
        Transform acc_total= SurferData.transform.Find("acc_Total_COM");
        acc_total.transform.Find("x-value").GetComponentInChildren<Text>().text = float.Parse(values[61]).ToString();
        acc_total.transform.Find("y-value").GetComponentInChildren<Text>().text = float.Parse(values[62]).ToString();
        acc_total.transform.Find("z-value").GetComponentInChildren<Text>().text = float.Parse(values[63]).ToString();

        //joints angles data
        GameObject jointsAnglesCanvas = GameObject.Find("JointsAnglesCanvas");
        Transform jointsSurferData = jointsAnglesCanvas.transform.Find("SurferData");
        Transform jointsviewport= jointsSurferData.transform.Find("viewPort");
        Transform jointscontent= jointsviewport.transform.Find("content");
        for (int i = 64; i <= 101; i++)
        {
            string jointAngleName = first_line[i];
            string Name = jointAngleName + " -value";
            Button valueButton = jointscontent.Find(Name).GetComponent<Button>();
            valueButton.GetComponentInChildren<Text>().text = float.Parse(values[i]).ToString();
        }     

        //surfing data
        GameObject surfingDataCanvas = GameObject.Find("SurfingDataCanvas");
        Transform SurfingData = surfingDataCanvas.transform.Find("SurferData");
        Button sailing_speed = SurfingData.Find("sailing speed -value").GetComponent<Button>();
        sailing_speed.GetComponentInChildren<Text>().text = float.Parse(values[102]).ToString();
        Button sailing_direction = SurfingData.Find("sailing direction -value").GetComponent<Button>();
        sailing_direction.GetComponentInChildren<Text>().text = float.Parse(values[103]).ToString();
        Button Upwind_Downwind = SurfingData.Find("Upwind - 1, Downwind - 0 -value").GetComponent<Button>();
        Upwind_Downwind.GetComponentInChildren<Text>().text = float.Parse(values[104]).ToString();   
        Button sidewind = SurfingData.Find("sidewind R-1, L-0 -value").GetComponent<Button>();
        sidewind.GetComponentInChildren<Text>().text = float.Parse(values[105]).ToString();
        Button wind_speed = SurfingData.Find("wind speed -value").GetComponent<Button>();
        wind_speed.GetComponentInChildren<Text>().text = float.Parse(values[106]).ToString();
        Button wind_direction = SurfingData.Find("wind direction -value").GetComponent<Button>();
        wind_direction.GetComponentInChildren<Text>().text = float.Parse(values[107]).ToString();

        if(first_line.Length==109)
        {
            Button t1=SurfingData.Find("t1").GetComponent<Button>();
            t1.GetComponentInChildren<Text>().text = first_line[108].ToString();
            Button t1_value=SurfingData.Find("t1-value").GetComponent<Button>();
            t1_value.GetComponentInChildren<Text>().text = float.Parse(values[108]).ToString();
        }
        if(first_line.Length==110)
        {
            Button t1=SurfingData.Find("t1").GetComponent<Button>();
            t1.GetComponentInChildren<Text>().text = float.Parse(first_line[108]).ToString();
            Button t1_value=SurfingData.Find("t1-value").GetComponent<Button>();
            t1_value.GetComponentInChildren<Text>().text = float.Parse(values[108]).ToString();

            Button t2=SurfingData.Find("t2").GetComponent<Button>();
            t2.GetComponentInChildren<Text>().text = float.Parse(first_line[109]).ToString();
            Button t2_value=SurfingData.Find("t2-value").GetComponent<Button>();
            t2_value.GetComponentInChildren<Text>().text = float.Parse(values[109]).ToString();

        }
        if(first_line.Length==111)
        {
            Button t1=SurfingData.Find("t1").GetComponent<Button>();
            t1.GetComponentInChildren<Text>().text = float.Parse(first_line[108]).ToString();
            Button t1_value=SurfingData.Find("t1-value").GetComponent<Button>();
            t1_value.GetComponentInChildren<Text>().text = float.Parse(values[108]).ToString();

            Button t2=SurfingData.Find("t2").GetComponent<Button>();
            t2.GetComponentInChildren<Text>().text = float.Parse(first_line[109]).ToString();
            Button t2_value=SurfingData.Find("t2-value").GetComponent<Button>();
            t2_value.GetComponentInChildren<Text>().text = float.Parse(values[109]).ToString();

            Button t3=SurfingData.Find("t3").GetComponent<Button>();
            t3.GetComponentInChildren<Text>().text = float.Parse(first_line[110]).ToString();
            Button t3_value=SurfingData.Find("t3-value").GetComponent<Button>();
            t3_value.GetComponentInChildren<Text>().text = float.Parse(values[110]).ToString();

        }
        if(first_line.Length==112)
        {
            Button t1=SurfingData.Find("t1").GetComponent<Button>();
            t1.GetComponentInChildren<Text>().text = float.Parse(first_line[108]).ToString();
            Button t1_value=SurfingData.Find("t1-value").GetComponent<Button>();
            t1_value.GetComponentInChildren<Text>().text = float.Parse(values[108]).ToString();

            Button t2=SurfingData.Find("t2").GetComponent<Button>();
            t2.GetComponentInChildren<Text>().text = float.Parse(first_line[109]).ToString();
            Button t2_value=SurfingData.Find("t2-value").GetComponent<Button>();
            t2_value.GetComponentInChildren<Text>().text = float.Parse(values[109]).ToString();

            Button t3=SurfingData.Find("t3").GetComponent<Button>();
            t3.GetComponentInChildren<Text>().text = float.Parse(first_line[110]).ToString();
            Button t3_value=SurfingData.Find("t3-value").GetComponent<Button>();
            t3_value.GetComponentInChildren<Text>().text = float.Parse(values[110]).ToString();

            Button t4=SurfingData.Find("t4").GetComponent<Button>();
            t4.GetComponentInChildren<Text>().text = float.Parse(first_line[111]).ToString();
            Button t4_value=SurfingData.Find("t4-value").GetComponent<Button>();
            t4_value.GetComponentInChildren<Text>().text = float.Parse(values[111]).ToString();

        }
        currentIndex++;

    }

Vector3 SetAccPositon(Vector3 acc_total)
    {
        float max_acc=0.0015f;
        float min_acc=0.02f;

        if(acc_total.x>max_acc) {
            acc_total.x=max_acc;
        }
        
        if(acc_total.y>max_acc) {
            acc_total.y=max_acc;
        }

        if(acc_total.z>max_acc) {
            acc_total.z=max_acc;
        }

        if(acc_total.x<min_acc) {
            acc_total.x=min_acc;
        }
        
        if(acc_total.y<min_acc) {
            acc_total.y=min_acc;
        }
        
        if(acc_total.z<min_acc) {
            acc_total.z=min_acc;
        }
        return acc_total;
        
    }

}
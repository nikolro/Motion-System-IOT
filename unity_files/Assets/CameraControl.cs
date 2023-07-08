using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 
    This file responsible to handle the camera in the avatar scene.
    conrolling the movments, rotation and the zoom.
*/

public class CameraControl : MonoBehaviour
{
    public GameObject parentModel;

    private float rotationSpeed = 500.0f;
    private Vector3 mouseWorldPosStart;
    private float zoomScale = 10.0f;
    public float maxFieldOfView = 160.0f;
    public float minFieldOfView = 0.0f;
    public float defaultFieldOfView = 60.0f;

    private GameObject stickman;
    private Camera camera;

    void Start()
    {
        stickman = GameObject.Find("Stickman");
        stickman.transform.position = Vector3.zero;
        camera = GameObject.Find("StickmanCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //mousebuttom 1 controll movment in the scene
        if (Input.GetMouseButton(1))
        {
            CamOrbit();
        }
        if(Input.GetKey(KeyCode.LeftShift)&&Input.GetKey(KeyCode.F))
        {
            FitToScreen();
        }
        //mousebuttom 2 controll rotation in the scene
        if(Input.GetMouseButtonDown(2)&& !Input.GetKey(KeyCode.LeftShift))
        {
            mouseWorldPosStart=GetPerspectivePos();
        }
        if(Input.GetMouseButton(2)&& !Input.GetKey(KeyCode.LeftShift))
        {
            Pan();
        }
        //mousebuttom 3 (scrollWheel) controll zoom in the scene
        Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    // get the mouse position data and rotate accordingly
    private void CamOrbit()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            float verticalInput = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.right,-verticalInput);
            transform.Rotate(Vector3.up,horizontalInput,Space.World);
        }
    }

    private Bounds GetBound(GameObject parentGameObj)
    {
        Bounds bound = new Bounds(parentGameObj.transform.position, Vector3.zero);
        var rList = parentGameObj.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rList)
        {
            bound.Encapsulate(r.bounds);
        }
        return bound;
    }

    // fit the scene view to the user screen
    public void FitToScreen()
    {
        camera.fieldOfView=defaultFieldOfView;
        Bounds bound =GetBound(parentModel);
        Vector3 boundSize=bound.size;
        float boundDiagonal=Mathf.Sqrt((boundSize.x+boundSize.x)+(boundSize.y+boundSize.y)+(boundSize.z+boundSize.z));
        float camDistanceToBoundCentre=boundDiagonal/2.0f/(Mathf.Tan(camera.fieldOfView/2.0f*Mathf.Deg2Rad));
        float camDistanceToBoundWithOffset=camDistanceToBoundCentre+boundDiagonal/2.0f-(camera.transform.position-transform.position).magnitude;
        transform.position=bound.center + (-transform.forward *camDistanceToBoundWithOffset);

    }
        private void Pan()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            Vector3 mouseWorldPosDiff = mouseWorldPosStart -GetPerspectivePos();
            transform.position += mouseWorldPosDiff;
        }
    }
        private void Zoom(float zoomDiff)
    {
        if (zoomDiff != 0)
        {
            mouseWorldPosStart = GetPerspectivePos();
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView - zoomDiff * zoomScale, minFieldOfView, maxFieldOfView);
            Vector3 mouseWorldPosDiff = mouseWorldPosStart - GetPerspectivePos();
            transform.position += mouseWorldPosDiff;
        }
    }

    // get the camera position to set the zoom accordingly
    public Vector3 GetPerspectivePos()
    {
        Ray ray=camera.ScreenPointToRay(Input.mousePosition);
        Plane Plane =new Plane(transform.forward,0.0f);
        float dist;
        Plane.Raycast(ray,out dist);
        return ray.GetPoint(dist);
    }



}

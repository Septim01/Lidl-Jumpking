using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{

    public Camera[] cameras;

    public float jumpCam = 17.4f;

    private Transform body;

    public int whichCam = 0;


    void Start()
    { 

        //Turn all cameras off, except the first default one
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        //If any cameras were added to the controller, enable the first one
        if (cameras.Length > 0)
        {
            cameras[0].gameObject.SetActive(true);
        }
    }

    private void Awake()
    {
        body = GetComponent<Transform>();
    }

    private void Update()
    {

        if(body.position.y > (whichCam+1) * jumpCam)
        {
            incCam();
        }
        if (body.position.y < whichCam * jumpCam)
        {
            decCam();
        }
    }

    void incCam()
    {
        cameras[whichCam].gameObject.SetActive(false);
        whichCam++;
        cameras[whichCam].gameObject.SetActive(true); //kamera sa uz zvysila
        Debug.Log("aaaaaaaaaa");
    }

    void decCam()
    {
        cameras[whichCam].gameObject.SetActive(false);
        whichCam--;
        cameras[whichCam].gameObject.SetActive(true); //kamera sa uz znizila
        Debug.Log("Camera");
    }

}

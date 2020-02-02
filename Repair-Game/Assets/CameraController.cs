using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    public float panSpeed = 20f;
    public Transform target;
    public float angle = 142f;
    private float radius = 150f;
    private GUIStyle guiStyle = new GUIStyle(); //create a new variable


    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if (Input.GetKey("w")&&pos.y<20)
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s")&&pos.y>-22)
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") && pos.y > -10)
        {
            angle -= 1;
        }
        if (Input.GetKey("d") && pos.y > -22)
        {
            angle += 1;
        }



        pos.x = Mathf.Cos(Mathf.Deg2Rad * (angle)) * radius + target.transform.position.x;//center of the circle
        pos.z = Mathf.Sin(Mathf.Deg2Rad * (angle)) * radius + target.transform.position.z;
        transform.position = pos;

        transform.LookAt(target);
    }
    void OnGUI()
    {
        guiStyle.fontSize = 40; //change the font size
        guiStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 100, 20), "Use WASD to view the stage", guiStyle);
        ///////////////////////////

    }

}

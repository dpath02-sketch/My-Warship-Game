using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject cam;
    public float camSens = 5.3F;
    public float speed = 0;
    public float baseSpeed = 220;
    public float accel = 60;
    public float agil = 61.5F;
    private float camLat = 0;
    private float camLon = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        CamMove(20, 5);
        if (Input.GetKey(KeyCode.R))
        {
            speed += accel * Time.deltaTime;
            if (speed > baseSpeed)
            {
                speed = baseSpeed;
            }
        }
        if (Input.GetKey(KeyCode.F))
        {
            speed -= accel * Time.deltaTime;
            if (speed < 0)
            {
                speed = 0;
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(new Vector3(agil * Time.deltaTime * -1, 0, 0), Space.Self);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(new Vector3(agil * Time.deltaTime, 0, 0), Space.Self);
        }
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, agil * Time.deltaTime, 0), Space.Self);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, agil * Time.deltaTime * -1, 0), Space.Self);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(new Vector3(0, 0, agil * Time.deltaTime * -1), Space.Self);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0, 0, agil * Time.deltaTime), Space.Self);
        }
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);
    }

    public void CamMove(float dist, float height)
    {
        this.cam.transform.position = transform.position;
        this.cam.transform.rotation = transform.rotation;
        this.camLon -= Input.GetAxis("Mouse Y") * this.camSens;
        if (this.camLon > 90)
        {
            this.camLon = 90;
        }
        else if (this.camLon < -90)
        {
            this.camLon = -90;
        }
        this.camLat += Input.GetAxis("Mouse X") * this.camSens;
        if (this.camLat > 180)
        {
            this.camLat -= 360;
        }
        else if (this.camLat < -180)
        {
            this.camLat += 360;
        }
        this.cam.transform.Rotate(new Vector3(0, this.camLat, 0), Space.Self);
        this.cam.transform.Rotate(new Vector3(this.camLon, 0, 0), Space.Self);
        this.cam.transform.Translate(new Vector3(0, height, -1 * dist), Space.Self);
    }
}

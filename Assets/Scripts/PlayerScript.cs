using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public GameObject cam;
    public float camSens = 5.3F;
    public float speed = 0;
    public float baseSpeed = 220;
    public float accel = 60;
    public float agil = 61.5F;
    public int baseHull = 2750;
    public int baseShield = 2750;
    public float hull = 2750;
    public float shield = 2750;
    public Slider hudShield;
    public Slider hudHull;
    private float camLat = 0;
    private float camLon = 0;
    private float armour = 1.5F;
    private float deflection = 1.5F;
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

        //Debug
        float temp = hull + shield;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Damage(100, 1, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Damage(800, 1, 1);
        }
    }

    private void CamMove(float dist, float height)
    {
        cam.transform.position = transform.position;
        cam.transform.rotation = transform.rotation;
        camLon -= Input.GetAxis("Mouse Y") * camSens;
        if (camLon > 90)
        {
            camLon = 90;
        }
        else if (camLon < -90)
        {
            camLon = -90;
        }
        camLat += Input.GetAxis("Mouse X") * camSens;
        if (camLat > 180)
        {
            camLat -= 360;
        }
        else if (camLat < -180)
        {
            camLat += 360;
        }
        cam.transform.Rotate(new Vector3(0, camLat, 0), Space.Self);
        cam.transform.Rotate(new Vector3(camLon, 0, 0), Space.Self);
        cam.transform.Translate(new Vector3(0, height, -1 * dist), Space.Self);
    }

    public void Damage(int dmg, float hullx, float shieldx) //trying to recreate generic space hull shield damage. this bouta get crazy
    {
        float inDmg = dmg * shieldx; // incomming damage, currently for shield
        if (shield / baseShield > 0.1) // if shield is low consider damaging hull
        {
            shield -= Mathf.Ceil(inDmg - deflection); //Ceil to guarantee some damage
            if (shield < 0) //incase you take a hit from 10%+ shield to hull
            {
                hull -= Mathf.Ceil(-1 * shield * hullx / shieldx - armour);
                shield = 0; // No negetive shield
            }
        }
        else // decided to damage hull
        {
            float local = (inDmg - deflection) * shield / baseShield * 10; // realise since shield is in the calc hull's calc is affefcted by the damage taken
            shield -= Mathf.Ceil(local); // less sheild remaining = more hull damage
            inDmg -= local; // shield absored some damge
            inDmg = inDmg * hullx / shieldx - armour; // convert remaining shield damage for hull\
            hull -= Mathf.Ceil(inDmg); // finaly taking damage to the ship itself. K.I.S.S can go K.Y.S
            if (hull <= 0)
            {
                //TODO death
                Debug.Log("Dead");
            }
        }
        hudSliders();
    }

    public void hudSliders()
    {
        hudShield.maxValue = baseShield;
        hudHull.maxValue = baseHull;
        hudShield.value = shield;
        hudHull.value = hull;
    }
}

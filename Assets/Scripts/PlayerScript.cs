using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
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
    public float shieldRgn = 10.5F;
    public Slider hudShield;
    public Slider hudHull;
    public Slider hudSpeed;
    public List<GameObject> turrets;
    public Button close;
    public TMP_Text closeText;
    public TMP_Text dmgText;
    public TMP_Text hullText;
    public TMP_Text shieldText;
    private float camLat = 0;
    private float camLon = 0;
    private float armour = 1.5F;
    private float deflection = 1.5F;
    private List<float> damages = new List<float>();
    private List<float> times = new List<float>();
    //TODO alpha and dps meter. aplha in % of health and DPS in time left to death
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        HealthSliders();
        SpeedSlider();
        close.interactable = false;
        closeText.color = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse Lock
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                close.interactable = true;
                closeText.color = new Color(255, 255, 255, 255);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                close.interactable = false;
                closeText.color = new Color(0, 0, 0, 0);
            }
        }
        //Movement:
        CamMove(44.72F, 11.18F);
        if (Input.GetKey(KeyCode.R))
        {
            speed += accel * Time.deltaTime;
            if (speed > baseSpeed)
            {
                speed = baseSpeed;
            }
            SpeedSlider();
        }
        if (Input.GetKey(KeyCode.F))
        {
            speed -= accel * Time.deltaTime;
            if (speed < 0)
            {
                speed = 0;
            }
            SpeedSlider();
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(new Vector3(agil * Time.deltaTime * -1, 0, 0), Space.Self);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(new Vector3(agil * Time.deltaTime, 0, 0), Space.Self);
        }
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
        //Health:
        shield += shieldRgn * Time.deltaTime;
        if (shield > baseShield)
        {
            shield = baseShield;
        }
        HealthSliders();
        //Turrets
        AimTurrets();
        RenderDamage();
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
        float oldHP = hull + shield;
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
            if (shield < 0)
            {
                shield = 0;
            }
            inDmg -= local; // shield absored some damge
            inDmg = inDmg * hullx / shieldx - armour; // convert remaining shield damage for hull\
            hull -= Mathf.Ceil(inDmg); // finaly taking damage to the ship itself. K.I.S.S can go K.Y.S
            if (hull <= 0)
            {
                //TODO death
                Debug.Log("Dead");
            }
        }
        RegisterHit(oldHP - (hull + shield), ref damages, ref times);
    }

    public void HealthSliders()
    {
        hudShield.maxValue = baseShield;
        hudHull.maxValue = baseHull;
        hudShield.value = shield;
        hudHull.value = hull;
    }

    public void SpeedSlider()
    {
        hudSpeed.maxValue = baseSpeed;
        hudSpeed.value = speed;
    }

    public void AimTurrets()
    {
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hitPoint;
        Physics.Raycast(ray, out hitPoint, 5000);
        if (hitPoint.collider != null)
        {
            for(int i = 0; i < turrets.Count; i += 1)
            {
                turrets[i].GetComponent<TurretRelay>().Target(hitPoint.point);
            }
        }
        else
        {
           for(int i = 0; i < turrets.Count; i += 1)
            {
                turrets[i].GetComponent<TurretRelay>().Target(transform.position + cam.transform.forward * 5000);
            } 
        }
    }

    private void RegisterHit(float forDamage, ref List<float> toList, ref List<float> toTimeList)
    {
        toList.Add(forDamage);
        toTimeList.Add(Time.time);
    }

    private void RenderDamage()
    {
        CleanRegistry();
        float DPS = 0;
        float potentialAplha = 0;
        for (int i = 0; i < damages.Count; i += 1)
        {
            DPS += damages[i];
            if (damages[i] > potentialAplha)
            {
                potentialAplha = damages[i];
            }
        }
        int mins = Mathf.FloorToInt((hull + shield) / DPS / 60);
        int secs = Mathf.FloorToInt((hull + shield) / DPS - mins * 60);
        float relAlpha = potentialAplha / (hull + shield) * 100;
        if (potentialAplha != 0)
        {
            dmgText.text = mins.ToString() + ":" + secs.ToString() + ", " + relAlpha.ToString("0.00") + "%";
        }
        else
        {
            dmgText.text = "Infinite, " + relAlpha.ToString("0.00") + "%";
        }
        hullText.text = Mathf.FloorToInt(hull / baseHull * 100).ToString() + "%";
        shieldText.text = Mathf.FloorToInt(shield / baseShield * 100).ToString() + "%";
    }

    private void CleanRegistry()
    {
        List<float> tempTime = new List<float>();
        List<float> tempDamages = new List<float>();
        for (int i = 0; i < times.Count; i += 1)
        {
            if (times[i] >= Time.time - 2)
            {
                tempTime.Add(times[i]);
                tempDamages.Add(damages[i]);
            }
        }
        times = tempTime;
        damages = tempDamages;
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}

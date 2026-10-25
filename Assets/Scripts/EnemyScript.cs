using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed = 0;
    public float baseSpeed = 260;
    public float accel = 120;
    public float agil = 112F;
    public int baseHull = 200;
    public int baseShield = 200;
    public float hull = 200;
    public float shield = 200;
    public float shieldRgn = 2F;
    private float armour = 0F;
    private float deflection = 0F;
    //TODO how to add multiple targets???
    public PlayerScript playerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("isPilot").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        VirtualPilot();
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);
        shield += shieldRgn * Time.deltaTime;
        if (shield > baseShield)
        {
            shield = baseShield;
        }
    }

    private void VirtualPilot()
    {
        if ((playerScript.transform.position - transform.position).magnitude <= 625) //Also multiple targets would mean unique distances
        {
            if (speed < baseSpeed) //Target; max speed
            {
                speed += accel * Time.deltaTime;
                if (speed > baseSpeed)
                {
                    speed = baseSpeed;
                }
            }
        }
        else
        {
            if (speed > baseSpeed / 2) //No Target; half speed
            {
                speed -= accel * Time.deltaTime;
                if (speed < baseSpeed /2)
                {
                    speed = baseSpeed / 2;
                }
            }
            else if (speed < baseSpeed / 2) //Below half speed; speed up
            {
                speed += accel * Time.deltaTime;
                if (speed > baseSpeed / 2)
                {
                    speed = baseSpeed / 2;
                }
            }
        }
    }

    public void Damage(int dmg, float hullx, float shieldx) // It's official the damage script is universal
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
                //TODO propper death
                Destroy(gameObject);
            }
        }
    }
}

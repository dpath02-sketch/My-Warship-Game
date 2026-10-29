using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipScript : MonoBehaviour
{
    public float speed = 0;
    public float baseSpeed = 50; //Halving all speeds to try and dial things back
    public float accel = 27;
    public float agil = 61.5F;
    public int baseHull = 1375;
    public int baseShield = 1375;
    public float hull = 1375;
    public float shield = 1375;
    public float shieldRgn = 5.25F;
    public GameObject tBS;
    private float armour = 0.75F;
    private float deflection = 0.75F;
    private GameObject bigShot;
    private float fireRateS = 2; // 60 / RPM
    private bool trigHeld = false;
    private float coolDown = 0;
    private float realismFactor = 0; //Pasta
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
        // Weapons
        if (trigHeld && coolDown <= 0)
        {
            Fire();
            coolDown = fireRateS;
        }
        coolDown -= Time.deltaTime;
    }

    private void VirtualPilot()
    {
        Vector3 targetPos = new Vector3(0, 0, 0);
        if ((playerScript.transform.position - transform.position).magnitude <= 5000) //Also multiple targets would mean unique distances
        {
            SetSpeed(baseSpeed);
            // Goto random point within some distance
            if (targetPos == new Vector3(0, 0, 0) || (targetPos - transform.position).magnitude <= TurningRadius())
            {
                targetPos = playerScript.transform.position + Random.insideUnitSphere * 2500;
            }
            TurnTowards(targetPos);
            trigHeld = true;
        }
        else
        {
            SetSpeed(baseSpeed / 2);
            targetPos = new Vector3(0, 0, 0);
            TurnTowards(targetPos);
            trigHeld = false;
        }
    }

    private void SetSpeed(float speedT)
    {
        if (speed > speedT)
        {
            speed -= accel * Time.deltaTime;
            if (speed < speedT)
            {
                speed = speedT;
            }
        }
        else if (speed < speedT)
        {
            speed += accel * Time.deltaTime;
            if (speed > speedT)
            {
                speed = speedT;
            }
        }
    }

    private float TurningRadius()
    {
        float circumfrance = 180 / agil * speed;
        float radius = circumfrance / Mathf.PI;
        return radius;
    }

    private void TurnTowards(Vector3 otherTransform)
    {
        Quaternion oldRotation = transform.rotation;
        transform.LookAt(otherTransform, transform.up);
        Quaternion newRotation = transform.rotation;
        transform.rotation = Quaternion.Lerp(oldRotation, newRotation, agil * Time.deltaTime / Quaternion.Angle(oldRotation, newRotation));
    }

    private bool TargetInFOV(Vector3 target, float fov)
    {
        Quaternion oldRotation = transform.rotation;
        transform.LookAt(target, transform.up);
        Quaternion newRotation = transform.rotation;
        transform.rotation = oldRotation;
        return Quaternion.Angle(oldRotation, newRotation) < fov / 2; //half cause it goes both ways
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
            if (shield < 0)
            {
                shield = 0;
            }
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

    void Fire()
    {
        Quaternion oldDirection = transform.rotation;
        transform.LookAt(playerScript.transform.position, transform.up);
        Vector3 targetDir = transform.forward;
        transform.rotation = oldDirection;
        // FU
        bigShot = Instantiate(tBS, transform.position + transform.forward * 5.99251F + transform.up * 1.61515F, transform.rotation);
        // Realism factor is how much conservation of momentum affects things
        bigShot.GetComponent<Projectile>().OnSummon(targetDir * 200 + transform.forward * speed * realismFactor, 5000 / 200, 5, 1, 1, true);
         // FD
        bigShot = Instantiate(tBS, transform.position + transform.forward * 5.99251F + transform.up * -1.61515F, transform.rotation);
        // Realism factor is how much conservation of momentum affects things
        bigShot.GetComponent<Projectile>().OnSummon(targetDir * 200 + transform.forward * speed * realismFactor, 5000 / 200, 5, 1, 1, true);
         // BU
        bigShot = Instantiate(tBS, transform.position + transform.forward * -5.99251F + transform.up * 1.61515F, transform.rotation);
        // Realism factor is how much conservation of momentum affects things
        bigShot.GetComponent<Projectile>().OnSummon(targetDir * 200 + transform.forward * speed * realismFactor, 5000 / 200, 5, 1, 1, true);
         // BD
        bigShot = Instantiate(tBS, transform.position + transform.forward * -5.99251F + transform.up * -1.61515F, transform.rotation);
        // Realism factor is how much conservation of momentum affects things
        bigShot.GetComponent<Projectile>().OnSummon(targetDir * 200 + transform.forward * speed * realismFactor, 5000 / 200, 5, 1, 1, true);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShipScript : MonoBehaviour
{
    public float speed = 0;
    public float baseSpeed = 100; //Serialised to 50 so you can catch up
    public float accel = 27;
    public float agil = 61.5F;
    public int baseHull = 1375; //Nerf
    public int baseShield = 1375;
    public float hull = 1375;
    public float shield = 1375;
    public float shieldRgn = 5.3F;
    public GameObject tBS;
    private float armour = 0.75F;
    private float deflection = 0.75F;
    private GameObject bigShot;
    private float fireRateS = 1.33F; // 60 / RPM
    private bool trigHeld = false;
    private float coolDown = 0;
    private float realismFactor = 0; //Pasta
    //TODO how to add multiple targets???
    private Slider shieldInstance;
    private Slider hullInstance;
    private TMP_Text damageTextInstance;
    private float yTTL = 0;
    private float temp;
    public PlayerScript playerScript;
    public Canvas canvas;
    public Slider hullSlider;
    public Slider shieldSlider;
    public TMP_Text damageText;
    private Vector3 targetPos = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        playerScript = FindAnyObjectByType<PlayerScript>();
        canvas = FindAnyObjectByType<Canvas>();
        shieldInstance = Instantiate(shieldSlider, canvas.GetComponentInParent<RectTransform>());
        hullInstance = Instantiate(hullSlider, canvas.GetComponent<RectTransform>());
        shieldInstance.maxValue = baseShield;
        hullInstance.maxValue = baseHull;
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
        yTTL -= Time.deltaTime;
        if (null != damageTextInstance && yTTL <= 0)
        {
            Destroy(damageTextInstance.gameObject);
        }
    }

    void LateUpdate()
    {
        if (Camera.main.WorldToScreenPoint(transform.position).z >= 0)
        {
            shieldInstance.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            shieldInstance.transform.position += new Vector3(0, -4800 / shieldInstance.transform.position.z, 0);
            hullInstance.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            hullInstance.transform.position += new Vector3(0, -4800 / hullInstance.transform.position.z -10, 0);
            shieldInstance.value = shield;
            hullInstance.value = hull;
        }
        else
        {
            shieldInstance.transform.position = new Vector3(float.MaxValue, 0, 0);
            hullInstance.transform.position = new Vector3(float.MaxValue, 0, 0);
        }
    }

    void OnDestroy()
    {
        Destroy(shieldInstance.gameObject);
        Destroy(hullInstance.gameObject);
        if (null != damageTextInstance)
        {
            Destroy(damageTextInstance.gameObject);
        }
    }

    private void VirtualPilot()
    {
        if ((playerScript.transform.position - transform.position).magnitude <= 2500) //make enemies stay close to origin
        {
            SetSpeed(baseSpeed);
            // Goto random point within some distance
            if (targetPos == new Vector3(0, 0, 0) || (targetPos - transform.position).magnitude <= TurningRadius())
            {
                targetPos = playerScript.transform.position + Random.insideUnitSphere * 937.5F;
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
        //UI stuff
        if (null != damageTextInstance)
        {
            Destroy(damageTextInstance.gameObject);
        }
        else
        {
            temp = shield + hull;
        }
        damageTextInstance = Instantiate(damageText, canvas.GetComponent<RectTransform>());
        damageTextInstance.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Random.insideUnitSphere * 50;
        yTTL = 1;
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
                playerScript.score += Mathf.RoundToInt((baseHull + baseShield) * 18 / fireRateS);
                Destroy(gameObject);
            }
        }
        damageTextInstance.text = Mathf.RoundToInt(temp - (shield + hull)).ToString();
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
        bigShot.GetComponent<Projectile>().OnSummon(targetDir * 200 + transform.forward * speed * realismFactor, 4000 / 200, 30, 1, 1, true);
         // FD
        bigShot = Instantiate(tBS, transform.position + transform.forward * 5.99251F + transform.up * -1.61515F, transform.rotation);
        // Realism factor is how much conservation of momentum affects things
        bigShot.GetComponent<Projectile>().OnSummon(targetDir * 200 + transform.forward * speed * realismFactor, 4000 / 200, 30, 1, 1, true);
         // BU
        bigShot = Instantiate(tBS, transform.position + transform.forward * -5.99251F + transform.up * 1.61515F, transform.rotation);
        // Realism factor is how much conservation of momentum affects things
        bigShot.GetComponent<Projectile>().OnSummon(targetDir * 200 + transform.forward * speed * realismFactor, 4000 / 200, 30, 1, 1, true);
         // BD
        bigShot = Instantiate(tBS, transform.position + transform.forward * -5.99251F + transform.up * -1.61515F, transform.rotation);
        // Realism factor is how much conservation of momentum affects things
        bigShot.GetComponent<Projectile>().OnSummon(targetDir * 200 + transform.forward * speed * realismFactor, 4000 / 200, 30, 1, 1, true);
    }
}

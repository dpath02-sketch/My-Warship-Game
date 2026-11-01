using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public float speed = 0;
    public float baseSpeed = 50;
    public float accel = 46;
    public float agil = 112F;
    public int baseHull = 100;
    public int baseShield = 100;
    public float hull = 100;
    public float shield = 100;
    public float shieldRgn = 1;
    public GameObject tBS;
    private float armour = 0;
    private float deflection = 0;
    private GameObject bigShot;
    private float fireRate = 0.3F; // 60 / RPM
    private bool trigHeld = false;
    private float coolDown = 0;
    private bool fireSide = true;
    private float realismFactor = 0; //made it 1 since it's kinda faster than the projectiles so that's an issue
    //TODO how to add multiple targets???
    private Slider shieldInstance;
    private Slider hullInstance;
    private TMP_Text damageTextInstance;
    private float yTTL = 0;
    public PlayerScript playerScript;
    public Canvas canvas;
    public Slider hullSlider;
    public Slider shieldSlider;
    public TMP_Text damageText;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("isPilot").GetComponent<PlayerScript>();
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
            Fire(fireSide);
            coolDown = fireRate;
            if (fireSide)
            {
                fireSide = false;
            }
            else
            {
                fireSide = true;
            }
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
            shieldInstance.transform.position += new Vector3(0, -200 / shieldInstance.transform.position.z, 0);
            hullInstance.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            hullInstance.transform.position += new Vector3(0, -200 / hullInstance.transform.position.z -10, 0);
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
        if ((playerScript.transform.position - transform.position).magnitude <= 5000) //Also multiple targets would mean unique distances
        {
            SetSpeed(baseSpeed);
            //Target; look at it
            if ((playerScript.transform.position - transform.position).magnitude <= TurningRadius())
            {
                TurnTowards((playerScript.transform.position - transform.position) * -1 + transform.position);
            }
            else if ((playerScript.transform.position - transform.position).magnitude >= TurningRadius() + speed * 5 / 2) //extra buffer for turning. speed's coefficient is seconds of fire time
            {
                TurnTowards(playerScript.transform.position);
            }
            else if (TargetInFOV(playerScript.transform.position, 120))
            {
                TurnTowards(playerScript.transform.position);
            }
            trigHeld = TargetInFOV(playerScript.transform.position, 120);
        }
        else
        {
            SetSpeed(baseSpeed / 2);
            TurnTowards(new Vector3(0, 0, 0));
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
        float temp = shield + hull;
        //UI stuff
        if (null != damageTextInstance)
        {
            Destroy(damageTextInstance.gameObject);
        }
        damageTextInstance = Instantiate(damageText, canvas.GetComponent<RectTransform>());
        damageTextInstance.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Random.insideUnitSphere * 100;
        yTTL = 2;
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
        damageTextInstance.text = Mathf.RoundToInt(temp - shield - hull).ToString();
    }

    void Fire(bool right)
    {
        if (right)
        {
            bigShot = Instantiate(tBS, transform.position + transform.right * 2.15F + transform.forward * 1.62648F, transform.rotation);
            // Realism factor is how much conservation of momentum affects things
            bigShot.GetComponent<Projectile>().OnSummon(transform.forward * 200 + transform.forward * speed * realismFactor, 5000 / 200, 9, 1, 1, true);
        }
        else
        {
            bigShot = Instantiate(tBS, transform.position + transform.right * -2.15F + transform.forward * 1.62648F, transform.rotation);
            // Realism factor is how much conservation of momentum affects things
            bigShot.GetComponent<Projectile>().OnSummon(transform.forward * 200 + transform.forward * speed * realismFactor, 5000 / 200, 9, 1, 1, true);
        }
    }
}

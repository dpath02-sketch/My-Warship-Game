using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject tBS;
    public GameObject ship;
    //TODO not sure if I should bother publicing cause unity serialisation
    public float fireRate = 0.5F; // 60 / RPM
    public int damage = 10;
    private float coolDown = 0;
    private GameObject bigShot;
    public float realismFactor = 0; //kept as 0 FOR NOW since hitting still targets is more intuative this way. STILL KEEPING THE VARIABLE as an option though, made public for PDT reasons
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && coolDown <= 0)
        {
            Fire();
            coolDown = fireRate;
        }
        coolDown -= Time.deltaTime;
    }

    public void Target(Vector3 pos, Vector3 up)
    {
        transform.LookAt(pos, up);
    }

    public void Fire()
    {
        bigShot = Instantiate(tBS, transform.position, transform.rotation);
        // Realism factor is how much conservation of momentum affects things
        bigShot.GetComponent<Projectile>().OnSummon(transform.forward * 200 + ship.transform.forward * ship.GetComponent<PlayerScript>().speed * realismFactor, 5000 / 200, damage, 1, 1, false); // Add ships speed to projectile. slightly exploitable but idc
    }
}

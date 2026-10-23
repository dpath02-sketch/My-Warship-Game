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
    float fireRate = 60 / 60; // 60 / RPM
    private float coolDown = 0;
    private GameObject bigShot;
    private float realismFactor = 0.5F;
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
        bigShot.GetComponent<Projectile>().OnSummon(transform.forward * 110 + ship.transform.forward * ship.GetComponent<PlayerScript>().speed * realismFactor, 2500 / 110, 10, 1, 1); // Add ships speed to projectile. slightly exploitable but idc
    }
}

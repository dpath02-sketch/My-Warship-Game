using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject tBS;
    public GameObject ship;
    private GameObject bigShot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Target(Vector3 pos, Vector3 up)
    {
        transform.LookAt(pos, up);
        //Fire(); maybe don't constant fire. also no enemies or props to fire at
    }

    public void Fire()
    {
        bigShot = Instantiate(tBS, transform.position, transform.rotation);
        bigShot.GetComponent<Projectile>().OnSummon(transform.forward * 440 + ship.transform.forward * ship.GetComponent<PlayerScript>().speed, 2500 / 440, 10, 1, 1); // Add ships speed to projectile. slightly exploitable but idc
    }
}

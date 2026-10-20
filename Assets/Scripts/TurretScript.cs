using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject TBS;
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
        bigShot = Instantiate(TBS, transform.position, transform.rotation);
        bigShot.GetComponent<Projectile>().OnSummon(440, 2500);
    }
}

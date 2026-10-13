using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRelay : MonoBehaviour
{
    public GameObject barrel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Target(Vector3 pos)
    {
        transform.LookAt(pos);
        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);
        transform.Rotate(new Vector3(0, 0, 180), Space.Self); //sick + me no like rotations :(
        barrel.GetComponent<TurretScript>().Target(pos);
    }
}

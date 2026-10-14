using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRelay : MonoBehaviour
{
    public GameObject barrel;
    public Transform ship;
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
        transform.LookAt(pos, ship.forward * -1);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        //transform.Rotate(new Vector3(90, 0, 0), Space.Self);
        //transform.Rotate(new Vector3(0, 0, 180), Space.Self);
        Debug.Log(transform.localEulerAngles);
        //Debug.Log(transform.forward.ToString() + transform.right.ToString() + transform.up.ToString());
        barrel.GetComponent<TurretScript>().Target(pos);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
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
        transform.Rotate(new Vector3(90, 180, 0), Space.Self);
    }
}

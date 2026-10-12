using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "isPilot")
        {
            other.gameObject.GetComponent<PlayerScript>().Damage(450, 1, 1);
        }
    }
}

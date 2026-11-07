using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class TheUnrivaledPowerOfTheSun : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        PlayerScript playerScript = other.GetComponent<PlayerScript>();
        if (playerScript != null)
        {
            playerScript.Damage(Mathf.RoundToInt((2500 - (other.transform.position - transform.position).magnitude) * 3.67F * Time.deltaTime), 0.70F, 1.40F);
            playerScript.suned = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        PlayerScript playerScript = other.GetComponent<PlayerScript>();
        if (playerScript != null && playerScript.alive)
        {
            playerScript.suned = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector3 speed = new Vector3(0, 0, 0);
    float time = 0;
    float timeMax = 1;
    int damage = 0;
    float hullmul = 0;
    float shieldyield = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime, Space.World);
        time += Time.deltaTime;
        if (time > timeMax)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "isPilot")
        {
            // Target is a ship of some kind
            Debug.LogWarning("isPilot");
            if (collision.gameObject.GetComponent<PlayerScript>() == null)
            {
                //TODO Target is a ship that's not the player.
                Debug.LogWarning("notPlayer");
                Destroy(gameObject);
            }
            else
            {
                // Target is the player. don't damage
                Debug.LogWarning("isPlayer");
                Destroy(gameObject);
            }
        }
        else
        {
            // Target is not a ship, thats all we know
            Debug.LogWarning("notPilot");
            if (collision.gameObject.GetComponent<BreakableScript>() == null)
            {
                //TODO Target is not a ship or breakable
                Destroy(gameObject);
            }
            else
            {
                // Target is a breakable
                collision.gameObject.GetComponent<BreakableScript>().Damage(damage, hullmul, shieldyield);
                Destroy(gameObject);
            }
        }
    }

    public void OnSummon(Vector3 spd, float mx, int dmg, float hm, float sy)
    {
        speed = spd;
        timeMax = mx;
        damage = dmg;
        hullmul = hm;
        shieldyield = sy;
    }
}

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
    bool targetP = false;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().mass = damage;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > timeMax)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision) //TODO assumed that not targeting the player makes you the player and you know what they say about assuming.
    {
        if (collision.gameObject.tag == "isPilot")
        {
            // Target is a ship of some kind
            if (collision.gameObject.GetComponent<PlayerScript>() == null)
            {
                //Target is a ship that's not the player.
                if (targetP)
                {
                    Debug.LogWarning("notPlayer");
                }
                else
                {
                    if (collision.gameObject.GetComponent<EnemyScript>() == null && collision.gameObject.GetComponent<EnemyShipScript>() == null)
                    {
                        //Target isn't a player or enemy? idk just log smthn
                        Debug.LogWarning("UPO (Unidentifiable Pilot Object)");
                        Destroy(gameObject);
                    }
                    else
                    {
                        if (collision.gameObject.GetComponent<EnemyShipScript>() == null)
                        {
                            collision.gameObject.GetComponent<EnemyScript>().Damage(damage, hullmul, shieldyield);
                            Debug.Log(collision.gameObject.GetComponent<EnemyScript>().hull + collision.gameObject.GetComponent<EnemyScript>().shield);
                            Destroy(gameObject);
                        }
                        else
                        {
                            collision.gameObject.GetComponent<EnemyShipScript>().Damage(damage, hullmul, shieldyield);
                            Debug.Log(collision.gameObject.GetComponent<EnemyShipScript>().hull + collision.gameObject.GetComponent<EnemyShipScript>().shield);
                            Destroy(gameObject);
                        }
                    }
                }
            }
            else
            {
                // Target is the player. don't damage (unless your an enemy's shot)
                if (targetP)
                {
                    collision.gameObject.GetComponent<PlayerScript>().Damage(damage, hullmul, shieldyield);
                    Destroy(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            // Target is not a ship, thats all we know
            if (collision.gameObject.GetComponent<BreakableScript>() == null)
            {
                //Target is not a ship or breakable
                if (collision.gameObject.GetComponent<Projectile>() == null)
                {
                    //Target isn't even a projectile. wtf
                    Debug.LogWarning("UGO (Unidentifiable Generic Object)");
                    Destroy(gameObject);
                }
                else
                {
                    //Target is a projectile... so what?
                }
            }
            else
            {
                // Target is a breakable
                collision.gameObject.GetComponent<BreakableScript>().Damage(damage, hullmul, shieldyield);
                Destroy(gameObject);
            }
        }
    }

    public void OnSummon(Vector3 spd, float mx, int dmg, float hm, float sy, bool tP)
    {
        speed = spd;
        timeMax = mx;
        damage = dmg;
        hullmul = hm;
        shieldyield = sy;
        targetP = tP;
        gameObject.GetComponent<Rigidbody>().velocity = speed;
    }
}

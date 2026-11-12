using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakableScript : MonoBehaviour
{
    public AudioSource hitSound;
    float health; //TODO gotta somehow make this at least semi-data-driven
    float size;
    private PlayerScript playerScript; //TODO only here for score counter, which is honestly kinda temporary but I'm outta ideas (also 35 celcius make thinking pain)
    // Start is called before the first frame update
    void Start()
    {
        playerScript = FindAnyObjectByType<PlayerScript>();
        size = Random.Range(50, 200);
        health = size;
        transform.localScale = new Vector3(size / 2, size / 2, size / 2);
        gameObject.GetComponent<Rigidbody>().mass = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Damage(int dmg, float hullx, float shieldx) //Yes I stole the players damage script even though this could be alot simpler
    {
        health -= Mathf.Ceil(dmg * hullx);
        if (health <= 0)
        {
            NotSoMentalBreakDown();
        }
        hitSound.Play();
    }

    private void NotSoMentalBreakDown()
    {
        playerScript.score += Mathf.RoundToInt(size);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
        if (player != null)
        {
            player.Damage(Mathf.RoundToInt(Mathf.Abs(((collision.transform.forward * player.speed) - gameObject.GetComponent<Rigidbody>().velocity).magnitude) * 6.25F), 1.40F, 0.70F);
            playerScript.rocked = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
        if (player != null && playerScript.alive)
        {
            playerScript.rocked = false;
        }
    }
}

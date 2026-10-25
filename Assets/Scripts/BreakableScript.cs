using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableScript : MonoBehaviour
{
    float health; //TODO gotta somehow make this at least semi-data-driven
    // Start is called before the first frame update
    void Start()
    {
        float size = Random.Range(50, 200);
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
    }

    private void NotSoMentalBreakDown()
    {
        Debug.Log("Explosion SFX");
        Destroy(gameObject);
    }
}

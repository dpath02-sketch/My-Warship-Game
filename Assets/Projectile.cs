using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 0;
    float dist = 0;
    float distMax = 5000;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distMoved = speed * Time.deltaTime;
        transform.Translate(new Vector3(0, 0, distMoved), Space.Self);
        dist += distMoved;
        if (dist > distMax)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Theoretical code if enemies and props existed. also the keybind is shift + alt yk what I mean
        //if (collision.gameObject.GetComponent<EnemyScript>() == null)
        //{
        //    if (collision.gameObject.GetComponent<PropScript>() == null)
        //    {
        //        //not enemy or prop
        //    }
        //    else
        //    {
        //        // is prop
        //    }
        //}
        //else
        //{
        //    // is enemy
        //}
    }

    public void OnSummon(float spd, float mx)
    {
        speed = spd;
        distMax = mx;
    }
}

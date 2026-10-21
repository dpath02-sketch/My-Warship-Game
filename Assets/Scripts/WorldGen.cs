using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    public GameObject rockAsset;
    // Start is called before the first frame update
    void Start()
    {
        GenerateWorld();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateWorld()
    {
        for (int i = 0; i < 160; i ++) // My PC 'bouta go *incomprehensable screaming*. I really don't wanna have to go into multithreading on a small project
        {
            //rand pos
            transform.localEulerAngles = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
            transform.position = new Vector3(0, 0, 0);
            transform.Translate(new Vector3(0, 0, Random.Range(625, 1250) ^ 2));
            //rand dir
            transform.localEulerAngles = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
            //spawn
            Instantiate(rockAsset, transform.position, transform.rotation);
        }
    }
}

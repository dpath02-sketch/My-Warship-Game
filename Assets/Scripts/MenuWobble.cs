using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWobble : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Sin(Time.time * Mathf.PI * 0.5F) * 0.125F, Mathf.Sin(Time.time * Mathf.PI + Mathf.PI * 0.125F) * 0.125F, Mathf.Sin(Time.time * Mathf.PI + Mathf.PI * 0.0625F) * 0.0625F - 10);
    }
}

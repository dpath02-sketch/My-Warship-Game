using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretRelay : MonoBehaviour
{
    public GameObject barrel;
    public Transform ship;
    public int dir; //0 = up, 1 = right, 2 = left, 3 = down //TODO likely gonna have to update this system for angles not included in this
    private Vector3 dirResult;
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
        CalcDir();
        transform.LookAt(pos, dirResult);
        if (dir == 0)
        {
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }
        else if (dir == 1)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, -90);
        }
        else if (dir == 2)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, 90);
        }
        else if (dir == 3)
        {
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 180);
        }
        barrel.GetComponent<TurretScript>().Target(pos, dirResult);
    }

    private void CalcDir()
    {
        if (dir == 0)
        {
            dirResult = ship.up;
        }
        else if (dir == 1)
        {
            dirResult = ship.right;
        }
        else if (dir == 2)
        {
            dirResult = ship.right * -1;
        }
        else if (dir == 3)
        {
            dirResult = ship.up * -1;
        }
    }
}

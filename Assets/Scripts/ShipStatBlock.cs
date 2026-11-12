using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStatBlock : MonoBehaviour
{
    public float speed = 0;
    public float baseSpeed = 100;
    public float accel = 27;
    public float agil = 61.5F;
    public int baseHull = 2750;
    public int baseShield = 2750;
    public float hull = 2750;
    public float shield = 2750;
    public float shieldRgn = 10.5F;
    public List<GameObject> turrets;
    public List<string> turretNames;
    public List<string> turretSizes;
    public List<Vector3> turretPositions;
    public float armour = 1.5F;
    public float deflection = 1.5F;
    public void Init()
    {
        turretSizes = new List<string>();
        turretSizes.Add("S");
        turretSizes.Add("S");
        turretSizes.Add("S");
        turretSizes.Add("S");
        turretSizes.Add("P");
        turretSizes.Add("P");
        turretSizes.Add("P");
        turretSizes.Add("P");
        turretPositions = new List<Vector3>();
        turretPositions.Add(new Vector3(0, 2.75F, 3.125F));
        turretPositions.Add(new Vector3(0, -2.75F, 3.125F));
        turretPositions.Add(new Vector3(-5.25F, 0, 3.125F));
        turretPositions.Add(new Vector3(5.25F, 0, 3.125F));
        turretPositions.Add(new Vector3(0, 2.5625F, -4.75F));
        turretPositions.Add(new Vector3(0, -2.5625F, -4.75F));
        turretPositions.Add(new Vector3(-5.0625F, 0, -4.75F));
        turretPositions.Add(new Vector3(5.0625F, 0, -4.75F));
    }
}

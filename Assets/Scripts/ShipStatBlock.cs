using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStatBlock : MonoBehaviour
{
    public float baseSpeed = 100;
    public float accel = 27;
    public float agil = 61.5F;
    public int baseHull = 2750;
    public int baseShield = 2750;
    public float hull = 2750;
    public float shield = 2750;
    public float shieldRgn = 10.5F;
    public List<GameObject> turrets;
    public List<int> turretDirs;
    public List<string> turretNames;
    public List<string> turretSizes;
    public List<Vector3> turretPositions;
    public List<Quaternion> turretRotations;
    public float armour = 1.5F;
    public float deflection = 1.5F;
}

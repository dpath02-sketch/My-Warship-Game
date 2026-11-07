using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutHandler : MonoBehaviour
{
    public GameObject playerRoot;
    public GameObject playerShip;
    public List<GameObject> playerTrails;
    private PlayerScript playerScript;
    private MeshCollider playerHitBox;
    private Rigidbody playerRigidbody;
    private MeshFilter playerShipMesh;
    private MeshRenderer playerShipRenderer;
    public List<GameObject> tempTurrets1;
    public List<GameObject> tempTurrets2;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = playerRoot.GetComponent<PlayerScript>();
        playerHitBox = playerRoot.GetComponent<MeshCollider>();
        playerRigidbody = playerRoot.GetComponent<Rigidbody>();
        playerShipMesh = playerShip.GetComponent<MeshFilter>();
        playerShipRenderer = playerShip.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearTurrets()
    {
        for (int i = 0; i < playerScript.turrets.Count; i += 1)
        {
            Destroy(playerScript.turrets[i]);
        }
        playerScript.turrets.Clear();
    }

    public void SwitchTurret(int turretIndex, GameObject turret)
    {
        // Record old turret position
        Vector3 turretPos = playerScript.turrets[turretIndex].transform.position;
        Quaternion turretDir = playerScript.turrets[turretIndex].transform.rotation;
        int turretRootDir = playerScript.turrets[turretIndex].GetComponent<TurretRelay>().dir;
        // Out with the old...
        Destroy(playerScript.turrets[turretIndex]);
        // In with the new
        playerScript.turrets[turretIndex] = Instantiate(turret, playerShip.transform);
        playerScript.turrets[turretIndex].GetComponent<TurretRelay>().ship = playerShip;
        playerScript.turrets[turretIndex].GetComponentInChildren<TurretScript>().ship = playerShip;
        // Lock into place
        playerScript.turrets[turretIndex].transform.position = turretPos;
        playerScript.turrets[turretIndex].transform.rotation = turretDir;
        playerScript.turrets[turretIndex].GetComponent<TurretRelay>().dir = turretRootDir;
    }

    public void TempSwitchTurret1()
    {
        for (int i = 0; i < tempTurrets1.Count; i += 1)
        {
            SwitchTurret(i, tempTurrets1[i]);
        }
    }

    public void TempSwitchTurret2()
    {
        for (int i = 0; i < tempTurrets2.Count; i += 1)
        {
            SwitchTurret(i, tempTurrets2[i]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutHandler : MonoBehaviour
{
    public GameObject playerRoot;
    public GameObject playerShip;
    public List<GameObject> playerTrails;
    public LoadoutUIHandler UIHandler;
    private PlayerScript playerScript;
    private MeshCollider playerHitBox;
    private Rigidbody playerRigidbody;
    private MeshFilter playerShipMesh;
    private MeshRenderer playerShipRenderer;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = playerRoot.GetComponent<PlayerScript>();
        playerHitBox = playerRoot.GetComponent<MeshCollider>();
        playerRigidbody = playerRoot.GetComponent<Rigidbody>();
        playerShipMesh = playerShip.GetComponent<MeshFilter>();
        playerShipRenderer = playerShip.GetComponent<MeshRenderer>();
        CreateUI();
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
        playerScript.turrets[turretIndex].GetComponentInChildren<TurretScript>().ship = playerRoot; //TODO bro the Ship1 GameObject just doesn't seem to exist?!
        // Lock into place
        playerScript.turrets[turretIndex].transform.position = turretPos;
        playerScript.turrets[turretIndex].transform.rotation = turretDir;
        playerScript.turrets[turretIndex].GetComponent<TurretRelay>().dir = turretRootDir;
    }

    public void SwitchShip(ShipStatBlock stats, Mesh hitBox, Mesh mesh, List<Material> materials, List<Vector3> trailPositions, GameObject aPDT, GameObject anS, GameObject andAnMFromTheUIHandler)
    {
        LoadStatBlock(playerScript, stats);
        PurgeTurrets(playerScript);
        DefaultTurrets(playerScript, stats, aPDT, anS, andAnMFromTheUIHandler);
        playerHitBox.sharedMesh = hitBox; //TODO and here I have a bad feeling
        playerShipMesh.mesh = mesh;
        playerShipRenderer.SetMaterials(materials);
    }

    public void LoadStatBlock(PlayerScript playerScript, ShipStatBlock shipStatBlock)
    {
        playerScript.accel = shipStatBlock.accel;
        playerScript.agil = shipStatBlock.agil;
        playerScript.armour = shipStatBlock.armour;
        playerScript.baseHull = shipStatBlock.baseHull;
        playerScript.baseShield = shipStatBlock.baseShield;
        playerScript.baseSpeed = shipStatBlock.baseSpeed;
        playerScript.deflection = shipStatBlock.deflection;
        playerScript.hull = shipStatBlock.hull;
        playerScript.shield = shipStatBlock.shield;
        playerScript.shieldRgn = shipStatBlock.shieldRgn;
        playerScript.speed = 0;
        playerScript.turretSizes = shipStatBlock.turretSizes;
    }

    public void PurgeTurrets(PlayerScript playerScript)
    {
        for (int i = 0; i < playerScript.turrets.Count; i += 1)
        {
            Destroy(playerScript.turrets[i]);
        }
        playerScript.turrets.Clear();
        playerScript.turretNames.Clear();
    }

    private void DefaultTurrets(PlayerScript script, ShipStatBlock statBlock, GameObject aPDT, GameObject anS, GameObject andAnMFromTheUIHandler) //Ok fine I'll hold off on the public objects from now on (Neurodivergancy starts humming From Now On (Chapter 4 Battle))
    {
        for (int i = 0; i < script.turretNames.Count; i += 1)
        {
            if (script.turretNames[i] == "P")
            {
                script.turrets.Add(Instantiate(aPDT, statBlock.turretPositions[i], statBlock.turretRotations[i], playerShip.transform)); //TODO if stuff goes wrong just check here first the ship kinda don't exist for some reason
                script.turrets[script.turrets.Count - 1].GetComponentInChildren<TurretScript>().ship = playerShip; //TODO maybe this time? I know something else is gonna go wrong anyway may as well try
                script.turretNames.Add("Repeater");
            }
            else if (script.turretNames[i] == "S")
            {
                script.turrets.Add(Instantiate(anS, statBlock.turretPositions[i], statBlock.turretRotations[i], playerShip.transform));
                script.turrets[script.turrets.Count - 1].GetComponentInChildren<TurretScript>().ship = playerShip;
                script.turretNames.Add("Cannon");
            }
            else if (script.turretNames[i] == "M")
            {
                script.turrets.Add(Instantiate(andAnMFromTheUIHandler, statBlock.turretPositions[i], statBlock.turretRotations[i], playerShip.transform));
                script.turrets[script.turrets.Count - 1].GetComponentInChildren<TurretScript>().ship = playerShip;
                script.turretNames.Add("EggMansDong"); // I really shouldn't... BUT THE REFERENCE!!, plus it's kinda temporary it's only gonna exist on the git history
            }
            else
            {
                Debug.LogError("whelp time to add L turrets (sarcasm btw tho also not really)");
            }
        }
    }

    public void CreateUI()
    {
        UIHandler.PurgeDropdowns();
        for (int i = 0; i < playerScript.turrets.Count; i += 1)
        {
            UIHandler.CreateDropdown(playerScript.turretNames[i], playerScript.turretSizes[i]);
        }
    }
}

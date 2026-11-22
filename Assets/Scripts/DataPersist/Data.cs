using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data // Fine. You want me to keep things private and make wrappers. I HAVE DOUBLE THE LINE COUNT CAUSE OF UNITY
{
    public List<bool> shipsOwned = new List<bool>();
    public float highScore = 0;
    public string InitValues()
    {
        shipsOwned.Add(true); // corvette
        shipsOwned.Add(false); // frigate
        // high score is already 0
        return JsonUtility.ToJson(this, true);
    }
}

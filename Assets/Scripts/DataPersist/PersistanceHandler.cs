using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;

public class PersistanceHandler : MonoBehaviour
{
    private Data data = new Data();
    private IEnumerable<IData> iDataUsers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RequestSave()
    {
        RefreshIDataUsers();
        foreach (IData iDataUser in iDataUsers)
        {
            iDataUser.SaveRequested(ref data);
        }
    }

    private void RefreshIDataUsers()
    {
        iDataUsers = FindObjectsOfType<MonoBehaviour>().OfType<IData>();
    }

    public void SaveDataToFile(string fileName, Data data)
    {
        string path = Application.persistentDataPath;
        string name = fileName;
        string fullPath = Path.Combine(path, name);
        try
        {
            // Make directory incase it don't exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            // Turn data into a JSON string
            string dataToStore = JsonUtility.ToJson(data, true); // gotta look nice
            // Time to write
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
               using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
            Debug.Log("Wrote configs:\n    " + dataToStore + "\nTo:\n    " + fullPath);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error writting configs to:\n    " + fullPath + "\nError:\n    " + e);
        }
    }

    public Data Loadata(string fileName)
    {
        string path = Application.persistentDataPath;
        string name = fileName;
        string fullPath = Path.Combine(path, fileName);
        Data loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                // Data loaded; convert objects
                loadedData = JsonUtility.FromJson<Data>(dataToLoad);
                Debug.Log("Read configs:\n    " + dataToLoad + "\nFrom:\n    " + fullPath);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error reading data from file:\n    " + fullPath + "\nError:\n    " + e);
            }
        }
        return loadedData;
    }
}

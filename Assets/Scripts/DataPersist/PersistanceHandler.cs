using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;

public class PersistanceHandler : MonoBehaviour
{
    public string fileName;
    private Data data = new Data();
    private IEnumerable<IData> iDataUsers;

    // Start is called before the first frame update
    void Start() // Loading data on Start. where else?
    {
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnApplicationQuit() // Saving data on quit for now
    {
        SaveData();
    }

    public Data GetData() // OMG I MADE A WRAPPER !!! (I kinda already did with the entire file reading tutorial)
    {
        return data;
    }

    public void LoadData()
    {
        data = LoadDataFromFile(fileName);
        if (data == null)
        {
            Debug.Log("No Saves");
            NewData();
        }
        PushLoad();
    }

    public void SaveData()
    {
        RequestSave();
        SaveDataToFile(fileName, data);
    }

    public Data NewData()
    {
        data = new Data();
        data.InitValues();
        return data;
    }

    public void RequestSave()
    {
        RefreshIDataUsers();
        foreach (IData iDataUser in iDataUsers)
        {
            iDataUser.SaveRequested(ref data);
        }
    }

    public void PushLoad()
    {
        RefreshIDataUsers();
        foreach (IData iDataUser in iDataUsers)
        {
            iDataUser.DataLoaded(data);
        }
    }

    private void RefreshIDataUsers()
    {
        iDataUsers = FindObjectsOfType<MonoBehaviour>().OfType<IData>();
        Debug.Log(iDataUsers.Count());
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
            Debug.Log("Wrote data:\n    " + dataToStore + "\nTo:\n    " + fullPath);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error writting data to:\n    " + fullPath + "\nError:\n    " + e);
        }
    }

    public Data LoadDataFromFile(string fileName)
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
                Debug.Log("Read data:\n    " + dataToLoad + "\nFrom:\n    " + fullPath);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error reading data from file:\n    " + fullPath + "\nError:\n    " + e);
            }
        }
        return loadedData;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IData
{
    // LoadData is called when data is loaded
    void DataLoaded(Data data);
    // SaveData is called when data is being saved
    void SaveRequested(ref Data data);
}

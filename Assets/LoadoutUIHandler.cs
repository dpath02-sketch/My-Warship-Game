using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadoutUIHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> PTurrets;
    public List<GameObject> STurrets;
    public List<GameObject> MTurrets;
    public Transform ItemParent;
    public GameObject DropdownTemplate;
    public GameObject DropdownNameTemplate;
    List<GameObject> Dropdowns = new List<GameObject>();
    List<GameObject> DropdownNames = new List<GameObject>();
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PurgeDropdowns()
    {
        for (int i = 0; i < Dropdowns.Count; i += 1)
        {
            Destroy(Dropdowns[i]);
        }
        Dropdowns.Clear();
        DropdownNames.Clear();
    }

    public void CreateDropdown(string slotText) //TODO start taking in more info about the dropdown once I find out what and how
    {
        GameObject dropdown = Instantiate(DropdownTemplate, ItemParent);
        Dropdowns.Add(dropdown);
        GameObject text = Instantiate(DropdownNameTemplate, dropdown.transform);
        text.GetComponent<TMP_Text>().text = slotText;
        DropdownNames.Add(text);
    }
}

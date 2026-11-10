using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutUIHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> PTurrets;
    public List<string> PTurretNames;
    public List<Sprite> PTurretIcons;
    public List<GameObject> STurrets;
    public List<string> STurretNames;
    public List<Sprite> STurretIcons;
    public List<GameObject> MTurrets;
    public List<string> MTurretNames;
    public List<Sprite> MTurretIcons;
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

    public void CreateDropdown(string slotText, string size) //TODO start taking in more info about the dropdown once I find out what and how
    {
        GameObject dropdown = Instantiate(DropdownTemplate, ItemParent);
        dropdown.GetComponent<TMP_Dropdown>().ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        if (size == "P")
        {
            for (int i = 0; i < PTurrets.Count; i += 1)
            {
                options.Add(new TMP_Dropdown.OptionData(PTurretNames[i], PTurretIcons[i]));
            }
        }
        else if (size == "S")
        {
            for (int i = 0; i < STurrets.Count; i += 1)
            {
                options.Add(new TMP_Dropdown.OptionData(STurretNames[i], STurretIcons[i]));
            }
        }
        else if (size == "M")
        {
            for (int i = 0; i < MTurrets.Count; i += 1)
            {
                options.Add(new TMP_Dropdown.OptionData(MTurretNames[i], MTurretIcons[i]));
            }
        }
        dropdown.GetComponent<TMP_Dropdown>().AddOptions(options);
        Dropdowns.Add(dropdown);
        GameObject text = Instantiate(DropdownNameTemplate, dropdown.transform);
        text.GetComponent<TMP_Text>().text = slotText;
        DropdownNames.Add(text);
    }
}

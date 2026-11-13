using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadoutUIHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> pTurrets;
    public List<string> pTurretNames;
    public List<Sprite> pTurretIcons;
    public List<GameObject> sTurrets;
    public List<string> sTurretNames;
    public List<Sprite> sTurretIcons;
    public List<GameObject> mTurrets;
    public List<string> mTurretNames;
    public List<Sprite> mTurretIcons;
    public List<ShipStatBlock> stats;
    public List<Mesh> hitBoxes;
    public List<Mesh> meshes;
    public List<MaterialWrapper> meshMaterials;
    public List<Vector3Wrapper> trails;
    public Transform itemParent;
    public GameObject dropdownTemplate;
    public GameObject dropdownNameTemplate;
    public LoadoutHandler loadoutHandler;
    public PlayerScript playerScript;
    List<GameObject> dropdowns = new List<GameObject>();
    List<GameObject> dropdownNames = new List<GameObject>();
    List<string> dropdownSizes = new List<string>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PurgeDropdowns()
    {
        for (int i = 0; i < dropdowns.Count; i += 1)
        {
            Destroy(dropdowns[i]);
        }
        dropdowns.Clear();
        dropdownNames.Clear();
        dropdownSizes.Clear();
        playerScript.escDropdowns.Clear();
        playerScript.escDImages.Clear();
        playerScript.escDTexts.Clear();
    }

    public void CreateDropdown(string slotText, string size) //TODO start taking in more info about the dropdown once I find out what and how
    {
        GameObject dropdown = Instantiate(dropdownTemplate, itemParent);
        playerScript.escDropdowns.Add(dropdown.GetComponent<TMP_Dropdown>());
        playerScript.escDImages.Add(dropdown.GetComponent<TMP_Dropdown>().captionImage);
        playerScript.escDTexts.Add(dropdown.GetComponent<TMP_Dropdown>().captionText);
        dropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(DropdownOptionSelected);
        dropdown.GetComponent<TMP_Dropdown>().ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        if (size == "P")
        {
            for (int i = 0; i < pTurrets.Count; i += 1)
            {
                options.Add(new TMP_Dropdown.OptionData(pTurretNames[i], pTurretIcons[i]));
            }
        }
        else if (size == "S")
        {
            for (int i = 0; i < sTurrets.Count; i += 1)
            {
                options.Add(new TMP_Dropdown.OptionData(sTurretNames[i], sTurretIcons[i]));
            }
        }
        else if (size == "M")
        {
            for (int i = 0; i < mTurrets.Count; i += 1)
            {
                options.Add(new TMP_Dropdown.OptionData(mTurretNames[i], mTurretIcons[i]));
            }
        }
        dropdown.GetComponent<TMP_Dropdown>().AddOptions(options);
        dropdowns.Add(dropdown);
        GameObject text = Instantiate(dropdownNameTemplate, dropdown.transform);
        playerScript.escTexts.Add(text.GetComponent<TMP_Text>());
        text.GetComponent<TMP_Text>().text = slotText;
        dropdownNames.Add(text);
        dropdownSizes.Add(size);
    }

    public void DropdownOptionSelected(Int32 item)
    {
        for (int i = 0; i < dropdowns.Count; i += 1)
        {
            if (dropdowns[i].GetComponent<TMP_Dropdown>().value == item)
            {
                if (dropdownSizes[i] == "P")
                {
                    loadoutHandler.SwitchTurret(i, pTurrets[item]);
                }
                else if (dropdownSizes[i] == "S")
                {
                    loadoutHandler.SwitchTurret(i, sTurrets[item]);
                }
                else if (dropdownSizes[i] == "M")
                {
                    loadoutHandler.SwitchTurret(i, mTurrets[item]);
                }
            }
        }
    }

    [ContextMenu("Ship2...maybe")]
    public void TempShipSwitch()
    {
        loadoutHandler.SwitchShip(stats[1], hitBoxes[1], meshes[1], meshMaterials[1].myList, trails[1].myList, pTurrets[0], sTurrets[0], mTurrets[0]);
    }
}

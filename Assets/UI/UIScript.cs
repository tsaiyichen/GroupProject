using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Dropdown mode;
    [SerializeField] GameObject LM;
    [SerializeField] GameObject SL;
    [SerializeField] GameObject SF;
    [SerializeField] GameObject MD;
    [SerializeField] GameObject basketballCourt01;
    [SerializeField] GameObject JS;
    GameObject[] buildings;
    GameObject[] libraries;
    GameObject[] facilities;
    string currentFacility;
    public GameObject facilityBtnList;
    public GameObject facilityButtonList;
    public facilityMode facilityMode;
    public UIScript UIscript;
    public libraryMode libraryMode;
    public buildingMode buildingMode;
    int previousMode;

    public void OnModeChange()
    {
        switch (mode.value)
        {
            case 0:
                break;
            case 1:
                Debug.Log(mode.value);
                deleteTouchScript(previousMode);
                addTouchScript(mode.value);
                previousMode = mode.value;
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                Debug.Log(mode.value);
                deleteTouchScript(previousMode);
                addTouchScript(mode.value);
                previousMode = mode.value;
                break;
            case 5:
                Debug.Log(mode.value);
                deleteTouchScript(previousMode);
                addTouchScript(mode.value);
                touchScript instanceTouch = basketballCourt01.AddComponent<touchScript>();
                instanceTouch.mode = mode;
                instanceTouch.facilityMode = facilityMode;
                instanceTouch.UIScript = UIscript;
                previousMode = mode.value;
                break;
            case 6:
                break;           
        }
    }

    private void Awake()
    {
        buildings = new GameObject[] { LM, SL, SF, MD };
        libraries = new GameObject[] { JS };
    }
    void Start()
    {
        mode.value = 0;
        previousMode = 0;
    }

    public void deleteTouchScript(int index)
    {
        switch (index)
        {
            case 1:
                foreach(GameObject obj in buildings)
                {
                    if (obj.GetComponent<touchScript>() != null)
                    {
                        Destroy(obj.GetComponent<touchScript>());
                    }
                }
                break;
            case 4:
                foreach(GameObject obj in libraries)
                {
                    if (obj.GetComponent<touchScript>() != null)
                    {
                        Destroy(obj.GetComponent<touchScript>());
                    }
                }
                break;
            case 5:
                foreach (GameObject obj in facilities)
                {
                    if (obj.GetComponent<touchScript>() != null)
                    {
                        Destroy(obj.GetComponent<touchScript>());
                    }
                }
                break;
        }
    }
    public void addTouchScript(int index)
    {
        switch (index)
        {
            case 1:
                foreach (GameObject obj in buildings)
                {
                    touchScript instanceBuilding = obj.AddComponent<touchScript>();
                    instanceBuilding.mode = mode;
                    instanceBuilding.buildingMode = buildingMode;
                    instanceBuilding.UIScript = UIscript;
                }
                break;
            case 4:
                foreach (GameObject obj in libraries)
                {
                    touchScript instanceLib = obj.AddComponent<touchScript>();
                    instanceLib.mode = mode;
                    instanceLib.libraryMode = libraryMode;
                    instanceLib.UIScript = UIscript;
                }
                break;
            case 5:
                foreach (GameObject obj in facilities)
                {
                    touchScript instanceTouch = obj.AddComponent<touchScript>();
                    instanceTouch.mode = mode;
                    instanceTouch.facilityMode = facilityMode;
                    instanceTouch.UIScript = UIscript;
                }
                break;
        }
    }
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Dropdown mode;
    GameObject[] buildings;
    [SerializeField] GameObject LM;
    [SerializeField] GameObject SL;
    [SerializeField] GameObject SF;
    [SerializeField] GameObject MD;
    [SerializeField] GameObject basketballCourt01;
    [SerializeField] GameObject LA_LB_LC_AV_FL_FG;
    GameObject[] dormitories;
    public GameObject dormitory1;//宜真宜善
    public GameObject dormitory2;//宜美
    public GameObject dormitory3;//宜聖
    //public GameObject dormitory4;//立言
    //public GameObject dormitory5;//文德文舍
    //public GameObject dormitory6;//仁愛
    //public GameObject dormitory7;//信義和平
    //public GameObject dormitory8;//格物
    //public GameObject dormitory9;//學人旅店
    GameObject[] libraries;
    [SerializeField] GameObject JS;
    GameObject[] facilities;
    string currentFacility;
    public GameObject facilityBtnList;
    public GameObject facilityButtonList;
    public facilityMode facilityMode;
    public UIScript UIscript;
    public libraryMode libraryMode;
    public buildingMode buildingMode;
    public dormitoryMode dormitoryMode;
    public Camera mainCamera;
    int previousMode;

    public void OnModeChange()
    {
        switch (mode.value)
        {
            case 0:
                break;
            case 1:
                Debug.Log(mode.value);
                mainCamera.orthographicSize = 9;
                deleteTouchScript(previousMode);
                addTouchScript(mode.value);
                previousMode = mode.value;
                break;
            case 2:
                break;
            case 3:
                Debug.Log(mode.value);
                deleteTouchScript(previousMode);
                addTouchScript(mode.value);
                previousMode = mode.value;
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
        buildings = new GameObject[] { LM, SL, SF, MD, LA_LB_LC_AV_FL_FG };
        libraries = new GameObject[] { JS };
        dormitories = new GameObject[] { dormitory1, dormitory2, dormitory3, /*dormitory4, dormitory5, dormitory6, dormitory7, dormitory8, dormitory9*/ };
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
            case 3:
                foreach(GameObject obj in dormitories)
                {
                    if(obj.GetComponent<touchScript>() != null)
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
            case 3:
                foreach(GameObject obj in dormitories)
                {
                    touchScript dormitoryInstance = obj.AddComponent<touchScript>();
                    dormitoryInstance.mode = mode;
                    dormitoryInstance.dormitoryMode = dormitoryMode;
                    dormitoryInstance.UIScript = UIscript;
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

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    Dictionary<string, GameObject[]> facilityDic = new Dictionary<string, GameObject[]>();
    public Dropdown mode;
    GameObject[] buildings;
    GameObject[] labels;
    [SerializeField] GameObject LM;
    [SerializeField] GameObject labelLM;
    [SerializeField] GameObject SL;
    [SerializeField] GameObject labelSL;
    [SerializeField] GameObject SF;
    [SerializeField] GameObject labelSF;
    [SerializeField] GameObject MD;
    [SerializeField] GameObject labelMD;
    [SerializeField] GameObject basketballCourt01;
    [SerializeField] GameObject LA_LB_LC_AV_FL_FG;
    GameObject[] restaurants;
    [SerializeField] GameObject FY;
    [SerializeField] GameObject SY;
    [SerializeField] GameObject LY;
    [SerializeField] GameObject CY;
    GameObject[] dormitories;
    public GameObject dormitory1;//宜真宜善
    public GameObject dormitory2;//宜美
    public GameObject dormitory3;//宜聖
    public GameObject dormitory4;//立言
    public GameObject dormitory5;//文德文舍
    public GameObject dormitory6;//學人旅店
    public GameObject dormitory7;//信義和平
    public GameObject dormitory8;//格物
    GameObject[] libraries;
    [SerializeField] GameObject JS;
    GameObject[] facilities;
    string currentFacility;
    public GameObject facilityBtnList;
    [SerializeField] GameObject  printMachine1;
    [SerializeField] GameObject printMachine2;
    [SerializeField] GameObject store1;
    [SerializeField] GameObject store2;
    [SerializeField] GameObject store3;
    [SerializeField] GameObject bookstore1;
    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;
    [SerializeField] GameObject door3;
    [SerializeField] GameObject door4;
    [SerializeField] GameObject door5;
    [SerializeField] GameObject door6;
    [SerializeField] GameObject parking1;
    [SerializeField] GameObject parking2;
    [SerializeField] GameObject parking3;
    [SerializeField] GameObject parking4;
    [SerializeField] GameObject gym1;
    [SerializeField] GameObject bathroom1;
    [SerializeField] GameObject bathroom2;
    [SerializeField] GameObject bathroom3;
    [SerializeField] GameObject bathroom4;
    [SerializeField] GameObject bathroom5;
    [SerializeField] GameObject bathroom6;
    [SerializeField] GameObject bathroom7;
    [SerializeField] GameObject bathroom8;



    public facilityMode facilityMode;
    public UIScript UIscript;
    public libraryMode libraryMode;
    public buildingMode buildingMode;
    public dormitoryMode dormitoryMode;
    public restMode restMode;
    public Camera mainCamera;
    int previousMode;

    public void OnModeChange()
    {
        switch (mode.value)
        {
            case 0:
                Debug.Log(mode.value);
                mainCamera.orthographicSize = 9;
                deleteTouchScript(previousMode);
                addTouchScript(mode.value);
                previousMode = mode.value;
                break;
            case 1:
                Debug.Log(mode.value);
                mainCamera.orthographicSize = 9;
                deleteTouchScript(previousMode);
                addTouchScript(mode.value);
                previousMode = mode.value;
                break;
            case 2:
                Debug.Log(mode.value);
                mainCamera.orthographicSize = 9;
                deleteTouchScript(previousMode);
                addTouchScript(mode.value);
                previousMode = mode.value;
                break;
            case 3:
                Debug.Log(mode.value);
                mainCamera.orthographicSize = 9;
                deleteTouchScript(previousMode);
                addTouchScript(mode.value);
                previousMode = mode.value;
                break;
            case 4:
                Debug.Log(mode.value);
                mainCamera.orthographicSize = 9;
                deleteTouchScript(previousMode);
                addTouchScript(mode.value);
                previousMode = mode.value;
                break;
            case 5:
                mainCamera.orthographicSize = 9;
                Debug.Log(mode.value);
                deleteTouchScript(previousMode);
                addTouchScript(mode.value);
                facilityBtnList.SetActive(true);
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
        dormitories = new GameObject[] { dormitory1, dormitory2, dormitory3, dormitory4, dormitory5, dormitory6, dormitory7, dormitory8};
        restaurants = new GameObject[] { FY, SY, LY, CY };
        labels = new GameObject[] { labelSF, labelLM, labelMD, labelSL };
        InitialDic();
    }
    void InitialDic()
    {
        facilityDic.Add("printMachine", new GameObject[] {printMachine1, printMachine2});
        facilityDic.Add("store", new GameObject[] { store1, store2, store3});
        facilityDic.Add("bookStore", new GameObject[] { bookstore1});
        facilityDic.Add("door", new GameObject[] {door1, door2, door3, door4, door5, door6});
        facilityDic.Add("parking", new GameObject[] { parking1, parking2, parking3, parking4} );
        facilityDic.Add("gym", new GameObject[] { gym1 });
        facilityDic.Add("bathroom", new GameObject[] { bathroom1, bathroom2, bathroom3, bathroom4, bathroom5, bathroom6, bathroom7, bathroom8});
        facilityDic.Add("swim");
        facilityDic.Add("wheelChair");
        facilityDic.Add("waterMachine");
        facilityDic.Add("drinkMachine");
        facilityDic.Add("sportCourt");
        facilityDic.Add("elevator");
        facilityDic.Add("printShop");
        facilityDic.Add("hospital");
    }
    void Start()
    {
        mode.value = 0;
        previousMode = 0;
    }
    public void facilityTypeChoose(string type)
    {
        foreach(GameObject obj in facilities)
        {
            obj.SetActive(false);
        }
        currentFacility = type;
        GameObject[] choosed = facilityDic[type];
        foreach(GameObject obj in choosed)
        {
            obj.SetActive(true);
        }
    }
    public void deleteTouchScript(int index)
    {
        switch (index)
        {
            case 0:
                foreach(GameObject obj in labels)
                {
                    obj.SetActive(false);
                }
                break;
            case 1:
                foreach(GameObject obj in buildings)
                {
                    if (obj.GetComponent<touchScript>() != null)
                    {
                        Destroy(obj.GetComponent<touchScript>());
                    }
                }
                break;
            case 2:
                foreach (GameObject obj in restaurants)
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
                facilityBtnList.SetActive(false);
                foreach (GameObject obj in facilities)
                {
                    if (obj.GetComponent<touchScript>() != null)
                    {
                        Destroy(obj.GetComponent<touchScript>());
                    }
                }
                break;
            case 6:
                break;
        }
    }
    public void addTouchScript(int index)
    {
        switch (index)
        {
            case 0:
                foreach (GameObject obj in labels)
                {
                    obj.SetActive(true);
                }
                break;
            case 1:
                foreach (GameObject obj in buildings)
                {
                    touchScript instanceBuilding = obj.AddComponent<touchScript>();
                    instanceBuilding.mode = mode;
                    instanceBuilding.buildingMode = buildingMode;
                    instanceBuilding.UIScript = UIscript;
                }
                break;
            case 2:
                foreach (GameObject obj in restaurants)
                {
                    touchScript instanceRest = obj.AddComponent<touchScript>();
                    instanceRest.mode = mode;
                    instanceRest.restMode = restMode;
                    instanceRest.UIScript = UIscript;
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
            case 6:
                break;
        }
    }
}

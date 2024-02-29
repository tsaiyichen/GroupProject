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
    GameObject[] buildings;
    public GameObject facilityButtonList;
    public facilityMode facilityMode;
    public UIScript UIscript;

    public void OnModeChange()
    {
        switch (mode.value)
        {
            case 0:
                break;
            case 1:
                foreach (GameObject building in buildings)
                {
                    building.AddComponent<touchScript>();
                }
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                Debug.Log(mode.value);
                touchScript instanceTouch = basketballCourt01.AddComponent<touchScript>();
                instanceTouch.mode = mode;
                instanceTouch.facilityMode = facilityMode;
                instanceTouch.UIScript = UIscript;
                break;
            case 6:
                break;           
        }
    }

    private void Awake()
    {
        buildings = new GameObject[] { LM, SL, SF, MD };
    }
    void Start()
    {
        mode.value = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

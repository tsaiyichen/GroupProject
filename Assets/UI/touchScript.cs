using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class touchScript : MonoBehaviour
{
    public facilityMode facilityMode;
    public static bool canTouch;
    public UIScript UIScript;
    public Dropdown mode;
    public libraryMode libraryMode;
    public buildingMode buildingMode;
    // Start is called before the first frame update
    void Awake()
    {
        UIScript = GetComponent<UIScript>();
        facilityMode = GetComponent<facilityMode>();
        libraryMode = GetComponent<libraryMode>();
        buildingMode = GetComponent<buildingMode>();
        canTouch = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1 && canTouch)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                Collider2D hitCollider = Physics2D.OverlapPoint(touchPosition);

                if (hitCollider != null && hitCollider.gameObject == gameObject)
                {                 
                    movingScript.cameraMove = false;
                    canTouch = false;
                    string target = gameObject.name;
                    Debug.Log("hit¡I");
                    if(mode.value == 1)
                    {
                        Debug.Log("hit");
                        StartCoroutine(buildingMode.getBuildingData(target));
                    }
                    else if(mode.value == 4){
                        Debug.Log(target);
                        StartCoroutine(libraryMode.getLibraryData(target));
                    }
                    else if(mode.value == 5)
                    {
                        Debug.Log(target);
                        StartCoroutine(facilityMode.getfacilityData(target));
                    }
                }
            }
        }
    }
}

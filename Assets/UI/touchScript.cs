using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class touchScript : MonoBehaviour
{
    public facilityMode facilityMode;
    public static bool canTouch = true;
    public UIScript UIScript;
    public Dropdown mode;
    // Start is called before the first frame update
    void Awake()
    {
        UIScript = GetComponent<UIScript>();
        if (UIScript == null)
        {
            Debug.LogError("UIScript component not found on the GameObject.");
        }

        facilityMode = GetComponent<facilityMode>();
        if (facilityMode == null)
        {
            Debug.LogError("facilityMode component not found on the GameObject.");
        }
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
                    if(mode.value == 5)
                    {
                        Debug.Log(target);
                        StartCoroutine(facilityMode.getfacilityData(target));
                    }
                }
            }
        }
    }
}

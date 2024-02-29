using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class movingScript : MonoBehaviour
{
    public static bool cameraMove;
    public Vector2 startPos;
    public Vector2 direction;
    public bool directionChosen;
    [SerializeField] GameObject camera;
    float moveSpeed;
    [SerializeField] Camera mainCamera;
    public float zoomSpeed = 1f;
    float distance;
    public bool canDown = true;
    public bool canUp = true;
    public float minX, minY, maxX, maxY;

    private void Awake()
    {
        minX = -11f;
        maxX = 9.5f;
        minY = -13.3f;
        maxY = 2.85f;
        mainCamera.transform.Translate(0, 0, -10);
        cameraMove = true;
    }
    public void zoomIn()
    {
        if (canDown)
        {
            ModifyZoom(1);
        }
    }
    public void zoomOut()
    {
        if (canUp)
        {
            ModifyZoom(-1);
        }
    }

    void ModifyZoom(int amount)
    {
        if (mainCamera == null)
        {
            Debug.LogError("主相機為空");
            return;
        }

        if (mainCamera.orthographic)
        {
            mainCamera.orthographicSize += amount * zoomSpeed;
        }
        else
        {
            mainCamera.fieldOfView += amount * zoomSpeed;
        }
        if(mainCamera.orthographicSize <= 3)
        {
            canUp = false;
        }
        else
        {
            canUp = true;
        }
        if(mainCamera.orthographicSize >= 10)
        {
            canDown = false;
        }
        else
        {
            canDown = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    startPos = touch.position;
                    directionChosen = false;
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    direction = -(touch.position - startPos);
                    moveSpeed = Vector2.Distance(touch.position, startPos) * 0.006f;
                    directionChosen = true;
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    directionChosen = false;
                    break;
            }
        }

        if (directionChosen && cameraMove == true)
        {
            // Normalize the direction vector to ensure consistent speed in all directions
            direction.Normalize();

            Vector3 newPosition = mainCamera.transform.position + new Vector3(direction.x, direction.y, 0) * moveSpeed * Time.deltaTime;
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            // Move the object in the chosen direction with a constant speed
            mainCamera.transform.position = newPosition;
        }
        if (Input.touchCount > 1)
        {

            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            switch (touch2.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    Vector2 t1Pos = touch1.position;
                    Vector2 t2Pos = touch2.position;
                    distance = Vector2.Distance(t1Pos, t2Pos);
                    Debug.Log(t1Pos);
                    Debug.Log(t2Pos);
                    Debug.Log(distance);
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    break;
                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    t1Pos = touch1.position;
                    t2Pos = touch2.position;
                    float finalDistance = Vector2.Distance(t1Pos, t2Pos);
                    Debug.Log(t1Pos);
                    Debug.Log(t2Pos);
                    Debug.Log(distance);
                    Debug.Log(finalDistance);
                    if (distance > finalDistance && canDown && cameraMove == true)
                    {
                        zoomIn();
                    }
                    if (distance < finalDistance && canUp && cameraMove == true)
                    {
                        zoomOut();
                    }
                    break;
            }
        }
    }
}

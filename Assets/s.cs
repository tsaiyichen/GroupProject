using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s : MonoBehaviour
{
    public Dropdown dropdown;
    public Image image;
    public Sprite Sprite;

    private void Start()
    {
        dropdown.value = 0;
    }

    public void onclick()
    {
        dropdown.value = 1;
    }
    public void onmodeChange()
    {
        switch (dropdown.value)
        {
            case 0:
                Debug.Log("0");
                break;
            case 1:
                Debug.Log("1");
                dropdown.value = 2;
                break;
            case 2:
                Debug.Log("2");
                break;
        }
    }
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    Collider2D hitCollider = Physics2D.OverlapPoint(touchPosition);

                    if (hitCollider != null && hitCollider.gameObject == gameObject)
                    {
                        Debug.Log("hit¡I");
                    }
                }
            }
        }
    }

    public void ChangeTheImage()
    {
        Texture2D imageTexture = Resources.Load<Texture2D>("flutter");

        // Make sure the imageTexture is not null
        if (imageTexture != null)
        {
            // Create a new sprite using the imageTexture
            Sprite sprite = Sprite.Create(imageTexture, new Rect(0, 0, imageTexture.width, imageTexture.height), Vector2.one * 0.5f);

            image.sprite = sprite;
            image.color = new Vector4(1f, 1f, 1f, 1f);
        }
        else
        {
            Debug.LogError("Image texture is not found!");
        }
    }

}

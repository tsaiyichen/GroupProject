using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class facilityMode : MonoBehaviour
{
    [SerializeField] Text name;
    [SerializeField] Text description;
    [SerializeField] Image image;
    [SerializeField] Button cancelButton;
    [SerializeField] GameObject facilityPanel;
    
    string serverURL = "http://140.136.155.122/Unity/getFacilityData.php";
    
    public void closePanel()
    {
        facilityPanel.SetActive(false);
        movingScript.cameraMove = true;
        touchScript.canTouch = true;
    }
    public IEnumerator getfacilityData(string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("facilityName", name);

        UnityWebRequest www = UnityWebRequest.Post(serverURL, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string facilityResponse = www.downloadHandler.text;
            Debug.Log("facility: " + facilityResponse);
            Facility data = JsonUtility.FromJson<Facility>(facilityResponse);
            openFacilityPanel(data);
        }
        else
        {
            Debug.Log("facilityError");
        }
    }

    public class Facility
    {
        public string FACILITYID;
        public string FACILITYNAME;
        public string FACILITYTYPE;
        public string FACILITYDESCRIPTION;
    }

    public void openFacilityPanel(Facility data)
    {
        name.text = data.FACILITYNAME;
        description.text = data.FACILITYDESCRIPTION;
        string imageName = data.FACILITYTYPE + data.FACILITYID;
        description.gameObject.SetActive(true);
        ChangeTheImage(imageName, image);
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => closePanel());
        facilityPanel.SetActive(true);
    }

    public void ChangeTheImage(string name, Image image)
    {
        Texture2D imageTexture = Resources.Load<Texture2D>(name);

        if (imageTexture != null)
        {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class libraryMode : MonoBehaviour
{
    [SerializeField] Text name;
    [SerializeField] Text description;
    [SerializeField] Image image;
    [SerializeField] GameObject libraryPanel;

    string serverURL = "http://140.136.155.122/Unity/getLibraryData.php";
    // Start is called before the first frame update

    public void closePanel()
    {
        libraryPanel.SetActive(false);
        movingScript.cameraMove = true;
        touchScript.canTouch = true;
    }
    public IEnumerator getLibraryData(string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("libraryID", name);

        UnityWebRequest www = UnityWebRequest.Post(serverURL, form);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string facilityResponse = www.downloadHandler.text;
            Debug.Log("library: " + facilityResponse);
            Library data = JsonUtility.FromJson<Library>(facilityResponse);
            openLibraryPanel(data);
        }
        else
        {
            Debug.Log("facilityError");
        }
    }
    public class Library
    {
        public string LIBRARYID;
        public string LIBRARYNAME;
        public string LIBRARYDESCRIPTION;
    }

    void openLibraryPanel(Library data)
    {
        name.text = data.LIBRARYNAME;
        description.text = data.LIBRARYDESCRIPTION;
        string imageName = data.LIBRARYID;
        ChangeTheImage(imageName, image);
        libraryPanel.SetActive(true);
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

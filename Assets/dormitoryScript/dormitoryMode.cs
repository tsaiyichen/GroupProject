using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class dormitoryMode : MonoBehaviour
{
    [SerializeField] GameObject dormitoryPanel;
    [SerializeField] Text dormitoryName;
    [SerializeField] Image dormitoryImage;
    [SerializeField] Text dormitoryDescription;
    [SerializeField] Button closeButton;
    [SerializeField] Button linkButton;
    public void closePanel()
    {
        dormitoryPanel.SetActive(false);
        movingScript.cameraMove = true;
        touchScript.canTouch = true;
    }
    public IEnumerator getDormitoryData(string dormitory_name)
    {
        string url = "http://140.136.155.122/Unity/getDormitoryData.php";

        WWWForm form = new WWWForm();
        form.AddField("dormitory", dormitory_name);
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("dormitoryError");
        }
        else
        {
            string responseText = www.downloadHandler.text;
            Debug.Log("Dormitory: " + responseText);

            Dormitory data = JsonUtility.FromJson<Dormitory>(responseText);
            openDormitoryPanel(data);
            }
    }
    public class Dormitory
    {
        public string DORMITORYID;
        public string DORMITORYNAME;
        public string DORMITORYDESCRIPTION;
    }
    public void openDormitoryPanel(Dormitory data)
    {
        dormitoryName.text = data.DORMITORYNAME;
        ChangeTheImage("dormitory" + data.DORMITORYID, dormitoryImage);
        dormitoryDescription.gameObject.SetActive(false);
        linkButton.gameObject.SetActive(true);
        linkButton.onClick.RemoveAllListeners();
        linkButton.onClick.AddListener(() => openLink(data.DORMITORYDESCRIPTION));
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => closePanel());
        dormitoryPanel.SetActive(true);
        
    }

    public void ChangeTheImage(string name, Image image)
    {
        Texture2D imageTexture = Resources.Load<Texture2D>(name);
        image.color = new Vector4(0f, 0f, 0f, 0f);
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

    public void openLink(string name)
    {
        Application.OpenURL(name);
    }
}

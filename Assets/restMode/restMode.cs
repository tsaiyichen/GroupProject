using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static searchingScript;

public class restMode : MonoBehaviour
{
    Dictionary<string, string> restaurant_building = new Dictionary<string, string>();
    [SerializeField] GameObject introPanel;
    [SerializeField] Text introRes_BuildingName;
    [SerializeField] Image res_buildingImage;
    [SerializeField] Button changeToIntro_intro;
    [SerializeField] Button changeToRest_intro;
    [SerializeField] Button introCancelBtn;

    int restCurrentPage;
    int restTotalpage;
    [SerializeField] GameObject restPanel;
    [SerializeField] Text floorRes_BuildingName;
    [SerializeField] Button changeToIntro_rest;
    [SerializeField] Button changeToRest_rest;
    [SerializeField] Button previousBtn_rest;
    [SerializeField] Button nextBtn_rest;
    [SerializeField] Button restCancelBtn;
    [SerializeField] Button rest0, rest1, rest2, rest3, rest4;
    Button[] restBtns;
    List<string> restList = new List<string>();
    string currentBuilding;

    [SerializeField] GameObject descriptionPanel;
    [SerializeField] Text descriptionName;
    [SerializeField] Image descriptionImage;
    [SerializeField] Text descriptionText;
    [SerializeField] Button linkButton;
    [SerializeField] Button descriptionCancelBtn;

    public void closePanel()
    {
        restPanel.SetActive(false);
        introPanel.SetActive(false);
        movingScript.cameraMove = true;
        touchScript.canTouch = true;
    }
    public void openLink(string URL)
    {
        Application.OpenURL(URL);
    }
    private void Awake()
    {
        restaurant_building.Add("FY", "»²¶é");
        restaurant_building.Add("dormitory07", "¤ß¶é");
        restaurant_building.Add("dormitory08", "²z¶é");
        restaurant_building.Add("dormitory03", "»²¶é");
    }
    public IEnumerator getRestData(string res_building)
    {
        currentBuilding = restaurant_building[res_building];
        string restaurantURL = "http://140.136.155.122/Unity/getRestaurantData.php";
        WWWForm form = new WWWForm();
        form.AddField("resBuilding", currentBuilding);

        UnityWebRequest restaurantRequest = UnityWebRequest.Post(restaurantURL, form);

        yield return restaurantRequest.SendWebRequest();

        if (restaurantRequest.result == UnityWebRequest.Result.Success)
        {
            string restaurantResponse = restaurantRequest.downloadHandler.text;
            Debug.Log("restaurant: " + restaurantResponse);
            string pattern = @"\{.*?\}";
            addDataToList(restaurantResponse, pattern, restList);
            openIntroPanel();
            restCurrentPage = 1;
            restTotalpage = (int)(Math.Ceiling((double)(restList.Count) / restBtns.Length));
            restaurantPanelOn(restList, 1);
        }
    }
    public void addDataToList(string oriText, string pattern, List<string> list)
    {
        MatchCollection matches = Regex.Matches(oriText, pattern);
        int activeNumber = matches.Count;
        for (int i = 0; i < activeNumber; i++)
        {
            string data = matches[i].Value;
            list.Add(data);
        }
        Debug.Log(list);
        Debug.Log(list.Count);
    }
    void openIntroPanel()
    {
        changeToIntro_intro.onClick.RemoveAllListeners();
        changeToRest_intro.onClick.RemoveAllListeners();
        changeToRest_intro.onClick.AddListener(() => changeToRestPanel());
        introRes_BuildingName.text = currentBuilding;
        restCancelBtn.onClick.RemoveAllListeners();
        restCancelBtn.onClick.AddListener( () => closePanel());
        ChangeTheImage(currentBuilding, res_buildingImage);
        introPanel.SetActive(true);
    }
    void restaurantPanelOn(List<string> list, int currentPage)
    {
        restCancelBtn.onClick.RemoveAllListeners();
        restCancelBtn.onClick.AddListener(() => closePanel());
        previousBtn_rest.gameObject.SetActive(true);
        nextBtn_rest.gameObject.SetActive(true);
        previousBtn_rest.onClick.RemoveAllListeners();
        nextBtn_rest.onClick.RemoveAllListeners();
        previousBtn_rest.onClick.AddListener(() => restaurantPanelOn(list, currentPage - 1));
        nextBtn_rest.onClick.AddListener(() => restaurantPanelOn(list, currentPage + 1));
        if (currentPage == 1)
        {
            previousBtn_rest.gameObject.SetActive(false);
        }
        if (currentPage == restTotalpage)
        {
            nextBtn_rest.gameObject.SetActive(false);
        }

        int start = restBtns.Length * (currentPage - 1);
        for (int i = start; i < start + restBtns.Length; i++)
        {
            if (i >= list.Count)
            {
                restBtns[i % restBtns.Length].gameObject.SetActive(false);
            }
            else
            {
                restBtns[i % restBtns.Length].gameObject.SetActive(true);
                string jsonData = list[i];
                Debug.Log(jsonData);
                Restaurant data = JsonUtility.FromJson<Restaurant>(jsonData);
                string text = data.RESTAURANTNAME;
                Debug.Log(text);
                ChangeButtonText(restBtns[i % restBtns.Length], text);
                restBtns[i % restBtns.Length].onClick.RemoveAllListeners();
                restBtns[i % restBtns.Length].onClick.AddListener(() => openDescriptionPanel(data));
            }
        }
    }
    void openDescriptionPanel(Restaurant data)
    {
        linkButton.gameObject.SetActive(true);
        descriptionText.gameObject.SetActive(false);
        descriptionName.text = data.RESTAURANTNAME;
        ChangeTheImage("restaurant" + data.RESTAURANTID, descriptionImage);
        linkButton.onClick.RemoveAllListeners();
        linkButton.onClick.AddListener(() => openLink(data.RESTAURANTDESCRIPTION));
        descriptionCancelBtn.onClick.RemoveAllListeners();
        descriptionCancelBtn.onClick.AddListener(() => closeDescriptionPanel());
        descriptionPanel.SetActive(true);
    }
    public void ChangeButtonText(Button button, string newText)
    {
        if (button != null)
        {
            // Access the Text component of the button and set its text
            button.GetComponentInChildren<Text>().text = newText;
        }
        else
        {
            Debug.LogError("Button reference not set!");
        }
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
    public void changeToRestPanel()
    {
        restPanel.SetActive(true);
        introPanel.SetActive(false);
        changeToIntro_rest.onClick.AddListener(() => changeToIntroPanel());
        changeToRest_rest.onClick.RemoveAllListeners();
    }
    public void changeToIntroPanel()
    {
        restPanel.SetActive(false);
        introPanel.SetActive(true);
        changeToIntro_intro.onClick.RemoveAllListeners();
        changeToRest_intro.onClick.AddListener(() => changeToRestPanel());
    }
    public void closeDescriptionPanel()
    {
        descriptionPanel.SetActive(false);
        restPanel.SetActive(true);

    }
    public class Restaurant
    {
        public string RESTAURANTID;
        public string RESTAURANTNAME;
        public string RES_BUILDINGNAME;
        public string RESTAURANTDESCRIPTION;
    }
}

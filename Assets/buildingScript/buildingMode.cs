using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class buildingMode : MonoBehaviour
{
    Dictionary<string, int> buildingFloor = new Dictionary<string, int>();
    [SerializeField] GameObject introPanel;
    [SerializeField] Text introBuildingName;
    [SerializeField] Image buildingImage;
    [SerializeField] Button changeToIntro_intro;
    [SerializeField] Button changeToFloor_intro;
    [SerializeField] Button cancelBtn;
    //[SerializeField] Text buildingDesccription;

    int floorCurrentPage;
    int floorTotalpage;
    [SerializeField] GameObject floorPanel;
    [SerializeField] Text floorBuildingName;
    [SerializeField] Button changeToIntro_floor;
    [SerializeField] Button changeToFloor_floor;
    [SerializeField] Button previousBtn_floor;
    [SerializeField] Button nextBtn_floor;
    [SerializeField] Button floor0, floor1, floor2, floor3, floor4;
    Button[] floorBtns;
    string currentBuilding;
    string currentBuildingName;

    [SerializeField] GameObject roomPanel;
    [SerializeField] Text roomBuildingName;
    [SerializeField] Text roomFloor;
    [SerializeField] Button room0, room1, room2, room3, room4;
    Button[] roomBtns;
    List<string> roomList = new List<string>();
    int roomCurrentPage;
    int roomTotalPage;
    [SerializeField] Button previousBtn_room;
    [SerializeField] Button nextBtn_room;
    [SerializeField] Button BackToFloor;

    [SerializeField] GameObject descriptionPanel;
    [SerializeField] Text descriptionName;
    [SerializeField] Image descriptionImage;
    [SerializeField] Text descriptionText;
    [SerializeField] Button linkButton;

    string getBuildingURL = "http://140.136.155.122/Unity/getBuildingData.php";
    string getRoomURL = "http://140.136.155.122/Unity/getRoomData.php";
    // Start is called before the first frame update

    void initializeDictionary()
    {
        buildingFloor.Add("LM", 5);
        buildingFloor.Add("SL", 3);
        buildingFloor.Add("SF", 8);
        buildingFloor.Add("MD", 11);
    }
    public void Awake()
    {
        initializeDictionary();
        floorBtns = new Button[] { floor0, floor1, floor2, floor3, floor4 };
        roomBtns = new Button[] { room0, room1, room2, room3, room4 };
    }
    public void closePanel()
    {
        floorPanel.SetActive(false);
        roomPanel.SetActive(false);
        introPanel.SetActive(false);
        movingScript.cameraMove = true;
        touchScript.canTouch = true;
    }
    public IEnumerator getBuildingData(string buildingID)
    {
        WWWForm form = new WWWForm();
        form.AddField("buildingID", buildingID);

        UnityWebRequest www = UnityWebRequest.Post(getBuildingURL, form);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string buildingResponse = www.downloadHandler.text;
            Debug.Log("building: " + buildingResponse);
            Building data = JsonUtility.FromJson<Building>(buildingResponse);
            currentBuilding = data.BUILDINGID;
            currentBuildingName = data.BUILDINGNAME;
            openIntroPanel(data);
            setInitialFloorPanel(data);
        }
        else
        {
            Debug.Log("buildingError");
        }
    }
    public class Building
    {
        public string BUILDINGID;
        public string BUILDINGNAME;
        public string BUILDINGDESCRIPTION;
    }
    public class Room
    {
        public string BUILDINGID;
        public string ROOMID;
        public string ROOMNAME;
        public int FLOOR;
    }

    void openIntroPanel(Building data)
    {
        introBuildingName.text = data.BUILDINGID + "  " + data.BUILDINGNAME;
        string imageName = data.BUILDINGID;
        ChangeTheImage(imageName, buildingImage);
        cancelBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.AddListener(() => closePanel());
        introPanel.SetActive(true);
    }

    void setInitialFloorPanel(Building data)
    {
        int totalFloor = buildingFloor[data.BUILDINGID];
        floorBuildingName.text = data.BUILDINGID + " " + data.BUILDINGNAME;
        floorCurrentPage = 1;
        floorTotalpage = (int)(Math.Ceiling((double)(totalFloor) / floorBtns.Length));
        floorButtonSetting(floorCurrentPage, totalFloor);
    }
    void floorButtonSetting(int currentPage, int totalFloor)
    {
        if (currentPage == 1)
        {
            previousBtn_floor.gameObject.SetActive(false);
        }
        if (currentPage == floorTotalpage)
        {
            nextBtn_floor.gameObject.SetActive(false);
        }
        int start = floorBtns.Length * (currentPage - 1);
        for (int i = start; i < start + 5; i++)
        {
            if (i >= totalFloor)
            {
                floorBtns[i].gameObject.SetActive(false);
            }
            else
            {
                ChangeButtonText(floorBtns[i % floorBtns.Length], i + "¼Ó");
                floorBtns[i].onClick.RemoveAllListeners();
                floorBtns[i].onClick.AddListener(() => StartCoroutine(getRoomData(i, currentBuilding)));
                floorBtns[i].gameObject.SetActive(true);
            }
        }
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
    public IEnumerator getRoomData(int floor, string building)
    {
        roomList.Clear();
        WWWForm form = new WWWForm();
        form.AddField("floor", floor);
        form.AddField("buildingID", building);
        UnityWebRequest www = UnityWebRequest.Post(getRoomURL, form);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string roomResponse = www.downloadHandler.text;
            Debug.Log("room: " + roomResponse);
            string pattern = @"\{.*?\}";
            addDataToList(roomResponse, pattern, roomList);
            roomPanelOn(floor);
        }
        else
        {
            Debug.Log("roomError!");
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
    public void roomPanelOn(int floor)
    {
        roomPanel.SetActive(true);
        roomBuildingName.text = currentBuildingName + "¤j¼Ó";
        roomFloor.text = floor + "¼Ó  ¸Ô²Ó¸ê°T";
        roomCurrentPage = 1;
        roomTotalPage = (int)(Math.Ceiling((double)(roomList.Count) / roomBtns.Length));
        roomButtonSetting(roomCurrentPage, roomList.Count);
    }
    void roomButtonSetting(int currentPage, int totalRoom)
    {
        if (currentPage == 1)
        {
            previousBtn_room.gameObject.SetActive(false);
        }
        if (currentPage == roomTotalPage)
        {
            nextBtn_room.gameObject.SetActive(false);
        }
        int start = roomBtns.Length * (currentPage - 1);
        for (int i = start; i < start + 5; i++)
        {
            if (i >= totalRoom)
            {
                floorBtns[i].gameObject.SetActive(false);
            }
            else
            {
                Room data = JsonUtility.FromJson<Room>(roomList[i]);
                ChangeButtonText(roomBtns[i % roomBtns.Length], data.BUILDINGID + data.ROOMID);
                roomBtns[i].onClick.RemoveAllListeners();
                roomBtns[i].onClick.AddListener(() => openDescriptionPanel(data));
                roomBtns[i].gameObject.SetActive(true);
            }
        }
    }
    void openDescriptionPanel(Room data)
    {
        linkButton.gameObject.SetActive(false);
        descriptionName.text = data.BUILDINGID + data.ROOMID;
        ChangeTheImage(data.BUILDINGID + data.ROOMID, descriptionImage);
        descriptionText.text = data.ROOMNAME;
        descriptionPanel.SetActive(true);

    }


    public void changeToIntroPanel()
    {
        introPanel.SetActive(true);
        floorPanel.SetActive(false);
        changeToIntro_intro.onClick.RemoveAllListeners();
        changeToFloor_intro.onClick.AddListener(() => changeToFloorPanel());
    }
    public void changeToFloorPanel()
    {
        floorPanel.SetActive(true);
        introPanel.SetActive(false);
        changeToIntro_floor.onClick.AddListener(() => changeToIntroPanel());
        changeToFloor_floor.onClick.RemoveAllListeners();
    }
    public void roomPanelTofloorPanel()
    {
        roomPanel.SetActive(false);
        floorPanel.SetActive(true);
    }
    public void closeDescriptionPanel()
    {
        descriptionPanel.SetActive(false);
        roomPanel.SetActive(true);
    }
}

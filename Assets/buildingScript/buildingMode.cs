using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class buildingMode : MonoBehaviour
{
    Dictionary<string, int> buildingFloor = new Dictionary<string, int>();
    string [] hasB1;
    [SerializeField] GameObject introPanel;
    [SerializeField] Text introBuildingName;
    [SerializeField] Image buildingImage;
    [SerializeField] Button changeToIntro_intro;
    [SerializeField] Button changeToFloor_intro;
    [SerializeField] Button introCancelBtn;
    //[SerializeField] Text buildingDesccription;

    int floorCurrentPage;
    int floorTotalpage;
    [SerializeField] GameObject floorPanel;
    [SerializeField] Text floorBuildingName;
    [SerializeField] Button changeToIntro_floor;
    [SerializeField] Button changeToFloor_floor;
    [SerializeField] Button previousBtn_floor;
    [SerializeField] Button nextBtn_floor;
    [SerializeField] Button floorCancelBtn;
    [SerializeField] Button floor0, floor1, floor2, floor3, floor4;
    Button[] floorBtns;
    string currentBuilding;
    string currentBuildingName;
    int totalFloor;

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
    [SerializeField] Button roomCancelBtn;

    [SerializeField] GameObject descriptionPanel;
    [SerializeField] Text descriptionName;
    [SerializeField] Image descriptionImage;
    [SerializeField] Text descriptionText;
    [SerializeField] Button linkButton;
    [SerializeField] Button descriptionCancelBtn;

    string getBuildingURL = "http://140.136.155.122/Unity/getBuildingData.php";
    string getRoomURL = "http://140.136.155.122/Unity/getRoomData.php";
    // Start is called before the first frame update

    void initializeDictionary()
    {
        buildingFloor.Add("LM", 6);
        buildingFloor.Add("SL", 3);
        buildingFloor.Add("SF", 8);
        buildingFloor.Add("MD", 11);
        buildingFloor.Add("LA/LB/LC/AV/FL/FG", 10);
    }
    public void Awake()
    {
        initializeDictionary();
        hasB1 = new string[] { "LM", "ES" };
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
        changeToIntro_intro.onClick.RemoveAllListeners();
        changeToFloor_intro.onClick.AddListener(() => changeToFloorPanel());
        introCancelBtn.onClick.RemoveAllListeners();
        introCancelBtn.onClick.AddListener(() => closePanel());
        introPanel.SetActive(true);
    }

    void setInitialFloorPanel(Building data)
    {
        totalFloor = buildingFloor[data.BUILDINGID];
        floorBuildingName.text = data.BUILDINGID + " " + data.BUILDINGNAME;
        floorCurrentPage = 1;
        floorTotalpage = (int)(Math.Ceiling((double)(totalFloor) / floorBtns.Length));
        Debug.Log(floorTotalpage + " " + floorCurrentPage);
        floorCancelBtn.onClick.RemoveAllListeners();
        floorCancelBtn.onClick.AddListener(() => closePanel());
        floorButtonSetting(floorCurrentPage, data.BUILDINGID);
    }
    void floorButtonSetting(int currentPage, string buildingID)
    {
        previousBtn_floor.gameObject.SetActive(true);
        nextBtn_floor.gameObject.SetActive(true);
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
                floorBtns[i % floorBtns.Length].gameObject.SetActive(false);
            }
            else
            {
                int current = i + 1;
                if(hasB1.Contains(buildingID))
                {
                    current = i;
                }

                Debug.Log("current: " + current);

                if (current == 0)
                {
                    ChangeButtonText(floorBtns[i % floorBtns.Length], "地下室");
                }
                else
                {
                    ChangeButtonText(floorBtns[i % floorBtns.Length], current + "樓");
                }
                floorBtns[i % floorBtns.Length].onClick.RemoveAllListeners();
                floorBtns[i % floorBtns.Length].onClick.AddListener(() => StartCoroutine(getRoomData(current, currentBuilding)));
                floorBtns[i % floorBtns.Length].gameObject.SetActive(true);
            }
            previousBtn_floor.onClick.RemoveAllListeners();
            nextBtn_floor.onClick.RemoveAllListeners();
            previousBtn_floor.onClick.AddListener(() => floorButtonSetting(currentPage - 1, buildingID));
            nextBtn_floor.onClick.AddListener(() => floorButtonSetting(currentPage + 1, buildingID));
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
        Debug.Log(floor + " " + building);
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
        roomBuildingName.text = currentBuildingName + "大樓";
        if(floor == 0)
        {
            roomFloor.text = "地下室 詳細資訊";
        }
        else
        {
            roomFloor.text = floor + "樓  詳細資訊";
        }
        roomCurrentPage = 1;
        roomTotalPage = (int)(Math.Ceiling((double)(roomList.Count) / roomBtns.Length));
        roomButtonSetting(roomCurrentPage);
        roomCancelBtn.onClick.RemoveAllListeners();
        roomCancelBtn.onClick.AddListener(() => closePanel());
        roomPanel.SetActive(true);
    }
    void roomButtonSetting(int currentPage)
    {
        previousBtn_room.gameObject.SetActive(true);
        nextBtn_room.gameObject.SetActive(true);
        Debug.Log("current: " + currentPage + "total: " + roomTotalPage);
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
            if (i >= roomList.Count)
            {
                roomBtns[i % roomBtns.Length].gameObject.SetActive(false);
            }
            else
            {
                Room data = JsonUtility.FromJson<Room>(roomList[i]);
                ChangeButtonText(roomBtns[i % roomBtns.Length], data.BUILDINGID + data.ROOMID);
                roomBtns[i % roomBtns.Length].onClick.RemoveAllListeners();
                roomBtns[i % roomBtns.Length].onClick.AddListener(() => openDescriptionPanel(data));
                roomBtns[i % roomBtns.Length].gameObject.SetActive(true);
            }
            previousBtn_room.onClick.RemoveAllListeners();
            nextBtn_room.onClick.RemoveAllListeners();
            previousBtn_room.onClick.AddListener(() => roomButtonSetting(currentPage - 1));
            nextBtn_room.onClick.AddListener(() => roomButtonSetting(currentPage + 1));
        }
    }
    void openDescriptionPanel(Room data)
    {
        linkButton.gameObject.SetActive(false);
        descriptionName.text = data.BUILDINGID + data.ROOMID;
        ChangeTheImage(data.BUILDINGID + data.ROOMID, descriptionImage);
        descriptionText.text = data.ROOMNAME;
        descriptionCancelBtn.onClick.RemoveAllListeners();
        descriptionCancelBtn.onClick.AddListener(() => closeDescriptionPanel());
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

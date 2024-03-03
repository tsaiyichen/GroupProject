using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class searchingScript : MonoBehaviour
{
    [SerializeField] Button searchingIcon;
    [SerializeField] InputField keyWordInput;
    [SerializeField] Button result0;
    [SerializeField] Button result1;
    [SerializeField] Button result2;
    [SerializeField] Button result3;
    [SerializeField] Button result4;
    [SerializeField] Dropdown tableDropdown;
    [SerializeField] Text noResultText;
    Button[] resultArray;
    [SerializeField] GameObject searchingPanel;
    [SerializeField] Button nextPage;
    [SerializeField] Button previousPage;
    [SerializeField] Camera mainCamera;
    UIScript UIScript;

    int totalPage;
    int currentPage;
    int n;

    string buildingURL = "http://140.136.155.122/Unity/search_building.php";
    string roomURL = "http://140.136.155.122/Unity/search_room.php";
    string dormitoryURL = "http://140.136.155.122/Unity/search_dormitory.php";
    string restaurantURL = "http://140.136.155.122/Unity/search_restaurant.php";
    string facilityURL = "http://140.136.155.122/Unity/search_facility.php";
    string libraryURL = "http://140.136.155.122/Unity/search_library.php";

    List<string> buildingList = new List<string>();
    List<string> roomList = new List<string>();
    List<string> dormitoryList = new List<string>();
    List<string> restaurantList = new List<string>();
    List<string> facilityList = new List<string>();
    List<string> libraryList = new List<string>();

    private void Awake()
    {
        resultArray = new Button[] { result0, result1, result2, result3, result4};
        n = resultArray.Length;
        UIScript = GetComponent<UIScript>();
    }

    public void iconClick()
    {
        searchingPanel.SetActive(true);
        movingScript.cameraMove = false;
    }
    public void cancelClick()
    {
        searchingPanel.SetActive(false);
        movingScript.cameraMove = true;
    }
    public void sendKeyword()
    {
        buildingList.Clear();
        roomList.Clear();
        dormitoryList.Clear();
        restaurantList.Clear();
        facilityList.Clear();
        libraryList.Clear();
        string keyword = keyWordInput.text;
        StartCoroutine(getData(keyword));
        tableDropdown.gameObject.SetActive(true);
        tableDropdown.value = 0;
    }

    public void OnValueChanged(int index)
    {
        switch (index)
        {
            case 1:
                noResultText.gameObject.SetActive(false);
                totalPage = (int)(Math.Ceiling((double)(buildingList.Count) / n));
                currentPage = 1;
                buildingPanelOn(buildingList, currentPage);
                break;
            case 6:
                noResultText.gameObject.SetActive(false);
                totalPage = (int)(Math.Ceiling((double)(roomList.Count) / n));
                currentPage = 1;
                roomPanelOn(roomList, currentPage);
                break;
            case 3:
                noResultText.gameObject.SetActive(false);
                totalPage = (int)(Math.Ceiling((double)(dormitoryList.Count) / n));
                currentPage = 1;
                dormitoryPanelOn(dormitoryList, currentPage);
                break;
            case 2:
                noResultText.gameObject.SetActive(false);
                totalPage = (int)(Math.Ceiling((double)(restaurantList.Count) / n));
                currentPage = 1;
                restaurantPanelOn(restaurantList, currentPage);
                break;
            case 5:
                noResultText.gameObject.SetActive(false);
                totalPage = (int)(Math.Ceiling((double)(facilityList.Count) / n));
                currentPage = 1;
                facilityPanelOn(facilityList, currentPage);
                break;
            case 4:
                noResultText.gameObject.SetActive(false);
                totalPage = (int)(Math.Ceiling((double)(libraryList.Count) / n));
                currentPage = 1;
                libraryPanelOn(libraryList, currentPage);
                break;
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

    IEnumerator getData(string keyword)
    {
        WWWForm form = new WWWForm();
        form.AddField("keyword", keyword);

        UnityWebRequest buildingRequest = UnityWebRequest.Post(buildingURL, form);
        UnityWebRequest roomRequest = UnityWebRequest.Post(roomURL, form);
        UnityWebRequest dormitoryRequest = UnityWebRequest.Post(dormitoryURL, form);
        UnityWebRequest restaurantRequest = UnityWebRequest.Post(restaurantURL, form);
        UnityWebRequest facilityRequest = UnityWebRequest.Post(facilityURL, form);
        UnityWebRequest libraryRequest = UnityWebRequest.Post(libraryURL, form);

        yield return buildingRequest.SendWebRequest();
        yield return roomRequest.SendWebRequest();
        yield return dormitoryRequest.SendWebRequest();
        yield return restaurantRequest.SendWebRequest();
        yield return facilityRequest.SendWebRequest();
        yield return libraryRequest.SendWebRequest();

        if (buildingRequest.result == UnityWebRequest.Result.Success)
        {
            string buildingResponse = buildingRequest.downloadHandler.text;
            Debug.Log("building: " + buildingResponse);
            string pattern = @"\{.*?\}";
            addDataToList(buildingResponse, pattern, buildingList);
        }
        else
        {
            Debug.Log("buildingError");
        }

        if (roomRequest.result == UnityWebRequest.Result.Success)
        {
            string roomResponse = roomRequest.downloadHandler.text;
            Debug.Log("room: " + roomResponse);
            string pattern = @"\{.*?\}";
            addDataToList(roomResponse, pattern, roomList);

        }
        else
        {
            Debug.Log("roomError");
        }

        if (dormitoryRequest.result == UnityWebRequest.Result.Success)
        {
            string dormitoryResponse = dormitoryRequest.downloadHandler.text;
            Debug.Log("dormitory: " + dormitoryResponse);
            string pattern = @"\{.*?\}";
            addDataToList(dormitoryResponse, pattern, dormitoryList);

        }
        else
        {
            Debug.Log("dormitoryError");
        }
        if (restaurantRequest.result == UnityWebRequest.Result.Success)
        {
            string restaurantResponse = restaurantRequest.downloadHandler.text;
            Debug.Log("restaurant: " + restaurantResponse);
            string pattern = @"\{.*?\}";
            addDataToList(restaurantResponse, pattern, restaurantList);

        }
        else
        {
            Debug.Log("restaurantError");
        }
        if (facilityRequest.result == UnityWebRequest.Result.Success)
        {
            string facilityResponse = facilityRequest.downloadHandler.text;
            Debug.Log("facility: " + facilityResponse);
            string pattern = @"\{.*?\}";
            addDataToList(facilityResponse, pattern, facilityList);

        }
        else
        {
            Debug.Log("facilityError");
        }
        if (libraryRequest.result == UnityWebRequest.Result.Success)
        {
            string libraryResponse = libraryRequest.downloadHandler.text;
            Debug.Log("library: " + libraryResponse);
            string pattern = @"\{.*?\}";
            addDataToList(libraryResponse, pattern, libraryList);

        }
        else
        {
            Debug.Log("libraryError");
        }
    }

    void buildingPanelOn(List<string> list, int currentPage)
    {
        previousPage.gameObject.SetActive(true);
        nextPage.gameObject.SetActive(true);
        previousPage.onClick.RemoveAllListeners();
        nextPage.onClick.RemoveAllListeners();
        previousPage.onClick.AddListener(() => buildingPanelOn(list, currentPage - 1));
        nextPage.onClick.AddListener(() => buildingPanelOn(list, currentPage + 1));
        if (currentPage == 1)
        {
            previousPage.gameObject.SetActive(false);
        }
        if (currentPage == totalPage || totalPage == 0)
        {
            nextPage.gameObject.SetActive(false);
        }
        if(totalPage == 0)
        {
            noResultText.gameObject.SetActive(true);
            noResultText.text = "無配對結果";
        }
        int start = n * (currentPage - 1);
        for (int i = start; i < start + n; i++)
        {
            if (i >= list.Count)
            {
                resultArray[i % n].gameObject.SetActive(false);
            }
            else
            {
                resultArray[i % n].gameObject.SetActive(true);
                string jsonData = list[i];
                Debug.Log(jsonData);
                Building data = JsonUtility.FromJson<Building>(jsonData);
                string text = data.BUILDINGID + "   " + data.BUILDINGNAME;
                Debug.Log(text);
                ChangeButtonText(resultArray[i % n], text);

                resultArray[i % n].onClick.RemoveAllListeners();
                resultArray[i % n].onClick.AddListener(() => moveCameraToObj(data.BUILDINGID, 1));
            }
        }
    }

    void roomPanelOn(List<string> list, int currentPage)
    {
        previousPage.gameObject.SetActive(true);
        nextPage.gameObject.SetActive(true);
        previousPage.onClick.RemoveAllListeners();
        nextPage.onClick.RemoveAllListeners();
        previousPage.onClick.AddListener(() => roomPanelOn(list, currentPage - 1));
        nextPage.onClick.AddListener(() => roomPanelOn(list, currentPage + 1));
        if (currentPage == 1)
        {
            previousPage.gameObject.SetActive(false);
        }
        if (currentPage == totalPage || totalPage == 0)
        {
            nextPage.gameObject.SetActive(false);
        }
        if (totalPage == 0)
        {
            noResultText.gameObject.SetActive(true);
            noResultText.text = "無配對結果";
        }
        int start = n * (currentPage - 1);
        for (int i = start; i < start + n; i++)
        {
            if (i >= list.Count)
            {
                resultArray[i % n].gameObject.SetActive(false);
            }
            else
            {
                resultArray[i % n].gameObject.SetActive(true);
                string jsonData = list[i];
                Debug.Log(jsonData);
                Room data = JsonUtility.FromJson<Room>(jsonData);
                string text = data.BUILDINGID + data.ROOMID + "   " + data.ROOMNAME;
                Debug.Log(text);
                ChangeButtonText(resultArray[i % n], text);
                resultArray[i % n].onClick.RemoveAllListeners();
                resultArray[i % n].onClick.AddListener(() => moveCameraToObj(data.BUILDINGID, 1));
            }
        }
    }
    void dormitoryPanelOn(List<string> list, int currentPage)
    {
        previousPage.gameObject.SetActive(true);
        nextPage.gameObject.SetActive(true);
        previousPage.onClick.RemoveAllListeners();
        nextPage.onClick.RemoveAllListeners();
        previousPage.onClick.AddListener(() => dormitoryPanelOn(list, currentPage - 1));
        nextPage.onClick.AddListener(() => dormitoryPanelOn(list, currentPage + 1));
        if (currentPage == 1)
        {
            previousPage.gameObject.SetActive(false);
        }
        if (currentPage == totalPage || totalPage == 0)
        {
            nextPage.gameObject.SetActive(false);
        }
        if (totalPage == 0)
        {
            noResultText.gameObject.SetActive(true);
            noResultText.text = "無配對結果";
        }
        int start = n * (currentPage - 1);
        for (int i = start; i < start + n; i++)
        {
            if (i >= list.Count)
            {
                resultArray[i % n].gameObject.SetActive(false);
            }
            else
            {
                resultArray[i % n].gameObject.SetActive(true);
                string jsonData = list[i];
                Debug.Log(jsonData);
                Dormitory data = JsonUtility.FromJson<Dormitory>(jsonData);
                string text = data.DORMITORYNAME + "   " + data.DORMITORYDESCRIPTION;
                Debug.Log(text);
                ChangeButtonText(resultArray[i % n], text);
                resultArray[i % n].onClick.RemoveAllListeners();
                resultArray[i % n].onClick.AddListener(() => moveCameraToObj(data.DORMITORYID, 3));
            }
        }
    }
    void restaurantPanelOn(List<string> list, int currentPage)
    {
        previousPage.gameObject.SetActive(true);
        nextPage.gameObject.SetActive(true);
        previousPage.onClick.RemoveAllListeners();
        nextPage.onClick.RemoveAllListeners();
        previousPage.onClick.AddListener(() => restaurantPanelOn(list, currentPage - 1));
        nextPage.onClick.AddListener(() => restaurantPanelOn(list, currentPage + 1));
        if (currentPage == 1)
        {
            previousPage.gameObject.SetActive(false);
        }
        if (currentPage == totalPage || totalPage == 0)
        {
            nextPage.gameObject.SetActive(false);
        }
        if (totalPage == 0)
        {
            noResultText.gameObject.SetActive(true);
            noResultText.text = "無配對結果";
        }
        int start = n * (currentPage - 1);
        for (int i = start; i < start + n; i++)
        {
            if (i >= list.Count)
            {
                resultArray[i % n].gameObject.SetActive(false);
            }
            else
            {
                resultArray[i % n].gameObject.SetActive(true);
                string jsonData = list[i];
                Debug.Log(jsonData);
                Restaurant data = JsonUtility.FromJson<Restaurant>(jsonData);
                string text = data.RESTAURANTNAME + "   " + data.RES_BUILDINGNAME;
                Debug.Log(text);
                ChangeButtonText(resultArray[i % n], text);
                resultArray[i % n].onClick.RemoveAllListeners();
                resultArray[i % n].onClick.AddListener(() => moveCameraToObj(data.RES_BUILDINGNAME, 2));
            }
        }
    }
    void facilityPanelOn(List<string> list, int currentPage)
    {
        previousPage.gameObject.SetActive(true);
        nextPage.gameObject.SetActive(true);
        previousPage.onClick.RemoveAllListeners();
        nextPage.onClick.RemoveAllListeners();
        previousPage.onClick.AddListener(() => facilityPanelOn(list, currentPage - 1));
        nextPage.onClick.AddListener(() => facilityPanelOn(list, currentPage + 1));
        if (currentPage == 1)
        {
            previousPage.gameObject.SetActive(false);
        }
        if (currentPage == totalPage || totalPage == 0)
        {
            nextPage.gameObject.SetActive(false);
        }
        if (totalPage == 0)
        {
            noResultText.gameObject.SetActive(true);
            noResultText.text = "無配對結果";
        }
        int start = n * (currentPage - 1);
        for (int i = start; i < start + n; i++)
        {
            if (i >= list.Count)
            {
                resultArray[i % n].gameObject.SetActive(false);
            }
            else
            {
                resultArray[i % n].gameObject.SetActive(true);
                string jsonData = list[i];
                Debug.Log(jsonData);
                Facility data = JsonUtility.FromJson<Facility>(jsonData);
                string text = data.FACILITYTYPE + "   " + data.FACILITYNAME;
                Debug.Log(text);
                ChangeButtonText(resultArray[i % n], text);
                resultArray[i % n].onClick.RemoveAllListeners();
                resultArray[i % n].onClick.AddListener(() => moveCameraToObj(data.FACILITYID, data.FACILITYTYPE, 5));
            }
        }
    }
    void libraryPanelOn(List<string> list, int currentPage)
    {
        previousPage.gameObject.SetActive(true);
        nextPage.gameObject.SetActive(true);
        previousPage.onClick.RemoveAllListeners();
        nextPage.onClick.RemoveAllListeners();
        previousPage.onClick.AddListener(() => libraryPanelOn(list, currentPage - 1));
        nextPage.onClick.AddListener(() => libraryPanelOn(list, currentPage + 1));
        if (currentPage == 1)
        {
            previousPage.gameObject.SetActive(false);
        }
        if (currentPage == totalPage || totalPage == 0)
        {
            nextPage.gameObject.SetActive(false);
        }
        if (totalPage == 0)
        {
            noResultText.gameObject.SetActive(true);
            noResultText.text = "無配對結果";
        }
        int start = n * (currentPage - 1);
        for (int i = start; i < start + n; i++)
        {
            if (i >= list.Count)
            {
                resultArray[i % n].gameObject.SetActive(false);
            }
            else
            {
                resultArray[i % n].gameObject.SetActive(true);
                string jsonData = list[i];
                Debug.Log(jsonData);
                Library data = JsonUtility.FromJson<Library>(jsonData);
                string text = data.LIBRARYNAME + "   " + data.LIBRARYDESCRIPTION;
                Debug.Log(text);
                ChangeButtonText(resultArray[i % n], text);
                resultArray[i % n].onClick.RemoveAllListeners();
                resultArray[i % n].onClick.AddListener(() => moveCameraToObj(data.LIBRARYID, 4));
            }
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

    public void moveCameraToObj(string name, int mode)
    {
        searchingPanel.SetActive(false);
        GameObject targetObject = GameObject.Find(name);
        if (targetObject != null)
        {
            mainCamera.transform.position = targetObject.transform.position;
            UIScript.mode.value = mode;
            searchingPanel.SetActive(false);
        }
        else
        {
            Debug.Log("moving error");
        }
    }
    public void moveCameraToObj(string name, string type, int mode)
    {
        searchingPanel.SetActive(false);
        string target = "";
        if(name.Length == 1)
        {
            target = type + "0" + name;
        }
        else
        {
            target = type + name;
        }
        GameObject targetObject = GameObject.Find(target);
        if (targetObject != null)
        {
            mainCamera.transform.position = targetObject.transform.position;
        }
        else
        {
            Debug.Log("moving error");
        }

        UIScript.mode.value = mode;
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
    public class Dormitory
    {
        public string DORMITORYID;
        public string DORMITORYNAME;
        public string DORMITORYDESCRIPTION;
    }
    public class Restaurant
    {
        public string RESTAURANTID;
        public string RESTAURANTNAME;
        public string RES_BUILDINGNAME;
        public string RESTAURANTDESCRIPTION;
    }
    public class Facility
    {
        public string FACILITYID;
        public string FACILITYNAME;
        public string FACILITYTYPE;
        public string FACILITYDESCRIPTION;
    }
    public class Library
    {
        public string LIBRARYID;
        public string LIBRARYNAME;
        public string LIBRARYDESCRIPTION;
    }
}

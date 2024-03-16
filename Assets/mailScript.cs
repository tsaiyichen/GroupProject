using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class mailScript : MonoBehaviour
{
    [SerializeField] InputField inputVcode;
    [SerializeField] InputField inputSTD;
    [SerializeField] Button send;
    [SerializeField] Text upperText;
    string mailURL = "http://140.136.155.122/Unity/mail.php";
    string changeURL = "http://140.136.155.122/Unity/changeISVARIFIED.php";
    string Vcode;
    private void Awake()
    {
        PlayerPrefs.SetString("USERID", "testUser");
        Debug.Log(PlayerPrefs.GetString("USERID"));
        PlayerPrefs.SetInt("ISVARIFIED", 0);
        send.onClick.AddListener(() => sendmail());
    }

    public void sendmail()
    {
        if(inputSTD != null)
        {
            string email = inputSTD.text + "@m365.fju.edu.tw";
            StartCoroutine(getVcode(email));
        }
        else
        {

        }
    }

    public IEnumerator getVcode(string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        UnityWebRequest www = UnityWebRequest.Post(mailURL, form);

        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success)
        {
            Vcode = www.downloadHandler.text;
            Debug.Log(Vcode);
            upperText.text = "½Ð¿é¤JÅçÃÒ½X";
            inputSTD.gameObject.SetActive(false);
            inputVcode.gameObject.SetActive(true);
            send.onClick.RemoveAllListeners();
            send.onClick.AddListener(() => sendVcode());
        }
        else
        {
            Debug.LogError("getVcode error");
        }
    }
    void sendVcode()
    {
        if(inputVcode != null)
        {
            if(inputVcode.text == Vcode)
            {
                Debug.Log("Success");
                StartCoroutine(changeVarified(PlayerPrefs.GetString("USERID")));

            }
        }
        else
        {
            Debug.Log("fail");
        }
    }
    public IEnumerator changeVarified(string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);
        UnityWebRequest www = UnityWebRequest.Post(changeURL, form);

        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success)
        {
            string status = www.downloadHandler.text;
            Debug.Log(status);
            if(status == "success")
            {
                PlayerPrefs.SetInt("ISVARIFIED", 1);
                SceneManager.LoadScene("DebugScene");
            }
            else
            {
                Debug.LogError("status fail");
            }
        }
        else
        {
            Debug.LogError("Request Error");
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TimeGift : MonoBehaviour
{
    #region varibls
    private string urlDate = "http://worldclockapi.com/api/json/est/now"; // get datetime in internt
    private string sDate = "";
    private string url = "www.goggle.com";
    #endregion

    private void Awake()
    {
        StartCoroutine(CheckInternet());
    }

    #region praivet fun for dateTime and give your gift
    // check conction in internt
    private IEnumerator CheckInternet()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            if (!webRequest.isNetworkError)
            {
                Debug.Log("Success Internet");
                StartCoroutine(CheckDate());
            }
        }


    }

    // check Date in internt
    private IEnumerator CheckDate()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(urlDate))
        {
            yield return webRequest.SendWebRequest();
            if (!webRequest.isNetworkError)
            {
                string[] splitDate = webRequest.downloadHandler.text.Split(new string[] { "currentDateTime\":\"" }, StringSplitOptions.None);
                sDate = splitDate[1].Substring(0, 10);

            }
        }
        Debug.Log(sDate);
        SaveDataAndCheck();
    //    dailyButton.interactable = true;
    }

    // after one day or befor
    private void SaveDataAndCheck()
    {
        string oldData = PlayerPrefs.GetString(MyStringSave.oldData);
        if (string.IsNullOrEmpty(oldData))
        {
            Debug.Log("Is the first play");
            PlayerPrefs.SetString(MyStringSave.oldData, sDate);
        }
        else
        {
            DateTime _dateNow = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime _dateOld = Convert.ToDateTime(oldData);

            TimeSpan diff = _dateNow - _dateOld;
            print("diff.Days " + diff.Days);
            if(diff.Days >= 1)
            {
                PlayerPrefs.SetInt(MyStringSave.myHeart, 3);
                PlayerPrefs.SetString(MyStringSave.oldData, _dateNow.ToString());
            }
        }
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System;

public class MenuScene : MonoBehaviour
{
    #region Varibles

    private RewardedAd rewardedAd;
    private int myheart;

    [SerializeField]
    private Button AdsBtn;
    [SerializeField]
    private Button playBtn;
    [SerializeField]
    private Image[] hearts;
    [SerializeField]
    private Text textGift;
    [SerializeField]
    private Toggle sounToggle;

    #endregion

    #region unity Function

    private void Awake()
    {
        int checkLogin = PlayerPrefs.GetInt(MyStringSave.firstlogin);
        if (checkLogin == 0) // this in first Loginn for 3 heart 
            myheart = 3;
        else
            myheart = PlayerPrefs.GetInt(MyStringSave.myHeart);
        print(checkLogin + " :checkLogin ");
        print(myheart + " : myheart ");
        PlayButttonSwithes();
        ShowHeart();
    }
    public void Start()
    {

        #region ////////Ads/////
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
        #endregion
    }
    private void Update()
    {
        myheart = PlayerPrefs.GetInt(MyStringSave.myHeart);
        PlayButttonSwithes();
        ShowHeart();
        SoundOption(sounToggle);
    }

    #endregion

    #region Ads all Events
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        textGift.text = "your not  Gift for closed vidio";
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        // if wahtched vidoie
        textGift.text = "your Gift for whatched vidio 3 hearts";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetInt(MyStringSave.myHeart, 3);
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

    }
    // this fun for show vidio
    private void UserChoseToWatchAd()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }
    #endregion

    #region Praivt func
    // check my heart and select play btn with ads or normal play
    void PlayButttonSwithes()
    {
        if (myheart <= 0)
        {
            playBtn.gameObject.SetActive(false);
            AdsBtn.gameObject.SetActive(true);
        }
        else
        {
            playBtn.gameObject.SetActive(true);
            AdsBtn.gameObject.SetActive(false);
        }
    }

    // enabld heat in ui with heat val
    void ShowHeart()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < myheart; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }

    }

    // toggle for sound save val
    void SoundOption(Toggle objToggle)
    {
        int chehksound;
        if (objToggle.isOn == true)
        {
            chehksound = 0;
        }
        else
        {
            chehksound = 1;
        }
        PlayerPrefs.SetInt(MyStringSave.sfx, chehksound); /// save value sound 
    }

    #endregion

    #region public func

    // play game after watch vidio
    public void PlayGameWithAds()
    {
        UserChoseToWatchAd();
    }

    // play game befor watch vidio
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // if you dont need whatch vidio close game 
    public void NoPlayGame()
    {
        Application.Quit();
    }

    #endregion
}

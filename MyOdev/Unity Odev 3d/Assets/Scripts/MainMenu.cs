using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Varibles 
    public static MainMenu instantie;

    [SerializeField] 
    GameObject pauseMenuPanel;

    [SerializeField] 
    GameObject gameOverPanel;

    [SerializeField]
    Button RestBtn;

    [SerializeField]
    Toggle soundToggle;

    private int heartCount;
    private int chehkfirstLog; // value for check first login
    private int chehksound; // value for check toggle btn activ or not
    #endregion

    #region unity func
    private void Awake()
    {
        instantie = this;
    }
    void Start()
    {
        chehkfirstLog = PlayerPrefs.GetInt(MyStringSave.firstlogin);
        if (chehkfirstLog == 0)
        {
            chehkfirstLog = 1;
            PlayerPrefs.SetInt(MyStringSave.firstlogin, chehkfirstLog);
        }
    }
    private void Update()
    {
        SoundOption(soundToggle);
    }
    #endregion

    #region func for ui In game scene
    public void PauseBtn()
    {
        pauseMenuPanel.SetActive(true);
        StopTimeGame();
    }
    public void CountinueBtn()
    {
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GoToMainMenu()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void GameOver()
    {
        heartCount = PlayerPrefs.GetInt(MyStringSave.myHeart);
        if (heartCount <= 0)
        {
            RestBtn.interactable = false;
        }
        else
        {
            RestBtn.interactable = true;
        }
        Invoke("ShowGameOverPanel", 2.5f);
        Invoke("StopTimeGame", 4.0f);
    }

    void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }
   
    void StopTimeGame()
    {
        Time.timeScale = 0f;
    }

    void SoundOption( Toggle objToggle)
    {
       if(objToggle.isOn == true)
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
}

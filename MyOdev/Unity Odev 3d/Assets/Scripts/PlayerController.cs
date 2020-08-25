using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Varibles
    public static PlayerController player; // instantie player

    [SerializeField]
    private float speed; // speed foe Wallk

    [SerializeField]
    private Text scoreT; // scor text ui for coins

    [SerializeField]
    private Slider healthBar; 

    [SerializeField]
    private Image[] hearts; // 3 heart for player

    public float jumpSpeed = 9f; // jump speed for player
    public float dragSpeed = 3; // left and right wallk speed for player

    private int myHeart = 3;
    private int scoreV = 0; ///  score to player
    private float healthV;  // health for player
    private float fullHealthV = 100f; // full health to player
    private Vector3 dragOrigin; // first touch pos to player
    private bool isJump;
    private bool isGround;
    private float jdefaultSpeed; // jump speed not change 
    private Animator anim;
    private Rigidbody rb;
    private Text heathT; // text in health bar 
    private bool isDead = false;
    private int soundChek; // for toggle btn to sound

    public AudioClip hitAud;
    public AudioClip jummpAud;
    public AudioClip loseAud;
    public AudioMixer audMix;
    public AudioMixerSnapshot normalSnap;
    public AudioMixerSnapshot stopSnap;
    private AudioSource aud;

    #endregion

    #region Default fun from unity
    private void Awake()
    {
        player = this;
        NormalSnapAud();
    }
    private void Start()
    {
        string mydead = PlayerPrefs.GetString(MyStringSave.isDead); // return player dead or not dead
        if (mydead == "True")
        {
            myHeart = PlayerPrefs.GetInt(MyStringSave.myHeart);
            for (int i = 0; i < myHeart; i++)
            {
                hearts[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < myHeart; i++)
            {
                hearts[i].gameObject.SetActive(true);
            }
        }

        aud = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        heathT = healthBar.transform.GetChild(2).gameObject.GetComponent<Text>();
        healthV = fullHealthV;

        jdefaultSpeed = jumpSpeed;
        CheckSound();
    }
    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetInt(MyStringSave.myHeart, myHeart);
        print("My heart = " + myHeart);
        DragPlayer();
        Movement();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }
        else
        {
            isJump = false;
        }
        textUpdate();
        CheckSound();
    }

    #endregion

    #region Move For Player
    // run to forword
    void Movement()
    {
        transform.Translate(0f, 0f, speed * Time.deltaTime); // Move player to forwrd
    }
    // player Jump
    void Jumping()
    {
        if (isJump && isGround)
        {
            aud.PlayOneShot(jummpAud);
            anim.SetBool("Is_Jumping", true);
            jumpSpeed = jdefaultSpeed;
            rb.AddForce(new Vector3(0f, jumpSpeed, 0), ForceMode.Impulse);

        }
        else
        {
            anim.SetBool("Is_Jumping", false);
            jumpSpeed = 0f;
            rb.AddForce(new Vector3(0f, jumpSpeed, 0), ForceMode.Impulse);
        }
    }
    // left and right player
    void DragPlayer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(Mathf.Clamp(pos.x * dragSpeed, -0.1f, 0.1f), 0, 0);
        if (pos.y > move.y + 0.05f)
        {
            isJump = true;
        }
        else
        {
            isJump = false;
        }
        Debug.Log("is jump " + pos.y);

        Jumping();
        transform.Translate(move, Space.World);
    }

    #endregion

    #region Player health  hurt and die

    // is hurt player
    void GetHurt(float damge)
    {

        if (healthV > 0)
        {
            aud.PlayOneShot(hitAud);
            healthV -= damge;
        }
        else
        {
            healthV = 0f;
        }
        PlayerDie();
    }
    // player is dead
    void PlayerDie()
    {
        if (healthV <= 0)
        {        
            isDead = true;
            healthV = 0;
            textUpdate();
            if (myHeart > 0 && isDead)
            {
                myHeart--;
                for (int i = 0; i < hearts.Length; i++)
                {
                    hearts[i].gameObject.SetActive(false);
                }
                for (int i = 0; i < myHeart; i++)
                {
                    hearts[i].gameObject.SetActive(true);
                }
                
            }
            PlayerPrefs.SetInt(MyStringSave.myHeart, myHeart);
            PlayerPrefs.SetString(MyStringSave.isDead, isDead.ToString());
            speed = 0f;
            aud.PlayOneShot(loseAud);
            anim.SetBool("Is_lose", true);
            MainMenu.instantie.GameOver();
        }
    }

    #endregion

    #region Ui health 
    // health bar and text health 
    void textUpdate()
    {
        healthBar.value = healthV;
        heathT.text = "100 / " + healthV.ToString();
    }

    #endregion

    #region Sound for player noraml sound and stoping and toogle btn is active

    void NormalSnapAud()
    {
        normalSnap.TransitionTo(0.5f);
    }
    void StopSnapAud()
    {
        stopSnap.TransitionTo(0.5f);
    }
    void CheckSound()
    {
        soundChek = PlayerPrefs.GetInt(MyStringSave.sfx);
        if(soundChek == 0)
        {
            NormalSnapAud();
        }
        else
        {
            StopSnapAud();
        }
    }

    #endregion

    #region Trigger function 

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isGround = false;
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isGround = true;
        }
        else if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
            scoreV++;
            scoreT.text = "Coins : " + scoreV.ToString();

        }
        else if (collision.gameObject.tag == "Trap")
        {
            Destroy(collision.gameObject);
            GetHurt(10f);

        }
    }

    #endregion
}

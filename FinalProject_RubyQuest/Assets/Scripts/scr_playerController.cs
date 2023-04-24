using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class scr_playerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject projectilePrefab;

    public Rigidbody2D rb;

    public ParticleSystem healEffect;
    public ParticleSystem hitEffect;

    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    AudioSource audioSource;

    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip walkSound;
    public AudioClip questSound;
    public AudioClip fixSound;
    public AudioClip winSound;
    public AudioClip loseSound;

    public GameObject backgroundMusic;

    public int robotCount;
    public int totalRobots = 4;
    bool didTalk;
    public static int currentLevel;


    public int maxHealth = 5;
    public float timeInvincible = 2.0f;

    public int health {get {return currentHealth;}}
    int currentHealth;

    bool isInvincible;
    float invincibleTimer;

    public bool gameOver = false;

    public TextMeshProUGUI displayText;
    public TextMeshProUGUI displayAmmo;
    public TextMeshProUGUI displayQuest;
    public TextMeshProUGUI displayRobots;


    public int ammoCount {get {return currentAmmo;}}
    int currentAmmo;

    public int score;
    float hor;
    float vert;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        currentAmmo = 4;
        SetText(false);
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        hor = Input.GetAxis ("Horizontal");
        vert = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(hor, vert);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if(gameOver == true)
        {
            isInvincible = true;
            rb.simulated = false;
        }
        if(isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if(invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C) && ammoCount > 0 && !gameOver)
        {
            Launch();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                 NonPlayableCharacter character = hit.collider.GetComponent<NonPlayableCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                    didTalk = true;
                    SetText(false);
                    if (totalRobots == robotCount)
                    {
                        ChangeStage(2);
                    }
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(gameOver)
            {
                
                if(currentLevel == 2 && currentHealth > 0)
                {
                    RestartGame();
                }
                else
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }

    }

    void FixedUpdate()
    {
        Vector2 position = rb.position;
        position.x = position.x + moveSpeed * hor * Time.deltaTime;
        position.y = position.y + moveSpeed * vert * Time.deltaTime;

        rb.MovePosition(position);
    }
    public void ChangeHealth (int amount)
    {
        if(amount < 0)
        {
            if(isInvincible)
                return;
            isInvincible = true;
            invincibleTimer = timeInvincible;
            Instantiate(hitEffect, rb.position + Vector2.up * 0.5f, Quaternion.identity);
            PlaySound(hitSound);
            animator.SetTrigger("Hit");
        }
        else if (amount > 0)
        {
            Instantiate(healEffect, rb.position + Vector2.up * 0.5f, Quaternion.identity);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (currentHealth == 0)
        {
            gameOver = true;
            moveSpeed = 0;
            rb.Sleep();
            SetText(false);
        }
        UIHealthBar.instance.SetValue(currentHealth/ (float)maxHealth);


    }
    public void ChangeAmmo(int amount)
    {
        currentAmmo = currentAmmo + amount; 
        SetText(false);    
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        PlaySound(throwSound);
        currentAmmo --;
        SetText(false);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    void SetText(bool didWin)
   {
        displayAmmo.text = "X" + currentAmmo.ToString();
        displayText.text = "";
        if(!didTalk)
        {
            displayQuest.text = "Talk to Jambi";
        }
        else if (didTalk)
        {
            displayQuest.text = "Fix the Robots";
        }
        else if (didTalk && robotCount == totalRobots)
        {
            displayQuest.text = "Return to Jambi to advance";
        }
        if(currentLevel == 2)
        {
            displayQuest.text = "Fix the remaining robots";
        }

        if(gameOver && !didWin)
        {
            displayText.text = "You lose. <br>  Game Created by Jacob Dreyer <br> Press R to restart.";
            PlaySound(loseSound);
            backgroundMusic.SetActive(false);
        }
        else if (gameOver && didWin)
        {
            displayText.text = "You Win! <br>  Game Created by Jacob Dreyer <br> Press R to restart.";
            PlaySound(winSound);
            backgroundMusic.SetActive(false);
        }

   }
    public void ChangeScore(int amount)
    {
        robotCount = robotCount + amount;
        PlaySound(fixSound);
        displayRobots.text = "Fixed Robots:" + robotCount.ToString() + "/" + totalRobots.ToString();
        if (robotCount == totalRobots)
        {
            PlaySound(questSound);
            SetText(false);
            if(currentLevel == 2)
            {
                gameOver = true;
                SetText(true);
            }
        }
    }
    public void ChangeStage(int level)
    {
        currentLevel = level;
        if (level == 2)
        {
            SceneManager.LoadScene("level2");
        }

    }

    public void RestartGame()
    {
        currentLevel = 1;
        didTalk = false;
        gameOver = false;
        SetText(false);
        SceneManager.LoadScene("Main");

    }
}
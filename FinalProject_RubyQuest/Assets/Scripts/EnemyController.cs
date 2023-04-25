using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    public bool noMove;
    public bool isSentry;
    bool inAttackRange;
    float attackChargeTime = 3f;
    bool isCharging;

    public GameObject crosshair;



    Animator animator;

    private scr_playerController rubyController;

    AudioSource audioSource;

    public Rigidbody2D rb;
    public ParticleSystem smokeEffect;
    float timer;
    int direction = 1;
    public int damage = 2;

    bool broken = true;

    // Start is called before the first frame update
    void Start()
    {
        GameObject rubyControllerObject = GameObject.FindWithTag("Player");
        if (rubyControllerObject != null)
        {
            rubyController = rubyControllerObject.GetComponent<scr_playerController>(); //and this line of code finds the rubyController and then stores it in a variable
        }
        if (rubyController == null)
        {
            print("Cannot find player Script!");
        }
        rb = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        crosshair.SetActive(false);
        if(isSentry)
        {
            noMove = true;
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        if(!broken)
        {
            audioSource.Stop();

            return;
            
        }
        if(noMove)
        {
            speed = 0;
        }
        if(isSentry && inAttackRange)
        {
            crosshair.SetActive(true);
            crosshair.transform.position = rubyController.transform.position;
            StartCoroutine(ChargeAttack(inAttackRange));
        } else
        {
            crosshair.SetActive(false);
        }
       
    }

    void FixedUpdate()
    {
        if(isCharging)
        {
            return;
        }
        if (!broken)
        {
            return;
        }
        Vector2 position = rb.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction; ;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else if (isSentry)
        {
            animator.SetFloat ("Move X",0);
            animator.SetFloat ("Move Y", 0);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction; ;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rb.MovePosition(position);


    }
    void OnCollisionEnter2D(Collision2D other)
    {
        scr_playerController player = other.gameObject.GetComponent<scr_playerController>();

        if (player != null)
        {
            player.ChangeHealth(-damage);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(!isSentry)
        {
            return;
        }

        scr_playerController player = other.gameObject.GetComponent<scr_playerController>();

        if (player != null)
        {
            inAttackRange = true;
            isCharging = true;
            Debug.Log("Locked on");

             
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(!isSentry)
        {
            return;
        }

        scr_playerController player = other.gameObject.GetComponent<scr_playerController>();

        if (player != null)
        {
            Debug.Log("Out of range");
            inAttackRange = false;
            isCharging = false;
        }

    }
    public void Fix()
    {
        broken = false;
        rb.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        rubyController.ChangeScore(1);

    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private IEnumerator ChargeAttack(bool canShoot)
    {   
        if(canShoot && broken)
        {
            isCharging = true;
            yield return new WaitForSeconds(attackChargeTime);
            if(isCharging && broken)
            {
                Debug.Log("Firing");
                isCharging = false;
                rubyController.ChangeHealth(-damage);
            }
            yield break;

            
        }

    }

}

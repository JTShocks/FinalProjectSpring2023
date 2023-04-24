using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public bool isHealth;
    public bool isAmmo;
    public int healAmount = 1;
    public int ammoAmount = 1;
    public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        scr_playerController controller = other.GetComponent<scr_playerController>();

        if(controller != null)
       {
            if(controller.health < controller.maxHealth && isHealth)
            {
                controller.ChangeHealth(healAmount);
                Destroy(gameObject);

                controller.PlaySound(collectedClip);
            }
            else if(isAmmo)
            {
                controller.ChangeAmmo(ammoAmount);
                Destroy(gameObject);
                controller.PlaySound(collectedClip);
            }
       }
    }
}

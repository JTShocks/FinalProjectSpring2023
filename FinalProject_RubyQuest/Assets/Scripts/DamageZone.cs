using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damage = 1;


    void OnTriggerStay2D(Collider2D other)
    {
        scr_playerController controller = other.GetComponent<scr_playerController>();

        if (controller != null)
        {
            controller.ChangeHealth(-damage);
        }
    }
}

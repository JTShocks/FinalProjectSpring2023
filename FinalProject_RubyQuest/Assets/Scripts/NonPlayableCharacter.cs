using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class NonPlayableCharacter : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;
    AudioSource audioSource;
    public AudioClip talkSound;
    public float timerDisplay;
    public int stage;
    public bool endText;

    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        stage = 0;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                endText = true;
                dialogBox.SetActive(false);
                audioSource.Stop();
            }
        }
    }

    public void DisplayDialog()
    {
        endText = false;
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
        PlaySound(talkSound);
        if(stage == 0)
        {
            dialogText.text = "Help, my robots have gone wild! Use those gears to fix their heads.";
            stage++;
        }
        else if(stage == 1)
        {
            dialogText.text = "I heard one in the forest and a strong one near the cargo to the north.";
        }
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}

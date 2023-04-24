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
    public float timerDisplay;
    public int stage;

    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        stage = 0;
    }

    // Update is called once per frame
    public void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {

        timerDisplay = displayTime;
        dialogBox.SetActive(true);
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
}

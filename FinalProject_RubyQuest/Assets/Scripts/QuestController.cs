using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuestController : MonoBehaviour
{
    public int questState = 0;

    public int currentLevel;

    bool questFinished;

    public TextMeshProUGUI displayRobots;
    AudioSource audioSource;
    public AudioClip questSound;
    public AudioClip fixSound;

    public TextMeshProUGUI displayQuest;
    private scr_playerController rubyController;
    private NonPlayableCharacter Jambi;



    // Start is called before the first frame update
    void Start()
    {
        GameObject rubyControllerObject = GameObject.FindWithTag("Player");
        if (rubyControllerObject != null)

        {
            rubyController = rubyControllerObject.GetComponent<scr_playerController>(); 
            print(this + "Found the player Script!");
        }
        if (rubyController == null)
        {
            print(this +"Cannot find player Script!");
        }
        GameObject jambiObject = GameObject.FindWithTag("NPC");
        {
            if(jambiObject != null)
            {
                Jambi = jambiObject.GetComponent<NonPlayableCharacter>();
            }
        }
        SetQuestText(questState, currentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        //SetQuestText(questState, currentLevel);
    }
    public void SetQuestText(int progress, int level)
    {
        questState = progress;
        if(level == 1)
        {
            if(progress == 0 || progress == 2 )
            {
                displayQuest.text = "Talk to Jambi";
                displayRobots.text = "";
            }
            else if(progress == 1)
            {
                displayQuest.text = "Fix the Robots";
                
            }
            else if(progress == 3)
            {
                displayQuest.text = "Press X to advance to stage 2.";
                rubyController.gameOver = true;
            }
        }
        else if (level == 2)
        {
            displayQuest.text = "Fix the Robots";
        }

    }
    public void ChangeStage(int level)
    {
        currentLevel = level;
        questState = 0;
        if(currentLevel == 2)
        {
            SceneManager.LoadScene("level2");
            print(this + "Scene2 loaded!");
        }

    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}

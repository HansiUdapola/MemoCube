
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject LevelCompletePanel;
    GameObject WrongBoxSound;
    GameObject CorrectBoxSound;
    GameObject EndLevelSound;

    public Slider progress;

    int numberOfStagesPerLevel = 4;
    //int numberOfLevels = 10;
    static int stageLevel = 1;

    //bool gameEndStatus = false;

    static float userLevel = 0;
    public static Player currentUser;

    public Text scoreText;
    public Text LevelText;
    public Text HighScoreText;

    //For session handling

    static DateTime StageStartTime;
    static DateTime StageEndTime;

    int isCorrect = 0;

    private void Start()
    {


        WrongBoxSound = GameObject.Find("WrongSelection");
        CorrectBoxSound = GameObject.Find("CorrectSelection");
        EndLevelSound = GameObject.Find("EndLevelSound");

        SetLevel();
        SetScore();
        SetHighScore();
        SetProgressBar();

        StageStartTime = System.DateTime.Now;

    }

    public void SetScore()
    {
        scoreText.text = userLevel.ToString();
        SetHighScore(userLevel);
    }

    public void SetLevel()
    {
        LevelText.text = ((int)userLevel + 1).ToString();
    }

    public void SetHighScore()
    {
        HighScoreText.text = PlayerPrefs.GetFloat("HighScore").ToString();
    }

    public void SetHighScore(float highscore)
    {

        float currenthighscore = PlayerPrefs.GetFloat("HighScore", 0.0f);
        if (currenthighscore < highscore)
        {
            PlayerPrefs.SetFloat("HighScore", highscore);
            SetHighScore();
            UpdateHighScoreInFile(highscore);
        }
    }

    public void SetProgressBar()
    {
        progress.value = stageLevel;

    }

    public void EndStage(int corrects, int incorrects)
    {
        if (incorrects == 0) //if the stage is completed successfully
        {
            isCorrect = 1;
            SetScore();
            userLevel += (1.0f / numberOfStagesPerLevel);

            if (stageLevel == numberOfStagesPerLevel)
            {

                StartCoroutine(EndLevelSoundGenerator());

            }
            else
            {
                StartCoroutine(WinningSoundGenerator());
                

            }

        }
        else //unsuccessfull stage in a level
        {
            
            StartCoroutine(WrongSoundGenerator());
            
        }


    }

    IEnumerator WrongSoundGenerator()
    {
        WrongBoxSound.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.45f);
        
        Restart();
    }

    IEnumerator WinningSoundGenerator()
    {
        CorrectBoxSound.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.45f);
        stageLevel++;
        SetProgressBar();
        Restart();
    }

    IEnumerator EndLevelSoundGenerator()
    {

        EndLevelSound.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.45f);
        stageLevel = 1;
        EndLevel();
    }


    public void EndLevel()
    {

        SetLevel();
        SetProgressBar();

        FindObjectOfType<GameLauncher>().DisplayLevelCompletion();

    }

    public void Restart()
    {
        StageEndTime = System.DateTime.Now;
        WriteSessionRecord();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public float GetUserLevel()
    {
        return userLevel;
    }

    public void ResetGame()
    {
        userLevel = 0;
        stageLevel = 1;
    }

    public void SetCurrentPlayer(Player player)
    {
        currentUser = player;
    }

    public void UpdateHighScoreInFile(float highscore)
    {
        string currentusername = PlayerPrefs.GetString("Player", "");
        FindObjectOfType<DataSaver>().UpdateHighScore(highscore, currentusername);
    }

    public void WriteSessionRecord()
    {
        SessionManager sm = new SessionManager();

        sm.SetPlayerName(PlayerPrefs.GetString("Player", ""));
        sm.SetStageStartTime(StageStartTime);
        sm.SetStageEndTime(StageEndTime);


        int stage = stageLevel - 1;
        int lvl = ((int)GetUserLevel()) + 1;
        sm.LevelNo = lvl;
        sm.StageNo = stage;

        if (stage == 0 && isCorrect==1)
        {
            sm.StageNo = 4;
            sm.LevelNo = lvl - 1;
        }

        if (isCorrect == 0)
        {
            sm.StageNo += 1;
        }

        
        sm.CurrentScore = GetUserLevel();
        SetHighScore(sm.CurrentScore);

        sm.HighScore = PlayerPrefs.GetFloat("HighScore", 0.0f);
        sm.IsCompleted = isCorrect;
        sm.ReactionTimeinMS = FindObjectOfType<Spawn>().GetReactionTime();

        FindObjectOfType<DataSaver>().AddStageCompletionData(sm);
    }



}

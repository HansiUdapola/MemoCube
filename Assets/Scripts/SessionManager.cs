using System;

public class SessionManager
{
    string playerName;

    DateTime stageStartTime;
    DateTime stageEndTime;

    int levelNo;
    int stageNo;
    float currentScore;
    float highScore;

    int isCompleted;

    double reactionTimeinMS;

    public int LevelNo { get => levelNo; set => levelNo = value; }
    public int StageNo { get => stageNo; set => stageNo = value; }
    public float CurrentScore { get => currentScore; set => currentScore = value; }
    public float HighScore { get => highScore; set => highScore = value; }
    public int IsCompleted { get => isCompleted; set => isCompleted = value; }
    public double ReactionTimeinMS { get => reactionTimeinMS; set => reactionTimeinMS = value; }

    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public void SetStageStartTime(DateTime date)
    {
        stageStartTime = date;
    }

    public void SetStageEndTime(DateTime date)
    {
        stageEndTime = date;
    }

    public string GetPlayerName()
    {
        return this.playerName;
    }

    public DateTime GetStageStartTime()
    {
        return this.stageStartTime;
    }

    public DateTime GetStageEndTime()
    {
        return this.stageEndTime;
    }
}

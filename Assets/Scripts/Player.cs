using System;

[Serializable]
public class Player
{
    public string uname;
    public string age;
    public float highscore;
    public float currentscore;

    

    public void SetName(string name)
    {
        this.uname = name;
    }

    public string GetName()
    {
        return this.uname;
    }

    public void SetAge(string age)
    {
        this.age = age;
    }

    public string GetAge()
    {
        return this.age;
    }

    public void SetHighScore(float score)
    {
        this.highscore = score;
    }

    public float GetHighScore()
    {
        return this.highscore;
    }

    public void SetCurrentscore(float score)
    {
        this.currentscore = score;
    }

    public float GetCurrentscore()
    {
        return this.currentscore;
    }
}

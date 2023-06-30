using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataSaver : MonoBehaviour
{

    [SerializeField] InputField txtName;
    [SerializeField] InputField txtAge;
    public Text clearText;

    string userDetailsfileName = "UserDataFile.txt";
    string userLogfileName = "UserLogFile.csv";

    List<Player> playersList = new List<Player>();

    private void Start()
    {
        playersList = FileHandler.ReadListFromJSON<Player>(userDetailsfileName);

        //Set last user details in input field for improve usability
        Player p = GetLastPlayedUser(playersList);

        if (p != null && txtAge != null && txtName != null)
        {
            txtAge.text = p.GetAge();
            txtName.text = p.GetName();
        }

    }

    public void AddPlayerToList()
    {
        Player currentPlayer;
        string playername = txtName.text.Trim().ToUpper();
        string playerage = txtAge.text.Trim();

        if (playername == "" || playerage == "")
            return; // if fields are empty stop moving forward.



        if (!IsExistsUser(playersList, playername))
        {
            currentPlayer = new Player();
            currentPlayer.SetAge(playerage);
            currentPlayer.SetName(playername);
            currentPlayer.SetCurrentscore(0.0f);
            currentPlayer.SetHighScore(0.0f);

            playersList.Add(currentPlayer);
            FileHandler.SaveToJSON<Player>(playersList, userDetailsfileName);
        }
        else //if user already played
        {
            currentPlayer = GetExistingUser(playersList, playername);

            if (currentPlayer != null)
                Debug.Log("Data" + currentPlayer.GetName() + "," + currentPlayer.GetAge() + "," + currentPlayer.GetHighScore());
            else
                Debug.Log("Null");
        }

        //Set current user
        SetCurrentPlayerData(currentPlayer);

        //if succefull load nextscene Level1
        FindObjectOfType<GameLauncher>().ChangeScene("Level1");
    }

    public bool IsExistsUser(List<Player> players, string playerName)
    {
        bool flag = false;

        foreach (Player p in players)
        {
            if (p.GetName() == playerName)
            {
                flag = true;
                break;
            }
        }

        return flag;
    }

    public Player GetExistingUser(List<Player> players, string playerName)
    {
        Player player = null;

        foreach (Player p in players)
        {
            if (p.GetName() == playerName)
            {
                player = p;
                break;
            }
        }

        return player;
    }

    public int GetExistingUserIndex(List<Player> players, string playerName)
    {
        int i = -1, n = -1;

        foreach (Player p in players)
        {
            n++;
            if (p.GetName() == playerName)
            {
                i = n;
                break;
            }
        }

        return i;
    }

    //Get Last played user
    public Player GetLastPlayedUser(List<Player> players)
    {
        Player player = null;

        string lastuser = PlayerPrefs.GetString("Player", "");

        if (lastuser == "")
        {
            if (players != null && players.Count != 0)
            {
                player = players[players.Count - 1];
            }
        }
        else
        {
            player = GetExistingUser(players, lastuser);
        }



        return player;
    }




    //Set CurrentPlayer for the game session
    public void SetCurrentPlayerData(Player p)
    {
        if (p != null)
        {
            PlayerPrefs.SetString("Player", p.GetName());
            PlayerPrefs.SetFloat("HighScore", p.GetHighScore());
            PlayerPrefs.SetFloat("CurrentScore", p.GetCurrentscore());

        }
    }

    public void UpdateHighScore(float highscore, string currentusername)
    {

        int playerIndex = GetExistingUserIndex(playersList, currentusername);
        if (playerIndex != -1)
        {
            playersList[playerIndex].SetHighScore(highscore);
            FileHandler.SaveToJSON<Player>(playersList, userDetailsfileName);
        }

    }

    public string ConvertToCSV(SessionManager sm)
    {
        return (sm.GetPlayerName() + "," + sm.LevelNo + "," + sm.StageNo + "," + sm.IsCompleted + "," + sm.CurrentScore + "," + sm.HighScore + "," + sm.ReactionTimeinMS + "," + sm.GetStageStartTime() + "," + sm.GetStageEndTime() + "\n");
    }

    public void AddStageCompletionData(SessionManager sm)
    {
        FileHandler.AppendFile(userLogfileName, ConvertToCSV(sm));
    }

    public void Clear()
    {
        txtName.text = "";
        txtAge.text = "";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawn : MonoBehaviour
{

    Color LightBoxColor;


    public GameObject box;

    int minLevelOfRed = 4;

    int difficultyLevel;

    GameObject barrier;

    int correctSelections = 0;
    int incorrectSelections = 0;

    //int genNum1; // for random number 
    //int genNum2; // for random number 

    int max = 4; //Box Matix 

    public GameObject[,] boxes; // Generated box array

    int boxNo = 1;  // for naming boxes

    string[] coloredBoxNames; //names of the yellow colored boxes

    public List<string> selectedBoxNames = new List<string>(); // selecteed boxes by the user

    //To keep the reaction record
    double reactStart = 0;
    double reactStop = 0;


    // Start is called before the first frame update
    void Start()
    {

        ColorUtility.TryParseHtmlString("#6000E0", out LightBoxColor);

        barrier = GameObject.Find("BarrierCube");
        barrier.SetActive(true);

        //set the difficulty level as the user level (user level will be changed)
        difficultyLevel = (int) FindObjectOfType<GameManager>().GetUserLevel() + 1;
        
        coloredBoxNames = new string[difficultyLevel];        

        SpawnCubes();

    }

    void SpawnCubes()
    {
        boxes = new GameObject[max, max];

        for (int i = 0; i < max; i++)
        {
            for (int j = 0; j < max; j++)
            {
                boxes[i, j] = Instantiate(box, new Vector3(1.2f*(i-1.5f), (1.2f * j), 0), Quaternion.identity);
                boxes[i, j].GetComponent<MeshRenderer>().material.color = Color.black;

                boxes[i, j].name = "Box" + boxNo++;

            }
        }
        //Display the color pattern to remember
        StartCoroutine(LightTheBox());
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedBoxNames.Count == difficultyLevel)
        {
            //End Reaction Time
            reactStop = Time.time * 1000;

            for (int i=0; i<difficultyLevel; i++)
            {
                if (coloredBoxNames[i] == selectedBoxNames[i])
                {
                    correctSelections++;
                }
                else
                {
                    incorrectSelections++;
                }
            }

            Debug.Log("Win: "+correctSelections+"--- Lose: "+incorrectSelections);
      
            selectedBoxNames.Clear();

            StartCoroutine(DelayEnd());


        }

    }

    IEnumerator DelayEnd()
    {
        yield return new WaitForSeconds(0.45f);
        FindObjectOfType<GameManager>().EndStage(correctSelections, incorrectSelections);
    }

    IEnumerator LightTheBox()
    {
        int numberOfRedBoxes = difficultyLevel / minLevelOfRed;
        int x = numberOfRedBoxes; 
        int yellowcounter =0;
        
        for (int n = 0; n < difficultyLevel+ numberOfRedBoxes; n++)
        {
            int genNum1 = Random.Range(0, max);
            int genNum2 = Random.Range(0, max);
            int yellowORred = Random.Range(0, 2);

            if (n >= difficultyLevel && x > 0)
                yellowORred = 1;

            if(x > 0 && yellowORred == 1)
            {
                yield return new WaitForSeconds(0.8f);
                boxes[genNum1, genNum2].GetComponent<MeshRenderer>().material.color = Color.red;

                yield return new WaitForSeconds(0.8f);
                boxes[genNum1, genNum2].GetComponent<MeshRenderer>().material.color = Color.black;

                x--;
            }
            else
            {
                coloredBoxNames[yellowcounter++] = boxes[genNum1, genNum2].name;

                yield return new WaitForSeconds(0.8f);
                boxes[genNum1, genNum2].GetComponent<MeshRenderer>().material.color = LightBoxColor;
                //boxes[genNum1, genNum2].GetComponent<MeshRenderer>().material.color = Color.green;

                yield return new WaitForSeconds(0.8f);
                boxes[genNum1, genNum2].GetComponent<MeshRenderer>().material.color = Color.black;
            }

            
            
        }

        barrier.SetActive(false);
        //Start Reaction Time
        reactStart = Time.time*1000;

    }

    public double GetReactionTime()
    {
        double reactionTime = reactStop - reactStart;
        return reactionTime;
    }


}

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Numerics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public int round = 0;

    int currentNumber;
    public float timeRemaining = 16f;
    Text timer;

    public int[] allowedNumbers = new int[] { 2,4,6 } ;

    public GameObject numberPrefab;

    List<UnityEngine.Vector2> numberVectors = new List<UnityEngine.Vector2>();

    public Dictionary<UnityEngine.Vector2, int> numbersOnBoard = new Dictionary<UnityEngine.Vector2, int>();
    Dictionary<UnityEngine.Vector2, GameObject> gameObjectsOnBoard = new Dictionary<UnityEngine.Vector2, GameObject>();

    public int collectNum1 = -1, collectNum2 = -1, collectNum3 = -1, collectNum1P2 = -1, collectNum2P2 = -1, collectNum3P2 = -1;
    public Text collectText1, collectText2, collectText3,solutionText,numberText;
    public Text collectText1P2, collectText2P2, collectText3P2, numberTextP2;
    public GameObject tryAgainbutton, nextLevel;

    GameObject player, player2;
    UnityEngine.Vector2 playerOriginalTransform, player2OriginalTransform;
    public PlayerController pc,pc2;
    TextMeshPro playerNumberText, playerNumber2Text;

    int p1score, p2score;
    public Text p1scoreText, p2scoreText;

    private int currentSum, currentSumP2;

    public bool gamePaused = true;

    AudioSource audioSource;
  

    void Start()
    {
        timer = GameObject.Find("Timer").GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindWithTag("Player");
        player2 = GameObject.FindWithTag("Player2");

        
        playerOriginalTransform = player.transform.position;
        player2OriginalTransform = player2.transform.position;
      

        playerNumberText = player.GetComponent<PlayerController>().numberText;
        playerNumber2Text = player2.GetComponent<PlayerController>().numberText;

        StartCoroutine("WaitForStart", 3);


        

        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0 && !gamePaused)
        {
            timeRemaining -= Time.deltaTime;
            timer.text = Mathf.RoundToInt(timeRemaining).ToString();
        }

        if (timeRemaining < 0)
        {
            gamePaused = true;
            EndLevel(false, 0);

        }
    }

    public void StartLevel(int round)
    {
        

        timeRemaining = 16;

        pc.isAlive = true;
        pc2.isAlive = true;

   


        pc.currentdirP1 = PlayerController.Direction.Right;

        pc2.currentdirP2 = PlayerController.Direction.Left;


        player.GetComponent<SpriteRenderer>().color = Color.blue;
        player2.GetComponent<SpriteRenderer>().color = Color.red;



        
        gamePaused = false;

        int[] roundNumbers;

        switch (round)
        {
           

            case 1: // Faktoriser 12
                
                currentNumber = 12;
                numberText.text = "12";

                roundNumbers = new int[] { 2, 2, 2, 3, 4, 6 }; // kan lage funksjon for å forenkle

                PopulateLevel(roundNumbers, roundNumbers.Length); 

                break;

            case 0:
                
                currentNumber = 8;
                numberText.text = "8";

                roundNumbers = new int[] { 2, 2, 2, 4, 3, 6, 2 };

                PopulateLevel(roundNumbers, roundNumbers.Length);

                break;

            case 2:

                currentNumber = 36;
                numberText.text = "36";

                roundNumbers = new int[] { 8, 4, 2, 2, 2, 1, 6, 2, 3, 3 };

                PopulateLevel(roundNumbers, roundNumbers.Length);

                break;


            case 4:

                currentNumber = 30;
                numberText.text = currentNumber.ToString();
                roundNumbers = new int[] { 8, 4, 2, 2, 2, 1, 6, 2, 3, 3, 5, 5 };

                PopulateLevel(roundNumbers, roundNumbers.Length);

                break;

            case 3:

                currentNumber = 24;
                numberText.text = "24";
                roundNumbers = new int[] { 2, 2, 2, 6, 4, 3, 8 };

                PopulateLevel(roundNumbers, roundNumbers.Length);

                break;

            case 5:

                currentNumber = 256;
                numberText.text = "256";

                roundNumbers = new int[] { 16, 4, 8, 2, 32, 8, 6, 9, 24, 2, 2, 128 };
                PopulateLevel(roundNumbers, roundNumbers.Length);

                break;

            case 6:

                round = 0;
                EndLevel(false, 0);

                break;
        }

        

    }

    void PopulateLevel(int[] allowedNumbers, int numberCount)
    {
      //  GameObject instanced;

       /* if (gameObjectsOnBoard.Count == 0) // if first number
        {
            instanced = Instantiate(numberPrefab);
            instanced.transform.position = new UnityEngine.Vector2(Random.Range(-8, 8), Random.Range(-2, 4));
            gameObjectsOnBoard.Add(instanced.transform.position, instanced.gameObject);

        }*/



      
          /*  instanced = Instantiate(numberPrefab);
            instanced.transform.position = new UnityEngine.Vector2(Random.Range(-8, 8), Random.Range(-2, 4));


        // CheckFreeSpot(instanced);

        if (CheckFreeSpot(instanced) == true)
            {
                instanced.transform.position = new UnityEngine.Vector2(Random.Range(-8, 8), Random.Range(-2, 4));

                CheckFreeSpot(instanced);
            }

            gameObjectsOnBoard.Add(instanced.transform.position, instanced.gameObject);*/


        

            for (int i = 0; i < numberCount; i++)
        {
            var instanced = Instantiate(numberPrefab);

            instanced.transform.position = new UnityEngine.Vector2(Random.Range(-15,15), Random.Range(-6, 6));

            var numberScript = instanced.GetComponent<NumberScript>();

            //   numberScript.number = allowedNumbers[Random.Range(0, allowedNumbers.Length)];

            numberScript.number = allowedNumbers[i];
           


            numberScript.textMesh.text = numberScript.number.ToString();

            if (numberVectors.Count > 0)
            {
                CheckFreeSpot(instanced);

                if (CheckFreeSpot(instanced))
                {
                    instanced.transform.position = new UnityEngine.Vector2(Random.Range(-15, 15), Random.Range(-6, 6));

                    CheckFreeSpot(instanced);
                }


                numberVectors.Add(instanced.transform.position);

                numbersOnBoard.Add(instanced.transform.position, numberScript.number);

                gameObjectsOnBoard.Add(instanced.transform.position, instanced);



            } else
            {
                numberVectors.Add(instanced.transform.position);
                numbersOnBoard.Add(instanced.transform.position, numberScript.number);

                gameObjectsOnBoard.Add(instanced.transform.position, instanced);
            }









            }


    }


   public bool CheckFreeSpot(GameObject instanced)
    {

        /*  for (int i = 0; i < numberVectors.Count; i++)
          {
              if (instanced.transform.position.x == numberVectors[i].x) //check x
              {
                  //if X found, check y
                  if (instanced.transform.position.y == numberVectors[i].y) //Both values are found
                  {

                      //Give new position


                      return true;

                      //check again




                  }


              }
          }
          return false;*/

        foreach (KeyValuePair<UnityEngine.Vector2, GameObject> item in gameObjectsOnBoard)
        {
            if (instanced.transform.position.x == item.Value.transform.position.x) //Both values are found
            {
                if (instanced.transform.position.y == item.Value.transform.position.y)  //Both values are found
                {
                    return true;

                }
                
               
                                                                         

                    

                //check again




            }


        }
        return false;

    }

    public void UpdateNumber(int collectedNumber, bool isPlayerTwo)
    {




        if (!isPlayerTwo)
        {

            if (collectNum1 == -1) //updating text in blocks
            {
                collectNum1 = collectedNumber;
                collectText1.text = collectedNumber.ToString();

            }
            else if (collectNum2 == -1)
            {
                collectNum2 = collectedNumber;
                collectText2.text = collectedNumber.ToString();

            }
            else if (collectNum3 == -1)
            {

                collectNum3 = collectedNumber;
                collectText3.text = collectedNumber.ToString();

                //EndLevel();
            }

        }

            if (isPlayerTwo)
            {
                if (collectNum1P2 == -1) //updating text in blocks
                {
                    collectNum1P2 = collectedNumber;
                    collectText1P2.text = collectedNumber.ToString();
                   

            }
                else if (collectNum2P2 == -1)
                {
                    
                    collectNum2P2 = collectedNumber;
                    collectText2P2.text = collectedNumber.ToString();
                 

                }
                else if (collectNum3P2 == -1)
                {
                    
                    collectNum3P2 = collectedNumber;
                    collectText3P2.text = collectedNumber.ToString();
                   
            }
            }

        


        
        if (currentSum == 0 && !isPlayerTwo) // updating text in player
        {
            playerNumberText.text = collectedNumber.ToString();
            currentSum = collectedNumber;

        } else if (collectedNumber >0 && !isPlayerTwo)
        {
            
            currentSum = currentSum * collectedNumber;
            playerNumberText.text = currentSum.ToString();
            
        }



        if (currentSumP2 == 0 && isPlayerTwo) // updating text in player
        {
            playerNumber2Text.text = collectedNumber.ToString();
            currentSumP2 = collectedNumber;
        }
        else if (collectedNumber > 0 && isPlayerTwo)
        {

            currentSumP2 = currentSumP2 * collectedNumber;
            playerNumber2Text.text = currentSumP2.ToString();
            

        }


        if (currentSum == currentNumber) EndLevel(true, 1);

        if (currentSum > currentNumber) PausePlayer(1);

        if (currentSumP2 == currentNumber) EndLevel(true, 2);


        if (currentSumP2 > currentNumber) PausePlayer(2);

        if (gameObjectsOnBoard.Count == 0) EndLevel(false, 0);




    }

    public void DestroyNumber(UnityEngine.Vector2 onPosition)
    {

         foreach (KeyValuePair<UnityEngine.Vector2, GameObject> item in gameObjectsOnBoard)
         {

             if (item.Key == onPosition)
             {

                 gameObjectsOnBoard.Remove(item.Key);
                 Destroy(item.Value);
                 break;


             }

    }

       
        gameObjectsOnBoard.Remove(onPosition);
        


            
        

    }

    public void EndLevel(bool solved, int player)
    {

        if (solved) audioSource.Play();
    

        foreach (KeyValuePair<UnityEngine.Vector2, GameObject> item in gameObjectsOnBoard)
        {
                
                Destroy(item.Value);
                

        }
        gamePaused = true;
     


      


        if (solved && player == 1)
        {
            nextLevel.SetActive(true);
            if (collectNum3 != -1)
            {
                solutionText.text = "Riktig! " + collectNum1.ToString() + " * " + collectNum2.ToString() + " * " + collectNum3.ToString() + " = " + currentSum.ToString() + "!";

            }
            else if (collectNum2 != -2)
            {
                solutionText.text = "Riktig! " + collectNum1.ToString() + " * " + collectNum2.ToString() + " = " + currentSum.ToString() + "!";
            }

            p1score++;
            p1scoreText.text = p1score.ToString();

        } else if (!solved && player == 1)
        {
            tryAgainbutton.SetActive(true);

            solutionText.text = "Prøv igjen!";

        }

        if (solved && player == 2)
        {
            nextLevel.SetActive(true);
            if (collectNum3P2 != -1)
            {
                solutionText.text = "Riktig! " + collectNum1P2.ToString() + " * " + collectNum2P2.ToString() + " * " + collectNum3P2.ToString() + " = " + currentSumP2.ToString() + "!";

            }
            else if (collectNum2 != -2)
            {
                solutionText.text = "Riktig! " + collectNum1P2.ToString() + " * " + collectNum2P2.ToString() + " = " + currentSumP2.ToString() + "!";
            }

            p2score++;
            p2scoreText.text = p2score.ToString();
        }
        else if (!solved && player == 2)
        {
            tryAgainbutton.SetActive(true);

            solutionText.text = "Prøv igjen!";

        } else if (!solved && player == 0)
        {
            tryAgainbutton.SetActive(true);

            solutionText.text = "Prøv igjen!";

        }


    }


    public void PausePlayer(int playerToPause)
    {
        if (playerToPause == 1)
        {
            pc.isAlive = false;
            player.GetComponent<SpriteRenderer>().color = Color.grey;




        } else if (playerToPause == 2)
        {
            pc2.isAlive = false;
            player2.GetComponent<SpriteRenderer>().color = Color.grey;

        }

        if (!pc.isAlive && !pc2.isAlive) EndLevel(false, 0);
    }

    public void NextLevel()
    {
       
        
        currentSum = 0;
        currentSumP2 = 0;
        solutionText.text = "";
        playerNumberText.text = "";
        playerNumber2Text.text = "";

        collectNum1 = -1;
        collectNum2 = -1;
        collectNum3 = -1;
        collectText1.text = "";
        collectText2.text = "";
        collectText3.text = "";

        collectNum1P2 = -1;
        collectNum2P2 = -1;
        collectNum3P2 = -1;
        collectText1P2.text = "";
        collectText2P2.text = "";
        collectText3P2.text = "";



        gameObjectsOnBoard.Clear();
        numbersOnBoard.Clear();

        round++;

        timeRemaining = 15;

       

        nextLevel.SetActive(false);

        StartCoroutine("WaitForStart", 1);


    }
    
    public void TryAgain()
    {
        tryAgainbutton.SetActive(false);

        timeRemaining = 15;
        
        
        currentSum = 0;
        currentSumP2 = 0;
        solutionText.text = "";
        playerNumberText.text = "";
        playerNumber2Text.text = "";


        collectNum1 = -1;
        collectNum2 = -1;
        collectNum3 = -1;
        collectText1.text = "";
        collectText2.text = "";
        collectText3.text = "";

        collectNum1P2 = -1;
        collectNum2P2 = -1;
        collectNum3P2 = -1;
        collectText1P2.text = "";
        collectText2P2.text = "";
        collectText3P2.text = "";


        gameObjectsOnBoard.Clear();
        numbersOnBoard.Clear();

        StartCoroutine("WaitForStart", 1);


    }

    IEnumerator WaitForStart (int wait)
    {
        player.transform.position = playerOriginalTransform;
        player2.transform.position = player2OriginalTransform;

        gamePaused = true;

        float duration = 3f; // 3 seconds you can change this 
                             //to whatever you want
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            
            normalizedTime += Time.deltaTime / duration;

            timeRemaining = (int)normalizedTime;
            timer.text = "Get ready!";




            yield return null;

            
        }
        gamePaused = false;

        
        StartLevel(round);

        //yield return new WaitForSeconds(wait);
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerController : MonoBehaviour
{

    public enum Direction { Right, Left, Up, Down};


    public Direction currentdirP1;
    public Direction currentdirP2;


    public GameController gc;
    public TextMeshPro numberText;

    public Animator ac;

    public GameObject P2;

    public bool isPlayer2;

    public bool isAlive = true;

    public AudioSource pickUpSound;
    public AudioClip walkSound, pickSound;



    // Start is called before the first frame update
    void Start()
    {
        

        currentdirP1 = Direction.Right;

        currentdirP2 = Direction.Left;

        MovePlayer();

        gc = GameObject.Find("GameController").GetComponent<GameController>();
        P2 = GameObject.Find("Player2");

        pickUpSound = GetComponent<AudioSource>();

        
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            currentdirP1 = Direction.Left;
        } else if (Input.GetAxis("Horizontal") > 0) 
        {
            currentdirP1 = Direction.Right;
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            currentdirP1 = Direction.Down;

        } else if (Input.GetAxis("Vertical") > 0)
        {

            currentdirP1 = Direction.Up;
        }


        if (isPlayer2 && Input.GetAxis("Horizontal2") < 0)
        {
            currentdirP2 = Direction.Left;
        }
        else if (isPlayer2 && Input.GetAxis("Horizontal2") > 0)
        {
            currentdirP2 = Direction.Right;
        }

        if (isPlayer2 && Input.GetAxis("Vertical2") < 0)
        {
            currentdirP2 = Direction.Down;

        }
        else if (isPlayer2 && Input.GetAxis("Vertical2") > 0)
        {

            currentdirP2 = Direction.Up;
        }


        
    }



    // Update is called once per frame
    void MovePlayer()
    {
        if (!gc.gamePaused && isAlive)
        {
            if (!isPlayer2)
            {
                switch (currentdirP1)
                {

                    case (Direction.Right):
                        transform.position = new Vector2(transform.position.x + 1, transform.position.y);
                        break;

                    case (Direction.Left):
                        transform.position = new Vector2(transform.position.x - 1, transform.position.y);
                        break;

                    case (Direction.Up):
                        transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                        break;

                    case (Direction.Down):
                        transform.position = new Vector2(transform.position.x, transform.position.y - 1);
                        break;


                }
            }

            if (isPlayer2)
            { 
                
                switch (currentdirP2)
                {
                    

                    case (Direction.Right):
                        transform.position = new Vector2(transform.position.x + 1, transform.position.y);
                        break;

                    case (Direction.Left):
                        
                        transform.position = new Vector2(transform.position.x - 1, transform.position.y);
                        break;

                    case (Direction.Up):
                        transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                        break;

                    case (Direction.Down):
                        transform.position = new Vector2(transform.position.x, transform.position.y - 1);
                        break;


                }
            }


            // Check if hit number

         


                if (gc.CheckFreeSpot(gameObject))
            {

            // if returns true, then give message 
           foreach (KeyValuePair<Vector2,int> item in gc.numbersOnBoard) // denne funksjonen kan med fordel legges til GameController
           {



                    if (item.Key == new Vector2(transform.position.x, transform.position.y)) //if found vector
                    {
                        gc.UpdateNumber(item.Value, isPlayer2);
                        gc.DestroyNumber(item.Key);

                        pickUpSound.clip = pickSound;
                        pickUpSound.Play();

                        ac.SetTrigger("PickUp");



                    
                    }

           }

          

        }

        }


        //Check if level is done

        //Wait to move again


        if (transform.position.x <= -16) transform.position = new Vector2(15, transform.position.y);
        if (transform.position.x >= 16) transform.position = new Vector2(-15, transform.position.y);

        if (transform.position.y <= -7) transform.position = new Vector2(transform.position.x, 6);
        if (transform.position.y >= 7) transform.position = new Vector2(transform.position.x,- 6);


      

        StartCoroutine("WaitToMove");

        

    }

    IEnumerator WaitToMove()
    {

        yield return new WaitForSeconds(0.3f);
        MovePlayer();

    }


   
}

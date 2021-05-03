using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{    
    public float speed = 10;
    public TextMeshProUGUI PickUpIndicator; //BONUS : ... / Your PickUps : Non
    public TextMeshProUGUI RemainTime; //Remaining Time : ...
    public float Remain_time = 0;
    public TextMeshProUGUI Warning; //Warning : Ghost ahead behind the wall
    public GameObject WinText; //You Win !
    public GameObject LoseText; //You Lose !

    public GameObject player;
    public GameObject Ghost1;
    public GameObject Ghost2;

    private Rigidbody rb;   
    private int Bonus = 0; //indicator of bonus mode (0~3)

    private Vector3 P; //position of the player
	private Vector3 G1; //position of the ghost1
    private Vector3 G2; //position of the ghost2
    private float distance; //distance of a ghost and the player
    public Vector3 POSITION = Vector3.zero; // the position of an opening door (player is making a sound)

//INITIALLIZATION
     void Start()
     {
         rb = GetComponent<Rigidbody>();

         SetText(); //reset the text
         WinText.SetActive(false); //disable win text
         LoseText.SetActive(false); //disable the lose text
     }

//RESET THE TEXT
     void SetText()
     {
         PickUpIndicator.text = "Your PickUps : Non";
         RemainTime.text = " ";
         Warning.text = " ";
     }

//Record the position when opening a door: set it to the ghost
     public void Positioning()
     {
         POSITION = player.transform.position; // the position of an opening door (player is making a sound)
     }

//COLLISION DECTEROR
    private void OnTriggerEnter(Collider other)
    {
    //Silence the sound of opening a door
        if (Bonus == 1 ) //Bonus Mode 1: silence door opening
        {
            if (other.gameObject.CompareTag("RealDoor"))
            {
                other.gameObject.SetActive(false); //disable the door when hit
                other.gameObject.SetActive(true);  
            }
        }
    //Leave the door open
        else if (Bonus == 2) // Bonus Mode 2: Leave the door open
        {
            if (other.gameObject.CompareTag("RealDoor"))
            {
                other.gameObject.SetActive(false); //disable the door when hit
                Positioning();  
            }
        }
    //No Bonus
        else
        {
            if (other.gameObject.CompareTag("RealDoor"))
            {
                other.gameObject.SetActive(false); //disable the door when hit
                other.gameObject.SetActive(true);
                Positioning();    
            }
        }
    //Player heat the exit: game wined!
        if (other.gameObject.CompareTag("Exit"))
        {
            other.gameObject.SetActive(false); //disable the door when hit
            WinText.SetActive(true);
        }    
    //PickUp 1 (Bonus 1) : Silence of sound of opening doors       
        if (other.gameObject.CompareTag("PickUp1")) // Silence of sound of opening doors
        {
            PickUpIndicator.text = "BONUS : Silence the sound of opening doors";
            other.gameObject.SetActive(false); //disable the pickup when hit
            Remain_time = 30; //30 seconds for the bonus   
            Bonus = 1; //flag the bonus
        }
    //PickUp 2 (Bonus 2) : Leave the doors open 
        if (other.gameObject.CompareTag("PickUp2")) // Leave the door open
        {
            PickUpIndicator.text = "BONUS : Leave the Doors open";
            other.gameObject.SetActive(false); //disable the pickup when hit
            Remain_time = 10; //10 seconds for the bonus
            Bonus = 2; //flag the bonus
        }
    //PickUp 3 (Bonus 3) : Warning you if there is a ghost behind the door
        if (other.gameObject.CompareTag("PickUp3")) // Warning you if there is a ghost behind the door
        {
            PickUpIndicator.text = "BONUS : Warn you if there is a ghost behind the door";
            other.gameObject.SetActive(false); //disable the pickup when hit
            Remain_time = 60; //60 seconds for the bonus
            Bonus = 3; //flage the bonus
        }
    //Player caught by the ghost
        if (other.gameObject.CompareTag("Ghost")) // Lose the game when caught by the ghost
        {
            other.gameObject.SetActive(false);
            LoseText.SetActive(true);
        }
    }

//UPDATE
    void Update()
     {
    //In a Bonus mode
        if (Remain_time > 0)
        {
        //Count down
            Remain_time -= Time.deltaTime;
            RemainTime.text = "Remaining Time : " + Remain_time.ToString();
        //Warning you if there is a ghost behind the door
            if (Bonus == 3) // Bonus Mode 3: Warning you if there is a ghost behind the door
            {
                P = player.transform.position; //position of the player
                G1 = Ghost1.transform.position; //position of the ghost1
                G2 = Ghost2.transform.position; //position of the ghost2
                distance = Math.Min(Vector3.Distance(P,G1) , Vector3.Distance(P,G2)); //minimum distance between the player and the ghost
                if (distance <= 6)
                {
                    Warning.text = "Warning : Ghost ahead behind the wall"; //warning enabled
                }
                else
                {
                    Warning.text = " "; //warning disabled
                }
            }
    //No bonus or countdown to zero
        }
        else
        {
            SetText(); //reset the text
            Bonus = 0; //reset the bonus
        }      
    }
}
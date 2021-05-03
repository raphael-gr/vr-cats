using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMove : MonoBehaviour
{
    public float speed = 12; //moving speed of the ghost
    public GameObject player;
    private Rigidbody rb; 
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero; //move direction of the ghost
    public int moveFlag = 1; //move direction of the ghost (x or z)
    public float Remain_time = 0; //frozen time of the ghost when going through a door

    private Vector3 P; //position of the player
	private Vector3 G; //position of the ghost
    private float distance; //distance of a ghost and the player

    public static System.Random ran = new System.Random();

    private PlayerController Pposition = new PlayerController();
    public Vector3 P_door => Pposition.POSITION;
    Vector3 P_ghost = Vector3.zero;
    public float time = 0;

//INITIALLIZATION
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
    }

//COLLISION DECTEROR
    private void OnTriggerEnter(Collider other)
    {
    //The ghost heat a real door
        if (other.gameObject.CompareTag("RealDoor"))
        {
            Remain_time = 2; //frozen 2 seconds      
        }
    //The ghost heat a fake door 
        if (other.gameObject.CompareTag("FakeDoor")) //Reset the position of the ghost to the position before moving 6 
        {
            time = 0;
            transform.Translate(P_ghost.x, P_ghost.y , P_ghost.z);
        }
    //The ghost catches the player
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.SetActive(false); //Lose the game: player disabled
        }
    //Ghost going through the pickups
        if (other.gameObject.CompareTag("PickUp1") || other.gameObject.CompareTag("PickUp2") || other.gameObject.CompareTag("PickUp3"))
        {
            other.gameObject.SetActive(false); //ghost through the pickup
            other.gameObject.SetActive(true); //reset the pickup
        }
    }

//UPDATE FUNCTION
    void Update()
    {
    //Delay due to going through the door
        if (Remain_time > 0)
        {      
            Remain_time -= Time.deltaTime; //Count down
        }
    //Normal moving
        else
        {
        //Calculate the distance between the ghost and the player
            P = player.transform.position; //position of the player
            G = transform.position; //position of the ghost
            distance = Vector3.Distance(P,G); //distance between the ghost and the player
        //If there is no door between the ghost and the player: the ghost heading directly to the player
            if (distance <= 5)
            {
                 moveDirection = (P - G) * speed; //moving direction: directly to the players
                 characterController.Move(moveDirection * Time.deltaTime);   
            }
        //The ghost move either in x or z directions (6,0,0) or (0,0,6), not in both, 
            else
            {
            //X movement
                if (moveFlag == 1 && time <= 6/speed ) //move in x direction
                {
                    if (P_door.x - G.x > 0 ) //move in +x direction
                    {
                        characterController.Move(new Vector3 (1,0,0) * speed * Time.deltaTime );
                        time += Time.deltaTime;
                    }
                    if (P_door.x - G.x < 0 ) //move in -x direction
                    {
                        characterController.Move(new Vector3 (-1,0,0) * speed * Time.deltaTime );
                        time += Time.deltaTime;
                    }
                }
            //Z movement
                if (moveFlag == 2 && time <= 6/speed) //move in z direction
                {
                    if (P_door.z - G.z > 0 ) //move in +z direction
                    {
                        characterController.Move(new Vector3 (0,0,1) * speed * Time.deltaTime );
                        time += Time.deltaTime;
                    }
                    if (P_door.z - G.z < 0) //move in -z direction
                    {
                        characterController.Move(new Vector3 (0,0,-1) * speed * Time.deltaTime );
                        time += Time.deltaTime;
                    }
                }
            //Reset the time of moving towards one direction
                if (time >= 6/speed)
                {
                    time = 0;
                    moveFlag = ran.Next(1,3); //generate a random number of 1 or 2
                    P_ghost = transform.position; // record the position of the ghost before moving 6.
                }
             }
        }
    }
}

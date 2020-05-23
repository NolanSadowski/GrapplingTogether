using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;

public class MainPlayer : MonoBehaviour
{

    //the following is in order to use rewired
    [Tooltip("Reference for using rewired")]
    [HideInInspector]
    public Player player1;
    public Player player2;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;

    [Header("Movement")]

    public float speed;

    bool isGrounded;

    Vector3 velocity;
    float velocityY = 0;

    Rigidbody rb;

    private void Awake()
    {
        //Rewired Code
        player1 = ReInput.players.GetPlayer(0);
        player2 = ReInput.players.GetPlayer(1);
        ReInput.ControllerConnectedEvent += OnControllerConnected;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        Gravity();
    }

    private void FixedUpdate()
    {
        FixedMovement();
    }

    void Movement()
    {
        if (playerNum == 1)
        {
            velocity = new Vector3(player2.GetAxis("MoveX"), velocityY, player2.GetAxis("MoveZ")) * speed;
        }
        else if(playerNum == 2)
        {
            velocity = new Vector3(player1.GetAxis("MoveX"), velocityY, player1.GetAxis("MoveZ")) * speed;
        }
        velocity = transform.worldToLocalMatrix.inverse * velocity;
    }

    void FixedMovement()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    void Gravity()
    {
        
    }

    //[REWIRED METHODS]
    //these two methods are for ReWired, if any of you guys have any questions about it I can answer them, but you don't need to worry about this for working on the game - Buscemi
    void OnControllerConnected(ControllerStatusChangedEventArgs arg)
    {
        CheckController(player1);
        CheckController(player2);
    }

    void CheckController(Player player)
    {
        foreach (Joystick joyStick in player.controllers.Joysticks)
        {
            var ds4 = joyStick.GetExtension<DualShock4Extension>();
            if (ds4 == null) continue;//skip this if not DualShock4
            switch (playerNum)
            {
                case 4:
                    ds4.SetLightColor(Color.yellow);
                    break;
                case 3:
                    ds4.SetLightColor(Color.green);
                    break;
                case 2:
                    ds4.SetLightColor(Color.blue);
                    break;
                case 1:
                    ds4.SetLightColor(Color.red);
                    break;
                default:
                    ds4.SetLightColor(Color.white);
                    Debug.LogError("Player Num is 0, please change to a number > 0");
                    break;
            }
        }
    }

}

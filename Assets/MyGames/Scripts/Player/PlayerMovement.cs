using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("DangSon/PlayerMovement")]
public class PlayerMovement : MonoBehaviour
{
    #region Public variable
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumHeght = 3f;
    public Transform groundCheck;
    public float groundDistance =  0.4f;
    public LayerMask groundMask;
    //private bool isGround = false;
    #endregion
    #region private variable
    private Vector3 velocity;
    private Vector3 lastPosition = new Vector3(0f,0f,0f);
    private CharacterController controller;
    #endregion
    //private Player player;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        //player = GetComponent<Player>();
    }
    // Update is called once per frameif
    void Update()
    {
      //  if (player != null && player.isPlayerDead) return; 
        if (IsCheckGround() && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward*z;
        controller.Move(move*speed*Time.deltaTime);
        //nhay
        if(Input.GetButtonDown("Jump")&&IsCheckGround())
        {
            velocity.y = Mathf.Sqrt(jumHeght - 2f* gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity*Time.deltaTime);
        lastPosition = transform.position;
    }
     bool IsCheckGround()
    {
       bool isGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
       return isGround;
    }
}

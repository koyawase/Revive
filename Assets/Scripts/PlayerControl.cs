﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{

    //Movement
    Vector2 input;
    public float walkSpeed = 7;
    public float runSpeed = 14;
    public float gravity = 30f;
    public float jumpSpeed = 8.0F;
    public float jumpHeight = 10;
    public Vector3 velocity;
    public float velocityY;
    // Lantern Object - Tui
    public GameObject Lantern;
    public GameObject Bow;
    public GameObject Umbrella;
    int flowersPicked = 0;

    //Rotation
    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;


    //Controllers
    Transform camera;
    CharacterController controller;

    public Text task;
    public Text instruction;
    public Text flower;

    //Animator
    Animator anim;
    bool movingHorizontal = false;
    bool movingVertical = false;

    //Scale
    Vector3 startingScale;
    bool isStartingScale = true;

    //abilities
    bool umbrella = false;
    bool colourChange = false;

    //colour changing in according to enemy following
    public int colour = 0;

    void Start()
    {
        camera = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        startingScale = transform.localScale;
        Lantern.SetActive(false);
        Bow.SetActive(false);
        Umbrella.SetActive(false);
        UpdateTask();
    }

    //LanterTurnOn + LightTurnOn - Tui
   void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LanternTag")
        {
            Lantern.SetActive(true);
        }

        if(other.tag == "Bow"){
            other.gameObject.SetActive(false);
            Bow.SetActive(true);
            colourChange = true;
        }

        //picking up the umbrella, abilities?
        if(other.tag == "Umbrella"){
            other.gameObject.SetActive(false);
            Umbrella.SetActive(true);
            umbrella = true;
        }

        //the task system to get the flowers
        if(other.tag == "Flower"){
            other.gameObject.SetActive(false);
            flowersPicked++;
            Debug.Log(flowersPicked);
            UpdateTask();
        }

        if (Input.GetButtonDown("Fire3") && other.tag == "LightTag")
        {
            Debug.Log("Hello");
        }

        if(other.tag == "Movement"){
            instruction.text = "Press wasd for movements, shift to move faster";
        }

        if (other.tag == "FlowerInstruction")
        {
            flower.text = "Congrats! You just picked up a flower." +
                "Follow the flowers to find the path and " +
                "collect five of them to go to the next level!";
        }

        if(other.tag == "Tentacle"){
            Debug.Log("..");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Movement"){
            instruction.text = "";
        }

        if (other.tag == "FlowerInstruction"){
            flower.text = "";
            other.gameObject.SetActive(false);
        }
    }


    void Update()
    {
        CheckAnimation();

        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        CheckJump();

        CheckScale();

        //Rotating the character
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + camera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);

        }

        bool running = Input.GetKey(KeyCode.LeftShift);
        float speed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;

        velocityY += Time.deltaTime * -gravity;

        velocity = transform.forward * speed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
        {
            velocityY = 0;
        }
    }

    void CheckJump()
    {
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            velocityY = jumpHeight;
        }
    }

    //This method makes sense logically but is broken in game.
    void CheckAnimation()
    {
        //Checking inputs to determine animations
        if (Input.GetKey("a") || Input.GetKey("d"))
        {
            movingHorizontal = true;
        }
        if (Input.GetKey("w") || Input.GetKey("s"))
        {
            movingVertical = true;
        }
        if (!Input.GetKey("a") && !Input.GetKey("d"))
        {
            movingHorizontal = false;
        }
        if (!Input.GetKey("w") && !Input.GetKey("s"))
        {
            movingVertical = false;
        }

        //fall slower when holding space bar
        //if have the ability of course
        if (Input.GetButtonDown("Jump") & (!controller.isGrounded) && umbrella)
        {
            gravity = 3f;
        }
        if (Input.GetButtonUp("Jump"))
        {
            gravity = 30f;
        }

        //Setting the animation based on the input
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            anim.SetTrigger("jump");
        }


        if (movingHorizontal == true || movingVertical == true)
        {
            anim.SetBool("isRunning", true);
        }
        else if (movingHorizontal == false && movingVertical == false)
        {
            //Debug.Log("Idle Anim");
            anim.SetBool("isRunning", false);
        }
        //Booleans aren't working correctly, maybe for accuracy try calling button directly

    }

    //Changes the scale based off input
    //Player may only be able to access certain areas or do certain things when a certain scale
    void CheckScale()
    {
        if (Input.GetKeyDown("c"))
        {
            if (isStartingScale)
            {
                transform.localScale = new Vector3(5f, 5f, 5f);
                isStartingScale = false;
            }
            else if (!isStartingScale)
            {
                transform.localScale = startingScale;
                isStartingScale = true;
            }
        }
    }

    public void SetColour(int newColour)
    {
        colour = newColour;
    }

    public int getFlowers(){
        return flowersPicked;
    }

    //Player has health and lives. Loses life when dies
    //And loses health when attacked or in fog/smoke etc.
    void CalculateHealth()
    {

    }

    void UpdateTask(){
        task.text = "Tasks: \n" +
            "Collect flowers[" + flowersPicked.ToString();
    }
}
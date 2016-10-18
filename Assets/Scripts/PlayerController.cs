﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    //Private instances 
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private float _move;
    private float _jump;
    private bool isFacingRight = true;
    private bool _isGrounded;
    private Animator _animator;
    private GameObject _camera;
    private GameObject _spawnPoint;
    //private GameObject _gameControllerObject;
    //private GameController _gameController;
    private GameController _gameController;

    //Public instances variables
    public float Velocity = 10f;
    public float JumpForce = 100f;

    //public AudioSource JumpSound;
	// Use this for initialization
	void Start () {
        this._initialize();
	
	}
	
	// Update is called once per frame (Physics)
	void FixedUpdate () {

        if (_isGrounded)
        {
            //check if input is present for movment
            this._move = Input.GetAxis("Horizontal");
            if (this._move > 0f)
            {
                this._animator.SetInteger("HeroState", 1);
                this._move = 1;
                this.isFacingRight = true;
                this.flip();
            }
            else if (this._move < 0f)
            {
                this._animator.SetInteger("HeroState", 1);
                this._move = -1;
                this.isFacingRight = false;
                this.flip();
            }
            else
            {
                //set animator state 
                this._animator.SetInteger("HeroState", 0);
                this._move = 0f;
            }
            //to jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this._animator.SetInteger("HeroState", 2);
                this._jump = 1f;
                //this.JumpSound.Play();

            }

            this._rigidbody.AddForce(new Vector2(this._move * this.Velocity, this._jump * this.JumpForce), ForceMode2D.Force);
        }
        else
        {
            this._move = 0f;
            this._jump = 0f;
        }
        

        this._camera.transform.position = new Vector3(this._transform.position.x, this._transform.position.y, -10f);
        
	}


    /*Methods/**
     * 
    *This method initialize variables and objects when called
    */
    private void _initialize()
    {
        this._transform = GetComponent<Transform>();
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._animator = GetComponent<Animator>();
        this._camera = GameObject.FindWithTag("MainCamera");
        this._spawnPoint = GameObject.FindWithTag("SpawnPoint");
        //this._gameControllerObject = GameObject.Find("GameController");
        //this._gameController = this._gameControllerObject.GetComponent<GameController> () as GameController;
        this._gameController = FindObjectOfType(typeof(GameController)) as GameController;

        this._move = 0;
        this.isFacingRight = true;
        this._isGrounded = false;
    }

    /*
     *This method flips the character's bitmap the x-axis
     *  * */
    private void flip(){
        if(this.isFacingRight){
            this._transform.localScale = new Vector2 (1f,1f);
        }else{
            this._transform.localScale = new Vector2(-1f, 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("DeathPlane"))
        {
            //move player position to spawn point's position
            this._transform.position = this._spawnPoint.transform.position;
            this._gameController.LivesValue -= 1;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Enemy"))
        {
            this._isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {

        this._animator.SetInteger("HeroState", 2);
        this._isGrounded = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //when pick up a coin
        if (other.gameObject.CompareTag("Coin"))
        {
            //coin score +100
            this._gameController.ScoreValue += 100;
            Destroy(other.gameObject);
        }
    }



}

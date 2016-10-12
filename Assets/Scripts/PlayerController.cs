using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    //Private instances 
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private float _move;
    private float _jump;
    private bool isFacingRight = true;
    private bool _isGrounded;

    //Public instances variables
    public float Velocity = 10f;
    public float JumpForce = 100f;
    public Camera camera;
    public Transform SpawnPoint;
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
                this._move = 1;
                this.isFacingRight = true;
                this.flip();
            }
            else if (this._move < 0f)
            {
                this._move = -1;
                this.isFacingRight = false;
                this.flip();
            }
            else
            {
                this._move = 0f;
            }
            //to jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
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
        

        this.camera.transform.position = new Vector3(this._transform.position.x, this._transform.position.y, -10f);
        
	}


    /*Methods/**
     * 
    *This method initialize variables and objects when called
    */
    private void _initialize()
    {
        this._transform = GetComponent<Transform>();
        this._rigidbody = GetComponent<Rigidbody2D>();
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
        if (other.gameObject.CompareTag("DeadPlane"))
        {
            //move player position to spawn point's position
            this._transform.position = this.SpawnPoint.position;

        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Plataform"))
        {
            this._isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        this._isGrounded = false;
    }


}

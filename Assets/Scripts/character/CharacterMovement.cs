using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Parameters
    [SerializeField] private float playerSpeed = 2.0f;          // Player Movement Speed Value
    [SerializeField] private float sprintSpeed = 5.0f;


    [SerializeField] private float switchDepthSpeed = 0.3f;


    [SerializeField] private float initialJump = 1.0f; // Player jump "force"


    [SerializeField] private float gravity = -5f;

    [SerializeField] private float jumpGravityMult = 0.5f; // Changes the gravity when jumping
    

	[SerializeField] private TrailRenderer _trail_renderer;

	/// <Summary> Enemies that can bet attacked</Summary>
	private List<Mob_base_behavior> on_attack_range = new List<Mob_base_behavior>();

	/// <Summary> Items in range</Summary>
	private List<GrabbableItem> on_item_range = new List<GrabbableItem>();

	/// <sumamry> Weapon Sprite renderer </Summary>
	[SerializeField] private SpriteRenderer _sprite_renderer;

	/// <Summary> Weapon damage
	private float _current_weapon_dmg = 0.0f;

	private float duringJumpGravity;

    private bool spritting = false;

    // Cache
    private CharacterController characterController;
    private Transform bodyController;
    private Animator animator;
    public float depth = 0.0f;
    private float maxDepth = 2.0f;
    private float minDepth = 0.0f;
    private float horizontalInput = 0.0f;

    private bool jump = false;
    private float y_velocity = 0.0f;
    private float direction = 1f;

    private Vector3 force = Vector2.zero;
    private bool useForce = false;
    
    private bool onGround = false;

    private GameObject nearHole;
    private bool enterHole = false;

    private float timerAnimation = 0.0f;
    private bool activatedCamera = false;


    public float verticalPositions;




    // Start is called before the first frame update
    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        bodyController = transform.GetChild(0);
        animator = bodyController.GetComponent<Animator>();
        duringJumpGravity = gravity * jumpGravityMult;
		_sprite_renderer.enabled = false;
		_trail_renderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ( enterHole )
        {
            Vector3 movement = new Vector3(0f, 0f, 0f);

            //activate animation
            timerAnimation += Time.deltaTime;
            if (timerAnimation >= 3f)
            {
                //finnish animation
                enterHole = false;
                activatedCamera = false;
            }
            else if (timerAnimation >= 1.2f)
            {
                if (!activatedCamera)
                {
					if(nearHole) {
						GameCore.Instance.viewCore.mainCamera.GetComponent<CameraController>().EnterHole(nearHole);
                    	activatedCamera = true;
					}
					else{
						SetExitHoleFinish();
					}
                }
            }
        }
        else
        {
            ReadInput();
            ApplyMovement();
        }
    }

    private void ReadInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
		animator.SetBool("walk", Mathf.Abs(horizontalInput) > 0);
        
        if (Input.GetKeyDown(KeyCode.W)){
            depth++;
        }
		
        if(Input.GetKeyDown(KeyCode.S)) {
            depth--;
        }

		depth = Mathf.Clamp(depth,minDepth, maxDepth);

        if ( Input.GetKeyDown(KeyCode.Space) && !jump)
        {
            jump = true;
            animator.SetTrigger("jump");
        }

		spritting = Input.GetKey(KeyCode.LeftShift);
		animator.SetBool("sprint", Input.GetKey(KeyCode.LeftShift));
        

		/// Interaction calls
        if ( Input.GetKeyDown(KeyCode.F))
        {
			if(_current_weapon_dmg != 0.0f){
				animator.SetTrigger("attack");
				
				if( on_attack_range.Count != 0){
					AttackMobs();
				}
			}

			if(on_item_range.Count != 0){
				ActivateItems();
			}
        }

        if ( Input.GetKeyDown(KeyCode.E) && !enterHole)
        {
			if(!nearHole)
				return;

            enterHole = true;
            timerAnimation = 0f;
            //do player animation
            animator.SetTrigger("hole");
        }
    }

    private void ApplyMovement()
    {

		if(GameCore.Instance.isOnBrighter())
        	onGround = characterController.isGrounded;
		else 
			onGround = (transform.position.y >= verticalPositions);

		if(GameCore.Instance.isOnBrighter()){
			if ( onGround && y_velocity < 0f)
					{
						y_velocity = -0.5f;
					}
		}
		else {
			if ( onGround && y_velocity >= 0f)
			{
				y_velocity = 0.5f;
			}
		}
        

        Vector3 movement = new Vector3(0f, 0f, 0f);

        if (spritting)
        {
            movement.x = characterController.transform.right.x * horizontalInput * sprintSpeed;
        }
        else
        {
            movement.x = characterController.transform.right.x * horizontalInput * playerSpeed;
        }

        if ( horizontalInput < 0f)
        {
            direction = -1f;
        }
        else if ( horizontalInput > 0f)
        {
            direction = 1f;
        }


        if ( bodyController.localScale.x != direction)
        {
            bodyController.localScale = new Vector3(direction, 1f, 1f);
        }

        if( jump && onGround)
        {
			if(GameCore.Instance.isOnBrighter())
				y_velocity += Mathf.Sqrt(
					initialJump * -3.0f * gravity);
			else
				y_velocity -= Mathf.Sqrt(
						initialJump * 3.0f * gravity);
        }
        if ( !jump )
        {
            y_velocity += gravity * Time.deltaTime;
        }
        else
        {
            y_velocity += duringJumpGravity * Time.deltaTime;
        }

		if(GameCore.Instance.isOnBrighter()){
			if ( y_velocity <= 0.1f)
			{
				jump = false;
                animator.SetTrigger("jumpOver");
            }
		}
		else {
			if ( y_velocity >= -0.1f)
			{
				jump = false;
                animator.SetTrigger("jumpOver");
            }
		}

        if(useForce)
        {
            force -= force * playerSpeed * Time.deltaTime;
            movement += force;

            if( force.magnitude <= 0.01f)
            {
                useForce = false;
            }
        }

        movement.y = y_velocity;

        characterController.Move(movement * Time.deltaTime);

        if (transform.position.z != depth)
        {
            float toDepth = Mathf.Lerp(transform.position.z, depth, switchDepthSpeed);
            if (Mathf.Abs(transform.position.z - depth) < 0.1)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, depth);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, toDepth);
            }
        }
    }

    public void ApplyForce(Vector3 vec)
    {
        if ( !enterHole )
        {
            force = vec;
            useForce = true;
        }
    }

    public void SetNearHole(GameObject hole)
    {
        nearHole = hole;
    }

	public void ClearNearHole(){
		nearHole = null;
	}

    public void SetExitHoleFinish()
    {
        enterHole = false;
		activatedCamera = false;
		timerAnimation = 0.0f;
    }


	/// <summary> Handles the view switch changes</summary>
	public void switchView(){
		gravity *= -1f;
		transform.position = new Vector3(
			transform.position.x,
			GameCore.Instance.isOnBrighter() ? 1.1f : -1.8f,
			transform.position.z
		);

		transform.localScale = new Vector3(
			transform.localScale.x, -transform.localScale.y, transform.localScale.x
		);
		duringJumpGravity = gravity * jumpGravityMult;
		y_velocity = 0.0f;
	

	}

	/// <Summary> Regist an eneny </Summary>
	public void RegistMobInRange(Mob_base_behavior inRange){
		if(!on_attack_range.Contains(inRange)){
			on_attack_range.Add(inRange);
		}
	}

	/// <Summary> Remove a registed enemy</Summary>
	public void RemoveMobInRange(Mob_base_behavior inRange){
		if(on_attack_range.Contains(inRange)){
			on_attack_range.Remove(inRange);
		}
	}

	/// <Summary> Regist an item in range</Summary>
	public void RegistItemInRange(GrabbableItem inRange){
		if(!on_item_range.Contains(inRange)){
			on_item_range.Add(inRange);
		}
	}


	/// <Summary> </Summary>
	public void RemoveItemInRange(GrabbableItem inRange){
		if(on_item_range.Contains(inRange)){
			on_item_range.Remove(inRange);
		}
	}

	/// <summary> Trigger attack mobs</summary>
	public void AttackMobs(){
		for (int i = on_attack_range.Count - 1; i >= 0; i--)
		{
			on_attack_range[i].OnTakeDamage(_current_weapon_dmg);
		}
	}

	public void ActivateItems(){
		for(int i = on_item_range.Count - 1; i >= 0; i--){
			on_item_range[i].activate_item();
		}

	}

	/// Defines the current weapon
	public void DefineWeapon(Sprite sprite, float dmg){
		_trail_renderer.enabled = true;
		_current_weapon_dmg = dmg;
		_sprite_renderer.sprite = sprite;
		_sprite_renderer.enabled = true;
	}


	
}
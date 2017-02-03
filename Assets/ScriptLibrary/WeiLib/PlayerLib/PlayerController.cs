using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	//=======================================
	//----------Generic----------------------
	//=======================================
	#region Generic
	public string AxisUpDown;
	public string AxisLeftRight;

	public float walkSpeed =2;
	public float runSpeed = 6;
	public bool isRunning = false;
	public float ccJumpHeight = 1.0f;
	public float rgJumpForce = 200.0f;
	public KeyCode jumpKey = KeyCode.Space;

	Vector3 velocityDir;  					//By default velocity Direction is transform.forward.
	float postProcessedMoveSpeed = 0;       //speed is only a Scalar
	Vector3 velocity3D;						//Velocity contains speed and Direction.

	public float turnSmoothTime = 0.2f; 	//the bigger the slower
	float turnSmoothVelocity;

	public float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	float currentSpeed;

	Vector2 moveInput;
	public float defaultYRotation = 0;

	void UpdateInput()
	{
		moveInput.x = Input.GetAxisRaw(AxisLeftRight);
		moveInput.y = Input.GetAxisRaw(AxisUpDown);
	}
	//Facing Direction
	void FacingInputDir()
	{
		if(moveInput != Vector2.zero)
			transform.eulerAngles = Vector3.up * Mathf.Atan2(moveInput.x,moveInput.y) * Mathf.Rad2Deg;
		else
			transform.eulerAngles = Vector3.up * defaultYRotation;
	}
	void FacingInputDir_Stay()
	{
		if(moveInput != Vector2.zero){
			transform.eulerAngles = Vector3.up * Mathf.Atan2(moveInput.x,moveInput.y) * Mathf.Rad2Deg;	
		}
	}
	void FacingInputDir_Stay_AlignCameraDir()
	{
		if(moveInput != Vector2.zero){
			transform.eulerAngles = Vector3.up * (Mathf.Atan2(moveInput.x,moveInput.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y);
		}
	}
	void FacingInputDir_Stay_AlignCamaraDir_Smooth()
	{
		if(moveInput != Vector2.zero){
			float targetRotation =  Mathf.Atan2(moveInput.x,moveInput.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y,targetRotation,ref turnSmoothVelocity,turnSmoothTime);
		}
	}
	//Velocity Direction
	void UpdateVelocityDir(Vector3 v)
	{
		velocityDir = v;
	}
	void UpdateVelocityDir_WithFacingDir()
	{
		velocityDir = transform.forward;
	}
	void UpdateVelocityDir_WithInputDir()
	{
		velocityDir = new Vector3(moveInput.x,0,moveInput.y);
	}
	//Post process Speed and Velocity
	void PostProcessed_MoveSpeed()
	{
        postProcessedMoveSpeed = isRunning ? moveInput.magnitude * runSpeed : moveInput.magnitude * walkSpeed;
    }
	void PostProcessed_MoveSpeed_Velocity()
	{
		postProcessedMoveSpeed = isRunning ?  moveInput.magnitude * runSpeed : moveInput.magnitude * walkSpeed;
		velocity3D = postProcessedMoveSpeed * velocityDir;
	}
	void PostProcessed_MoveSpeed_Velocity_Smooth()
	{
		float targetSpeed = isRunning ?  moveInput.magnitude * walkSpeed : moveInput.magnitude * runSpeed;
		postProcessedMoveSpeed =  Mathf.SmoothDamp(postProcessedMoveSpeed,targetSpeed,ref speedSmoothVelocity,speedSmoothTime);
		velocity3D = postProcessedMoveSpeed * velocityDir;
	}
	//Motion
	public void GenericMotion()
	{
		UpdateInput();
        FacingInputDir_Stay_AlignCamaraDir_Smooth();
		UpdateVelocityDir_WithFacingDir();
		PostProcessed_MoveSpeed_Velocity();
	}
	public void GenericMotion_FixedFacing()
	{
		UpdateInput();
		UpdateVelocityDir_WithInputDir();
		PostProcessed_MoveSpeed_Velocity();
	}
		
	#endregion
	//=======================================
	//-----------Camera-----------------------
	//=======================================
	#region Camera
	Transform cameraT;
	public void InitCamera()
	{
		cameraT = Camera.main.transform;
	}
	#endregion
	//=======================================
	//-----------Animation-------------------
	//=======================================
	#region Animation
	Animator animator;
    [HideInInspector]
    public bool onJump;
	public void InitAnimation()
	{
		animator = GetComponent<Animator>();
	}
	void MoveAnimation()
	{
		float animationSpeedPercent = ((isRunning)?1:.5f) * moveInput.magnitude;
		animator.SetFloat("speedPercent", animationSpeedPercent);
	}
	void MoveAnimationSmooth()
	{
        
		float animationSpeedPercent = ((isRunning)?1:.5f) * moveInput.magnitude;
       // Debug.Log(animationSpeedPercent);
        animator.SetFloat("speedPercent",animationSpeedPercent,speedSmoothTime,Time.deltaTime);
	}

    void RGJumpAnimation()
    {
        if (onJump)
        {
            animator.SetBool("jump", true);
        }
        else if (!onJump)
        {
            animator.SetBool("jump", false);
        }
        
    }
    public void PunchAnimation()
    {
       // Debug.Log("Punch George");
    }
    #endregion
    //=======================================
    //------------RigidBodyController--------
    //=======================================
    #region RigidBodyController
    Rigidbody rg;
	public GameObject feetObj;
	Feet feet;
	float jumpTime = 0.0f;
    public float jumpPreAnimationTime = 0.5f;
	public float JumpCD =1;
	public void InitRigidBody()
	{
		rg = GetComponent<Rigidbody>();
		feet = feetObj.GetComponent<Feet>();
	}
	//Call this function inside FixedUpdate
	public void UpdateRigidBodyController()
	{	//Velocity3D proccessed by Generic Motion
        MoveAnimationSmooth();
        RGJumpAnimation();

        //Whatever other animation
        rg.MovePosition(rg.position + velocity3D * Time.fixedDeltaTime);
		RGJump();
	}
	void RGJump()
	{
		if(Input.GetButton("Jump"))
		{
			if(jumpTime	> Time.time)
			{
				return;
			}

			if(feet.isGround && !onJump){
                StartCoroutine(JumpForceDelay());
                StartCoroutine(ResetJumpStatus());
			}
		}

	}

    IEnumerator JumpForceDelay()
    {
        onJump = true;
        yield return new  WaitForSeconds(jumpPreAnimationTime);
        rg.AddForce(Vector3.up * rgJumpForce);
        jumpTime = Time.time + JumpCD; 
    }
    IEnumerator ResetJumpStatus()
    {
        yield return new WaitForSeconds(JumpCD);
        onJump = false;
    }
	#endregion
	//=======================================
	//------------CharacterController--------
	//=======================================
	#region CharacterController

	public float ccGravity = -12;
	float velocityY;

	[Range(0,1)]
	public float airControlPercent;
	CharacterController controller;

	public void InitCharacterController()
	{
		//Debug.Log("InitCharacterController");
		controller = GetComponent<CharacterController>();
	}

	public void UpdateCCGrabity()
	{
		velocityY += Time.deltaTime * ccGravity;
		velocity3D += Vector3.up * velocityY;
	}

	void FaceDirectionImmediately_AndStay(){
		UpdateInput();
		FacingInputDir();

		PostProcessed_MoveSpeed_Velocity();
		transform.Translate(velocity3D * Time.deltaTime, Space.World);
	}

	void MoveWithAnimation(){
		UpdateInput();
		FacingInputDir();
		PostProcessed_MoveSpeed_Velocity();
		MoveAnimation();
		transform.Translate(velocity3D * Time.deltaTime, Space.World);
	}

	void MoveWithAnimationSmooth(){
		UpdateInput();
		FacingInputDir();
		PostProcessed_MoveSpeed_Velocity_Smooth();
		MoveAnimationSmooth();
		transform.Translate(velocity3D * Time.deltaTime, Space.World);
	}

	public void SimpleMove()
	{
		UpdateInput();
		FacingInputDir_Stay();
		UpdateVelocityDir_WithFacingDir();
		PostProcessed_MoveSpeed_Velocity();
		//UpdateCCGrabity();
		//CCMove();
		//CCJump();
	}

	void UpdateCharacterController_Full()
	{
		UpdateInput();
		FacingInputDir();
		UpdateVelocityDir_WithFacingDir();
		PostProcessed_MoveSpeed_Velocity_Smooth();
		MoveAnimationSmooth();
		UpdateCCGrabity();
		CCMove();
		CCJump();
	}

	void CCMove()
	{
		//Debug.Log(velocity3D );
		controller.Move(velocity3D * Time.deltaTime);
		if(controller.isGrounded){
			velocityY = 0.0f;
		}
	}
		
	void JumpWithAirControl(){
		// input
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		Vector2 inputDir = input.normalized;
		bool running = Input.GetKey(KeyCode.LeftShift);

		Move (inputDir, running);

		if (Input.GetKeyDown (KeyCode.Space)) {
			CCJump();
		}
		// animator
		float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
		animator.SetFloat ("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
	}

	void Move(Vector2 inputDir, bool running) {
		if (inputDir != Vector2.zero) {
			float targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
		}

		float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

		velocityY += Time.deltaTime * ccGravity;
		Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

		controller.Move (velocity * Time.deltaTime);
		currentSpeed = new Vector2 (controller.velocity.x, controller.velocity.z).magnitude;

		if (controller.isGrounded){
			velocityY = 0;
		}

	}

	void CCJump(){
		//So is we are grounded, we can add velocity up
		if(Input.GetKeyDown(jumpKey))
		{
			if(controller.isGrounded){
				float jumpVelocity = Mathf.Sqrt(-2*ccGravity * ccJumpHeight);
				velocityY = jumpVelocity;	
			}
		}
	}

	float GetModifiedSmoothTime(float smoothTime){
		if (controller.isGrounded) {
			return smoothTime;
		}

		if (airControlPercent == 0) {
			return float.MaxValue;
		}
		return smoothTime / airControlPercent;
	}

	#endregion
	//---------------------------------------
}

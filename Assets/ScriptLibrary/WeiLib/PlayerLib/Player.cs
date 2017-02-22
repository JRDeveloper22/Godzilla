using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : LivingEntity {

    PlayerController pController;

    public GameObject pickUpHandler;
    public float pickDistance;
    public LayerMask pickUpLayer;
    public float ThrowForce = 2;
    public int playerIndex;

    // Use this for initialization
    public KeyCode pickUpKey = KeyCode.E;
    public KeyCode runningKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode SwipingKey = KeyCode.F;
    public KeyCode PunchKey = KeyCode.R;
    public KeyCode drop_kickKey = KeyCode.G;
    public string AxisUpDown;
    public string AxisLeftRight;

    //Input Information
    bool pickKeyDown = false;
    bool isRunning = false;
    bool isJumpKeyDown = false;
    bool isSwipingKeyDown = false;
    bool isPunchKeyDown = false;
    bool isDrop_kickKeyDown = false;
    Vector2 moveInput = Vector2.zero;

    public override void Start () {
        base.Start();
        pController = GetComponent<PlayerController>();
        pController.InitCamera();
        pController.InitAnimation();
        pController.InitRigidBody();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateInputInfo();
        UpdatePlayerControllerInfo();
        UpdateAdditionalAction();
    }

    private void FixedUpdate()
    {
        pController.UpdateRigidBodyController();
    }

    #region UpdateGroupes
    /// <summary>
    ///     Update all nesseccery Input information inside Player class. and save those information
    /// so PlayerController skillAngent or whatever all can chek Input In side Player class. Then we can
    /// deal with more Input combal. Also can find out the Input conflict very eazily
    /// </summary>
    void UpdateInputInfo()
    {
        pickKeyDown = Input.GetKeyDown(pickUpKey);
        isRunning = Input.GetKey(runningKey);
        isJumpKeyDown = Input.GetKeyDown(jumpKey);
        isSwipingKeyDown = Input.GetKeyDown(SwipingKey);
        isPunchKeyDown = Input.GetKeyDown(PunchKey);
        isDrop_kickKeyDown = Input.GetKeyDown(drop_kickKey);
        moveInput.x = Input.GetAxisRaw(AxisLeftRight);
        moveInput.y = Input.GetAxisRaw(AxisUpDown);
    }

    void UpdatePlayerControllerInfo()
    {
        pController.isRunning = isRunning;
        pController.moveInput = moveInput;
        pController.jumpKeyDown = isJumpKeyDown;
        pController.swipingKeyDown = isSwipingKeyDown;
        pController.punchKeyDown = isPunchKeyDown;
        pController.drop_kickKeyDown = isDrop_kickKeyDown;
        pController.UpdateAnimation();
        pController.GenericMotion();
        //pController.UpdateRigidBodyController(); //we should put Physics part in FixedUpdate
    }

    void UpdateAdditionalAction()
    {
        if (pickKeyDown)
        {
            PickTheBuilding();
        }
        if (pController.throwBuilding)
        {
            if (pickUpHandler.transform.childCount > 0)
            {
                ThrowBuilding(); 
            }
            pController.throwBuilding = false;
        }
    }

    #endregion

    #region AdditionActionFunction

    void PickTheBuilding()
    {
       Collider[] cs =  Physics.OverlapSphere(transform.position, pickDistance, pickUpLayer);

        if (cs.Length > 0)
        {
            if (cs[0].GetComponent<Rigidbody>()) { Destroy(cs[0].GetComponent<Rigidbody>()); }
            cs[0].transform.parent = pickUpHandler.transform;
            cs[0].transform.GetComponent<BuildingHealth>().bePicked = true;

            cs[0].transform.GetComponent<BuildingHealth>().holderPlayerIndex = playerIndex;
            if(playerIndex == 0)
                cs[0].transform.GetComponent<BuildingHealth>().otherplayer = 1;
            else if(playerIndex == 1)
                cs[0].transform.GetComponent<BuildingHealth>().otherplayer = 0;
        }

    }

    void ThrowBuilding()
    {
        GameObject pickUpHolder = pickUpHandler.transform.GetChild(0).gameObject;
        if (pickUpHolder != null)
        {
            pickUpHolder.transform.parent = null;
            Rigidbody rPickup;
            if (!pickUpHolder.GetComponent<Rigidbody>())
            {
                rPickup = pickUpHolder.transform.gameObject.AddComponent<Rigidbody>();
                rPickup.useGravity = true;
            }
            else {
                rPickup = pickUpHolder.transform.GetComponent<Rigidbody>();
            }

            pickUpHolder.transform.GetComponent<BuildingHealth>().BeThrowed();
            rPickup.AddForce(transform.forward * ThrowForce);   
        }
    }

    #endregion
}

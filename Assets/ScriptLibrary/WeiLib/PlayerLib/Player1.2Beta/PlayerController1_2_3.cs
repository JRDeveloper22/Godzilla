﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Test1_2
{
    //This partial class is mainly to deal with IK system;
    public partial class PlayerController1_2
    {        
        IKObject currentIKTarget;
        //Hand IK parts
        //==========================================
        public GameObject leftHandIKTargetTF;
		public GameObject rightHandIKTargetTK;

		public GameObject pb;

        [HideInInspector]
        public float lefthandPositionWeight = 1;
        [HideInInspector]
        public float rightHandPositionWeight = 1;
        [HideInInspector]
        public float leftHandRotationWeight = 1;
        [HideInInspector]
        public float rightHandRotationWeigt = 1;

        public bool leftHandIK = false;
        public bool rightHandIK = false;

        public bool isPickKeyDown = false;
        //=============================================
        //Hip IK parts
        //=============================================

        //=============================================
        //Shouder IK parts
        //Transform rightShouderTransform;
        //Transform leftShoderTRansform;
        public NavMeshAgent naveMeshAngent;
        Player1_2 player;
        Transform enemyTF;
        void Start3()
        {
            player = GetComponent<Player1_2>();
            //rightShouderTransform = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
            //leftShoderTRansform = animator.GetBoneTransform(HumanBodyBones.LeftShoulder);
            naveMeshAngent = GetComponent<NavMeshAgent>();
            naveMeshAngent.speed = walkSpeed;

            if (player.tag == "Player1")
            {
                enemyTF = GameObject.FindGameObjectWithTag("Player2").transform;
            }
            else
            {
                enemyTF = GameObject.FindGameObjectWithTag("Player1").transform;
            }
            
        }


        void FixedUpdate3()
        {
            //fieldOfView.DebugDrawFielOfView();
            GetAllIKObjectInRange();

            if (inCheckRangeIKObjs.Count > 0)
            {
                UpdateinRangeIkObjs();
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (leftHandIK)
            {
				
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, lefthandPositionWeight);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTargetTF.transform.position);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandRotationWeight);
				animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTargetTF.transform.rotation);
            }
            if (rightHandIK)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandPositionWeight);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKTargetTK.transform.position);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandRotationWeigt);
				animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandIKTargetTK.transform.rotation);
            }  
        }

        float nexTimePressPickKey = 0.0f;
        void UpdateAnimationSmooth_IK() //call this method from PlayerController1_2_2.
        {  
            if (isPickKeyDown && !blockMovementInput)
            {
                if (nexTimePressPickKey < Time.time)
                {
                    nexTimePressPickKey = Time.time + 0.5f;
                    if (currentIKTarget == null)
                    {
                        GetClosetIKObject();
                        StartPickIKTarget();
                    }
                    else
                    {
                        animator.SetBool("Special", true);
                        animator.SetInteger("specialType", 2);
                        Invoke("Throw", 0.5f);
                    }
                }
            }
        }

        void Throw()
        {
            player.PickedUp = false;
            Rigidbody rg = currentIKTarget.gameObject.GetComponent<Rigidbody>();
            if (rg == null)
            {
                rg = currentIKTarget.gameObject.AddComponent<Rigidbody>();
                if (fieldOfView.IfObjectInFieldOfViewAngle(enemyTF.position))
                {
                    Vector3 enemyDir = (enemyTF.position - transform.position).normalized;
                    rg.AddForce(enemyDir * 3000);
                }
                else
                {
                    rg.AddForce(hipTransform.forward * 3000);
                }
                
            }
            else
            {
                rg.velocity = Vector3.zero;
                if (fieldOfView.IfObjectInFieldOfViewAngle(enemyTF.position))
                {
                    Vector3 enemyDir = (enemyTF.position - transform.position).normalized;
                    rg.AddForce(enemyDir * 3000);
                }
                else
                {
                    rg.AddForce(hipTransform.forward * 3000);
                }
                
            }

            currentIKTarget.updateDel -= currentIKTarget.FollowTarget;
            currentIKTarget.StartCollisionCheck(player.tag);
            currentIKTarget = null;
            leftHandIK = false;
            rightHandIK = false;
            leftHandIKTargetTF.transform.parent = null;
            rightHandIKTargetTK.transform.parent = null;
        }

        /// <summary>
        ///     When we start PickIKTarget. We use naveMeshAngent to auto move to the IKTarget
        /// and also add the all necessary animation to the Update delegate.
        /// </summary>
        void StartPickIKTarget()
        {
            if(currentIKTarget == null) { return; }

            blockMovementInput = true;
            blockAttackAnimation = true;

            naveMeshAngent.enabled = true;
            naveMeshAngent.SetDestination(currentIKTarget.closetPointToPlayer);

            updateDel -= OnMoveToTarget;
            updateDel += OnMoveToTarget;
        }

        /// <summary>
        ///     Add this function the update delegate. As the player auto move the a target. we auto set the move animation.
        /// when we get close enough to the IK Object. we auto player the pick object animation.
        /// </summary>
        void OnMoveToTarget()
        {
            animator.SetFloat("speedPercent", 0.5f);
            if (naveMeshAngent.remainingDistance <= 2f)
            {
                animator.SetBool("Special", true);
                naveMeshAngent.enabled = false;
                updateDel -= OnMoveToTarget;
                blockMovementInput = false;
                blockAttackAnimation = false;
            }
        }

        /// <summary>
        ///     When there are several IKObject in field of View. We only get the cloest IKObject
        /// </summary>
        void GetClosetIKObject()
        {
            if (inCheckRangeIKObjs.Count <= 0) { return; }
            float minDist = float.MaxValue;
            IKObject closetIkObj = inCheckRangeIKObjs[0];
            foreach (IKObject i in inCheckRangeIKObjs)
            {
                i.closetPointToPlayer = i.Bounds.ClosestPoint(transform.position);
                float dst = (transform.position - i.closetPointToPlayer).magnitude;
                if(dst < minDist) { minDist = dst; closetIkObj = i; i.dstToPlayer = dst; }
            }
            closetIkObj.TintColor(Color.cyan);
            currentIKTarget = closetIkObj;
        }

        public Transform LeftHandBoneTF
        {
            get { return (animator.GetBoneTransform(HumanBodyBones.LeftHand)); }
        }
        public Transform RightHandBoneTF
        {
            get { return (animator.GetBoneTransform(HumanBodyBones.RightHand)); }
        }

        public void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 50, 20), "Stop"))
            {
                animator.speed = 0.0f;
            }
            if (GUI.Button(new Rect(10, 35, 50, 20), "Play"))
            {
                animator.speed = 1.0f;
            }
        }

        #region IKObjects
        float nexTimeToCheckIKCollider = 0.0f;
        List<IKObject> inCheckRangeIKObjs = new List<IKObject>();
        public AiUtility.FieldOfView fieldOfView;

        [Range(0.1f,1.0f)]
        public float updateTime = 0.5f;
        //Update this function every 2 second, to improve the performance.
        void GetAllIKObjectInRange()
        {  
            if (Time.time < nexTimeToCheckIKCollider) { return; }
            nexTimeToCheckIKCollider = Time.time + updateTime;
            IKObject.SetTintColor(Color.red);
            fieldOfView.GetAllColliderInsideFieldOfView<IKObject>(ref inCheckRangeIKObjs, TintIKObjectColor,true);//this call back function will only be called when a new object add in to the List.
        }
        void TintIKObjectColor()
        {
            inCheckRangeIKObjs[inCheckRangeIKObjs.Count-1].TintColor();
        }
        /// <summary>
        /// This should be check every 0.1 seconds, to improve the peformance.
        ///     Check if the IKObject in inCheckRangeIKObjs update out off range. If it is, we than will remove
        /// it from inCheckRangeIKObjs.
        /// </summary>
        float nextTimeToUpdateIKObjs = 0.0f;
        void UpdateinRangeIkObjs()
        {     
            if (Time.time < nextTimeToUpdateIKObjs) { return; } 
            nextTimeToUpdateIKObjs = Time.time + 0.1f;
            //Debug.Log(inCheckRangeIKObjs.Count);
            for (int i = inCheckRangeIKObjs.Count - 1; i >= 0; i--)
            {//iterate backwards by index, removing matching items
                if (inCheckRangeIKObjs[i] == null) { inCheckRangeIKObjs.RemoveAt(i); continue; }
                if (!fieldOfView.IfObjectInFieldOfView(inCheckRangeIKObjs[i],true))
                {
                    inCheckRangeIKObjs[i].TintColor(Color.white);
                    inCheckRangeIKObjs.RemoveAt(i);
                }
            }
        }
        #endregion

        #region AnimationCallBack
        /// <summary>
        ///     this is a call back function. 
        /// We call it when the pickUp animation exit 
        /// </summary>
        public void GetPickUpBuilding()
        {
			if (currentIKTarget == null) return;
            currentIKTarget.SetAnimatorTargetTF(transform);

			if (leftHandIKTargetTF == null) {
				leftHandIKTargetTF = GameObject.Instantiate(pb);
			}

			if (rightHandIKTargetTK == null) {
				rightHandIKTargetTK = GameObject.Instantiate(pb);
			}

            leftHandIKTargetTF.transform.position = currentIKTarget.Bounds.ClosestPoint(LeftHandBoneTF.position);
            rightHandIKTargetTK.transform.position = currentIKTarget.Bounds.ClosestPoint(RightHandBoneTF.position);
            
            leftHandIKTargetTF.transform.SetParent(currentIKTarget.transform);
            rightHandIKTargetTK.transform.SetParent(currentIKTarget.transform);

            currentIKTarget.updateDel -= currentIKTarget.FollowTarget;
            currentIKTarget.updateDel += currentIKTarget.FollowTarget;

            leftHandIK = true;
            rightHandIK = true;
        }
        #endregion

        public Vector3 originaPos;
        public Vector3 forwardEndPos;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, fieldOfView.viewDistance);

            Gizmos.color = Color.grey;
            Gizmos.DrawSphere(originaPos, 0.1f);

            Gizmos.color = Color.black;
            Gizmos.DrawSphere(forwardEndPos, 0.1f);

            Gizmos.DrawLine(originaPos, forwardEndPos);
        }
    }
}
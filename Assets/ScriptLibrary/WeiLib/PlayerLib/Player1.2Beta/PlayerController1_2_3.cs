﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test1_2
{
    //This partial class is mainly to deal with IK system;
    public partial class PlayerController1_2
    {   
        //Hand IK parts
        //==========================================
        public Transform leftHandIKTargetTF;
        public Transform rightHandIKTargetTK;

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
        //=============================================
        //Hip IK parts
        //=============================================
        float hipMaxHeight = 0.0f;
        float distToIKObject = 0.0f;
        //=============================================
        //Shouder IK parts
        Transform rightShouderTransform;
        Transform leftShoderTRansform;

        void Start3()
        {
            hipMaxHeight = hipTransform.transform.localPosition.y * transform.localScale.y;
            rightShouderTransform = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
            leftShoderTRansform = animator.GetBoneTransform(HumanBodyBones.LeftShoulder);
        }

        void FixedUpdate3()
        {
            fieldOfView.DebugDrawFielOfView();
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
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTargetTF.position);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandRotationWeight);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTargetTF.rotation);
            }
            if(rightHandIK){
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandPositionWeight);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKTargetTK.position);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandRotationWeigt);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandIKTargetTK.rotation);
            }
            

        }

        public void CaculateDisToTargetObj()
        {

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
            fieldOfView.GetAllColliderInsideFieldOfView<IKObject>(ref inCheckRangeIKObjs,true);
            foreach (IKObject ioc in inCheckRangeIKObjs)
            {
                ioc.TintColor(Color.red);
            }
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
                if (!fieldOfView.IfObjectInFieldOfView(inCheckRangeIKObjs[i]))
                {
                    inCheckRangeIKObjs[i].TintColor(Color.white);
                    inCheckRangeIKObjs.RemoveAt(i);
                }
            }
        }
        #endregion
        public Vector3 originaPos;
        public Vector3 forwardEndPos;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 3f);

            Gizmos.color = Color.grey;
            Gizmos.DrawSphere(originaPos, 0.1f);

            Gizmos.color = Color.black;
            Gizmos.DrawSphere(forwardEndPos, 0.1f);

            Gizmos.DrawLine(originaPos, forwardEndPos);
        }
    }
}
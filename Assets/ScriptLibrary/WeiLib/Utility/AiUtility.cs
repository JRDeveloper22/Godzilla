using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AiUtility{

    //Dont use Serializable proporty and Constructor at the same time
    //Because when we use Constructor. We actually create a instance of eye Transform.
    //the eye Transform will not be update with the original Transform
    [System.Serializable]
    public class FieldOfView
    {
        public Transform eye;
        public float viewDistance;
        public float viewAngle;
        /*
        public FieldOfView(float _distance,float _angle) {
            //eye = _eye; //dont assign it in constructor. it will create a instance of eye.
            viewDistance = _distance;
            viewAngle = _angle;
        }*/

        public T GetNearestObjInsideFieldOfView<T>() where T : MonoBehaviour {
            List<T> allObjectInrange = new List<T>();
            List<T> allObjectInView = new List<T>();

            //Find all Object in the Radius range
            AiFind.Finds<T>(eye.position, viewDistance,ref allObjectInrange);

            //find all Object in View
            foreach (T t in allObjectInrange) {
                if (Vector3.Angle(eye.forward, t.transform.position - eye.position) < viewAngle / 2.0f)
                {
                    allObjectInView.Add(t);
                }
            }

            float miniDis = float.MaxValue;
            T nearestObj = null;

            foreach (T obj in allObjectInView)
            {
                float dist = (obj.transform.position - eye.position).magnitude;
                if (dist < miniDis) {
                    miniDis = dist;
                    nearestObj = obj;
                }
            }

            return nearestObj;

        }

        public List<Collider> GetAllColliderInsideFieldOfView()
        {
            List<Collider> allColliderInView = new List<Collider>();

            //get all Collider in a sphere
            Collider[] allColliders =  Physics.OverlapSphere(eye.position, viewDistance);

            //get all Collider in View
            foreach (Collider c in allColliders) {
                if (Vector3.Angle(eye.forward, c.transform.position - eye.position) < viewAngle / 2) {
                    allColliderInView.Add(c);
                }
            }
            return allColliderInView;
        }

        public List<Collider> GetAllColliderInsideFieldOfView(LayerMask layerMask)
        {
            List<Collider> allColliderInView = new List<Collider>();

            //get all Collider in a sphere
            Collider[] allColliders = Physics.OverlapSphere(eye.position, viewDistance,layerMask);

            //get all Collider in View
            foreach (Collider c in allColliders)
            {
                if (Vector3.Angle(eye.forward, c.transform.position - eye.position) < viewAngle / 2)
                {
                    allColliderInView.Add(c);
                }
            }
            return allColliderInView;
        }

        public void GetAllColliderInsideFieldOfView<T>(ref List<T> allColliderInView,bool boundsRaycast = false) where T : MonoBehaviour
        {
            //get all Collider in a sphere
            Collider[] allColliders = Physics.OverlapSphere(eye.position, viewDistance);

            //get all Collider in View
            foreach (Collider c in allColliders)
            {
                T t = c.GetComponent<T>();
                if (t)
                {
                    if (boundsRaycast)
                    {
                        Ibounds b = c.GetComponent<Ibounds>();
                        
                        Vector3[] vertices = b.Vertices;
                        float boundsHeight_1 = vertices[0].y - 0.1f;    //get bounds height
                        Vector3 orginalPos_fixed = new Vector3(eye.position.x, boundsHeight_1, eye.position.z);

                        Vector3 forwardUnnormaledDir = eye.forward * viewDistance + eye.position;
                        Vector3 forwardUnnormaledDir_Fixed = new Vector3(forwardUnnormaledDir.x, boundsHeight_1, forwardUnnormaledDir.z) - orginalPos_fixed;


                        //staticOrigin = orginalPos_fixed;
                        //staticForward = forwardUnnormaledDir_Fixed;

                        RaycastHit hitinfo;
                        // check if we can directly get raycast from forward direction  
                        if (Physics.Raycast(orginalPos_fixed, forwardUnnormaledDir_Fixed,out hitinfo, viewDistance))
                        {
                            if (hitinfo.collider.gameObject == c.gameObject)
                            {
                                if (!allColliderInView.Contains(t))
                                {
                                    allColliderInView.Add(t);
                                }
                            }
                        }
                        else
                        {
                            //check if any vertices of the bounds inside view angle. if there is, we raycast the tow view edge
                            //if hit the collider. means we can see the bounds
                            foreach (Vector3 v in vertices)
                            {
                                //Check if corner inside the view angle
                                if (Vector3.Angle(forwardUnnormaledDir_Fixed, v - orginalPos_fixed) < viewAngle / 2.0f)
                                {
                                    //Debug.Log(Vector3.Angle(forwardPos_Fixed, v - orginalPos_fixed));
                                    Vector3 forwardEdge_fixed1 = Quaternion.AngleAxis(-viewAngle / 2.0f, Vector3.up) * forwardUnnormaledDir_Fixed;
                                    Vector3 forwardEdge_fixed2 = Quaternion.AngleAxis(viewAngle / 2.0f, Vector3.up) * forwardUnnormaledDir_Fixed;

                                    if (Physics.Raycast(orginalPos_fixed,forwardEdge_fixed1,viewDistance) || Physics.Raycast(orginalPos_fixed, forwardEdge_fixed2, viewDistance))
                                    {
                                        if (!allColliderInView.Contains(t))
                                            allColliderInView.Add(t);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    //Dont use bounds raycast
                    else if (Vector3.Angle(eye.forward, c.transform.position - eye.position) < viewAngle / 2)
                    {
                        allColliderInView.Add(t);
                    }
                }
            }  
        }

        public Collider GetNearestColliderInsideFieldOfView() {

            List<Collider> allColliderInView = GetAllColliderInsideFieldOfView();

            //Get Check the nearest collider
            Collider nearestCollider = null;
            float nearestDis = float.MaxValue;
            foreach (Collider collider in allColliderInView)
            {
                float dist = (collider.transform.position - eye.position).magnitude;
                if (dist < nearestDis)
                {
                    nearestDis = dist;
                    nearestCollider = collider;
                }
            }
            return nearestCollider;
        }

        public Collider GetNearestColliderInsideFieldOfView(LayerMask layerMask)
        {
            List<Collider> allColliderInView = GetAllColliderInsideFieldOfView(layerMask);

            //Get Check the nearest collider
            Collider nearestCollider = null;
            float nearestDis = float.MaxValue;
            foreach (Collider collider in allColliderInView)
            {
                float dist = (collider.transform.position - eye.position).magnitude;
                if (dist < nearestDis)
                {
                    nearestDis = dist;
                    nearestCollider = collider;
                }
            }
            return nearestCollider;
        }

        public Collider GetFirstColliderInsideFieldOfView()
        {

            //get all Collider in a sphere
            Collider[] allColliders = Physics.OverlapSphere(eye.position, viewDistance);

            //get all Collider in View
            foreach (Collider c in allColliders)
            {
                if (Vector3.Angle(eye.forward, c.transform.position - eye.position) < viewAngle / 2)
                {
                    return c; //Get the first in FieldOf View
                }
            }
            return null;
        }

        public Collider GetFirstColliderInsideFieldOfView(LayerMask layerMask)
        {
            //get all Collider in a sphere
            Collider[] allColliders = Physics.OverlapSphere(eye.position, viewDistance, layerMask);

            //get all Collider in View
            foreach (Collider c in allColliders)
            {
                if (Vector3.Angle(eye.forward, c.transform.position - eye.position) < viewAngle / 2)
                {
                    return c;
                }
            }
            return null;
        }

        public bool GetFirstColliderCanSeeInFieldOfView(LayerMask layerMask, string tag, ref Collider collider)
        {
            //get all Collider in a sphere
            Collider[] allColliders = Physics.OverlapSphere(eye.position, viewDistance, layerMask);

            foreach (Collider c in allColliders)
            {
                Vector3 dirToTarget = (c.transform.position - eye.position).normalized;
                //Check if it is Indide Field Of View
                if (Vector3.Angle(eye.forward, c.transform.position - eye.position) < viewAngle / 2)
                {
                    //Check if we can see it Directly.
                    RaycastHit hit;
                    if (Physics.Raycast(eye.transform.position, dirToTarget, out hit, viewDistance))
                    {
                        if (hit.collider.tag == tag)
                        {
                            collider = c;
                            return true;
                        }
                    }
                }
            }
            collider = null;
            return false;
        }

        //need to fix
        public bool IfObjectInFieldOfView(Ibounds ib)
        {
            Vector3[] vertices = ib.Vertices;
            float boundsHeight_1 = vertices[0].y - 0.1f;    //get bounds height
            Vector3 orginalPos_fixed = new Vector3(eye.position.x, boundsHeight_1, eye.position.z);

            Vector3 forwardPos = eye.forward * viewDistance + eye.position;
            Vector3 forwardPos_Fixed = new Vector3(forwardPos.x, boundsHeight_1, forwardPos.z) - orginalPos_fixed;

            // check if we can directly get raycast from forward direction  
            if (Physics.Raycast(orginalPos_fixed, forwardPos_Fixed, viewDistance))
            {
                return true;
            }else {
                //check if any vertices of the bounds inside view angle. if there is, we raycast the tow view edge
                //if hit the collider. means we can see the bounds
                foreach (Vector3 v in vertices)
                {
                    //Check if corner inside the view angle
                    if (Vector3.Angle(forwardPos_Fixed, v - orginalPos_fixed) < viewAngle / 2.0f)
                    {
                        Vector3 forwardEdge_fixed1 = Quaternion.AngleAxis(-viewAngle / 2.0f, Vector3.up) * forwardPos_Fixed;
                        Vector3 forwardEdge_fixed2 = Quaternion.AngleAxis(viewAngle / 2.0f, Vector3.up) * forwardPos_Fixed;

                        if (Physics.Raycast(orginalPos_fixed, forwardEdge_fixed1, viewDistance) || Physics.Raycast(orginalPos_fixed, forwardEdge_fixed2, viewDistance))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;        
        }

        static Vector3 staticOrigin;
        static Vector3 staticForward;

        public void DebugDrawFielOfView()
        {
            Vector3 forwardPos = eye.forward * viewDistance; 
            Vector3 pos1 = Quaternion.AngleAxis(-viewAngle/2.0f, Vector3.up) * forwardPos;
            Vector3 pos2 = Quaternion.AngleAxis(viewAngle/2.0f, Vector3.up) * forwardPos;

            Debug.DrawRay(eye.position, forwardPos);
            Debug.DrawRay(eye.position, pos1);
            Debug.DrawRay(eye.position, pos2);
            //Debug.Log(eye.position);

            //Debug.DrawRay(staticOrigin, staticForward, Color.red);
        }
    }


    //Static Filed........
    public static class AiFind
    {
        //Find the nearest T; center is a given position;
        public static T FindNearestObj<T>(Vector3 position) where T : MonoBehaviour
        {

            float minDistance = float.MaxValue;

            T retObj = null;
            foreach (T obj in Object.FindObjectsOfType(typeof(T)))
            {
                //Debug.Log(obj);
                float dist = (obj.gameObject.transform.position - position).magnitude;
                if (dist < minDistance)
                {
                    minDistance = dist;
                    retObj = obj;
                }
            }
            return retObj;
        }

        //Find the nearest T which inside the radius of range; center is a given position;
        public static T FindNearestObj<T>(Vector3 position, float range) where T : MonoBehaviour
        {

            float minDistance = float.MaxValue;

            T retObj = null;
            foreach (T obj in Object.FindObjectsOfType(typeof(T)))
            {
                //Debug.Log(obj);
                float dist = (obj.gameObject.transform.position - position).magnitude;
                if (dist < minDistance)
                {
                    minDistance = dist;
                    retObj = obj;
                }
            }

            //Check the range
            if ((retObj.gameObject.transform.position - position).magnitude <= range)
            {
                return retObj;
            }
            else
            {
                return null;
            }
        }

        public static Object[] Finds<T>() where T : MonoBehaviour
        {
           return Object.FindObjectsOfType(typeof(T));
        }

        //Find all T Objects inside the raduis range; center is a given position. and out put to the out_objs. Return all object inside the Scene
        public static Object[] Finds<T>(Vector3 position, float range, ref List<T> out_objs) where T : MonoBehaviour
        {
            Object[] objs = Object.FindObjectsOfType(typeof(T));
            foreach (T obj in objs)
            {
                if ((obj.transform.position - position).magnitude <= range)
                {
                    out_objs.Add(obj);
                }
            }
            return objs;
        }

        //Find all Collider with LayerMask of targetMask inside the raduis range; center is a given position
        public static Collider[] FindCollidersOverlapSphere(Vector3 position, float range)
        {
            Collider[] targetsInRangeRadius = Physics.OverlapSphere(position, range); //gameobject with targetMask will be selected
            return targetsInRangeRadius;
        }

        //Find all Collider with LayerMask of targetMask inside the raduis range; center is a given position
        public static Collider[] FindCollidersOverlapSphere(Vector3 position, float range, LayerMask targetMask)
        {
            Collider[] targetsInRangeRadius = Physics.OverlapSphere(position, range, targetMask); //gameobject with targetMask will be selected
            return targetsInRangeRadius;
        }

        //Find the nearest Collider with LayerMask of targetMask inside the raduis range; center is a given position
        public static Collider FindNearestColliderOverlapSphere(Vector3 position, float range)
        {

            Collider[] targetsInRangeRadius = Physics.OverlapSphere(position, range); //gameobject with targetMask will be selected

            Collider nearestCollider = null;
            float nearestDis = float.MaxValue;

            foreach (Collider collider in targetsInRangeRadius)
            {
                float dist = (collider.transform.position - position).magnitude;
                if (dist < nearestDis)
                {
                    nearestDis = dist;
                    nearestCollider = collider;
                }
            }
            return nearestCollider;
        }

        //Find the nearest Collider with LayerMask of targetMask inside the raduis range; center is a given position
        public static Collider FindNearestColliderOverlapSphere(Vector3 position, float range, LayerMask targetMask)
        {

            Collider[] targetsInRangeRadius = Physics.OverlapSphere(position, range, targetMask); //gameobject with targetMask will be selected

            Collider nearestCollider = null;
            float nearestDis = float.MaxValue;

            foreach (Collider collider in targetsInRangeRadius)
            {
                float dist = (collider.transform.position - position).magnitude;
                if (dist < nearestDis)
                {
                    nearestDis = dist;
                    nearestCollider = collider;
                }
            }
            return nearestCollider;
        }

        //Find the nearest Collider with LayerMask of targetMask inside the raduis range; center is a given position
        public static Collider FindNearestColliderOverlapSphere(Vector2 position, float range, LayerMask targetMask)
        {
            Collider[] targetsInRangeRadius = Physics.OverlapSphere(position, range, targetMask); //gameobject with targetMask will be selected

            Collider nearestCollider = null;
            float nearestDis = float.MaxValue;

            foreach (Collider collider in targetsInRangeRadius)
            {
                float dist = (new Vector2(collider.transform.position.x, collider.transform.position.z) - position).magnitude;
                if (dist < nearestDis)
                {
                    nearestDis = dist;
                    nearestCollider = collider;
                }
            }
            return nearestCollider;
        }
    }

}

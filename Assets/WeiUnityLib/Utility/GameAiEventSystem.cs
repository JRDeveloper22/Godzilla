using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SinglePlayerGame
{

    public class GameAiEventSystem : MonoBehaviour
    {

        public static GameAiEventSystem Managerinstance = null;

        //Make a singleTon
        void Awake()
        {
            if (Managerinstance != null)
            {
                Destroy(this);
            }
            Managerinstance = this;
        }


        public static List<GameObject> enemys = new List<GameObject>();

        void Start() {
          GameObject[] enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject e in enemiesArray) {
                enemys.Add(e);
            }
        }

        public static Vector3 GetPlayerPosition()
        {
            return Vector3.zero;
        }

        public static Vector3 GetDirectionToPlayer3D(Vector3 ownPosition) {
            return Vector3.Normalize(GetPlayerPosition() - ownPosition);
        }

        public static float DistanceToPlayer3D(Vector3 ownPosition) {
            return Vector3.Magnitude(GetPlayerPosition() - ownPosition);
        }

        public static List<GameObject> GetTheEnemys() {
            return enemys;
        }

        public static void AllEnemiesInRadiusGoesTo(Vector3 origin ,Vector3 dst,float Radius) {
            foreach (GameObject e in enemys) {
                if (Vector3.Magnitude(e.transform.position - origin) <= Radius)
                {
                    e.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = dst;
                }
            }
        }

        public static void SpawnEnemies(Vector3 origin,float radius,int number) {
            for (int i = 0; i < number; i++)
            {
              
            }

        }
    }
}

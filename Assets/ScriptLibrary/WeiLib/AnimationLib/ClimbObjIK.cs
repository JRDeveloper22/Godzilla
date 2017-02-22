using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbObjIK : MonoBehaviour {
    public Collider c;

    [HideInInspector]
    public Bounds bounds;
    float topY;

    public Vector3[] vertices = new Vector3[4];

	void Start () {
        bounds = c.bounds;
        vertices[0] = bounds.center + new Vector3(bounds.size.x,bounds.size.y,bounds.size.z)/2;
        vertices[1] = bounds.center + new Vector3(-bounds.size.x, bounds.size.y, bounds.size.z) / 2;
        vertices[2] = bounds.center + new Vector3(-bounds.size.x, bounds.size.y, -bounds.size.z) / 2;
        vertices[3] = bounds.center + new Vector3(bounds.size.x, bounds.size.y, -bounds.size.z) / 2;
        topY = transform.position.y + bounds.size.y / 2;
    }
  
    public int GetClosestVertex(Vector3 pos)
    {
        int index = 0;
        float minDst = float.MaxValue;

        for(int i = 0;i <vertices.Length;i++)
        {
            float dst = (vertices[i] - pos).magnitude;
            if (dst< minDst) {
                index = i;
                minDst = dst;
            }
        }
        return index;
    }

    //Used for Debug.
    public void DrawReachableToEdge(Vector3 pos)
    {
        int i = GetClosestVertex(pos);
        Debug.DrawLine(vertices[i],vertices[(i + 1) % 4], Color.green);
        if (i == 0)
            Debug.DrawLine(vertices[i], vertices[3], Color.green);
        else
            Debug.DrawLine(vertices[i], vertices[i - 1], Color.green);

        Debug.DrawLine(pos, vertices[i], Color.green);
        
        Vector3 projectionPoint = bounds.ClosestPoint(pos);
        Debug.DrawLine(pos,new Vector3(projectionPoint.x, topY,projectionPoint.z), Color.red);
    }

    public Vector3 GetClosestPointFromTopEdge(Vector3 pos)
    {
        Vector3 projectionPoint = bounds.ClosestPoint(pos);
        return new Vector3(projectionPoint.x, topY, projectionPoint.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(vertices[0], 0.1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(vertices[1], 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(vertices[2], 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(vertices[3], 0.1f);

    }
}

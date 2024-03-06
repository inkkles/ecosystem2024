using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    //thanks sebastian lauge for his field of view tutorial very helpful

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<GameObject> visibleCreatuers = new List<GameObject>();

    private void Start()
    {
        StartCoroutine("FindCreaturesWithDelay", .2f);
    }

    IEnumerator FindCreaturesWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleCreatures();
        }
    }
    public List<GameObject> FindVisibleCreatures() 
    {
        visibleCreatuers.Clear();
        List<GameObject> creaturesInView = new List<GameObject>();
        Collider[] collidersInScope = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        GameObject[] creaturesInScope = new GameObject[collidersInScope.Length];
    
        for(int i = 0; i < collidersInScope.Length; i++)
        {
            creaturesInScope[i] = collidersInScope[i].gameObject;

            Transform target = creaturesInScope[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) //is in view angle
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask)) //is not obstructed
                {
                    creaturesInView.Add(creaturesInScope[i]);
                }
            }
            
        }
        visibleCreatuers = creaturesInView;
        return visibleCreatuers;
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    public static Vector3 DirFromAngle(Transform t, float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += t.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

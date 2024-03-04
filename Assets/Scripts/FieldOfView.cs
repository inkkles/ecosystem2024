using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    //thanks sebastian lauge for his field of view tutorial
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleCreatuers = new List<Transform>();

    private void Start()
    {
        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleCreatures();
        }
    }
    public List<Transform> FindVisibleCreatures() 
    {
        visibleCreatuers.Clear();
        List<Transform> creaturesInView = new List<Transform>();
        Collider[] creaturesInScope = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for(int i = 0; i < creaturesInScope.Length; i++)
        {
            Transform target = creaturesInScope[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) //is in view angle
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask)) //is not obstructed
                {
                    creaturesInView.Add(target);
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
}

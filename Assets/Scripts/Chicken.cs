using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.GraphicsBuffer;

public class Chicken : MonoBehaviour
{
    public float moveSpeed = 1;
    public float rotSpeed = 100f;

    public float wanderRadius = 5f;

    private Transform target;

    Rigidbody rb;
    Vector3 velocity;
    public Vector3 randomDirection = new Vector3(0, 0, 0);

    public FieldOfView fov;

    enum States
    {
        Wandering,  //
        Hungry,     //
        Fleeing,    //
        Dead        //
    }
    States state;

    // Start is called before the first frame update
    void Start()
    {
        state = States.Wandering;
        rb = GetComponent<Rigidbody>();
        StartCoroutine(SetRandomDirectionEveryFewSeconds());
    }

    IEnumerator SetRandomDirectionEveryFewSeconds()
    {
        Debug.Log("Coroutine Start");
        while (state == States.Wandering)
        {
            yield return new WaitForSeconds(Random.Range(0.2f, 3f));

            // Generate a random direction in 3D space
            Vector3 randomDirection = Random.onUnitSphere;

            // Ensure the character stays upright (only rotate around the Y-axis)
            randomDirection.y = 0f;

            // Set the character's forward direction to the random direction
            transform.forward = randomDirection;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (state) {
            case States.Wandering:
                velocity = transform.forward.normalized * moveSpeed;
                if(fov.visibleCreatuers.Count > 0)
                {
                    
                }
                break;
            case States.Hungry:

                break;
            case States.Fleeing:

                break;
            case States.Dead:

                break;
            default:
                break;
        }
    }

    private Transform getNearestCreatureInFOV()
    {
        Transform nearestSoFar = null;
        float nearestDistance = float.MaxValue;
        foreach (Transform visibleTarget in fov.visibleCreatuers)
        {
            if (nearestSoFar == null) {
                nearestSoFar = visibleTarget;
                nearestDistance = Vector3.Distance(transform.position, visibleTarget.position);
            }
            else
            {
                float currDistance = Vector3.Distance(transform.position, visibleTarget.position);
                if (currDistance < nearestDistance)
                {
                    nearestSoFar = visibleTarget;
                    nearestDistance = currDistance;
                }
            }
        }
        return nearestSoFar;
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
        Vector3 newPoint = -transform.forward + new Vector3(Random.Range(-15, 15), 0, Random.Range(-15, 15));
        transform.LookAt(newPoint);
    }


    void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

}

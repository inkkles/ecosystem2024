using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Wolf : MonoBehaviour
{
    public float moveSpeed;
    [Range(0f, 0.1f)]
    public float rotSpeed;

    private Vector3 targetAngle;

    private Transform target;
    Rigidbody rb;
    Vector3 velocity;

    public FieldOfView fov;


    enum States
    {
        Wandering,  //
        Hungry,     //
        Eating      //
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
        Debug.Log("Wolf's Wander Coroutine Start");
        while (state == States.Wandering)
        {
            yield return new WaitForSeconds(Random.Range(0.3f, 2f));

            // Generate a random direction in 3D space
            Vector3 randomDirection = Random.onUnitSphere;

            // Ensure the character stays upright (only rotate around the Y-axis)
            randomDirection.y = 0f;

            // Set the character's forward direction to the random direction
            targetAngle = randomDirection;
            Debug.Log("Wolf Wander");
        }
    }

    void FixedUpdate()
    {
        transform.forward = Vector3.Lerp(transform.forward, targetAngle, rotSpeed);
        velocity = transform.forward.normalized * moveSpeed;

        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

    }
    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case States.Wandering:
                if (fov.visibleCreatuers.Count == 0) break;
                state = States.Hungry;
                break;
            case States.Hungry:
                GameObject chicken = getNearestCreatureInFOV();
                if (chicken != null && chicken.CompareTag("Chicken"))
                {
                    //Debug.Log("Chicken Spotted!");
                    targetAngle = FieldOfView.DirFromAngle(transform, Vector3.Angle(transform.position, chicken.transform.position), false);
                }
                else
                {
                    state = States.Wandering;
                }

                break;
            case States.Eating:
                GetComponent<AudioSource>().Play(); //munch
                state = States.Wandering;

                break;
            default:
                break;
        }
    }

    private GameObject getNearestCreatureInFOV()
    {
        if (fov.visibleCreatuers.Count == 1) { return fov.visibleCreatuers[0]; }
        GameObject nearestSoFar = null;
        float nearestDistance = float.MaxValue;
        foreach (GameObject visibleTarget in fov.visibleCreatuers)
        {
            if (nearestSoFar == null)
            {
                nearestSoFar = visibleTarget;
                nearestDistance = Vector3.Distance(transform.position, visibleTarget.transform.position);
            }
            else
            {
                float currDistance = Vector3.Distance(transform.position, visibleTarget.transform.position);
                if (currDistance < nearestDistance)
                {
                    nearestSoFar = visibleTarget;
                    nearestDistance = currDistance;
                }
            }
        }
        return nearestSoFar;
    }

    //tag argument to specify what kind of creature to look for
    private GameObject getNearestCreatureInFOV(string tag)
    {

        if (fov.visibleCreatuers.Count == 1 && fov.visibleCreatuers[0].tag == tag) { return fov.visibleCreatuers[0]; }
        GameObject nearestSoFar = null;
        float nearestDistance = float.MaxValue;
        foreach (GameObject visibleTarget in fov.visibleCreatuers)
        {
            if (visibleTarget.tag != tag) continue;
            if (nearestSoFar == null)
            {
                nearestSoFar = visibleTarget;
                nearestDistance = Vector3.Distance(transform.position, visibleTarget.transform.position);
            }
            else
            {
                float currDistance = Vector3.Distance(transform.position, visibleTarget.transform.position);
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
        Debug.Log("Wolf collided with " + collision);
        if (collision.gameObject.tag == "Chicken" && state == States.Hungry)
        {
            Destroy(collision.gameObject); //eat yummy chicken
            Debug.Log("Chicken Consumed");
            state = States.Eating;
        }
        else
        {
            Debug.Log("TURN AROUND!");
            transform.forward = -targetAngle;
        }
    }
}

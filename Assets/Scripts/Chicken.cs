using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Chicken : MonoBehaviour
{
    public float moveSpeed;
    [Range(0f, 0.1f)]
    public float rotSpeed;

    private Vector3 targetAngle;

    private Transform target;
    Rigidbody rb;
    Vector3 velocity;

    public FieldOfView fov;

    public float hungerTime;
    private float hunger;

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
        hunger = hungerTime;
        state = States.Wandering;
        rb = GetComponent<Rigidbody>();
        StartCoroutine(SetRandomDirectionEveryFewSeconds());
    }

    IEnumerator SetRandomDirectionEveryFewSeconds()
    {
        Debug.Log("Coroutine Start");
        while (state == States.Wandering)
        {
            yield return new WaitForSeconds(Random.Range(0.3f, 2f));

            // Generate a random direction in 3D space
            Vector3 randomDirection = Random.onUnitSphere;

            // Ensure the character stays upright (only rotate around the Y-axis)
            randomDirection.y = 0f;

            // Set the character's forward direction to the random direction
            targetAngle = randomDirection;
            //Debug.Log("Wander");
        }
    }

    private void OnMouseOver()
    {
        Debug.Log("Hunger Level: " + hunger + "/" + hungerTime);
    }

    void FixedUpdate()
    {
        transform.forward = Vector3.Lerp(transform.forward, targetAngle, rotSpeed);
        velocity = transform.forward.normalized * moveSpeed;

        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

        if (hunger > 0) hunger--; //hunger decreases every game tick
        else state = States.Dead; //death by starvation
    }
    // Update is called once per frame
    void Update()
    {
        
        switch (state) {
            case States.Wandering:
                foreach(GameObject visibleCreature in fov.visibleCreatuers)
                {
                    if (visibleCreature.CompareTag("Wolf"))
                    {
                        state = States.Fleeing; break;
                    }
                    if(visibleCreature.CompareTag("Berry"))
                    {
                        if(isHungry()) state = States.Hungry; break;
                    }
                }
                break;
            case States.Hungry:
                GameObject berry = getNearestCreatureInFOV();
                //nearest berry is ACTUALLY A WOLF OK WE HAVE OTHER PRIORITIES
                if (berry != null && berry.CompareTag("Berry"))
                {
                    //Debug.Log("Berry Spotted!");
                    targetAngle = FieldOfView.DirFromAngle(transform, Vector3.Angle(transform.position, berry.transform.position), false);
                }

                break;
            case States.Fleeing:
                GameObject target = getNearestCreatureInFOV("Wolf");
                //we just saw a wolf!
                //squawk in fear
                if (target.CompareTag("Wolf"))
                {
                    targetAngle = FieldOfView.DirFromAngle(transform, Vector3.Angle(transform.position, target.transform.position) + 180, false);
                }
                break;
            case States.Dead:
                if(hunger <= 0)
                {
                    //die of starvation
                    Debug.Log("Starved");
                }
                else
                {
                    //die by wolf
                }
                Destroy(this.gameObject);
                break;
            default:
                break;
        }
    }

    private bool isHungry()
    {
        return hunger < hungerTime / 1.1f;
    }

    private GameObject getNearestCreatureInFOV()
    {
        if (fov.visibleCreatuers.Count == 1) { return fov.visibleCreatuers[0]; }
        GameObject nearestSoFar = null;
        float nearestDistance = float.MaxValue;
        foreach (GameObject visibleTarget in fov.visibleCreatuers)
        {
            if (nearestSoFar == null) {
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
        Debug.Log("TURN AROUND!");
        transform.forward = -targetAngle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Berry")
        {
            if(state == States.Hungry)
            {
                Destroy(other.gameObject); //eat yummy berry
                Debug.Log("Berry Consumed");
                hunger = hungerTime;
                state = States.Wandering;
            }
        }
    }



}

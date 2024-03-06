using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bush : MonoBehaviour
{

    public GameObject berry;

    [Min(0f)]
    public float radius;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StartSpawningBerries");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartSpawningBerries()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0f, 2f));
            //Debug.Log("BerrySpawned");
            spawnBerry();
        }
    }

    void spawnBerry()
    {
        Vector3 position = transform.position + (Random.insideUnitSphere * radius);
        position.y = 0;

        GameObject.Instantiate(berry, position, Quaternion.identity);
    }


}

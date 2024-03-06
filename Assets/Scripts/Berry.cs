using System.Collections;
using UnityEngine;

public class Berry : MonoBehaviour
{
    public float despawnTime = 5f; // Adjust this value to set the despawn time in seconds
    public float shrinkSpeed = 0.001f; // Adjust this value to set the shrinking speed

    private void Start()
    {
        Invoke("DespawnObject", despawnTime);
    }

    private void DespawnObject()
    {
        StartCoroutine(ShrinkAndDestroy());
    }

    private IEnumerator ShrinkAndDestroy()
    {
        while (transform.localScale.x > 0 && transform.localScale.y > 0 && transform.localScale.z > 0)
        {
            transform.localScale -= new Vector3(shrinkSpeed, shrinkSpeed, shrinkSpeed);
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenCoop : MonoBehaviour
{
    public GameObject chicken;

    private AudioSource chickenBawk;
    private void Start()
    {
        chickenBawk = GetComponent<AudioSource>();
    }
    public void spawnChicken()
    {
        GameObject.Instantiate(chicken, transform);
        chickenBawk.Play();
    }
}

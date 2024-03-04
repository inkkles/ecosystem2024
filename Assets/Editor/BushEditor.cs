using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(Bush))]
public class BushEditor : Editor
{
    void OnSceneGUI()
    {
        Bush bush = (Bush)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(bush.transform.position, Vector3.up, Vector3.forward, 360, bush.radius);

    }
}

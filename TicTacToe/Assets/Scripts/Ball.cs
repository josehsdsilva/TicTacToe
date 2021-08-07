using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Material[] materials;
    public int player;

    public void SetMaterial()
    {
        transform.GetComponent<MeshRenderer>().material = materials[player];
    }
}

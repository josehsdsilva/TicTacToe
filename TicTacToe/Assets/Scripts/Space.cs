using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
    public int x, y;

    public void UpdateName()
    {
        transform.name = x + ", " + y;
    }
}

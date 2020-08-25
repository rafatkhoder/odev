using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, 5.0f)); // rotate coin
    }
}

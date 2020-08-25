using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField]
    private float timer = 5.0f; // timer for destroy obj 
    void Start()
    {
        Destroy(this.gameObject, timer);
    }

}

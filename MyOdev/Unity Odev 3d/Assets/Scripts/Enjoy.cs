using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enjoy : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;

    private float ran1, ran2, ran3 ,ran4; // random pos number for instanie
    private int objRan;  // chose random obj
    Vector3 posRandom;
    Vector3 playerPos;
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(InstaiteObject());
    }
    private void Update()
    {
        playerPos = PlayerController.player.transform.position;
        objRan = Random.Range(0, objects.Length);
        ran1 = Random.Range(-1.5f, 1.5f);
        ran2 = Random.Range(0.3f, 0.5f);
        ran3 = Random.Range(playerPos.z+5f,playerPos.z + 15.5f);
        ran3 = Random.Range(playerPos.z+20f,playerPos.z + 30.5f);
        posRandom.x = ran1;
        posRandom.y = ran2;
        posRandom.z = ran3;
       
    }

    // creat prafab coins or or trap 
    IEnumerator InstaiteObject()
    {
        yield return new WaitForSeconds(5.1f);
        Instantiate(objects[objRan], posRandom,objects[objRan].transform.rotation);
        yield return new WaitForSeconds(5.0f);
        posRandom.z = ran4;
        Instantiate(objects[objRan], posRandom, objects[objRan].transform.rotation);
    }
}

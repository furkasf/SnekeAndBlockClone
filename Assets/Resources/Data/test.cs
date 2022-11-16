using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 100; i++)
        {
            Debug.Log(Random.Range(0, 2));
        }
    }

   
}

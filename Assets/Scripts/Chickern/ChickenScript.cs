using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenScript : MonoBehaviour
{    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
       //     GameManager.instance.CollectEgg();
            Destroy(gameObject);
        }
    }
}


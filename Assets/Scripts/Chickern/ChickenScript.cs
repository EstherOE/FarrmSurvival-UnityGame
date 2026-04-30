using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenScript : MonoBehaviour

{
    public static int collectionValue=1;
        void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.CollectEgg(collectionValue);
      
            Destroy(gameObject);
        }
    }
}


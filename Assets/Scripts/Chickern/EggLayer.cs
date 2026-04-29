using UnityEngine;

public class EggLayer : MonoBehaviour
{
    public GameObject eggPrefab;
    public float minTime = 3f;
    public float maxTime = 8f;

    void Start()
    {
        Invoke(nameof(LayEgg), Random.Range(minTime,maxTime));
    }

    void LayEgg()
    {
        Instantiate(eggPrefab, transform.position + Vector3.right, Quaternion.identity);
        Invoke(nameof(LayEgg), Random.Range(minTime,maxTime));
    }
}
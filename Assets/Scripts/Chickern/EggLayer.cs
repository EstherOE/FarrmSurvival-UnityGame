
using UnityEngine;

public class EggLayer : MonoBehaviour
{
    public GameObject eggPrefab;

    public float baseMinTime = 5f;
    public float baseMaxTime = 8f;

    public float firstPosx;
    public float secondPosx;
    public float firstPosz;
    public float secondPosz;

    void Start()
    {
        ScheduleNextEgg();
    }

    void ScheduleNextEgg()
    {
        int level = GameManager.Instance.currentLevel;

        float minTime = Mathf.Max(1f, baseMinTime - (level * 0.3f));
        float maxTime = Mathf.Max(2f, baseMaxTime - (level * 0.3f));

        Invoke(nameof(LayEgg), Random.Range(minTime, maxTime));
    }

    void LayEgg()
    {
        Vector3 offset = new Vector3(
            Random.Range(firstPosx, secondPosx),
            0,
            Random.Range(firstPosz, secondPosz)
        );

        Vector3 spawnPos = transform.position + offset;

        Instantiate(eggPrefab, spawnPos, Quaternion.identity);

        ScheduleNextEgg();
    }
}

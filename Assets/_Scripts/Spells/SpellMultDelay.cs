using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMultDelay : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    int numOfProjectiles;
    [SerializeField]
    float rotAngle = 0f;
    [SerializeField]
    float spawnDelay = 0.5f; // Adjust this to control the delay between spawns

    private void Awake()
    {
        StartCoroutine(SpawnWithDelay(numOfProjectiles));
    }

    IEnumerator SpawnWithDelay(int n)
    {
        if (n % 2 == 0) transform.Rotate(0f, 0f, -rotAngle);
        else transform.Rotate(0f, 0f, -rotAngle * 2);

        for (int i = 0; i < n; i++)
        {
            ObjectPool.SpawnObject(prefab, transform.position, transform.rotation);
            transform.Rotate(0f, 0f, rotAngle);
            yield return new WaitForSeconds(spawnDelay); // Delay between spawns
        }

        Destroy(this.gameObject);
    }
}

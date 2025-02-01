using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;
    [SerializeField] GameObject enemy3;
    [SerializeField] GameObject enemy4;
    [SerializeField] GameObject enemy5;
   
    [Header("Spawn Positions")]
    [SerializeField] GameObject point1;
    [SerializeField] GameObject point2;
    [SerializeField] GameObject point3;
    [SerializeField] GameObject point4;
    [SerializeField] GameObject point5;

    // Start is called before the first frame update
    void Start()
    {
        point1 = GameObject.FindGameObjectWithTag("Point1");
        SpawnEnemy1();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnEnemy1()
    {
        if (enemy1 != null)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector3 spawnPosition = point1.transform.position + new Vector3 (i * 1.1f, 0, 0);
                Instantiate(enemy1, spawnPosition, Quaternion.identity);
            }
        }
    }
}

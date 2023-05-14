using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject EnemySpawn1;
    [SerializeField] GameObject EnemySpawn2;
    [SerializeField] GameObject EnemySpawn3;
    [SerializeField] Transform SpawnPoint1;
    [SerializeField] Transform SpawnPoint2;
    [SerializeField] Transform SpawnPoint3;
    private bool CanSpawn = true;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(SaveScript.EnemiesOnScreen);
        if (other.gameObject.CompareTag("Player"))
        {
            if (SaveScript.EnemiesCurrent < SaveScript.MaxEnemiesInGame)
            {
                if (SaveScript.EnemiesOnScreen < SaveScript.MaxEnemiesOnScreen)
                {
                    if (CanSpawn == true)
                    {
                        CanSpawn = false;
                        Instantiate(EnemySpawn1, SpawnPoint1.position, SpawnPoint1.rotation);
                        SaveScript.EnemiesOnScreen++;
                        SaveScript.EnemiesCurrent++;
                        Instantiate(EnemySpawn2, SpawnPoint2.position, SpawnPoint2.rotation);
                        SaveScript.EnemiesOnScreen++;
                        SaveScript.EnemiesCurrent++;
                        Instantiate(EnemySpawn3, SpawnPoint3.position, SpawnPoint3.rotation);
                        SaveScript.EnemiesOnScreen++;
                        SaveScript.EnemiesCurrent++;
                        
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float mapSizeX;
    public float mapSizeY;

    public int enemyAmount;
    [HideInInspector]
    public int _currentEnemyAmount;

    public Transform PlayerTransform;

    public float minDistance;

    void Update()
    {
        if(_currentEnemyAmount < enemyAmount)
        {
            Vector2 position = new Vector2(Random.Range(-mapSizeX, mapSizeX), Random.Range(-mapSizeY, mapSizeY));
            if(Vector2.Distance(PlayerTransform.position, position) > minDistance)
            {
                Instantiate(enemyPrefab, position, Quaternion.identity);
                _currentEnemyAmount++;
            }
        }
    }
}

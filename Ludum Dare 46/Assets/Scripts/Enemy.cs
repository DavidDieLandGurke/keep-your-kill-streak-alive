using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D _rb;

    public GameObject player;
    public PlayerCharacterController playerController;

    private Vector2 _dir;

    public float speed;

    public EnemySpawner spawner;

    public float minDistance;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        spawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        playerController = player.GetComponent<PlayerCharacterController>();
    }

    void Update()
    {
        _dir = player.GetComponent<Rigidbody2D>().position - _rb.position;
    }

    void FixedUpdate()
    {
        /*if (Vector2.Distance(player.GetComponent<Rigidbody2D>().position, _rb.position) < minDistance)
        {
            _rb.velocity = _dir.normalized * speed;
        }*/

        _rb.velocity = _dir.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            spawner._currentEnemyAmount--;
            playerController.KillStreakCooldown = 0;
            playerController.KillStreak++;
            GetComponentInChildren<AudioSource>().Play();
            GetComponentInChildren<ParticleSystem>().Play();
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 1);
        }
    }
}

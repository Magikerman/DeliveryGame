using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform[] positions = new Transform[8];
    [SerializeField] private GameObject enemy;
    [SerializeField] private float timeToSpawn;
    private float timer;

    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody playerRb;

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            timer = timeToSpawn;
            Context enemyContext = Instantiate(enemy, positions[Random.Range(0, 8)].position, Quaternion.identity).GetComponent<Context>();
            enemyContext.player = player;
            enemyContext.playerRb = playerRb;
        }
    }
}

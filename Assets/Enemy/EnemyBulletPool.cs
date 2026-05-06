using UnityEngine;
using System.Collections.Generic;

public class EnemyBulletPool : MonoBehaviour
{
    private Queue<GameObject> bulletPool = new Queue<GameObject>();
    [SerializeField] private GameObject bulletPrefab;

    public static EnemyBulletPool instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            AddBullet(3);
        }
        else
            Destroy(gameObject);
    }

    public void AddBullet(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bulletPool.Enqueue(bullet);
            bullet.SetActive(false);
        }
    }

    public void AddBullet(GameObject bullet)
    {
        bulletPool.Enqueue(bullet);
        bullet.SetActive(false);
    }

    public GameObject GetBullet()
    {
        if (bulletPool.Count == 0) AddBullet();
        GameObject bullet = bulletPool.Dequeue();
        return bullet;
    }
}

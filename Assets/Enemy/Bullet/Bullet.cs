using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject explotion;
    [SerializeField] private GameObject guide;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float distanceWarning;

    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        explotion.GetComponent<ExplotionAnim>().DefineBullet(this);
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        guide.SetActive(false);
        _bullet.SetActive(true);
        explotion.SetActive(false);
        col.enabled = true;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 100f, ground);
        if (rb.linearVelocity != Vector3.zero) transform.forward = rb.linearVelocity;

        if (hit.collider != null && _bullet.activeSelf)
        {
            guide.SetActive(true);
            guide.transform.position = hit.point + Vector3.up/10;
            guide.transform.rotation = Quaternion.Euler(-90f, 0, 0);
            float scale = (1.1f - Mathf.Clamp01(hit.distance/distanceWarning)) * 13f;
            guide.transform.localScale = new Vector3(scale, scale, scale);
        }
        else guide.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        explotion.SetActive(true);
        _bullet.SetActive(false);
        guide.SetActive(false);

        col.enabled = false;
        rb.linearVelocity = Vector3.zero;
        rb.useGravity = false;
    }

    public void DestroyBullet()
    {
        rb.useGravity = true;
        EnemyBulletPool.instance.AddBullet(gameObject);
    }
}

public class ThrowData
{
    public float angle;
    public float deltaY;
    public float deltaXZ;

    public Vector3 throwVelocity;

    public ThrowData(float angle, float deltaY, float deltaXZ, Vector3 throwVelocity)
    {
        this.angle = angle;
        this.deltaY = deltaY;
        this.deltaXZ = deltaXZ;
        this.throwVelocity = throwVelocity;
    }
}

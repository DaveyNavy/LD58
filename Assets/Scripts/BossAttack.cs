using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] int Lifetime;
    [SerializeField] int Damage;
    [SerializeField] int Knockback;
    [SerializeField] ParticleSystem blast1;
    [SerializeField] ParticleSystem blast2;
    [SerializeField] Collider2D bigCol;
    [SerializeField] Collider2D smallCol;
    private int _lifetimeTimer;

    private void Awake()
    {
        _lifetimeTimer = Lifetime;
    }
    private void FixedUpdate()
    {
        _lifetimeTimer = Mathf.Max(0, _lifetimeTimer - 1);
        if (_lifetimeTimer == 0) return;

        if (_lifetimeTimer == 1) {
            Daddy();
            blast1.transform.parent = null;
            blast2.transform.parent = null;
            blast1.Play();
            blast2.Play();
            transform.localScale = Vector3.zero;
            Destroy(gameObject, 5f);
        }
    }

    private void Daddy()
    {
        Debug.Log("Boss Attack goes boom!");
        // Check intersections for bigCol
        Collider2D[] bigHits = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D();
        int bigCount = bigCol.Overlap(filter, bigHits);

        for (int i = 0; i < bigCount; i++)
        {
            var player = bigHits[i]?.GetComponent<PlayerMovement>();
            if (player != null)
            {
                Vector2 knockback = (PlayerStats.Instance.player.transform.position - transform.position).normalized;
                player.ApplyKnockback(knockback * Knockback);
                player.TakeDamage(Damage);
            }
        }

        // Check intersections for smallCol (does more damage)
        Collider2D[] smallHits = new Collider2D[10];
        int smallCount = smallCol.Overlap(filter, smallHits);

        for (int i = 0; i < smallCount; i++)
        {
            var player = smallHits[i]?.GetComponent<PlayerMovement>();
            if (player != null)
            {
                Vector2 knockback = (PlayerStats.Instance.player.transform.position - transform.position).normalized;
                player.ApplyKnockback(knockback * Knockback);
                player.TakeDamage(Damage * 20);
            }
        }
    }
}

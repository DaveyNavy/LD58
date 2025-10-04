using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] int Lifetime;
    [SerializeField] int Damage;
    [SerializeField] int Knockback;
    private int _lifetimeTimer;

    private void Awake()
    {
        _lifetimeTimer = Lifetime;
    }
    private void FixedUpdate()
    {
        _lifetimeTimer = Mathf.Max(0, _lifetimeTimer - 1);
        if (_lifetimeTimer <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damagable = collision.GetComponent<Damagable>();
        if (damagable != null && !damagable.IsPlayer)
        {
            if (PlayerStats.Instance.limbs > 1)
            {
                Vector2 knockback = (collision.transform.position - PlayerStats.Instance.player.transform.position).normalized;
                damagable.ApplyKnockback(knockback * Knockback);
            }
            damagable.TakeDamage(Damage);
        }
    }
}

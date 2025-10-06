using UnityEngine;

public class AttackPound : MonoBehaviour
{
    [SerializeField] int Lifetime;
    [SerializeField] public int Damage;
    [SerializeField] int Knockback;
    [SerializeField] float StunDuration;
    private int _lifetimeTimer;

    private void Awake()
    {
        _lifetimeTimer = Lifetime;
        transform.localScale = transform.localScale * PlayerStats.Instance.player.Attack.GetLimbMultiplier();
    }
    private void FixedUpdate()
    {
        _lifetimeTimer = Mathf.Max(0, _lifetimeTimer - 1);
        if (_lifetimeTimer <= 0)
        {
            Destroy(gameObject, 5f);
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damagable = collision.GetComponent<Damagable>();
        if (damagable != null && !damagable.IsPlayer)
        {
            damagable.TakeDamage((int)(Damage * PlayerStats.Instance.player.Attack.GetLimbMultiplier()));
            Vector2 knockback = (collision.transform.position - PlayerStats.Instance.player.transform.position).normalized;
            damagable.ApplyKnockback(knockback * Knockback);
        }
        var enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.StunTimer = StunDuration;
        }
    }
}

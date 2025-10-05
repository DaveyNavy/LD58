using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] int Lifetime;
    [SerializeField] int Damage;
    [SerializeField] int Knockback;
    [SerializeField] ParticleSystem blast;
    private int _lifetimeTimer;

    private void Awake()
    {
        _lifetimeTimer = Lifetime;
        transform.localScale = Vector3.zero;
    }
    private void FixedUpdate()
    {
        _lifetimeTimer = Mathf.Max(0, _lifetimeTimer - 1);
        if (_lifetimeTimer == 0) return;
        float t = 1f - (_lifetimeTimer / (float)Lifetime);
        float scale = Mathf.Lerp(0f, 3f, t);
        transform.localScale = Vector3.one * scale;
        if (_lifetimeTimer == 2)
        {
            GetComponent<CircleCollider2D>().enabled = true;
        }
        if (_lifetimeTimer == 1) {
            blast.Play();
            transform.localScale = Vector3.zero;
            GetComponent<CircleCollider2D>().enabled = false;
            Destroy(gameObject, 5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Vector2 knockback = (PlayerStats.Instance.player.transform.position - transform.position).normalized;
            player.ApplyKnockback(knockback * Knockback);
            player.TakeDamage(Damage);
        }
    }
}

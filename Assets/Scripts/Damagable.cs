using TMPro;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    public GameObject flesh;
    protected virtual void Awake()
    {
        _curHealth = _maxHealth;
        _rb = GetComponent<Rigidbody2D>();
    }

    #region Health
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _curHealth;
    [SerializeField] public bool IsPlayer;
    [SerializeField] private int _fleshDrops;
    protected Rigidbody2D _rb;

    public virtual bool TakeDamage(int amount)
    {
        _curHealth = Mathf.Max(_curHealth - amount, 0);
        bool isAlive = _curHealth > 0;
        if (!isAlive)
            OnDeath();
        return isAlive;
    }
    public virtual void Heal(int amount)
    {
        _curHealth = Mathf.Min(_curHealth + amount, _maxHealth);
    }

    public virtual void OnDeath()
    {
        if (!IsPlayer)
        {
            for (int i = 0; i < _fleshDrops; i++)
            {
                Instantiate(flesh, transform.position, Quaternion.identity);
            }
        }
        Destroy(gameObject);
    }

    #endregion Health

    #region Contact
    [SerializeField] private int _contactDamage;
    [SerializeField] private int _contactDamageGiveKB;
    [SerializeField] private int _contactDamageTakeKB;

    public void ApplyKnockback(Vector2 kb)
    {
        _rb.AddForce(kb);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // Find player to damage:
        if (_contactDamage > 0 && collision.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        {
            player.TakeDamage(_contactDamage);
            Vector2 knockback = (collision.transform.position - transform.position).normalized;
            player.ApplyKnockback(knockback * _contactDamageGiveKB);
            this.ApplyKnockback(-knockback * _contactDamageTakeKB);
        }
    }

    #endregion Contact

    public bool knockbackImmune;
}

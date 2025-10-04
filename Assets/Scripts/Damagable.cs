using UnityEngine;

public class Damagable : MonoBehaviour
{
    protected virtual void Awake()
    {
        _curHealth = _maxHealth;
        _rb = GetComponent<Rigidbody2D>();
    }

    #region Health
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _curHealth;
    private Rigidbody2D _rb;

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
        Debug.Log("Damagable died!");
        Destroy(gameObject);
    }

    #endregion Health

    #region Contact
    [SerializeField] private int _contactDamage;
    [SerializeField] private int _contactDamageGiveKB;
    [SerializeField] private int _contactDamageTakeKB;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Find player to damage:
        if (_contactDamage > 0 && collision.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        {
            player.TakeDamage(_contactDamage);
            Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
            player.Rigidbody.AddForce(knockbackDir * _contactDamageGiveKB);
            _rb.AddForce(-knockbackDir * _contactDamageTakeKB);
        }
    }

    #endregion Contact
}

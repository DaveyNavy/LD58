using UnityEngine;

public class Damagable : MonoBehaviour
{
    private void Awake()
    {
        _curHealth = _maxHealth;
    }

    #region Health
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _curHealth;

    public virtual bool TakeDamage(int amount)
    {
        _curHealth = Mathf.Max(_curHealth - amount, 0);
        bool isAlive = _curHealth > 0;
        if (isAlive)
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
    [SerializeField] private int _contactDamageKB;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Find player to damage:
        if (_contactDamage > 0 && collision.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        {
            player.TakeDamage(_contactDamage);
            Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
            player.Rigidbody.AddForce(knockbackDir * _contactDamageKB);
        }
    }

    #endregion Contact
}

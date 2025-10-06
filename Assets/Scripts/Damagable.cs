using TMPro;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    public GameObject flesh;

    private int _damageSoundTimer;
    private bool _shouldPlayDamageSound = false;
    protected virtual void Awake()
    {
        _curHealth = _maxHealth;
        _rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void FixedUpdate()
    {
        _damageSoundTimer = Mathf.Max(0, _damageSoundTimer - 1);
        if (_damageSoundTimer == 0 && _shouldPlayDamageSound)
        {
            _shouldPlayDamageSound = false;
            _damageSoundTimer = 5;
            float pitch = (IsPlayer) ? Random.Range(0.8f, 1.2f) : Random.Range(0.5f, 0.9f);
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.hurt1, 0.7f, pitch);
        }
    }

    #region Health
    [SerializeField] public int _maxHealth;
    [SerializeField] public int _curHealth;
    [SerializeField] public bool IsPlayer;
    [SerializeField] private int _fleshDrops;
    protected Rigidbody2D _rb;

    public virtual bool TakeDamage(int amount)
    {
        _curHealth = Mathf.Max(_curHealth - amount, 0);
        ParticleSystem vfx = Instantiate(PlayerStats.Instance.hitvfx, transform.position, Quaternion.identity);
        vfx.transform.localScale = transform.localScale;
        _shouldPlayDamageSound = true;
        //

        vfx.Play();
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
        ParticleSystem vfx = Instantiate(PlayerStats.Instance.deathvfx, transform.position, Quaternion.identity);
        vfx.transform.localScale = transform.localScale / 2f;
        vfx.Play();
        float pitch = Random.Range(0.8f, 1.2f);
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.death2, 0.7f, pitch);

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

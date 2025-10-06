using UnityEngine;

public class BossAttackSpin : MonoBehaviour
{
    [SerializeField] int Lifetime;
    [SerializeField] public int Damage;
    private int _lifetimeTimer;

    private void Awake()
    {
        _lifetimeTimer = Lifetime;
        SoundManager.PlayOnAudioSource(transform, SoundManager.Instance.misc1, true);
    }
    private void FixedUpdate()
    {
        _lifetimeTimer = Mathf.Max(0, _lifetimeTimer - 1);
        if (_lifetimeTimer <= 0) Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var damagable = collision.GetComponent<Damagable>();
        if (damagable != null && damagable.IsPlayer)
        {
            damagable.TakeDamage(Damage);
        }
    }
}

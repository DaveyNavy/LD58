using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int AttackCD;
    [SerializeField] private int AttackPoundCD;
    [SerializeField] private int AttackSpinCD;
    [SerializeField] private int AttackDashCD;
    [SerializeField] private GameObject AttackPrefab;
    [SerializeField] private GameObject AttackPoundPrefab;
    [SerializeField] private GameObject AttackSpinPrefab;
    [SerializeField] private GameObject AttackDashPrefab;
    private int _attackTimer;
    private int _attackAnimTimer;
    private int _attackPoundTimer;
    private int _attackPoundAnimTimer;
    private int _attackSpinTimer;
    private int _attackDashTimer;
    private PlayerMovement _player;
    private float _speed;
    private Vector2 _facing;

    private void Awake()
    {
        _player = GetComponent<PlayerMovement>();
        _speed = PlayerStats.Instance.player.Speed;
    }
    public void ResetCooldowns()
    {
        _attackTimer = 0;
        _attackPoundTimer = 0;
        _attackSpinTimer = 0;
        _attackDashTimer = 0;
    }
    private void FixedUpdate()
    {
        _attackTimer = Mathf.Max(0, _attackTimer - 1);
        _attackPoundTimer = Mathf.Max(0, _attackPoundTimer - 1);
        _attackSpinTimer = Mathf.Max(0, _attackSpinTimer - 1);
        _attackAnimTimer = Mathf.Max(0, _attackAnimTimer - 1);
        _attackPoundAnimTimer = Mathf.Max(0, _attackPoundAnimTimer - 1);
        _attackDashTimer = Mathf.Max(0, _attackDashTimer - 1);

        if (_attackAnimTimer == 1)
        {
            Instantiate(AttackPrefab, GetSpawnPos(_facing, 1.5f), GetSpawnRot(_facing));
            PlayerStats.Instance.player.Speed = _speed;
        }
        if (_attackPoundAnimTimer == 1)
        {
            Instantiate(AttackPoundPrefab, transform.position, Quaternion.identity);
            PlayerStats.Instance.player.Speed = _speed;
        }
    }

    public void OnAttack()
    {
        int limb = PlayerStats.Instance.limbs;
        if (limb < 1) return;

        Attack();
    }
    public void OnAttackPound()
    {
        int limb = PlayerStats.Instance.limbs;
        if (limb < 2) return;

        AttackPound();
    }
    public void OnAttackSpin()
    {
        int limb = PlayerStats.Instance.limbs;
        if (limb < 3) return;

        AttackSpin();
    }
    public void OnAttackDash()
    {
        int limb = PlayerStats.Instance.limbs;
        if (limb < 4) return;

        AttackDash();
    }

    private void Attack()
    {
        if (_attackTimer > 0) return;
        _attackTimer = AttackCD;
        Debug.Log("Player Attack!");
        PlayerStats.Instance.limbDecay();
        _facing = _player.Facing;
        if (PlayerStats.Instance.limbs <= 1)
        {
            PlayerStats.Instance.player.Speed = 0;
        }
        if (PlayerStats.Instance.limbs == 1)
        {
            _attackAnimTimer = 35;
        }
        else if (PlayerStats.Instance.limbs == 2)
        {
            _attackAnimTimer = 25;
        }
        else if (PlayerStats.Instance.limbs >= 3)
        {
            _attackAnimTimer = 5;
        }
    }
    private void AttackPound()
    {
        if (_attackPoundTimer > 0) return;
        _attackPoundTimer = AttackPoundCD;
        Debug.Log("Player Attack Pound!");
        PlayerStats.Instance.limbDecay();
        PlayerStats.Instance.player.Speed = 0;
        _attackPoundAnimTimer = (PlayerStats.Instance.limbs > 2) ? 5 : 20;
    }
    private void AttackSpin()
    {
        if (_attackSpinTimer > 0) return;
        _attackSpinTimer = AttackSpinCD;
        Debug.Log("Player Attack Spin!");
        PlayerStats.Instance.limbDecay();
        Instantiate(AttackSpinPrefab, transform.position, Quaternion.identity, _player.transform);
    }

    private void AttackDash()
    {
        if (_attackDashTimer > 0) return;
        _attackDashTimer = AttackDashCD;
        Debug.Log("Player Attack Dash!");
        PlayerStats.Instance.limbDecay();

        Instantiate(AttackDashPrefab, GetSpawnPos(_player.Facing, 2f), GetSpawnRot(_player.Facing));
        _player.ApplyKnockback(_player.Facing * 500f);
    }

    private Vector2 GetSpawnPos(Vector2 facing, float distance)
    {
        float angle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;
        angle = Mathf.Round(angle / 45f) * 45f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector3 spawnOffset = Quaternion.Euler(0, 0, angle) * Vector3.right * distance * GetLimbMultiplier();
        return transform.position + spawnOffset;
    }
    private Quaternion GetSpawnRot(Vector2 facing)
    {
        float angle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;
        angle = Mathf.Round(angle / 45f) * 45f;
        return Quaternion.Euler(0, 0, angle);
    }

    public float GetLimbMultiplier()
    {
        int limb = PlayerStats.Instance.limbs;
        if (limb == 1) return 0.6f;
        if (limb == 2) return 0.9f;
        if (limb == 3) return 1.25f;
        if (limb == 4) return 1.5f;
        return 0f;
    }
}

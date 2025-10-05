using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int AttackCD;
    [SerializeField] private int AttackPoundCD;
    [SerializeField] private int AttackSpinCD;
    [SerializeField] private GameObject AttackPrefab;
    [SerializeField] private GameObject AttackPoundPrefab;
    [SerializeField] private GameObject AttackSpinPrefab;
    private int _attackTimer;
    private int _attackAnimTimer;
    private int _attackPoundTimer;
    private int _attackSpinTimer;
    private PlayerMovement _player;
    private float _speed;
    private Vector2 _facing;

    private void Awake()
    {
        _player = GetComponent<PlayerMovement>();
    }
    private void FixedUpdate()
    {
        _attackTimer = Mathf.Max(0, _attackTimer - 1);
        _attackPoundTimer = Mathf.Max(0, _attackPoundTimer - 1);
        _attackSpinTimer = Mathf.Max(0, _attackSpinTimer - 1);
        _attackAnimTimer = Mathf.Max(0, _attackAnimTimer - 1);

        if (_attackAnimTimer == 1)
        {
            Instantiate(AttackPrefab, GetSpawnPos(_facing), GetSpawnRot(_facing));
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

    private void Attack()
    {
        if (_attackTimer > 0) return;
        _attackTimer = AttackCD;
        Debug.Log("Player Attack!");
        _facing = _player.Facing;
        _speed = PlayerStats.Instance.player.Speed;
        PlayerStats.Instance.player.Speed = 0;
        _attackAnimTimer = (PlayerStats.Instance.limbs > 1) ? 2 : 35;
    }
    private void AttackPound()
    {
        if (_attackPoundTimer > 0) return;
        _attackPoundTimer = AttackPoundCD;
        Debug.Log("Player Attack Pound!");
        Instantiate(AttackPoundPrefab, transform.position, Quaternion.identity);
    }
    private void AttackSpin()
    {
        if (_attackSpinTimer > 0) return;
        _attackSpinTimer = AttackSpinCD;
        Debug.Log("Player Attack Spin!");
        Instantiate(AttackSpinPrefab, transform.position, Quaternion.identity, _player.transform);
    }

    private Vector2 GetSpawnPos(Vector2 facing)
    {
        float angle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;
        angle = Mathf.Round(angle / 45f) * 45f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector3 spawnOffset = Quaternion.Euler(0, 0, angle) * Vector3.right * 1.5f;
        float distance = 1f; // Distance in front of player
        return transform.position + spawnOffset * distance;
    }
    private Quaternion GetSpawnRot(Vector2 facing)
    {
        float angle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;
        angle = Mathf.Round(angle / 45f) * 45f;
        return Quaternion.Euler(0, 0, angle);
    }
}

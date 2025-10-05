using TMPro;
using UnityEngine;

public class BigDaddy : Damagable
{
    [SerializeField] TextMeshPro popup;
    [SerializeField] TextMeshPro popup2;

    protected override void Awake()
    {
        base.Awake();

        popup.gameObject.SetActive(true);
        popup2.gameObject.SetActive(true);
        popup.alpha = 0f;
        popup2.alpha = 0f;
    }
    private void FixedUpdate()
    {
        // Text:
        float distance = Vector3.Distance(transform.position, PlayerStats.Instance.playerTransform);
        bool inRange = distance < 3.5f;
        
        if (inRange && popup.alpha < 1f)
        {
            popup.alpha = Mathf.Min(1f, popup.alpha + 0.05f);
            popup2.alpha = popup.alpha;
        }
        else if (!inRange && popup.alpha > 0f)
        {
            popup.alpha = Mathf.Max(0f, popup.alpha - 0.05f);
            popup2.alpha = popup.alpha;
        }
    }

    public override bool TakeDamage(int amount)
    {
        // Give flesh:
        amount = Mathf.Min(amount, PlayerStats.Instance.flesh);
        PlayerStats.Instance.UpdateFlesh(-amount);
        PlayerStats.Instance.DaddyFlesh += amount;
        PlayerStats.Instance.player.Heal(amount);
        return true;
    }
}

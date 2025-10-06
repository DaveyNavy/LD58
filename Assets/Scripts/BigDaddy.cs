using System.Collections;
using TMPro;
using UnityEngine;

public class BigDaddy : Damagable
{
    [SerializeField] TextMeshPro popup;
    [SerializeField] TextMeshPro popup2;
    [SerializeField] Transform[] spawns;
    [SerializeField] GameObject boss;

    private bool _moveUp = false;

    protected override void Awake()
    {
        base.Awake();

        popup.gameObject.SetActive(true);
        popup2.gameObject.SetActive(true);
        popup.alpha = 0f;
        popup2.alpha = 0f;
    }
    private void Start()
    {
        PlayerStats.Instance.bigDaddy = this;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

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

        inRange = distance < 10f;
        if (inRange)
        {
            PlayerStats.Instance.player.Heal(1);
        }

        // Move up if triggered
        if (_moveUp)
        {
            transform.position += new Vector3(0f, 0.02f, 0f);
        }
    }

    public override bool TakeDamage(int amount)
    {
        // Give flesh:
        amount = Mathf.Min(1, PlayerStats.Instance.flesh);
        PlayerStats.Instance.UpdateFlesh(-amount);
        PlayerStats.Instance.DaddyFlesh += amount;
        PlayerStats.Instance.player.Heal(amount);
        PlayerStats.Instance.player.Attack.ResetCooldowns();
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.feed1, 0.7f, 2f);
        GetComponent<Collider2D>().enabled = false;

        if (PlayerStats.Instance.DaddyFlesh >= 200)
        {
            PlayerStats.Instance.DaddyFlesh = 200;
            StartCoroutine(EndGame());
        }
        return true;
    }

    private IEnumerator EndGame()
    {
        PlayerStats.Instance.player.Speed = 0.5f;

        popup.text = "";
        popup2.text = "";
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.cutscene1, 0.7f, 2f);

        yield return new WaitForSeconds(2f);

        popup.text = "thanks bro";
        popup2.text = "ima head out now";

        yield return new WaitForSeconds(2f);

        // Start moving up 0.2 units per FixedUpdate
        _moveUp = true;

        yield return new WaitForSeconds(8f);

        // Instantiate a boss at each spawn point
        foreach (var spawn in spawns)
        {
            Instantiate(boss, spawn.position, spawn.rotation);
        }
        PlayerStats.Instance.IsGameOver = true;
    }
}

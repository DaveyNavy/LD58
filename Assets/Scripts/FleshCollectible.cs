using TMPro;
using UnityEngine;

public class FleshCollectible : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damagable = collision.GetComponent<Damagable>();
        if (damagable != null && damagable.IsPlayer)
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.eat1, 1f, 2f);
            PlayerStats.Instance.UpdateFlesh(1);
            Destroy(gameObject);
        }
    }
}
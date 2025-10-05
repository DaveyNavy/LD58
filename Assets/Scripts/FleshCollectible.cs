using TMPro;
using UnityEngine;

public class FleshCollectible : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damagable = collision.GetComponent<Damagable>();
        if (damagable != null && damagable.IsPlayer)
        {
            PlayerStats.Instance.UpdateFlesh(1);
            Destroy(gameObject);
        }
    }
}
using UnityEngine;

public class RiverDamage : MonoBehaviour
{
    [Header("River Settings")]
    [SerializeField]
    private float riverDamage;
    [SerializeField]
    private float riverInterval;

    private float nextRiverCheck = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Time.time > nextRiverCheck)
        {
            PlayerController controller = other.gameObject.GetComponent<PlayerController>();
            controller.DamagePlayer(riverDamage);

            nextRiverCheck = Time.time + riverInterval;
        }
    }
}

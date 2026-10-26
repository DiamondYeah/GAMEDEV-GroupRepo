using UnityEngine;

public class KickBoost : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform kickThigh;          // the thigh bone of the kicking leg
    [SerializeField] Vector3 extraRotation = new Vector3(-40f, 0f, 0f);
    [SerializeField] string kickStateName = "Kicking";

    void LateUpdate()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        float weight = 0f;
        if (info.IsName(kickStateName))
        {
            float t = Mathf.Clamp01(info.normalizedTime);
            weight = Mathf.Sin(t * Mathf.PI);   // 0 at start, 1 at the middle, 0 at the end
        }

        if (weight > 0f)
            kickThigh.localRotation *= Quaternion.Euler(extraRotation * weight);
    }
}
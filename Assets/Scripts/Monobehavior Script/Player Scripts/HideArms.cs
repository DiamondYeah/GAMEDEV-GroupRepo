using UnityEngine;

public class HideArms : MonoBehaviour
{
    [SerializeField] Transform[] armBones;   // upper arm bones only, not the spine

    void LateUpdate()
    {
        foreach (Transform bone in armBones)
        {
            if (bone != null) bone.localScale = Vector3.zero;
        }
    }
}
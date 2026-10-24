using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    Animator playerAnim;

    void Start()
    {
        playerAnim = GetComponent<Animator>();
    }

    void Update()
    {
        var kb = Keyboard.current;

        if (kb != null)
        {
            bool moving = kb.wKey.isPressed || kb.aKey.isPressed ||
                          kb.sKey.isPressed || kb.dKey.isPressed;
            playerAnim.SetBool("Running", moving);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            playerAnim.SetTrigger("Kicking");
        }


    }
}
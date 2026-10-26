using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTextHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] TMP_Text buttonText; // Reference to the TextMeshProUGUI component of the button

    [Header("Font Sizes")]
    [SerializeField] float normalFontSize = 64f;
    [SerializeField] float hoverFontSize = 72f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonText.fontSize = normalFontSize;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.fontSize = hoverFontSize;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.fontSize = normalFontSize;
    }
}

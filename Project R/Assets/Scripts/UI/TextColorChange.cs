using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TextColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public TMPro.TMP_Text theText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        theText.color = Color.yellow; //Or however you do your color
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theText.color = Color.white; //Or however you do your color
    }
}
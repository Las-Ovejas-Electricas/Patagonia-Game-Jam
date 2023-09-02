using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public void CloseWindow()
    {
        if (LeanTween.isTweening(gameObject)) return;
        LeanTween.move(gameObject, Vector3.zero, 0.2f).setEaseOutCubic();
        LeanTween.scale(gameObject, Vector3.zero, 0.2f).setEaseInCubic();
        GameManager.instance.StartClientSequence();
        SetPivot(GetComponent<RectTransform>(), new Vector2(0.5f, 0));
    }

    public void SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        Vector3 deltaPosition = rectTransform.pivot - pivot;    // get change in pivot
        deltaPosition.Scale(rectTransform.rect.size);           // apply sizing
        deltaPosition.Scale(rectTransform.localScale);          // apply scaling
        deltaPosition = rectTransform.rotation * deltaPosition; // apply rotation

        rectTransform.pivot = pivot;                            // change the pivot
        rectTransform.localPosition -= deltaPosition;           // reverse the position change
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InsuranceFile : MonoBehaviour, IPointerClickHandler
{
    private bool _showPolicy = true;

    [SerializeField]
    private RectTransform _policy;

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleWindow();
        transform.SetAsLastSibling();
    }

    private void ToggleWindow()
    {
        if (LeanTween.isTweening(_policy)) return;

        if (_showPolicy) SetPivot(_policy, Vector3.one);
        LeanTween.moveLocal(_policy.gameObject, new Vector2(-100, -100), 0.1f);
        LeanTween.scale(_policy, _showPolicy ? Vector3.one : Vector3.zero, 0.1f).setOnComplete(() =>
        {
            _showPolicy = !_showPolicy;
            SetPivot(_policy, Vector3.one * (_showPolicy ? 1f : 0.5f));
        });
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

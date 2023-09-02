using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Webcam : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool _showWebcam = true;

    [SerializeField]
    private Image _highlight;

    [SerializeField]
    private RectTransform _webcam;

    [Space]
    [Header("Policy Data")]
    private PolicySO _policySO;
    public PolicySO PolicySO
    {
        get { return _policySO; }
        set
        {
            _policySO = value;
            _webcamPhoto.sprite = value.photo;
        }
    }

    [SerializeField]
    private Image _webcamPhoto;

    private float lastClick = 0f;
    private float interval = 0.5f;
    public void OnPointerClick(PointerEventData eventData)
    {
        if ((lastClick + interval) > Time.time)
        {
            ToggleWindow();
            _webcam.SetAsLastSibling();
        }
        else
        {
            lastClick = Time.time;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _highlight.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _highlight.enabled = false;
    }

    private void ToggleWindow()
    {
        _webcam.parent.SetAsLastSibling();
        if (LeanTween.isTweening(_webcam)) return;

        if (_showWebcam) SetPivot(_webcam, Vector3.one);
        LeanTween.moveLocal(_webcam.gameObject, new Vector2(-100, -100), 0.1f);
        LeanTween.scale(_webcam, _showWebcam ? Vector3.one : Vector3.zero, 0.1f).setOnComplete(() =>
        {
            _showWebcam = !_showWebcam;
            SetPivot(_webcam, Vector3.one * (_showWebcam ? 1f : 0.5f));
        });
    }

    public void SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        Vector3 deltaPosition = rectTransform.pivot - pivot;
        deltaPosition.Scale(rectTransform.rect.size);
        deltaPosition.Scale(rectTransform.localScale);
        deltaPosition = rectTransform.rotation * deltaPosition;

        rectTransform.pivot = pivot;
        rectTransform.localPosition -= deltaPosition;
    }

    public void SetWindowState(bool isOpen)
    {
        _showWebcam = isOpen;
        ToggleWindow();
    }
}

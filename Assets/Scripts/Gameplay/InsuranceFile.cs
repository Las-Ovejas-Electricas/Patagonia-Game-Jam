using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InsuranceFile : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool _showPolicy = true;

    [SerializeField]
    private Image _highlight;

    [SerializeField]
    private RectTransform _policy;

    [Space][Header("Policy Data")]
    private PolicySO _policySO;
    public PolicySO PolicySO
    {
        get { return _policySO; }
        set { 
            _policySO = value;
            _nameTMP.text = value.name;
            _personalIdTMP.text = value.personalID.ToString();
            _addressTMP.text = value.address;
            _expirationTMP.text = value.expirationDate;
            _modelTMP.text = value.model;
            _colorTMP.text = value.color;
            _licensePlateTMP.text = value.licensePlate;
            _coverageTMP.text = string.Join(", ", value.coverage);
        }
    }
    [SerializeField]
    private TextMeshProUGUI _nameTMP, _personalIdTMP, _addressTMP, _expirationTMP, _modelTMP, _colorTMP, _licensePlateTMP, _coverageTMP;

    private float lastClick = 0f;
    private float interval = 0.5f;
    public void OnPointerClick(PointerEventData eventData)
    {
        if ((lastClick + interval) > Time.time)
        {
            ToggleWindow();
            _policy.SetAsLastSibling();
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
        _policy.parent.SetAsLastSibling();
        if (LeanTween.isTweening(_policy)) return;

        if (_showPolicy) SetPivot(_policy, Vector3.one);
        LeanTween.moveLocal(_policy.gameObject, new Vector2(-100, -100), 0.1f);
        LeanTween.scale(_policy, _showPolicy ? Vector3.one : Vector3.zero, 0.1f).setOnComplete(() =>
        {
            _showPolicy = !_showPolicy;
            SetPivot(_policy, Vector3.one * (_showPolicy ? 1f : 0.5f));
        });

        UpdateData();
    }

    private void UpdateData()
    {
        Selectable[] fields = FindObjectsOfType<Selectable>();
        foreach (Selectable p in fields)
        {
            p.UpdateValue();
        }
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
        _showPolicy = isOpen;
        ToggleWindow();
    }
}

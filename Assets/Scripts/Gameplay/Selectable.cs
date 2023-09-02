using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public enum Source
{
    Data,
    UserInput,
}

public enum DataType
{
    Name,
    Photo,
    Date,
    Coverage,
    LicensePlate,
    Model,
    Address,
    Signature,
    Color,
    PersonalID,
}

public class Selectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Source Source;
    public DataType DataType;
    public string Value;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.instance.SetPointerCursor(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.instance.SetPointerCursor(false);
    }

    public void UpdateValue()
    {
        switch (DataType)
        {
            case DataType.Photo:
                break;
            default:
                if (TryGetComponent(out TextMeshProUGUI tmp))
                {
                    Value = tmp.text;
                }
                break;
        }
    }
}

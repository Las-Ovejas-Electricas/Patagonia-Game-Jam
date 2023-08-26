using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

public class Selectable : MonoBehaviour
{
    public Source Source;
    public DataType DataType;
    public string Value;
}

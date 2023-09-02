using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Policy", menuName = "Clients/Policy", order = 1)]
public class PolicySO : ScriptableObject
{
    public string clientName;
    public int personalID;
    public string address;
    public string expirationDate;
    public string licensePlate;
    public string model;
    public string color;
    public string[] coverage;
    public Sprite photo;
}

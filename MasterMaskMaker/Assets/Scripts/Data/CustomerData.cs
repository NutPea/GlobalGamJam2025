using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Customer", order = 1)]
public class CustomerData : ScriptableObject
{
    [SerializeField] private List<Tool> neededTools;
}

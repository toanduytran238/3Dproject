using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "new Ball", menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    public List<Image> list = new List<Image>();
    
}

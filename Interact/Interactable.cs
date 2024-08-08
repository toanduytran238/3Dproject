using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public Image img;
    public TextMeshProUGUI textMeshProUGUI;
    
    void Start()
    {
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void Interact()
    {
        
        Destroy(gameObject);
    }
    public void showText()
    {
        
        textMeshProUGUI.SetText("pick up");
    }
    public void hideText() 
    {
        
        textMeshProUGUI.SetText("");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    public Inventory inventory  ;
    public int x;
    public int y;
    public Animator animator;
    void Start()
    {
        for (int i = 0; i < inventory.list.Count; i++)
        {
            var obj = Instantiate(inventory.list[i], Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = new Vector3(x +(i * 100), y, 0);

        }

        animator = GetComponent<Animator>();
    }
    public void Load()
    {
        for (int i = 0; i < inventory.list.Count; i++)
        {
            var obj = Instantiate(inventory.list[i], Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = new Vector3(x + (i * 100), y, 0);

        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}

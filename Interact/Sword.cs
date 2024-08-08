using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float maxHeight = 0;
    public Transform origin;
    public bool _canDamage = false;
    public Transform direction;
    public RaycastHit hit;
    public LayerMask LayerMask;
    private Enymy enemy;
    void Start()
    {
        maxHeight =Vector3.Distance(origin.position, direction.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(_canDamage)
        {
            
            bool isHit = Physics.Raycast(origin.position, (direction.position - origin.position) , out hit, maxHeight,LayerMask);
            if(isHit)
            {
                //Debug.Log("hit");
                if (hit.transform.TryGetComponent<Enymy>(out enemy))
                {
                    
                    enemy.beDamaged(1);
                    //Debug.Log("Damaged");
                    _canDamage = false;
                }
            }
            else if(enemy !=null) 
            {
                enemy.isDamaging = false;
            }
        }
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(origin.position, (direction.position-origin.position)*maxHeight);
    }
}

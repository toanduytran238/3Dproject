using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySword : MonoBehaviour
{
    public float maxHeight = 0;
    public Transform origin;
    public bool _canDamage = false;
    public Transform direction;
    public RaycastHit hit;
    public LayerMask LayerMask;
    private PlayerStateMachine playerState;
    void Start()
    {
        maxHeight = Vector3.Distance(origin.position, direction.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (_canDamage)
        {

            bool isHit = Physics.Raycast(origin.position, (direction.position - origin.position), out hit, maxHeight, LayerMask);
            if (isHit)
            {
                Debug.Log("hit");
                if (hit.transform.TryGetComponent<PlayerStateMachine>(out playerState))
                {

                    playerState.beDamage();
                    //Debug.Log("Damaged");
                    _canDamage = false;
                }
            }
            else if (playerState != null)
            {
                playerState.isDamaging = false;
            }
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(origin.position, (direction.position - origin.position) * maxHeight);
    }
}

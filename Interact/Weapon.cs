using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Enymy enemy;
    Enymy enemy1;
    public Vector3 origin;
    public float maxDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        origin = transform.position;

    }
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(origin, transform.position) > maxDistance)
        {
            Destroy(gameObject); // H?y vi�n ??n
        }
    }
    void OnCollisionEnter(Collision other)
    {
        // Ki?m tra n?u vi�n ??n va ch?m v?i m?c ti�u
        if (other.gameObject.TryGetComponent<Enymy>(out enemy))
        {
            // Th?c hi?n h�nh ??ng khi vi�n ??n tr�ng m?c ti�u
            Debug.Log("Vi�n ??n ?� tr�ng m?c ti�u!");

            
            enemy.beDamaged(1);
            enemy.isDamaging = false;
            Destroy(gameObject); // H?y vi�n ??n
        }
        if (other.gameObject.TryGetComponent<Enymy>(out enemy1)&&enemy!=null)
        {
            // Th?c hi?n h�nh ??ng khi vi�n ??n tr�ng m?c ti�u
            Debug.Log("Vi�n ??n ?� tr�ng m?c ti�u!");


            enemy1.beDamaged(1);
            enemy1.isDamaging = false;
            Destroy(gameObject); // H?y vi�n ??n
        }
    }
}

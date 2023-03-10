using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : Projectile
{
    public float LifetimeSeconds = 3f;
    public float FireTime = 120f;

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();       
        body.isKinematic = true; // Disables velocity and stuff

        Invoke("Die", LifetimeSeconds);
    }

    // Update is called once per frame
    void Update()
    {
        if (Velocity != Vector3.zero)       
        {
            transform.position += Velocity * Time.deltaTime;
            float degrees = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg - 90;

            transform.eulerAngles = Vector3.forward * degrees;
        }
    }

    protected override void OnHitUnit(Unit unit)
    {
        unit.FireTime += FireTime;
    }
}

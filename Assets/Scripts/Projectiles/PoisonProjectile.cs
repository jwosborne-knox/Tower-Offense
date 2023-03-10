using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonProjectile : Projectile
{
    public Poisoner poisoner;

    public float LifetimeSeconds = 5f;
    public float PoisonTime = 100f;

    // Changing these values overrides the poisoner's life time
    // Used by the grand tower to be less powerful
    public float PoisonerTime = 0f;
    public float PoisonerPoisonTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        body.isKinematic = true;

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
        unit.PoisonTime += PoisonTime;
        Poisoner p = Instantiate(poisoner, unit.transform.position, Quaternion.identity);

        if (PoisonerTime > 0) p.LifeTimeSeconds = PoisonerTime;
        if (PoisonerPoisonTime > 0) p.PoisonTime = PoisonerPoisonTime;
    }
}

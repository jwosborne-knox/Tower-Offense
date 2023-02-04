using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperUnit : Unit
{
    public Projectile Projectile;

    public float projectileSpeed = 4f;

    public float maxHealth = 50f;
    public float health = 50f;

    public float shootRadius = 5f;
    public float shootCooldownSeconds = 4f;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFireTime();
        UpdatePoisonTime();

        if (health <= 0) Destroy(gameObject);

        UpdateDecceleratorCount();
        UpdateRangeRadius(shootRadius);
        ShowRangeIfMouseHover();
        ShootIfPossible(shootRadius, shootCooldownSeconds);

        // Turn towards closest tower
        Tuple<float, Vector3> target = GetClosestTarget();
        Vector3 directionVector = target.Item2 - transform.position;

        float degrees = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg + 180;
        transform.eulerAngles = Vector3.forward * degrees;

        healthBar.transform.position = transform.position + new Vector3((health/maxHealth-1)/2*0.6f, 0.4f, 0);
        healthBar.transform.rotation = Quaternion.identity;
    }

    public override void Shoot(Vector3 direction)
    {
        Projectile p = Instantiate(Projectile, transform.position + Vector3.back, transform.rotation);
        p.Velocity = direction.normalized * projectileSpeed * SpeedMultiplier;
        p.OwnerTag = tag;
    }

    public override void Damage(float amount)
    {
        health -= amount;
        transform.GetChild(1).GetComponent<HealthBar>().ChangeHealth(health/maxHealth);
    }
}
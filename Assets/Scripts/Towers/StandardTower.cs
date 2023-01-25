using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardTower : Tower
{
    public Projectile Projectile;
    public float ProjectileSpeed = 0.1f;
    public float maxHealth = 100f;
    public float Health = 100f;
    public float shootRadius = 3f;
    public float shootCooldownSeconds = 3f;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            // Destroy(this) only destroys the script, not the entire object
            Destroy(this.gameObject);
        }

        UpdateRangeRadius(shootRadius);
        ShowRangeIfMouseHover();
        ShootIfPossible(shootRadius, shootCooldownSeconds);
    }

    public override void Shoot(Vector3 direction)
    {
        // Vector3.back is used to change the z coordinate of the projectile so that
        // it renders on top of the tower
        Projectile p = Instantiate(Projectile, transform.position + Vector3.back, transform.rotation);
        p.Velocity = direction.normalized * ProjectileSpeed;
        p.OwnerTag = tag;
    }

    public override void Damage(float amount)
    {
        Health -= amount;
        transform.GetChild(1).GetComponent<HealthBar>().ChangeHealth(amount/maxHealth);
    }
}

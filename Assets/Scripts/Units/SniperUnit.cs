using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperUnit : Unit
{
    [SerializeField] public Projectile Projectile;
    [SerializeField] public AudioSource launchSound;
    [SerializeField] public AudioSource hitSound;

    public float projectileSpeed = 5f;

    public float maxHealth = 50f;
    public float health = 50f;
    public float speed = 1f;

    public float shootCooldownSeconds = 4f;

    //For animation
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveGoal = transform.position;
        actionRadius = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        selectionCircle.SetActive(isSelected);

        zAdjust();
        autoMoveGoalAndRotate();

        //Movement animation
        animator.SetFloat("DistToTarget", Vector3.Distance(transform.position, zAdjustedGoal));

        //Reset animation attack boolean
        animator.SetBool("IsAttacking", false);

        // For movement
        movement(moveGoal, speed);

        UpdateFireTime();
        UpdatePoisonTime();

        if (health <= 0) Destroy(gameObject);

        UpdateDecceleratorCount();
        UpdateRangeRadius(actionRadius);
        ShowRangeIfMouseHover();
        ShootIfPossible(actionRadius, shootCooldownSeconds);

        healthMeter.SetValue(health / maxHealth);
        healthMeter.transform.localRotation = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z);
    }

    public override void Shoot(Vector3 direction)
    {
        //Attack animation
        animator.SetBool("IsAttacking", true);

        launchSound.Play();

        Projectile p = Instantiate(Projectile, transform.position + Vector3.back, transform.rotation);
        p.Velocity = direction.normalized * projectileSpeed * SpeedMultiplier;
        p.OwnerTag = tag;
    }

    public override void Damage(float amount)
    {
        hitSound.Play();
        health -= amount;
    }

    public override void Heal(float amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
    }
}

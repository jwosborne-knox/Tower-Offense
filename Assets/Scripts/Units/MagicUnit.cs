using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicUnit : Unit
{
    [SerializeField] public Projectile projectile;
    [SerializeField] public AudioSource launchSound;
    [SerializeField] public AudioSource hitSound;

    public float projectileSpeed = 3f;

    public float maxHealth = 60f;
    public float health = 80f;
    public float speed = 1.3f;

    public float shootCooldownSeconds = 0.5f;
    public float shootDeviation = 10f;

    //For animation
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveGoal = transform.position;
        actionRadius = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        selectionCircle.SetActive(isSelected);

        zAdjust();
        autoMoveGoalAndRotate();

        // For movement
        movement(moveGoal, speed);

        UpdateFireTime();
        UpdatePoisonTime();

        //For calculating if movement spritesheet should animate
        animator.SetFloat("DistToTarget", Vector3.Distance(transform.position, zAdjustedGoal));

        //reset attack sprite animation boolean
        animator.SetBool("IsAttacking", false);

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
        //Play attack animation
        animator.SetBool("IsAttacking", true);
        
        launchSound.Play();
        
        Projectile p = Instantiate(projectile, transform.position + Vector3.back, transform.rotation);
        p.transform.localScale = Vector3.one * 1.5f;
        p.Velocity = Quaternion.Euler(0, 0, Random.Range(-shootDeviation, shootDeviation)) * (direction.normalized * projectileSpeed * SpeedMultiplier);
        p.OwnerTag = tag;
    }

    public override void Damage(float amount)
    {
        hitSound.Play();
        health -= amount;
    }

    public override void Heal(float amount)
    {
        health = System.MathF.Min(health + amount, maxHealth);
    }

}

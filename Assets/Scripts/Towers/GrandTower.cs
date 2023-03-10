using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandTower : Tower
{
    [SerializeField] public StandardProjectile standardProjectile;
    [SerializeField] public FireProjectile fireProjectile;
    [SerializeField] public PoisonProjectile poisonProjectile;
    [SerializeField] public LightningProjectile lightningProjectile;

    [SerializeField] public CircleCollider2D circleCollider;

    [SerializeField] public AudioSource hitSound;
    [SerializeField] public AudioSource defaultLaunchSound;
    [SerializeField] public AudioSource fireLaunchSound;

    public override Vector3 ClosestPoint(Vector3 p) => circleCollider.ClosestPoint(p);

    public float ProjectileSpeed = 8f;
    public float MaxHealth = 100f;
    public float Health = 100f;
    public float healAmount = 0.03f;
    public float hurtRadius = 2f;
    public float attractionStrength = 2f;
    public float attractionDamage = 15f;

    //animator
    public Animator animator;

    public override float ShootCooldownSeconds => 2f;
    public override float ShootRadius => 10f;
    public override int CreditReward => 150;

    private float coolDownMultiplier = 1f;
    public override float AcceleratedCooldown => coolDownMultiplier * (ShootCooldownSeconds / (accelerators + 1));

    public override void UpdateRangeRadius()
    {
        rangeSphere.transform.localScale = new Vector3(ShootRadius, 1, ShootRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Animation switch
        if (gameStatistics.levelNumber >= LevelGenerator.sniperTowerThreshold)
        {
            animator.SetInteger("CurrentState", 1);
        }
        if (gameStatistics.levelNumber >= LevelGenerator.supportTowerThreshold)
        {
            animator.SetInteger("CurrentState", 2);
        }
        if (gameStatistics.levelNumber >= LevelGenerator.accelerationTowerThreshold)
        {
            animator.SetInteger("CurrentState", 3);
        }
        if (gameStatistics.levelNumber >= LevelGenerator.fireTowerThreshold)
        {
            animator.SetInteger("CurrentState", 4);
        }
        if (gameStatistics.levelNumber >= LevelGenerator.poisonTowerThreshold)
        {
            animator.SetInteger("CurrentState", 5);
        }
        if (gameStatistics.levelNumber >= LevelGenerator.temporalTowerThreshold)
        {
            animator.SetInteger("CurrentState", 6);
        }
        if (gameStatistics.levelNumber >= LevelGenerator.attractorTowerThreshold)
        {
            animator.SetInteger("CurrentState", 7);
        }
        if (gameStatistics.levelNumber >= LevelGenerator.lightningTowerThreshold)
        {
            animator.SetInteger("CurrentState", 8);
        }

    }

    // Update is called once per frame
    void Update()
    {
      
        // No need to check if health is less than 0 because the level generator 
        // will automatically check for this

        UpdateAcceleratorCount();
        UpdateRangeRadius();
        ShowRangeIfMouseHover();
        ShootIfPossible();

        // Support + Attract Ability
        if (gameStatistics.levelNumber >= LevelGenerator.supportTowerThreshold)
        {
            SupportNearbyTowers();
        }

        if (gameStatistics.levelNumber >= LevelGenerator.attractorTowerThreshold)
        {
            AttractNearbyUnits();
        }

        healthMeter.SetValue(Health / MaxHealth);
        healthMeter.transform.localRotation = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z);
    }

    void ShootProjectile(Projectile projectile, Vector3 direction)
    {
        Projectile p = Instantiate(projectile, transform.position + Vector3.back, transform.rotation);
        p.Velocity = direction.normalized * ProjectileSpeed * ProjectileVelMultiplier;
        p.OwnerTag = tag;
    }

    void ShootStandardProjectile(Vector3 direction)
    {
        defaultLaunchSound.Play();
        ShootProjectile(standardProjectile, direction);
    }

    void SupportNearbyTowers()
    {
        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Tower"))
        {
            // Don't heal self
            if (tower.transform.position == transform.position) continue;

            Vector3 closestPoint = tower.GetComponent<Tower>().ClosestPoint(transform.position);

            float distance = Vector2.Distance(transform.position
            , new Vector2(closestPoint.x, closestPoint.y));

            if (distance <= ShootRadius)
            {
                tower.gameObject.GetComponent<Tower>().Heal(healAmount);
            }
        }
    }

    void ShootFireProjectile(Vector3 direction)
    {
        fireLaunchSound.Play();
        ShootProjectile(fireProjectile, direction);
    }

    void ShootPoisonProjectile(Vector3 direction)
    {
        defaultLaunchSound.Play();

        PoisonProjectile p = Instantiate(poisonProjectile, transform.position + Vector3.back, transform.rotation);
        p.PoisonTime /= 4f;
        p.LifetimeSeconds /= 4f;
        p.PoisonerTime = 2f;
        p.PoisonerPoisonTime = 25f;
        p.Velocity = direction.normalized * ProjectileSpeed * ProjectileVelMultiplier / 2f;
        p.OwnerTag = tag;
    }

    void AttractNearbyUnits()
    {
        // Attractor effect here
        foreach (GameObject unitObject in GameObject.FindGameObjectsWithTag("Unit"))
        {
            float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y)
            , new Vector2(unitObject.transform.position.x, unitObject.transform.position.y));

            if (distance > ShootRadius / 4) continue;

            if (distance <= hurtRadius)
            {
                unitObject.GetComponent<Unit>().Damage(attractionDamage * Time.deltaTime);
            }

            Vector3 force = (transform.position - unitObject.transform.position).normalized * attractionStrength * Time.deltaTime;
            unitObject.GetComponent<Unit>().transform.position += force;
        }
    }

    void ShootLightningProjectile(Vector3 direction)
    {
        LightningProjectile p = Instantiate(lightningProjectile, transform.position + Vector3.back, Quaternion.identity);
        p.Damage /= 4f;
        p.Piercing = 3;
        p.Zap(transform.position, transform.position + direction, "Unit");
    }

    // Will eventually randomly select from all available projectiles
    public override void Shoot(Vector3 direction)
    {
        // This shuld only ever not be 1 when in poison mode because poison is OP
        coolDownMultiplier = 1f;

        // Support Ability
        if (gameStatistics.levelNumber >= LevelGenerator.supportTowerThreshold)
        {
            SupportNearbyTowers();
        }
        
        // Projectiles
        if (gameStatistics.levelNumber >= LevelGenerator.lightningTowerThreshold)
        {
            coolDownMultiplier = 3f;
            ShootLightningProjectile(direction);
        }
        else if (gameStatistics.levelNumber >= LevelGenerator.poisonTowerThreshold)
        {
            ShootPoisonProjectile(direction);
            coolDownMultiplier = 3f;
        }
        else if (gameStatistics.levelNumber >= LevelGenerator.fireTowerThreshold)
        {
            ShootFireProjectile(direction);
        }
        else
        {
            ShootStandardProjectile(direction);
        }
    }

    public override void Damage(float amount)
    {
        hitSound.Play();
        Health -= amount;       
    }

    public override void Heal(float amount)
    {
        Health = Mathf.Min(MaxHealth, Health + amount);
    }
}

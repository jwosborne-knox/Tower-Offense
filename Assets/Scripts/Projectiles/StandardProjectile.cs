using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardProjectile : Projectile
{
    public float LifetimeSeconds = 3f;

    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        body.isKinematic = true; // disables velocity and stuff

        // Despawn if never hits anything
        Invoke("Die", LifetimeSeconds);
    }

    // Update is called once per frame
    void Update()
    {
        if (Velocity != Vector3.zero)
        {
            transform.position += Velocity * Time.deltaTime;

            // Gets the angle to rotate the sprite by by using trig ( tan angle = y/x ).
            // Offset by 90 degrees because the sprite is facing up in the png
            // Atan2: Finds the angle of a vector whose angle = y / x
            // Rad2Deg: Equal to 180 degrees / pi Radians (Converts radians to degrees)
            float degrees = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg - 90;

            // I have no idea wehat eulerAngles means, but if you set its value to x, it will
            // set the rotation to x degrees.
            // Vector3.forward: Shorthand for writing new Vector3(0, 0, 1) -- This is done because
            //  The sprite is being rotated about the z axis (imagine z axis as depth of the screen)
            //  so Vector3.forward * degrees is the same as: new Vector3(0, 0, degrees)
            transform.eulerAngles = Vector3.forward * degrees;
        }
    }
}

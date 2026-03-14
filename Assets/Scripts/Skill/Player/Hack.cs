using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Hack : SkillBase
{
    void Update()
    {
        Invoke("DestroyBullet", 10);
    }

    public override void Shoot(GameObject skillbullet, Transform firepoint)
    {
      
        GameObject bullet = Instantiate(
            skillbullet,
            firepoint.position,
            firepoint.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firepoint.forward * infor.speed;
    }


}

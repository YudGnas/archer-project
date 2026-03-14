using UnityEngine;
using System.Collections.Generic;

public class IceWorld : SkillBase
{

    void Update()
    {
        Invoke("DestroyBullet", 1f);
    }
    public override void Shoot(GameObject skillbullet, Transform firepoint)
    {

        GameObject bullet = Instantiate(
            skillbullet,
            firepoint.position,
            firepoint.rotation
        );
    }
}

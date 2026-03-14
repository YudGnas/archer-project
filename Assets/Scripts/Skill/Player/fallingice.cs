using System.Collections.Generic;
using UnityEngine;

public class fallingice : SkillBase
{
    
    void Start()
    {
        Invoke("DestroyBullet", 2);
    }

    public override void Shoot(GameObject skillbullet, Transform firepoint)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 spawnPosition = ray.GetPoint(distance);

            GameObject aoe = Instantiate(skillbullet, spawnPosition + new Vector3(0, 0.3f, 0), Quaternion.identity);
        }
    }

}

using System.Collections;
using System.Reflection;
using UnityEngine;

public class PurpleWitch : Boss
{
    public override IEnumerator Skill2() // mưa đạn
    {
        yield return new WaitForSeconds(0.5f);

        if (firePoint == null || skill2Prefab == null) yield break;

        Transform gunBarrel = firePoint;

        int half = bulletCount / 2;

        for (int i = -half; i <= half; i++)
        {
            GameObject bullet = GameObject.Instantiate(
                skill2Prefab,
                gunBarrel.position,
                Quaternion.identity
            );

            Vector3 targetPos = Player.transform.position;
            targetPos.y = gunBarrel.position.y;

            Vector3 shootDirection = (targetPos - gunBarrel.position).normalized;

            Vector3 spreadDirection =
                Quaternion.AngleAxis(i * spreadAngle, Vector3.up) * shootDirection;

            float bulletSpeed = IsPhase2() ? 45f : 25f;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = spreadDirection * bulletSpeed;
            }
        }
    }
    public override IEnumerator Skill1() // Cầu lửa khổng lồ
    {
        yield return new WaitForSeconds(0.5f);

        if (firePoint == null || skill1Prefab == null) yield break;

        Transform gunBarrel = firePoint;

        GameObject bullet = GameObject.Instantiate(
            skill1Prefab,
            gunBarrel.position,
            Quaternion.identity
        );

        Vector3 targetPos = Player.transform.position;
        targetPos.y = gunBarrel.position.y;

        Vector3 shootDirection = (targetPos - gunBarrel.position).normalized;

        Vector3 spreadDirection =
            Quaternion.AngleAxis(0, Vector3.up) * shootDirection;

        float bulletSpeed = IsPhase2() ? 45f : 25f;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = spreadDirection * bulletSpeed;
        }
    }
    public override IEnumerator Skill3()  // Mưa băng
    {


        yield return new WaitForSeconds(1f);

        float duration = 3f;        // thời gian mưa đá
        float spawnDelay = 0.2f;    // khoảng cách giữa mỗi viên
        float radius = 8f;          // bán kính quanh boss

        float timer = 0f;

        while (timer < duration)
        {
            Vector3 randomPos = Player.transform.position +
                new Vector3(
                    Random.Range(-radius, radius),
                    0,
                    Random.Range(-radius, radius)
                );


            GameObject rocks = GameObject.Instantiate(skill3prefab, randomPos, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
            timer += spawnDelay;
        }
    }

    public override IEnumerator Skill4() // đạn từ cổng không gian
    {
        yield return new WaitForSeconds(1f);

        foreach (Transform point in firePoints)
        {
            // tính hướng tới player
            Vector3 dir = (Player.transform.position - point.position).normalized;

            GameObject missile = Instantiate(skill4Prefab, point.position, Quaternion.LookRotation(dir));

            VoidSkill m = missile.GetComponent<VoidSkill>();
            if (m != null)
            {
                m.SetDirection(dir);
            }

            yield return new WaitForSeconds(0.2f); // optional delay cho đẹp
        }
    }

    public override bool IsPhase2()
    {
        if (!hasEnteredPhase2 && currentHP < enemy_Infor.maxHP / 2)
        {
            hasEnteredPhase2 = true;
            currentHP = enemy_Infor.maxHP;
            return true;
        }

        return false;
    }
}

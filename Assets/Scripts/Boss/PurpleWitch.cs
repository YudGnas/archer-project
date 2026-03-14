using System.Collections;
using UnityEngine;

public class PurpleWitch : Boss
{
    public override IEnumerator Skill2()
    {
        yield return new WaitForSeconds(0.5f);

        if (firePoint == null || fireballPrefab == null) yield break;

        Transform gunBarrel = firePoint;

        int half = bulletCount / 2;

        for (int i = -half; i <= half; i++)
        {
            GameObject bullet = GameObject.Instantiate(
                fireballPrefab,
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
    public override IEnumerator Skill1()
    {
        yield return new WaitForSeconds(0.5f);

        if (firePoint == null || fireballPrefab == null) yield break;

        Transform gunBarrel = firePoint;

        GameObject bullet = GameObject.Instantiate(
            rockprefab,
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
    public override IEnumerator Skill3()
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


            GameObject rocks = GameObject.Instantiate(aoePrefab, randomPos, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
            timer += spawnDelay;
        }
    }
}

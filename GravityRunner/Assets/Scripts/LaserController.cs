using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour
{
    private LineRenderer line;
    private BoxCollider2D col;

    public ParticleSystem laserSparks;

    public float warningDuration = 1.5f;
    public float fireDuration = 0.5f;

    public float warningWidth = 0.05f;
    public float fireWidth = 0.5f;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        col = GetComponent<BoxCollider2D>();
        col.enabled = false;

        line.SetPosition(0, new Vector3(-20, 0, 0));
        line.SetPosition(1, new Vector3(20, 0, 0));
    }

    void Start()
    {
        StartCoroutine(LaserCycle());
    }

    IEnumerator LaserCycle()
    {
        float timer = 0;
        line.startWidth = line.endWidth = warningWidth;

        if (laserSparks != null)
        {
            var emission = laserSparks.emission;
            emission.rateOverTime = 15f;
        }

        while (timer < warningDuration)
        {
            float alpha = Mathf.PingPong(Time.time * 10, 1);
            line.startColor = line.endColor = new Color(1, 0, 0, alpha * 0.3f);
            timer += Time.deltaTime;
            yield return null;
        }

        line.startWidth = line.endWidth = fireWidth;
        line.startColor = line.endColor = new Color(1, 0, 0, 1f);
        col.enabled = true;

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.laserShootSound);
        }

        if (CameraShake.instance != null) CameraShake.instance.TriggerShake();

        if (laserSparks != null)
        {
            var emission = laserSparks.emission;
            emission.rateOverTime = 300f;

            var main = laserSparks.main;
            main.startSpeed = 8f;
        }

        yield return new WaitForSeconds(fireDuration);

        line.enabled = false;
        col.enabled = false;

        if (laserSparks != null)
        {
            var emission = laserSparks.emission;
            emission.rateOverTime = 0f;
        }

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
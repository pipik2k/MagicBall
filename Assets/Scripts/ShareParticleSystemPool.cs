using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareParticleSystemPool : MonoBehaviour
{
    public static ShareParticleSystemPool Instance;
    public ParticleSystem prefab;
    public int poolSize = 20;

    private Queue<ParticleSystem> pool = new Queue<ParticleSystem>();

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        for (int i = 0; i < poolSize; i++)
        {
            ParticleSystem ps = Instantiate(prefab, transform);
            ps.gameObject.SetActive(false);
            pool.Enqueue(ps);
        }
    }

    public ParticleSystem PlayAt(Vector3 position, Color color, float size = 1f)
    {
        ParticleSystem ps = pool.Dequeue();
        ps.transform.position = position;
        var main = ps.main;
        main.startColor = color;
        main.startSize = size;
        ps.gameObject.SetActive(true);
        ps.Play();
        StartCoroutine(ReturnToPoolAfter(ps, main.duration + main.startLifetime.constantMax));
        pool.Enqueue(ps);
        return ps;
    }

    private System.Collections.IEnumerator ReturnToPoolAfter(ParticleSystem ps, float delay)
    {
        yield return new WaitForSeconds(delay);
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ps.gameObject.SetActive(false);
    }
}

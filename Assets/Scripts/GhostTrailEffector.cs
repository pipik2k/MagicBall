using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
public class GhostTrailEffector : MonoBehaviour
{
    [SerializeField] private Transform leader;
    [SerializeField] private List<Transform> followers;
    [SerializeField] private float trailDelay = 0.01f;
    [SerializeField] private int maxTrailPoints = 50;
    [SerializeField] private int ghostCount = 20;
    [SerializeField] private GameObject ghostObj;


    private NativeQueue<float3> positionQueue;
    private NativeArray<float3> positionsArray;
    private NativeArray<float3> scalesArray;
    private float timer = 0f;
    private bool isdone;
    private void Awake()
    {
        SetUpGhostObject();
        SpawnGhost();
        leader = this.transform;
        positionQueue = new NativeQueue<float3>(Allocator.Persistent);
        ghostCount = 10;
    }

    private void SetUpGhostObject()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var spriteRenderer1 = transform.GetComponentInChildrenOnly<SpriteRenderer>();
        ghostObj = new GameObject("Ghost");
        ghostObj.transform.localScale = this.transform.localScale;
        var child = new GameObject("Visual");
        child.transform.SetParent(ghostObj.transform);
        child.transform.localScale = Vector3.one * 1.03f;
        var sprite = ghostObj.AddComponent<SpriteRenderer>();
        sprite.sprite = spriteRenderer.sprite;
        sprite.color = spriteRenderer.color;
        var sprite1 = child.AddComponent<SpriteRenderer>();
        sprite1.sprite = spriteRenderer1.sprite;
        sprite1.color = spriteRenderer1.color;
        followers.Add(ghostObj.transform);
    }
    private void SpawnGhost()
    {
        for (int i = 0; i < ghostCount; i++)
        {
            var ghost = Instantiate(ghostObj, transform.position, Quaternion.identity);
            followers.Add(ghost.transform);
        }
        isdone = true;
    }
    private void OnDisable()
    {
        if (positionsArray.IsCreated)
            positionsArray.Dispose();
        if (scalesArray.IsCreated)
            scalesArray.Dispose();
        if (positionQueue.IsCreated)
            positionQueue.Dispose();
    }


    private void Update()
    {
        if (!isdone) return;

        timer += Time.deltaTime;
        if (timer >= trailDelay)
        {
            if (positionQueue.Count >= maxTrailPoints)
                positionQueue.Dequeue();

            positionQueue.Enqueue(leader.position);
            timer = 0f;
        }

        NativeArray<float3> positions = positionQueue.ToArray(Allocator.TempJob);
        int totalPositions = positions.Length;

        if (positionsArray.IsCreated) positionsArray.Dispose();
        if (scalesArray.IsCreated) scalesArray.Dispose();
        positionsArray = new NativeArray<float3>(followers.Count, Allocator.TempJob);
        scalesArray = new NativeArray<float3>(followers.Count, Allocator.TempJob);

        var job = new FollowerMoveAndScaleJob
        {
            positions = positionsArray,
            scales = scalesArray,
            leaderPosition = leader.position,
            leaderScale = leader.localScale,
            positionsBuffer = positions,
            totalPositions = totalPositions
        };

        var handle = job.Schedule(followers.Count, 1);
        handle.Complete();

        for (int i = 0; i < followers.Count; i++)
        {
            followers[i].position = positionsArray[i];
            followers[i].localScale = scalesArray[i];
        }

        positionsArray.Dispose();
        scalesArray.Dispose();
        positions.Dispose();
    }

    [BurstCompile]
    struct FollowerMoveAndScaleJob : IJobParallelFor
    {
        public NativeArray<float3> positions;
        [WriteOnly]
        public NativeArray<float3> scales;

        [ReadOnly]
        public NativeArray<float3> positionsBuffer;
        [ReadOnly]
        public int totalPositions;
        [ReadOnly]
        public float3 leaderPosition;
        [ReadOnly]
        public float3 leaderScale;

        public void Execute(int i)
        {
            int index = math.max(totalPositions - (i + 1), 0);
            Vector3 pos = positionsBuffer.Length > 0 ? positionsBuffer[index] : leaderPosition;
            pos.z = i * 0.1f;
            positions[i] = pos;
            scales[i] = leaderScale;
        }
    }
}

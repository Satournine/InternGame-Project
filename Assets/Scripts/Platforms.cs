using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    Player player;
    public float groundHeight;
    BoxCollider platformCollider;
    public float platEdge;
    public float spawnPoint = 150;
    bool didGenerate = false;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        platformCollider = GetComponent<BoxCollider>();
        groundHeight = transform.position.y + (platformCollider.size.y / 2) + 5;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.x -= player.velocity.x * Time.fixedDeltaTime;

        platEdge = transform.position.x + (platformCollider.size.x / 2);

        if(platEdge < -100)
        {
            Destroy(gameObject);
            return;
        }


        if (!didGenerate)
        {
            if (platEdge < spawnPoint)
            {
                didGenerate = true;
                generatePlatform();
            }
        }
        transform.position = pos;
    }

    void generatePlatform()
    {
        GameObject go = Instantiate(gameObject);
        BoxCollider goCollider = go.GetComponent<BoxCollider>();
        Vector3 pos = new Vector3(0, 0, 0);

        float height1 = player.jumpVelocity * player.maxHoldJumpTime;
        float t = player.jumpVelocity / -player.gravity;
        float height2 = player.jumpVelocity * t + (0.5f * (player.gravity * (t * t)));
        float maxJumpHeight = height1 + height2;
        float maxY = 0.7f * maxJumpHeight;
        maxY += groundHeight;
        float minY = -20;
        float actualY = UnityEngine.Random.Range(minY, maxY);
        pos.y = actualY- goCollider.size.y / 2;
        if (pos.y > 10f)
            pos.y = 10f;


        float t1 = t + player.maxHoldJumpTime;
        float t2 = Mathf.Sqrt((2.0f * (maxY - actualY)) / -player.gravity);
        float totalTime = t1 + t2;
        float maxX = totalTime * player.velocity.x;
        maxX *= 0.7f;
        maxX += platEdge;
        float minX = spawnPoint + 5;
        float actualX = UnityEngine.Random.Range(minX, maxX);
        pos.x = actualX + goCollider.size.x / 2;


        go.transform.position = pos;

        Platforms goGround = go.GetComponent<Platforms>();
        goGround.groundHeight = go.transform.position.y + (platformCollider.size.y / 2) + 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

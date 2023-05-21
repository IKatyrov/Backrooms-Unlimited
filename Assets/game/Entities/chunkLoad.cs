using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chunkLoad : MonoBehaviour
{
    private Generator generator;

    private Vector3 lastPos;
    private Vector3 pos;

    private int drawingDistance = 1;

    private float posX;
    private float posZ;
    // Start is called before the first frame update
    void Start()
    {
        generator = FindObjectOfType<Generator>();
        lastPos = generator.GetChunkPos(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        pos = generator.GetChunkPos(gameObject);


        if(lastPos != pos) {
            posX = pos.x;
            posZ = pos.z;
            generator.DrowChanks(lastPos.x, lastPos.z, drawingDistance, false, false);
            generator.DrowChanks(posX, posZ, drawingDistance, true, false);

            lastPos = pos;
            print("New chank");
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using System.Threading.Tasks;
using UnityEngine;

using UnityEditor;
using System;
using System.Linq;

public class Lamps : MonoBehaviour
{
    private float chunkLength;
    public int generationDistance;
    public int drawingDistance;

    public int chunkSideSize = 8;
    public Dictionary<Vector3, GameObject[,]> chunks = new Dictionary<Vector3, GameObject[,]>();//public ��� ������
    public Dictionary<Vector3, GameObject[,]> disabledChunks = new Dictionary<Vector3, GameObject[,]>();//public ��� ������

    [SerializeField] GameObject[] lamps;
    [SerializeField] GameObject mainLamp;
    public float spaceBetweenLamps;


    Vector3 lastChunkPos = new Vector3(0, 0, 0);



    void Start()
    {
        chunkLength = chunkSideSize * spaceBetweenLamps;
    }

    // Update is called once per frame
    void Update()
    {
        var chPos = GetChunkPos(transform.gameObject);
        if (chPos != lastChunkPos)
        {
            Generate(chPos.x, chPos.z, generationDistance);
            print(chPos - lastChunkPos);
            DisableChunks(chPos);
        }


        lastChunkPos = chPos;

    }
    public Vector3 GetChunkPos(GameObject obj)
    {
        float x = (float)Math.Floor(obj.transform.position.x / chunkLength) * chunkLength;
        float z = (float)Math.Floor(obj.transform.position.z / chunkLength) * chunkLength;
        return new Vector3(x, 0, z);
    }

    public void Generate(float posX, float posY, int generationDistance)
    {
        print("Generate");
        int generationSideLength = generationDistance * 2 + 1;//����� ������� �������� ����������
        float posYnow, posXnow;
        posXnow = posX - chunkLength * generationDistance;
        posYnow = posY - chunkLength * generationDistance;
        
        for (int i = 0; i < generationSideLength; i++)
        {
            for (int j = 0; j < generationSideLength; j++)
            {

                GenerateChunk(posXnow, posYnow);
                posXnow += chunkLength;
            }
            posYnow += chunkLength;
            posXnow = posX - chunkLength * generationDistance;
        }
    }

    private void GenerateChunk(float posX, float posZ)
    {
        if (!chunks.ContainsKey(new Vector3(posX, 0, posZ)))
        {
            var startPosX = posX;
            var startPosZ = posZ;

          
            
            if (disabledChunks.Count > 0)
            {
                var firstChunks = disabledChunks.FirstOrDefault();
                Vector3 keyDisabled = firstChunks.Key;
                
                GameObject[,] chunkArray = new GameObject[chunkSideSize, chunkSideSize];

                for (int i = 0; i < chunkSideSize; i++)
                {
                    for (int j = 0; j < chunkSideSize; j++)
                    {
                        chunkArray[i, j] = firstChunks.Value[i, j]; 
                        chunkArray[i, j].transform.SetLocalPositionAndRotation(new Vector3(posX, 3.97f, posZ), new Quaternion(0,0,0,0));
                        chunkArray[i, j].SetActive(true);
                        posX += spaceBetweenLamps;
                        chunkArray[i, j].transform.parent = mainLamp.transform;
                    }
                    posZ += spaceBetweenLamps;
                    posX -= chunkLength;
                }
                
                chunks.Add(new Vector3(startPosX, 0, startPosZ), chunkArray);
                disabledChunks.Remove(keyDisabled);
            }
            else
            {
                GameObject[,] chunk = new GameObject[chunkSideSize, chunkSideSize];//� ����������� 2-������ ������, ������, ��� � ���� ������� ����������, �������� ����� �������� ��� �� ����, ��� ���������� ������

                for (int i = 0; i < chunkSideSize; i++)
                {
                    for (int j = 0; j < chunkSideSize; j++)
                    {
                        GameObject lampType = lamps[UnityEngine.Random.Range(0, lamps.Length)];
                        chunk[i, j] = Instantiate(lampType, new Vector3(posX, 3.97f, posZ), new Quaternion(0,0,0,0));
                        posX += spaceBetweenLamps;
                        chunk[i, j].transform.parent = mainLamp.transform;
                    }
                    posZ += spaceBetweenLamps;
                    posX -= chunkLength;
                }
                chunks.Add(new Vector3(startPosX, 0, startPosZ), chunk);
            }

        }
    }
    
    private void DisableChunks(Vector3 chunkPos)
    {
        float minPosX = chunkPos.x - drawingDistance * chunkLength;
        float minPosZ = chunkPos.z - drawingDistance * chunkLength;
        float maxPosX = chunkPos.x + drawingDistance * chunkLength;
        float maxPosZ = chunkPos.z + drawingDistance * chunkLength;

        foreach (var chunk in chunks)
        {
            if (!(chunk.Key.x < maxPosX && chunk.Key.x > minPosX && chunk.Key.z < maxPosZ && chunk.Key.z > minPosZ))
            {
                foreach (GameObject ch in chunk.Value)
                {
                    ch.SetActive(false);
                }

                disabledChunks.TryAdd(chunk.Key, chunk.Value); //кладу в выключенные чанки

            }
            else
            {
                foreach (GameObject ch in chunk.Value)
                {
                    ch.SetActive(true);
                }
            }
        }

        //удаляю из основного словаря
        foreach (var chunk in disabledChunks)
        {
            chunks.Remove(chunk.Key);
        }
        
    }
}



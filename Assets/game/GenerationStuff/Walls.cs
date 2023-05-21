using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using System.Threading.Tasks;
using UnityEngine;

using UnityEditor;
using System;
using System.Linq;

public class Walls : MonoBehaviour
{
    private float chunkLength;
    public int generationDistance;
    public int drawingDistance;
    private List<GameObject> Items;
    public float itemSpawnChance;
    [SerializeField] List<GameObject> ItemsPrefabs;
    public int chunkSideSize = 8;
    public Dictionary<Vector3, GameObject[,]> chunks = new Dictionary<Vector3, GameObject[,]>();//public ��� ������

    [SerializeField] GameObject[] walls;
    [SerializeField] GameObject mainWall;
    public float spaceBetweenWalls;
    public float wallSpawnChance;


    Vector3 lastChunkPos = new Vector3(0, 0, 0);



    void Start()
    {
        chunkLength = chunkSideSize * spaceBetweenWalls;
        
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
        var startPosX = posX;
        var startPosZ = posZ;
        var rot = Quaternion.Euler(0f, 0, 0f);
        Dictionary<Vector3, GameObject[,]> disabledChunks = new Dictionary<Vector3, GameObject[,]>();

        if (!chunks.ContainsKey(new Vector3(posX, 0, posZ)))
        {
            Vector3 keyDisabled;

            if (disabledChunks.Count > 0)
            {
                var firstChunks = disabledChunks.First();
                keyDisabled = firstChunks.Key;
                
                GameObject[,] chunk = new GameObject[chunkSideSize, chunkSideSize];//� ����������� 2-������ ������, ������, ��� � ���� ������� ����������, �������� ����� �������� ��� �� ����, ��� ���������� ������
                List<GameObject> list = new List<GameObject>();
                for (int i = 0; i < chunkSideSize; i++)
                {
                    for (int j = 0; j < chunkSideSize; j++)
                    {
                        chunk[i, j] = firstChunks.Value[i, j]; 
                        chunk[i,j].transform.SetLocalPositionAndRotation(new Vector3(posX, 0, posZ), Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 90f), 0f));
                        chunk[i, j].SetActive(true);
                        chunk[i, j].transform.parent = mainWall.transform;
                        posX += spaceBetweenWalls;
                    }

                    posZ += spaceBetweenWalls;
                    posX -= chunkLength;
                    
                    if (UnityEngine.Random.Range(0, 100) < itemSpawnChance)
                    {
                        list.Add(Instantiate(ItemsPrefabs[UnityEngine.Random.Range(0, ItemsPrefabs.Count)], new Vector3(posX, 0, posZ), Quaternion.identity));  //������ �������� ����� ��������� ������ (    new Vector3(posX, 2 <- ��� ���, posZ))                 
                    }
                }
                chunks.Add(new Vector3(startPosX, 0, startPosZ), chunk);
                disabledChunks.Remove(keyDisabled);
            }
            else
            {
                GameObject[,] chunk = new GameObject[chunkSideSize, chunkSideSize];//� ����������� 2-������ ������, ������, ��� � ���� ������� ����������, �������� ����� �������� ��� �� ����, ��� ���������� ������
                List<GameObject> list = new List<GameObject>();
                for (int i = 0; i < chunkSideSize; i++)
                {
                    for (int j = 0; j < chunkSideSize; j++)
                    {
                        
                        if (wallSpawnChance > UnityEngine.Random.Range(0, 100))
                        {
                            GameObject lampType = walls[UnityEngine.Random.Range(0, walls.Length)];
                            chunk[i, j] = Instantiate(lampType, new Vector3(posX, 0, posZ), Quaternion.Euler(0f, UnityEngine.Random.Range(0, 2) * 90, 0f));
                            chunk[i, j].transform.parent = mainWall.transform;
                        }
                        posX += spaceBetweenWalls;
                    }
                    posZ += spaceBetweenWalls;
                    posX -= chunkLength;
                    if (UnityEngine.Random.Range(0, 100) < itemSpawnChance)
                    {
                        list.Add(Instantiate(ItemsPrefabs[UnityEngine.Random.Range(0, ItemsPrefabs.Count)], new Vector3(posX, 0, posZ), Quaternion.identity));  //������ �������� ����� ��������� ������ (    new Vector3(posX, 2 <- ��� ���, posZ))                 
                    }
                }
                chunks.Add(new Vector3(startPosX, 0, startPosZ), chunk);
            }
        /*
            GameObject[,] chunk = new GameObject[chunkSideSize, chunkSideSize];//� ����������� 2-������ ������, ������, ��� � ���� ������� ����������, �������� ����� �������� ��� �� ����, ��� ���������� ������
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < chunkSideSize; i++)
            {
                for (int j = 0; j < chunkSideSize; j++)
                {
                    if (wallSpawnChance > UnityEngine.Random.Range(0, 100))
                    {
                        GameObject lampType = walls[UnityEngine.Random.Range(0, walls.Length)];
                        chunk[i, j] = Instantiate(lampType, new Vector3(posX, 0, posZ), Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 90f), 0f));
                        chunk[i, j].transform.parent = mainWall.transform;
                    }
                    posX += spaceBetweenWalls;
                }
                posZ += spaceBetweenWalls;
                posX -= chunkLength;
                if (UnityEngine.Random.Range(0, 100) < itemSpawnChance)
                {
                    list.Add(Instantiate(ItemsPrefabs[UnityEngine.Random.Range(0, ItemsPrefabs.Count)], new Vector3(posX, 0, posZ), Quaternion.identity));  //������ �������� ����� ��������� ������ (    new Vector3(posX, 2 <- ��� ���, posZ))                 
                }
            }
            chunks.Add(new Vector3(startPosX, 0, startPosZ), chunk);
            */
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
                    if (ch != null)
                        ch.SetActive(false);
                }
            }
            else
            {
                foreach (GameObject ch in chunk.Value)
                {
                    if (ch != null)
                        ch.SetActive(true);
                }
            }
        }
    }
}

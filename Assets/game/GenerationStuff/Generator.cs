using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] List<GameObject> Rooms;
    [SerializeField] GameObject RoomEmptyPrefab;

    [SerializeField] GameObject Pers;
    public int Level1ExitChance;
    [SerializeField] GameObject Level1Exit;

    public float itemSpawnChance;
    [SerializeField] List<GameObject> ItemsPrefabs;
    public float enemySpawnChance;
    [SerializeField] GameObject EnemyPrefab;

    private List<GameObject> Items;

    public int chunkSideSize = 8; //��������� ������, � ����� ������� �����

    public int generationDistance = 8;
    public int drawingDistance = 8;

    public float chankPosX, chankPosZ;
    private Vector3 currentPos;

    private int posTracker;

    public int emptinesChanse; // ���� ������ ������ ������� (�� 0 �� 100)
    private int roomSize = 20;
    public int chunkLength; //����� ����� (�� � ��������)

    private Vector3 playerPosNow; //public ��� ������
    private Vector3 lastPlayerPos;

    public Dictionary<Vector3, GameObject[,]> chunks = new Dictionary<Vector3, GameObject[,]>();//public ��� ������
    public Dictionary<Vector3, List<GameObject>> items = new Dictionary<Vector3, List<GameObject>>();

    public int startGenerationDistance = 8;

    private bool isLoading = true;
    public int numberOfGeneratedChunks = 0;
    // Start is called before the first frame update
    void Start()
    {
        chunkLength = roomSize * chunkSideSize;
    }

    public void StartLoad(int seed)
    {
        playerPosNow = GetChunkPos(Pers);
        StartCoroutine(StartGenerate(playerPosNow.x, playerPosNow.z, startGenerationDistance));
        lastPlayerPos = playerPosNow;
        UnityEngine.Random.InitState(seed);
    }

    // Update is called once per frame
    void Update()
    {
        if(isLoading) 
            return;
        playerPosNow = GetChunkPos(Pers);
        if (playerPosNow != lastPlayerPos)
        {
            Generate(playerPosNow.x, playerPosNow.z, generationDistance);
            StartCoroutine(DrowChanks(playerPosNow.x, playerPosNow.z, drawingDistance, true, true));
            StartCoroutine(DrowChanks(playerPosNow.x, playerPosNow.z, drawingDistance, true, false));
            lastPlayerPos = playerPosNow;
        }
       
    }


    public Vector3 GetChunkPos(GameObject obj) //�������� ������, � ���������� ���� � ������� �� ����������
    {
        float x = (float)Math.Floor(obj.transform.position.x / chunkLength) * chunkLength;
        float z = (float)Math.Floor(obj.transform.position.z / chunkLength) * chunkLength;
        //float x = obj.transform.position.x % chunkLength == 0 ? (float)Math.Floor(obj.transform.position.x / chunkLength) * chunkLength : ((float)Math.Floor(obj.transform.position.x / chunkLength) + 1) * chunkLength;
        //float z = obj.transform.position.z % chunkLength == 0 ? (float)Math.Floor(obj.transform.position.z / chunkLength) * chunkLength : ((float)Math.Floor(obj.transform.position.z / chunkLength) + 1) * chunkLength;
        return new Vector3(x, 0, z);
    }


    public IEnumerator DrowChanks(float posX, float posZ, float drawingDistance, bool active, bool off)
    {
        float minPosX = posX - drawingDistance * chunkLength;
        float minPosZ = posZ - drawingDistance * chunkLength;
        float maxPosX = posX + drawingDistance * chunkLength;
        float maxPosZ = posZ + drawingDistance * chunkLength;

        foreach (var chunk in chunks)
        {
            if (chunk.Key.x <= maxPosX && chunk.Key.x >= minPosX && chunk.Key.z <= maxPosZ && chunk.Key.z >= minPosZ)
                yield return StartCoroutine(ActiveChunk(chunk.Key.x, chunk.Key.z, active));
            else if (off)
                yield return StartCoroutine(ActiveChunk(chunk.Key.x, chunk.Key.z, false));
        }
    }

    public IEnumerator ActiveChunk(float posX, float posZ, bool active)
    {
        GameObject[,] chunk;
        chunks.TryGetValue(new Vector3(posX, 0, posZ), out chunk);
        for (int i = 0; i < chunkSideSize; i++)
        {
            for (int j = 0; j < chunkSideSize; j++)
            {
                chunk[i, j].SetActive(active);
            }
        }

        List<GameObject> list;
        items.TryGetValue(new Vector3(posX, 0, posZ), out list);


        foreach (var item in list)
        {
            if (item != null)
                item.SetActive(active);
        }
        yield return null;
    }



    public void Generate(float posX, float posY, int generationDistance)
    {
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
            GameObject[,] chunk = new GameObject[chunkLength, chunkLength];//� ����������� 2-������ ������, ������, ��� � ���� ������� ����������, �������� ����� �������� ��� �� ����, ��� ���������� ������
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < chunkSideSize; i++)
            {
                for (int j = 0; j < chunkSideSize; j++)
                {
                    //����� ���� �������
                    GameObject roomType;
                    if (UnityEngine.Random.Range(0, 100) < emptinesChanse)
                    {
                        roomType = RoomEmptyPrefab;
                    }
                    else if (UnityEngine.Random.Range(0, 10000) < Level1ExitChance)
                    {
                        roomType = Level1Exit;
                    }
                    else
                    {
                        roomType = Rooms[UnityEngine.Random.Range(0, Rooms.Count)];
                    }

                    chunk[i, j] = Instantiate(roomType, new Vector3(posX, 0, posZ), Quaternion.identity);
                    //������� � ������ �������/����������
                    if (UnityEngine.Random.Range(0, 100) < itemSpawnChance)
                    {
                        list.Add(Instantiate(ItemsPrefabs[UnityEngine.Random.Range(0, ItemsPrefabs.Count)], new Vector3(posX, 0.479f, posZ), Quaternion.identity));  //������ �������� ����� ��������� ������ (    new Vector3(posX, 2 <- ��� ���, posZ))                 
                    }
                    if (UnityEngine.Random.Range(0, 100) < enemySpawnChance)       //���� ������ � ���� �� ����� ��������� � ����� ����� - �����  if  ����� ��������� else
                    {
                        Instantiate(EnemyPrefab, new Vector3(posX, 2, posZ), Quaternion.identity);  //������ �������� ����� ��������� ������ (    new Vector3(posX, 2 <- ��� ���, posZ))
                    }
                    posX += roomSize;
                }
                posZ += roomSize;
                posX -= chunkLength;
            }
            chunks.Add(new Vector3(startPosX, 0, startPosZ), chunk);
            items.Add(new Vector3(startPosX, 0, startPosZ), list);
        }        
    }



   

    //������������
    public IEnumerator StartGenerate(float posX, float posY, int generationDistance)
    {
        int generationSideLength = generationDistance * 2 + 1;//����� ������� �������� ����������
        float posYnow, posXnow;
        posXnow = posX - chunkLength * generationDistance;
        posYnow = posY - chunkLength * generationDistance;
        for (int i = 0; i < generationSideLength; i++)
        {
            for (int j = 0; j < generationSideLength; j++)
            {
                yield return StartCoroutine(StartGenerateChunk(posXnow, posYnow));
                posXnow += chunkLength;
                numberOfGeneratedChunks++;
            }
            posYnow += chunkLength;
            posXnow = posX - chunkLength * generationDistance;
            
        }
        StartCoroutine(DrowChanks(playerPosNow.x, playerPosNow.z, drawingDistance, true, true));
        isLoading = false;
    }

    private IEnumerator StartGenerateChunk(float posX, float posZ)
    {

        if (!chunks.ContainsKey(new Vector3(posX, 0, posZ)))
        {
            var startPosX = posX;
            var startPosZ = posZ;
            GameObject[,] chunk = new GameObject[chunkLength, chunkLength];//� ����������� 2-������ ������, ������, ��� � ���� ������� ����������, �������� ����� �������� ��� �� ����, ��� ���������� ������
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < chunkSideSize; i++)
            {
                for (int j = 0; j < chunkSideSize; j++)
                {
                    IEnumerator<GameObject> coroutine = StartGenerateRoom(posX, posZ);
                    yield return StartCoroutine(coroutine);
                    chunk[i, j] = coroutine.Current;
                    chunk[i, j].SetActive(false);
                    //������� � ������ �������/����������
                    if (UnityEngine.Random.Range(0, 100) < itemSpawnChance)
                    {
                        var it = Instantiate(ItemsPrefabs[UnityEngine.Random.Range(0, ItemsPrefabs.Count)], new Vector3(posX, 0.479f, posZ), Quaternion.identity);
                        list.Add(it);  //������ �������� ����� ��������� ������ (    new Vector3(posX, 2 <- ��� ���, posZ))                 
                        it.SetActive(false);
                    }
                    if (UnityEngine.Random.Range(0, 100) < enemySpawnChance)       //���� ������ � ���� �� ����� ��������� � ����� ����� - �����  if  ����� ��������� else
                    {
                        Instantiate(EnemyPrefab, new Vector3(posX, 2, posZ), Quaternion.identity);  //������ �������� ����� ��������� ������ (    new Vector3(posX, 2 <- ��� ���, posZ))
                    }
                    posX += roomSize;
                }
                posZ += roomSize;
                posX -= chunkLength;
            }
            chunks.Add(new Vector3(startPosX, 0, startPosZ), chunk);
            items.Add(new Vector3(startPosX, 0, startPosZ), list);
        }
        yield return null;
    }

    private  IEnumerator<GameObject> StartGenerateRoom(float posX, float posZ)
    {
        //����� ���� �������
        GameObject roomType;
        if (UnityEngine.Random.Range(0, 100) < emptinesChanse)
        {
            roomType = RoomEmptyPrefab;
        }
        else if (UnityEngine.Random.Range(0, 10000) < Level1ExitChance)
        {
            roomType = Level1Exit;
        }
        else
        {
            roomType = Rooms[UnityEngine.Random.Range(0, Rooms.Count)];
        }

        GameObject room = Instantiate(roomType, new Vector3(posX, 0, posZ), Quaternion.identity);

       
        yield return room;
    }

}
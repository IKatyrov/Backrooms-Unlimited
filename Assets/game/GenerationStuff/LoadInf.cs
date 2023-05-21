using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadInf : MonoBehaviour
{
    [SerializeField] GameObject startObj;
    [SerializeField] InputField seedType;

    [SerializeField] GameObject loadObj;
    [SerializeField] Text loadProgersText;
    [SerializeField] Text TimeText;
    [SerializeField] Scrollbar loadProgressScrollbar;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject UserIntf;
    private float timeElapsed;

    private Generator generator;

    private bool isStarted = false;

    private int chunkLoaded; //������� ������ ���������
    private int needChunkLoaded; //C������ ������ ����� ����� ���������
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        generator = FindObjectOfType<Generator>();
        needChunkLoaded = (generator.startGenerationDistance * 2 + 1) * (generator.startGenerationDistance * 2 + 1);
        UserIntf.SetActive(false);
        timeElapsed = 0f;
    }

    private void Update()
    {

        if (!isStarted)
        {

            return;
        }
        timeElapsed += Time.deltaTime;
        TimeText.text = "Времени прошло: "+(Mathf.Round(timeElapsed*100)/100).ToString();

        chunkLoaded = generator.numberOfGeneratedChunks;
        if(chunkLoaded >= needChunkLoaded )
        {
            panel.SetActive(false);
            UserIntf.SetActive(true);
        }
        else
        {
            loadProgressScrollbar.size = (float)((float)chunkLoaded / (float)needChunkLoaded);
            loadProgersText.text = "Загружено чанков " + chunkLoaded.ToString() + " из " + needChunkLoaded + " чанков" + "В процентах: " + Mathf.Round((float)((float)chunkLoaded / (float)needChunkLoaded)*100);
        }
    }
   public void StartGeneration()
    {
        int seed = int.Parse(seedType.text);
        startObj.SetActive(false);
        loadObj.SetActive(true);
        Cursor.visible = false;
        isStarted = true; 
        generator.StartLoad(seed);
        Cursor.lockState = CursorLockMode.Locked;
    }
}

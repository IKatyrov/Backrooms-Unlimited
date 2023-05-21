using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Stats _stats = null;
    [SerializeField] private MoveSettings _settings = null;
    private PlayerMovement playerMovement;
    [SerializeField] private Text currStaminaMeter;
    public float hunger;
    public float thirst;
    public float sanity;

    private float reducingHunger;
    private float reducingThirst;
    private float reducingSanity;

    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        startTime = Time.realtimeSinceStartup;

        hunger = _stats.maxHunger;
        thirst = _stats.maxThirst;
        sanity = _stats.maxSanity;
    }

    // Update is called once per frame
    void Update()
    {
        currStaminaMeter.text = sanity.ToString();
        reducingHunger = _stats.hungerChangeFromTime;
        reducingThirst = _stats.thirstChangeFromTime;
        reducingSanity = _stats.sanityChangeFromTime;
        if (playerMovement._moveDirection.x != 0 && playerMovement._moveDirection.y != 0)
        {
            reducingHunger += _stats.hungerChangeFromWalking;
            reducingThirst += _stats.thirstChangeFromWalking;
            
        }
        if(sanity < 50)
        {
            reducingHunger += _stats.hungerReductionFromSanity;
            reducingThirst += _stats.thirstReductionFromSanity;
        }
        else if(sanity > 90)
        {
            reducingHunger += _stats.hungerIncreaseFromSanity;
            reducingThirst += _stats.thirstIncreaseFromSanity;
        }
        if(hunger < 20)
        {
            reducingSanity += _stats.sanityChangeFromHunger;
        }
        if (thirst < 20)
        {
            reducingSanity += _stats.sanityChangeFromThirst;
        }

        float timeElapsed = Time.realtimeSinceStartup - startTime;

        if (timeElapsed >= 600f) // ������ ��� ����� 10 �������
        {
            reducingSanity += 0.004f;
        }
        else if (timeElapsed >= 1200f) // ������ ��� ����� 20 �������
        {
            reducingSanity += 0.006f;
        }
        hunger -= reducingHunger;
        thirst -= reducingThirst;
        sanity -= reducingSanity;
        //����� �������� -= 1 �� = 100, ���� ������������
        if (hunger > 101)
            hunger -= 1;
        else if (hunger < 101 && hunger > 100)
            hunger = 100;
        else if (hunger < 0)
            hunger = 0;
        if(thirst > 101)
            thirst -= 1;
        else if (thirst < 101 &&  thirst > 100)
            thirst = 100;
        else if (thirst < 0)
            thirst = 0;
        if (sanity > 101)
            sanity -= 1;
        else if(sanity < 101 && sanity > 100)
            sanity = 100;
        else if (sanity < 0)
            sanity = 0;

    }
}

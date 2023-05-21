using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable_Objects/Ststs")]

public class Stats : ScriptableObject
{
    public float maxHunger { get { return _hunger; } private set { _hunger = value; } }
    [SerializeField] private float _hunger = 100.0f;
    public float hungerChangeFromTime { get { return _hungerChangeFromTime; } private set { _hungerChangeFromTime = value; } }
    [SerializeField] private float _hungerChangeFromTime = 0f;
    public float hungerChangeFromWalking { get { return _hungerChangeFromWalking; } private set { _hungerChangeFromWalking = value; } }
    [SerializeField] private float _hungerChangeFromWalking = 0.003f;
    public float hungerChangeFromRunning { get { return _hungerChangeFromRunning; } private set { _hungerChangeFromRunning = value; } }
    [SerializeField] private float _hungerChangeFromRunning = 0.03f;
    public float hungerReductionFromSanity { get { return _hungerReductionFromSanity; } private set { _hungerReductionFromSanity = value; } } //Reduction if sainty < 50
    [SerializeField] private float _hungerReductionFromSanity = 0.007f;
    public float hungerIncreaseFromSanity { get { return _hungerIncreaseFromSanity; } private set { _hungerIncreaseFromSanity = value; } } //Increse if sainty > 90
    [SerializeField] private float _hungerIncreaseFromSanity = -0.001f;
    
    public float maxThirst { get { return _thirst; } private set { _thirst = value; } }
    [SerializeField] private float _thirst = 100.0f;
    public float thirstChangeFromTime { get { return _thirstChangeFromTime; } private set { _thirstChangeFromTime = value; } }
    [SerializeField] private float _thirstChangeFromTime = 0f;
    public float thirstChangeFromWalking { get { return _thirstChangeFromWalking; } private set { _thirstChangeFromWalking = value; } }
    [SerializeField] private float _thirstChangeFromWalking = 0.003f;
    public float thirstChangeFromRunning { get { return _thirstChangeFromRunning; } private set { _thirstChangeFromRunning = value; } }
    [SerializeField] private float _thirstChangeFromRunning = 0.007f;
    public float thirstReductionFromSanity { get { return _thirstReductionFromSanity; } private set { _thirstReductionFromSanity = value; } } //Reduction if sainty < 50
    [SerializeField] private float _thirstReductionFromSanity = 0.007f;
    public float thirstIncreaseFromSanity { get { return _thirstIncreaseFromSanity; } private set { _thirstIncreaseFromSanity = value; } } //Increse if sainty > 90
    [SerializeField] private float _thirstIncreaseFromSanity = -0.001f;

    public float maxSanity { get { return _sanity; } private set { _sanity = value; } }
    [SerializeField] private float _sanity = 100.0f;
    public float sanityChangeFromTime { get { return _sanityChangeFromTime; } private set { _sanityChangeFromTime = value; } }
    [SerializeField] private float _sanityChangeFromTime = 0.0008f;
    public float sanityChangeFromTime2 { get { return _sanityChangeFromTime2; } private set { _sanityChangeFromTime2 = value; } }
    [SerializeField] private float _sanityChangeFromTime2 = 0.0004f;
    public float sanityChangeFromTime3 { get { return _sanityChangeFromTime3; } private set { _sanityChangeFromTime3 = value; } }
    [SerializeField] private float _sanityChangeFromTime3 = 0.0002f;
    public float sanityChangeFromHunger { get { return _sanityChangeFromHunger; } private set { _sanityChangeFromHunger = value; } }
    [SerializeField] private float _sanityChangeFromHunger = 0.0008f;
    public float sanityChangeFromThirst { get { return _sanityChangeFromThirst; } private set { _sanityChangeFromThirst = value; } }
    [SerializeField] private float _sanityChangeFromThirst = 0.0004f;

}

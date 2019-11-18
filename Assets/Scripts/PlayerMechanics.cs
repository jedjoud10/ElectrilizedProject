using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMechanics : MonoBehaviour
{
    [Header("Thirst")]
    public Slider thirstbar;
    public float thirst = 100;//How much water you contain
    public float sweatingmult;//How big does the sweating cool
    public float thirstloss;//How much water do we loose when sweating
    [Header("Food")]
    public Slider foodbar;
    public float food = 100;//How much food you ate
    public float foodmult;//How big does the food heat
    public float foodloss;//How much food do we loose when heating up the body
    [Header("Heat")]
    public AnimationCurve heataltitude;
    public float sweat = 0;//The value to remove from the body heat when sweating
    public float heatfood = 0;//The value to add from the body heat when already ate food
    public float SpeedHeatLoss = 1;//How fast your body heat changes from the external heat
    public float CurrentBodyHeat;
    public float CurrentOutsideHeat;
    public float timeofday;//The time of day specified from the SunDirection.cs
    public Slider HeatBar;
    public Vector2 TempBodyRange;
    private float velheat;
    public float smoothing;
    [Header("Oxygen")]
    public AnimationCurve densityaltitude;//How much does air pressure go down when player altitude goes up
    private float oxygenvel = 0.0f;
    public float OxygenLossSpeed = 1;//How much oxygen do you loose when not breathing (hold breath)
    public float OxygenSmooth;//How much the oxygen value gets smoothed out
    public float AirDensity;//The current air pressure/density
    public float GasEmmiter;//All gas emmiters that make gases that remove oxygen
    public float CurrentOxygen;
    public Slider OxygenBar;
    public bool IsHoldingbreath;//Are we holding our breath ?
    [Header("Player Settings")]
    public float playeraltitude;
    public float playerClothingThickness = 1;
    public BiomeData[] Biomes;
    public BiomeData currentbiome;
    public float physicalEffort;
    public float physicalEffortMult;
    private PlayerController pc;
    public bool IsDead;
    public Vector2 heatDeathRange;
    public float MinOxygenBeforeDeath;
    [System.Serializable]
    public struct BiomeData
    {
        public float tempday;
        public float tempnight;
    }
    public void ChangeBiome(int newbiome)
    {
        currentbiome = Biomes[newbiome];
    }
    private void Start()
    {
        pc = GetComponent<PlayerController>();
        currentbiome = new BiomeData();
    }
    // Update is called once per frame
    void Update()
    {
        IsHoldingbreath = Input.GetKey(KeyCode.H);
        playeraltitude = gameObject.transform.position.y;
        physicalEffort = pc.effort * physicalEffortMult;
        CurrentBodyHeat = Mathf.SmoothDamp(CurrentBodyHeat,  (HeatTransfer(CurrentOutsideHeat, CurrentBodyHeat) * Time.deltaTime) + CurrentBodyHeat, ref velheat, smoothing);
        if (Time.frameCount % 60 == 0)
        {
            CurrentOutsideHeat = ExternalHeatTimeOfDay(timeofday);
        }        
        HeatBar.value = CurrentBodyHeat;
        if (!IsHoldingbreath)
        {
            AirDensity = densityaltitude.Evaluate(playeraltitude);
            CurrentOxygen = Mathf.SmoothDamp(CurrentOxygen, AirDensity - GasEmmiter, ref oxygenvel, OxygenSmooth);
        }
        else
        {
            CurrentOxygen = Mathf.SmoothDamp(CurrentOxygen, CurrentOxygen - OxygenLossSpeed * Time.deltaTime, ref oxygenvel, OxygenSmooth);
        }
        if (CurrentBodyHeat > TempBodyRange.y)
        {
            sweat = sweatingmult;
            thirst -= thirstloss;
            thirst = Mathf.Clamp(thirst, 0, 100);
        }
        else
        {
            sweat = 0;
        }
        if (CurrentBodyHeat < TempBodyRange.x)
        {
            heatfood = foodmult;
            food -= foodloss;
            food = Mathf.Clamp(food, 0, 100);
        }
        else
        {
            heatfood = 0;
        }
        OxygenBar.value = CurrentOxygen;
        thirstbar.value = thirst;
        foodbar.value = food;
        IsDead = CheckIfHeatDead() || CurrentOxygen <= 0 || thirst <= 0 || food <= 0;
    }
    public bool CheckIfHeatDead()
    {
        return (CurrentBodyHeat < heatDeathRange.x || CurrentBodyHeat > heatDeathRange.y);
    }
    public float HeatTransfer(float outsidetemp, float currenttemp)
    {
        return (SpeedHeatLoss * (outsidetemp - currenttemp) - sweat + heatfood + physicalEffort) / playerClothingThickness;
    }
    public float ExternalHeatTimeOfDay(float timeofday)
    {
        return Mathf.Lerp(currentbiome.tempday, currentbiome.tempnight, timeofday) + heataltitude.Evaluate(playeraltitude);
    }
}

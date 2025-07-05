using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Info")]
    [Space(5)]
    [SerializeField] private int days; // Done

    // population, wood, gold, food, stone, iron, tools, 

    [Header("Resources")]
    [Space(5)]

    //[SerializeField] private int population;
    [SerializeField] private int workers; // Done
    [SerializeField] private int unemployed; // Done

    [SerializeField] private int wood; // Done
    [SerializeField] private int stone; 
    [SerializeField] private int iron;
    [SerializeField] private int gold;
    [SerializeField] private int food; // Done
    [SerializeField] private int tools;

    // farm, house, iron mines, gold mines, woodcutter, blacksmith, quarry

    [Header("Buildings")]
    [Space(5)]
    [SerializeField] private int house; // 1 house takes 4 population  // Done
    [SerializeField] private int farm; // Done
    [SerializeField] private int woodcutter; // Done
    [SerializeField] private int quarry;
    [SerializeField] private int ironMines;
    [SerializeField] private int goldMines;
    [SerializeField] private int blacksmith;

    [Header("Resources Text")]
    [Space(5)]
    [SerializeField] private TMP_Text daysText;
    [SerializeField] private TMP_Text populationText;
    [SerializeField] private TMP_Text foodText;
    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text stoneText;
    [SerializeField] private TMP_Text ironText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text toolsText;

    [Header("Buildings Text")]
    [Space(5)]
    [SerializeField] private TMP_Text houseText;
    [SerializeField] private TMP_Text farmText;
    [SerializeField] private TMP_Text woodcutterText;
    [SerializeField] private TMP_Text quarryText;
    [SerializeField] private TMP_Text ironMineText;
    
    [SerializeField] private TMP_Text notificationText;

    private float timer;
    private bool isGameRunning = false;
    private Coroutine notificationCoroutine;

    public Texture2D cursorTexture;

    private void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Time.timeScale = 10;
        }


        TimeOfDay();
    }

    /// <summary>
    /// One day passes every real time minute
    /// </summary>
    private void TimeOfDay()
    {
        if (!isGameRunning) return;
        
        timer += Time.deltaTime;
        if (timer >= 60)
        {
            days++;

            WoodProduction();
            StoneProduction();

            FoodProduction();
            FoodConsumption();
            
            IncreasePopulation();

            UpdateText();

            timer = 0;
        }
    }

    public void InitializeGame()
    {
        isGameRunning = true;
        UpdateText();
    }

    #region Food

    /// <summary>
    /// Consumes one food per pop per day
    /// </summary>
    private void FoodConsumption()
    {
        food -= Population();
    }

    /// <summary>
    /// Calculates total food gathered by unemployed population
    /// </summary>
    private void FoodGathering()
    {
        food += unemployed / 2;
    }

    /// <summary>
    /// Calculates total food production
    /// </summary>
    private void FoodProduction()
    {
        FoodGathering();
        food += farm * 4; // Food from farms
    }

    #endregion

    #region Population

    private int Population()
    {
        return workers + unemployed;
    }

    /// <summary>
    /// Returns the max number of population
    /// </summary>
    /// <returns>Max population</returns>
    private int GetMaxPopulation()
    {       
        int maxPopulation = house * 4;
        return maxPopulation;
    }

    /// <summary>
    /// Increases population every 2 days
    /// </summary>
    private void IncreasePopulation()
    {
        if (days % 2 == 0)
        {
            if (GetMaxPopulation() > Population())
            {
                if (Population() + house > GetMaxPopulation()) unemployed += (GetMaxPopulation() - Population());
                else unemployed += house;
            }
        }
    }

    #endregion

    #region Production

    /// <summary>
    /// Calculates total wood production
    /// </summary>
    private void WoodProduction()
    {
        wood += woodcutter * 2;
    }

    /// <summary>
    /// Calculates total stone production
    /// </summary>
    private void StoneProduction()
    {
        stone += quarry * 3;
    }

    #endregion

    #region Build Buildings

    //TODO: Make this method a class
    //private void BuildCost(int woodCost, int stoneCost, int workerAssign)
    //{
    //    if (wood >= woodCost && stone >= stoneCost && unemployed >= workerAssign)
    //    {
    //        wood -= woodCost;            
    //        stone -= stoneCost;
    //        workers += workerAssign;
    //        unemployed -= workerAssign;
    //    }
    //}

    /// <summary>
    /// Assigns the workers to the building
    /// </summary>
    /// <param name="assignedWorkers">Number of workers we want to assign</param>
    private void AssignWorkers(int assignedWorkers)
    {        
        workers += assignedWorkers;
        unemployed -= assignedWorkers;
    }

    private bool CanAssign(int assignedWorkers)
    {
        return unemployed >= assignedWorkers;
    }

    public void BuildHouse()
    {
        if (wood >= 2)
        {
            wood -= 2;
            house++;
            UpdateText();
            notificationCoroutine = StartCoroutine(NotificationText($"You have built a house"));
        }
        else
        {
            notificationCoroutine = StartCoroutine(NotificationText($"Not enough resources to build a House, you need {2 - wood} wood"));
        }
    }

    public void BuildFarm()
    {        
        if (wood >= 10 && CanAssign(2))
        {
            wood -= 10;
            farm++;
            AssignWorkers(2);
            UpdateText();
            notificationCoroutine = StartCoroutine(NotificationText($"You have built a farm"));
        }
        else
        {
            notificationCoroutine = StartCoroutine(NotificationText($"Not enough resources to build a Farm, you need {10 - wood} wood and {2 - unemployed} workers!"));
        }
    }

    public void BuildWoodcutter()
    {
        if (wood >= 5 && iron >= 1 && CanAssign(1))
        {
            iron--;
            wood -= 5;
            woodcutter++;
            AssignWorkers(1);
            UpdateText();
            notificationCoroutine = StartCoroutine(NotificationText($"You have built a woodcutter"));
        }
        else
        {
            notificationCoroutine = StartCoroutine(NotificationText($"Not enough resources to build a Woodcutter, you need {10 - wood} wood, {1 - iron} and {1 - unemployed} workers!"));
        }
    }

    public void BuildQuarry()
    {
        if (wood >= 15 && CanAssign(4))
        {
            wood -= 15;
            quarry++;
            AssignWorkers(4);
            UpdateText();
            notificationCoroutine = StartCoroutine(NotificationText($"You have built a quarry"));
        }
        else
        {
            notificationCoroutine = StartCoroutine(NotificationText($"Not enough resources to build a Quarry, you need {15 - wood} wood and {4 - unemployed} workers!"));
        }
    }

    #endregion

    #region UIUpdates

    private void UpdateText()
    {
        // Update Resources Text
        daysText.text = days.ToString();
        populationText.text = $"Population: {Population()}/{GetMaxPopulation()}\n   Workers: {workers}\n   Unemployed: {unemployed}";
        foodText.text = $"Food: {food}";
        woodText.text = $"Wood: {wood}";
        stoneText.text = $"Stone: {stone}";
        ironText.text = $"Iron: {iron}";
        goldText.text = $"Gold: {gold}";
        toolsText.text = $"Tools: {tools}";

        // Update Buildings Text
        houseText.text = $"Houses: {house}";
        farmText.text = $"Farms: {farm}";
        woodcutterText.text = $"Woodcutters: {woodcutter}";
        quarryText.text = $"Quarries: {quarry}";
        ironMineText.text = $"Iron Mines: {ironMines}";
    }

    private IEnumerator NotificationText(string text) 
    {
        if (notificationCoroutine != null) StopCoroutine(notificationCoroutine);

        notificationText.text = text;
        yield return new WaitForSeconds(2);
        notificationText.text = string.Empty;
    }

    #endregion
}

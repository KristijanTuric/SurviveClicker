using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private TMP_Text goldMineText;
    [SerializeField] private TMP_Text blacksmithText;
    
    [Header("Buildings Production")]
    [Space(5)]
    [SerializeField] private TMP_Text houseProductionText;
    [SerializeField] private TMP_Text farmProductionText;
    [SerializeField] private TMP_Text woodcutterProductionText;
    [SerializeField] private TMP_Text quarryProductionText;
    [SerializeField] private TMP_Text ironMineProductionText;
    [SerializeField] private TMP_Text goldMineProductionText;
    [SerializeField] private TMP_Text blacksmithProductionText;
    
    [Header("Misc UI")]
    [SerializeField] private TMP_Text notificationText;
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private Image timeImage;
    [SerializeField] private GameMenuUI gameMenuUI;

    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private Button playerInputButton;
    [SerializeField] private TMP_Text leaderboardTMPText;

    [SerializeField] private SoundEffects soundEffects;

    private float timer;
    private bool isGameRunning = false;
    private Coroutine notificationCoroutine;
    
    private SaveSystemJSON saveSystem = new SaveSystemJSON();
    private List<PlayerInfo> playerInfos = new List<PlayerInfo>();

    [Header("Custom Cursor")]
    public Texture2D cursorTexture;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        playerInfos = saveSystem.LoadFromJson();
        SortLeaderboard();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time.timeScale = 1;
            speedText.text = $"Speed 1x";
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale = 2;
            speedText.text = $"Speed 2x";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale = 3;
            speedText.text = $"Speed 3x";
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Time.timeScale = 10;
            speedText.text = $"Speed 10x";
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Time.timeScale = 20;
            speedText.text = $"Speed 20x";
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
        timeImage.fillAmount = 1 - (timer / 60);
        if (timer >= 60)
        {
            days++;

            WoodProduction();
            StoneProduction();
            IronProduction();
            GoldProduction();
            ToolsProduction();

            FoodProduction();
            FoodConsumption();
            
            IncreasePopulation();

            UpdateText();

            if (HasWon())
            {
                gameMenuUI.ShowGameWinPanel();
                isGameRunning = false;
            }
            else if (HasLost())
            {
                gameMenuUI.ShowGameOverPanel();
                soundEffects.PlayGameOver();
                isGameRunning = false;
            }
            
            timer = 0;
        }
    }

    public void SavePlayerInfo()
    {
        PlayerInfo currentPlayerInfo = new PlayerInfo();
        currentPlayerInfo.daysPlayed = days;
        currentPlayerInfo.playerName = playerNameInput.text.ToUpper();
        
        playerInfos.Add(currentPlayerInfo);
        saveSystem.SaveToJson(playerInfos);

        // Disable the input and button
        playerNameInput.interactable = false;
        playerInputButton.interactable = false;

        SortLeaderboard();
    }

    private void SortLeaderboard()
    {
        playerInfos.Sort((a, b) => b.daysPlayed.CompareTo(a.daysPlayed));

        string leaderboardText = $"Leaderboard:\n";
        
        for (int i = 0; i < (playerInfos.Count <= 5 ? playerInfos.Count : 5); i++)
        {
            leaderboardText += $"{i + 1}. {playerInfos[i].playerName} - {playerInfos[i].daysPlayed}\n";
        }

        leaderboardTMPText.text = leaderboardText;
    }
    
    private bool HasLost()
    {
        if (Population() <= 0) return true;
        return false;
    }

    private bool HasWon()
    {
        if (gold >= 200) return true;
        return false;
    }

    public void InitializeGame()
    {
        soundEffects.RestartMusic();
        Time.timeScale = 1;
        speedText.text = $"Speed 1x";
        isGameRunning = true;
        UpdateText();
        
        // Enable the input and button
        playerNameInput.interactable = true;
        playerInputButton.interactable = true;
        playerNameInput.text = string.Empty;
    }

    public void PlayAgain()
    {
        gameMenuUI.ShowPlayAgain();
        ResetResources();
        InitializeGame();
    }

    public void MainMenu()
    {
        gameMenuUI.ShowMainMenu();
        ResetResources();
        InitializeGame();
    }

    private void ResetResources()
    {
        days = 0;
        
        workers = 0;
        unemployed = 20;
        
        wood = 15;
        stone = 0;
        iron = 5;
        gold = 0;
        tools = 0;
        
        food = 50;

        house = 0;
        farm = 0;
        woodcutter = 0;
        quarry = 0;
        ironMines = 0;
        goldMines = 0;
        blacksmith = 0;

        timer = 0;
    }

    #region Food

    /// <summary>
    /// Consumes food
    /// </summary>
    private void FoodConsumption()
    {
        if (food <= 0)
        {
            if (unemployed > 0)
            {
                unemployed += food;
                unemployed = Mathf.Clamp(unemployed, 0, Population());
            }
            else if (workers > 0)
            {
                workers += food;
                workers = Mathf.Clamp(workers, 0, Population());
            }
        }
        
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
        food += farm * 8; // Food from farms
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
        if (days % 2 == 0 && food >= 0)
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

    private void IronProduction()
    {
        iron += ironMines * 2;
    }

    private void GoldProduction()
    {
        gold += goldMines * 10;
    }

    private void ToolsProduction()
    {
        tools += blacksmith * 3;
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

    public void BuildIronMine()
    {
        if (wood >= 22 && stone >= 20 && CanAssign(5))
        {
            wood -= 22;
            stone -= 20;
            ironMines++;
            AssignWorkers(5);
            UpdateText();
            notificationCoroutine = StartCoroutine(NotificationText($"You have built an Iron Mine!"));
        }
        else
        {
            notificationCoroutine = StartCoroutine(NotificationText($"Not enough resources to build an Iron Mine, you need {22 - wood} wood, {20 - stone} stone and {5 - unemployed} workers!"));
        }
    }
    
    public void BuildGoldMine()
    {
        if (wood >= 22 && stone >= 20 && tools >= 20 && CanAssign(10))
        {
            wood -= 22;
            stone -= 20;
            tools -= 20;
            goldMines++;
            AssignWorkers(10);
            UpdateText();
            notificationCoroutine = StartCoroutine(NotificationText($"You have built a Gold Mine!"));
        }
        else
        {
            notificationCoroutine = StartCoroutine(NotificationText($"Not enough resources to build a Gold Mine, you need {22 - wood} wood, {20 - stone} stone, {20 - tools} tools and {10 - unemployed} workers!"));
        }
    }
    
    public void BuildBlacksmith()
    {
        if (wood >= 30 && stone >= 30 && iron >= 15 && CanAssign(5))
        {
            wood -= 30;
            stone -= 30;
            iron -= 15;
            blacksmith++;
            AssignWorkers(5);
            UpdateText();
            notificationCoroutine = StartCoroutine(NotificationText($"You have built a Blacksmith!"));
        }
        else
        {
            notificationCoroutine = StartCoroutine(NotificationText($"Not enough resources to build a Blacksmith, you need {30 - wood} wood, {30 - stone} stone, {15 - iron} iron and {5 - unemployed} workers!"));
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
        goldMineText.text = $"Gold Mines: {goldMines}";
        blacksmithText.text = $"Blacksmiths: {blacksmith}";
        
        UpdateProductionText();
    }

    private void UpdateProductionText()
    {
        houseProductionText.text = $"Capacity: {house * 4}";
        farmProductionText.text = $"Farm production: {farm * 8}";
        woodcutterProductionText.text = $"Wood production: {woodcutter * 2}";
        quarryProductionText.text = $"Stone production: {quarry * 3}";
        ironMineProductionText.text = $"Iron production: {ironMines * 2}";
        goldMineProductionText.text = $"Gold production: {goldMines * 10}";
        blacksmithProductionText.text = $"Tool production: {blacksmith * 3}";
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

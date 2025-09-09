using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

public class GameStatsConfigEditorWindow : EditorWindow
{
    // Reference fields for drag and drop
    private GameStatsConfigs _config;
    private TextAsset statsCSV;

    // UI styling
    private GUIStyle headerStyle;
    private GUIStyle boxStyle;
    private bool stylesInitialized = false;

    [MenuItem("Tools/Game Stats Editor")]
    public static void ShowWindow()
    {
        GameStatsConfigEditorWindow window = GetWindow<GameStatsConfigEditorWindow>("Game Stats Editor");
        window.minSize = new Vector2(400, 300);
        window.Show();
    }

    private void OnEnable()
    {
        stylesInitialized = false;
    }

    private void InitializeStyles()
    {
        if (stylesInitialized) return;

        headerStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 16,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.white }
        };

        boxStyle = new GUIStyle(GUI.skin.box)
        {
            padding = new RectOffset(10, 10, 10, 10),
            margin = new RectOffset(5, 5, 5, 5)
        };

        stylesInitialized = true;
    }

    private void OnGUI()
    {
        InitializeStyles();

        GUILayout.BeginVertical();

        // Header
        GUILayout.Space(10);
        GUILayout.Label("Map Importer Tool", headerStyle);
        GUILayout.Space(20);

        DrawGameStatsPanel();

        GUILayout.EndVertical();
    }

    private void DrawGameStatsPanel()
    {
        // CSV File Section ======================================
        GUILayout.BeginVertical(boxStyle);
        GUILayout.Label("Game Stats CSV Reference", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        statsCSV = (TextAsset)EditorGUILayout.ObjectField(
            "Game Stats File",
            statsCSV,
            typeof(TextAsset),
            false
        );
        if (EditorGUI.EndChangeCheck())
        {
            // Handle when CSV is changed
            if (statsCSV != null)
            {
                Debug.Log($"CSV file loaded: {statsCSV.name}");
            }
        }

        GUILayout.EndVertical();

        //=======================================================

        // Game Stats Configs Section ===========================
        GUILayout.BeginVertical(boxStyle);
        GUILayout.Label("Game Stats Configs", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        _config = (GameStatsConfigs)EditorGUILayout.ObjectField(
            "Game Level Configs",
            _config,
            typeof(GameStatsConfigs),
            false
        );
        if (EditorGUI.EndChangeCheck())
        {
            // Handle when GameStatsConfigs is changed
            if (_config != null)
            {
                Debug.Log($"GameLevelConfigs loaded: {_config.name}");
            }
        }

        GUILayout.EndVertical();

        //=======================================================

        // Process Button Section
        GUILayout.Space(20);

        // Validation
        bool canProcess = _config != null && statsCSV != null;

        if (!canProcess)
        {
            GUILayout.BeginVertical(boxStyle);
            GUILayout.Label("⚠️ Missing Requirements:", EditorStyles.boldLabel);

            if (_config == null)
                GUILayout.Label("• Drag a GameStatsConfigs ScriptableObject", EditorStyles.helpBox);
            if (statsCSV == null)
                GUILayout.Label("• Drag a CSV TextAsset file", EditorStyles.helpBox);

            GUILayout.EndVertical();
        }

        EditorGUI.BeginDisabledGroup(!canProcess);

        if (GUILayout.Button("Process Game Stats Import", GUILayout.Height(40)))
        {
            ProcessSingleConfig(_config, statsCSV);
            // Save all modified assets
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        EditorGUI.EndDisabledGroup();
    }

    /// <summary>
    /// Process a single GameStatsConfig - customize this method based on your requirements
    /// </summary>
    private void ProcessSingleConfig(GameStatsConfigs _config, TextAsset csvAsset)
    {
        Debug.Log($"Processing config: {_config.name}");

        string csvContent = csvAsset.text;

        List<string> rowConfigs = csvContent.Split('\n').ToList();

        {
            int coinWinRow = 0;
            _config.coinWinGame = TryGetValueInSimpleTitleValueStr(rowConfigs[coinWinRow]);
        }

        {
            int starterCoinRow = 1;
            _config.starterCoin = TryGetValueInSimpleTitleValueStr(rowConfigs[starterCoinRow]);
        }

        {
            int startWorkerAmountRow = 2;
            _config.startWorkerAmount = TryGetValueInSimpleTitleValueStr(rowConfigs[startWorkerAmountRow]);
        }

        {
            int startFarmTileAmountRow = 3;
            _config.startFarmTileAmount = TryGetValueInSimpleTitleValueStr(rowConfigs[startFarmTileAmountRow]);
        }

        {
            int maxColFarmTileRow = 4;
            _config.maxColFarmTile = TryGetValueInSimpleTitleValueStr(rowConfigs[maxColFarmTileRow]);
        }

        {
            int startEquipmentLvRow = 5;
            _config.startEquipmentLv = TryGetValueInSimpleTitleValueStr(rowConfigs[startEquipmentLvRow]);
        }

        {
            int maxEquipmentLevelRow = 6;
            _config.maxEquipmentLevel = TryGetValueInSimpleTitleValueStr(rowConfigs[maxEquipmentLevelRow]);
        }

        {
            int equipmentBoostRow = 7;
            _config.equipmentBoost = (float)TryGetValueInSimpleTitleValueStr(rowConfigs[equipmentBoostRow]) / 100f;
        }

        {
            int costUpgradeEquipmentRow = 8;
            _config.costUpgradeEquipment = TryGetValueInSimpleTitleValueStr(rowConfigs[costUpgradeEquipmentRow]);
        }

        {
            int storageSize = 9;
            _config.storageSize = TryGetValueInSimpleTitleValueStr(rowConfigs[storageSize]);
        }
    }

    private int TryGetValueInSimpleTitleValueStr(string titleValStr)
    {
        string[] titleValueData = titleValStr.Split(',');
        if (titleValueData.Length > 1 && int.TryParse(titleValueData[1], out var value))
        {
            return value;
        }
        return 0;
    }
}

using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralManager : MonoBehaviour
{
    public string playerName;
    public string bestPlayerName;
    public int bestScore = 0;
    public TextMeshProUGUI bestScoreText;
    public TMP_InputField playerNameInputField;

    public static GeneralManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadBestScore();
        bestScoreText.text = $"Best Score : {bestPlayerName} : {bestScore}";

        playerNameInputField.text = bestPlayerName;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndGame();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameStart();
        }
    }

    public void GameStart()
    {
        SceneManager.LoadScene(1);
        playerName = GameObject.Find("PlayerNameInput").GetComponent<TMP_InputField>().text;
    }

    public void EndGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void SetBestScore(string playerName, int score)
    {
        if (score > bestScore)
        {
            bestScore = score;
            bestPlayerName = playerName;
            if (bestScoreText != null)
            {
                bestScoreText.text = $"Best Score : {bestPlayerName} : {bestScore}";
            }
            SaveBestScore();
        }
    }

    [System.Serializable]
    class SaveData
    {
        public string bestPlayerName;
        public int bestScore;
    }

    public void SaveBestScore()
    {
        SaveData data = new SaveData();
        data.bestPlayerName = bestPlayerName;
        data.bestScore = bestScore;
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("BestScoreData", json);
    }

    public void LoadBestScore()
    {
        if (PlayerPrefs.HasKey("BestScoreData"))
        {
            string json = PlayerPrefs.GetString("BestScoreData");
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            bestPlayerName = data.bestPlayerName;
            bestScore = data.bestScore;
        }
    }

    public void ResetBestScore()
    {
        bestPlayerName = "";
        bestScore = 0;
        if (bestScoreText != null)
        {
            bestScoreText.text = $"Best Score : {bestPlayerName} : {bestScore}";
        }
        PlayerPrefs.DeleteKey("BestScoreData");
    }
}

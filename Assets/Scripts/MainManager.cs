using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public string PlayerName;
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;
    private int m_highScore;
    private bool m_GameOver = false;
    private string highScoreFilePath;

    void Start()
    {
        PlayerName = PlayerPrefs.GetString("PlayerName", "Player");
        highScoreFilePath = Application.persistentDataPath + "/HighScore.json";
        LoadHighscore();

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(1);
                SaveHighScore();
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        if (m_Points > m_highScore)
        {
            m_highScore = m_Points;
            SaveHighScore();
        }
    }

    public void SaveHighScore()
    {
        PlayerName = PlayerPrefs.GetString("PlayerName", "Player");

        HighScoreData data = new HighScoreData
        {
            highscore = m_highScore,
            playerName = PlayerName
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(highScoreFilePath, json);
    }

    public void LoadHighscore()
    {
        if (File.Exists(highScoreFilePath))
        {
            string json = File.ReadAllText(highScoreFilePath);
            HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);

            m_highScore = data.highscore;
            PlayerName = data.playerName;

        }
        else
        {
            m_highScore = 0;
            PlayerName = "Player";
        }

        HighScoreText.text = "BestScore: " + PlayerName + " : " + m_highScore;
    }


}

[System.Serializable]
public class HighScoreData
{
    public int highscore;
    public string playerName;
}

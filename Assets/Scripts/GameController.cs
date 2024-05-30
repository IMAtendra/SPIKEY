using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("SPIKES MONSTERS PREFAB")]
    public GameObject[] enemyPrefab;
    private GameObject spawnPrefab;
    private Vector3 spawnPos;

    [Header("Score")]
    public int currentScore = 0;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    private const string tag_Score = "Score";
    private const string tag_HiScore = "HighScore";
    private const string tag_BestScore = "HighScore";

    public static bool isPlaying = false;

    void Start()
    {
        if (Bird.isAlive) InvokeRepeating("SpawnSpikes", 2f, 3f);
    }

    void Update()
    {
        HighScoreSaver();
        ShowHighScore();

        if (Bird.isAlive == false) CancelInvoke();
    }

    #region SceneRestart Method
    public void SceneRestart()
    {
        Debug.Log("Game Restarted !!!");
        // FindObjectOfType<SceneTransition>().GoFade();
        FindObjectOfType<Bird>().CloseGameOverUI();
        // currentScore = 0;
        // FindObjectOfType<Bird>().isAlive = true;
    }
    #endregion

    #region To Add Touch Input in Game
    public bool WasTouchedOrClicked()
    {
        return Input.GetMouseButtonDown(0)
               || Input.GetKeyDown(KeyCode.Space)
               || Input.GetButtonUp("Jump")
               || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended);
    }
    #endregion

    #region Score Incrementer Method
        public void UpdateScore(int scoretoadd)
        {
            currentScore += scoretoadd;
            GameObject.Find(tag_Score).GetComponent<TMP_Text>().text = currentScore.ToString("0");
    
            HighScoreSaver();
        }
    #endregion

    #region ShowHighScore Method
        public void ShowHighScore()
        {
            if (highScoreText == null)
                highScoreText = GameObject.Find(tag_HiScore).GetComponent<TMP_Text>();
            else
                highScoreText.text = "BEST : " + PlayerPrefs.GetInt(tag_BestScore).ToString();
        }
    #endregion

    #region HighScoreSaver Method
    private void HighScoreSaver()
    {
        if (PlayerPrefs.GetInt(tag_BestScore) >= currentScore)
        {
            return;
        }

        PlayerPrefs.SetInt(tag_BestScore, currentScore);
    }
    #endregion

    #region SpawnSpikes Method
    public void SpawnSpikes()
    {
        var min = 0.2f;
        var max = 0.8f;
        var direction = Random.Range(-1, 2);

        // var result = (direction < 0) ? 0.05f : 0.95f;
        // print(Camera.main.ViewportToWorldPoint(new Vector3(
        //         result,
        //         Random.Range(min, max),
        //         10f)));


        if (direction < 0)
        {
            spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(
                0.05f,
                Random.Range(min, max),
                10f));

            spawnPrefab = Instantiate(enemyPrefab[0],
                         spawnPos,
                         enemyPrefab[0].transform.rotation);
        }
        else
        {
            spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(
                0.95f,
                Random.Range(min, max),
                10f));

            spawnPrefab = Instantiate(enemyPrefab[1],
                         spawnPos,
                         enemyPrefab[1].transform.rotation);
        }

        if (spawnPrefab != null)
        {
            // Debug.Log(spawnPrefab.name + " is Eliminate");
            Destroy(spawnPrefab, 1f);
        }

    }
    #endregion

}
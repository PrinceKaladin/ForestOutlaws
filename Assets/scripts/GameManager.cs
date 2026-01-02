using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public enum GameState { Start, Ready, Shooting, Reset }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public Button startButton;
    public GameObject bowGO;
    public GameObject timingBarGO;
    public GameObject tapText;
    public Text scoreText;
    public TimingManager timingManager;
    public arrowShooter arrowShooter;
    public AudioClip ready;
    public AudioClip shoot;
    public Text triestext;
    private int totalScore = 0;
    private GameState state = GameState.Start;
    int tries = 10;
    void Awake()
    {
        Instance = this;
        scoreText.text = "SCORE\n0";
    }

    void Start()
    {
        startButton.onClick.AddListener(OnStartClick);
        SetState(GameState.Start);
    }

    void Update()
    {
        if (state == GameState.Ready && Input.GetMouseButtonDown(0))
        {
            if (PlayerPrefs.GetInt("sound", 1) == 1)  this.GetComponent<AudioSource>().PlayOneShot(shoot);
            Shoot();
        }
    }

    void OnStartClick()
    {
        if (PlayerPrefs.GetInt("sound",1)==1) this.GetComponent <AudioSource>().PlayOneShot(ready);
        SetState(GameState.Ready);
    }

    void Shoot()
    {
        float accuracy = timingManager.GetAccuracy();
        SetState(GameState.Shooting);
        arrowShooter.ShootWithAccuracy(accuracy);
    }

    public void AddScore(int points)
    {
        totalScore += points;
        scoreText.text = "SCORE\n" + totalScore;
        PlayerPrefs.SetInt("lastscore",totalScore);
        tries--;
        triestext.text = "Shots: " + tries.ToString();
        if (tries == 0) {
            SceneManager.LoadScene(3);
        }
    }

    public void ShotComplete()
    {
        // Задержка 2 сек после попадания, потом новый раунд
        Invoke(nameof(SetStateReady), 2f);
    }

    void SetStateReady()
    {
        SetState(GameState.Ready);
    }

    void SetState(GameState newState)
    {
        state = newState;
        switch (state)
        {
            case GameState.Start:
                startButton.gameObject.SetActive(true);
                bowGO.SetActive(false);
                timingBarGO.SetActive(false);
                tapText.gameObject.SetActive(false);
                break;
            case GameState.Ready:
                startButton.gameObject.SetActive(false);
                bowGO.SetActive(true);
                timingBarGO.SetActive(true);
                tapText.gameObject.SetActive(true);
                timingManager.StartTiming();
                break;
            case GameState.Shooting:
                timingManager.StopTiming();
                tapText.gameObject.SetActive(false);
                break;
        }
    }
}
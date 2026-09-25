using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Parameters")]
    
    public Vector2 platformSpacing;
    public float difficultyRamp;
    public float difficultyRampInterval;
    public float playerJumpSpeed;

    [Space(20)]

    public CameraScroll cameraScroll;
    public Transform player;
    public Text scoreText;
    public Text platformsText;
    public Text goScoreText;
    public Text goBestText;
    public InputField wordInputField;
    public GameObject platformPrefab;
    public GameObject gameOverPanel;
    public Image pauseButton;
    public Sprite[] pauseImages;

    public AudioSource jumpCorrect;
    public AudioSource jumpIncorrect;
    public AudioSource scoreBoardFail;

    private string[] words;
    private int currentWord;
    private Transform currentPlatform;

    private Animator playerAnim;

    private Vector2 playerPos;
    private Vector2 platformPos;

    private int score;
    private int best;
    private int platforms;
    private bool jumping = false;
    private bool gameOver = false;

    private void Awake()
    {
        cameraScroll.SetPlatformSpacing(platformSpacing);
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        playerAnim = player.GetChild(0).GetComponent<Animator>();
        playerAnim.speed = playerJumpSpeed;

        platformPos = new Vector2(0, -2);
        playerPos = player.position;

        LoadWordsFromCSV();
        Shuffle(words);
        currentWord = 0;
        score = 0;
        scoreText.text = score.ToString();
        best = PlayerPrefs.GetInt("best", 0);
        goBestText.text = best.ToString();
        platforms = 0;
        platformsText.text = platforms.ToString();

        currentPlatform = CreateNewPlatform(platformPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            player.position = Vector2.Lerp(player.position, new Vector2(playerPos.x, player.position.y), Time.deltaTime * 4);
            return;
        }
        

        if (!wordInputField.isFocused)
        {
            wordInputField.Select();
        }

        player.position = Vector2.Lerp(player.position, playerPos, Time.deltaTime * 4);
    }

    public void CheckAnswer()
    {
        string answer = RemoveChars(wordInputField.text);

        if (answer != "")
        {
            if (answer == words[currentWord])
            {
                jumpCorrect.Play();

                score += 150;
                scoreText.text = score.ToString();

                platforms += 1;
                platformsText.text = platforms.ToString();

                if (score > best)
                {
                    best = score;
                    PlayerPrefs.SetInt("best", best);
                    goBestText.text = best.ToString();
                }

                //looping and shuffling again when all words reached
                if (currentWord >= words.Length - 1)
                {
                    currentWord = 0;
                    Shuffle(words);
                }
                else
                {
                    currentWord++;
                }

                platformPos += platformSpacing;
                MovePlayer(false);
                currentPlatform = CreateNewPlatform(platformPos);
                cameraScroll.MoveToPos(playerPos);

                //speeding up game
                if (score % difficultyRampInterval == 0 && score != 0)
                {
                    cameraScroll.SpeedUp(difficultyRamp);
                }

                StartCoroutine(WaitForJump());
            }
            else
            {
                jumpIncorrect.Play();

                MovePlayer(true);
                player.GetComponent<Rigidbody2D>().gravityScale = 1;
                currentPlatform.GetComponent<Animator>().SetTrigger("fall");
                GameOver();
            } 
        }
    }

    void MovePlayer(bool dead)
    {   
        if (dead)
        {
            playerAnim.SetTrigger("highJump");
        }
        else
        {
            playerAnim.SetTrigger("jump");
        }
        playerPos = currentPlatform.GetChild(0).transform.position;
    }

    Transform CreateNewPlatform(Vector2 pos)
    {
        GameObject newPlatform = Instantiate(platformPrefab, pos, Quaternion.identity);
        newPlatform.transform.GetChild(1).GetComponent<TextMeshPro>().text = words[currentWord];

        return newPlatform.transform;
    }

    public void GameOver()
    {
        scoreBoardFail.Play();

        gameOver = true;
        Camera.main.GetComponent<FollowPlayer>().StopFollowing();
        cameraScroll.StopMoving();
        wordInputField.gameObject.SetActive(false);
        goScoreText.text = score.ToString();

        gameOverPanel.SetActive(true);
    }

    public void PauseGame()
    {
        if (Time.timeScale > 0)
        {
            pauseButton.sprite = pauseImages[1];
            wordInputField.interactable = false;
            Time.timeScale = 0;
        }
        else
        {
            pauseButton.sprite = pauseImages[0];
            wordInputField.interactable = true;
            wordInputField.Select();
            wordInputField.ActivateInputField();
            Time.timeScale = 1;
        }
    }

    IEnumerator WaitForJump()
    {
        jumping = true;

        wordInputField.text = "";
        wordInputField.interactable = false;

        yield return new WaitForSeconds(0.75f);

        jumping = false;

        wordInputField.interactable = true;
        wordInputField.text = "";
        wordInputField.Select();
        wordInputField.ActivateInputField();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        
        SceneManager.LoadScene(0);
    }

    //function for loading all words from CSV file
    void LoadWordsFromCSV()
    {
        TextAsset matrix = Resources.Load<TextAsset>("data");
        string[] lines = matrix.text.Split('\n');
        words = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] == "")
                continue;

            words[i] = RemoveChars(lines[i].ToLower());
        }
    }

    //function to shuffle words
    void Shuffle(string[] w)
    {
        string temp;

        for (int i = 0; i < w.Length; i++)
        {
            int rnd = UnityEngine.Random.Range(i, w.Length);
            temp = w[rnd];
            w[rnd] = w[i];
            w[i] = temp;
        }
    }

    //function to remove extra characters from text
    private string RemoveChars(string text)
    {
        string word = "";
        for (int k = 0; k < text.Length; k++)
        {
            if (Char.IsLetter(text[k]) || text[k] == ' ')
            {
                word += text[k];
            }
        }

        return word;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CupGameManager : MonoBehaviour
{
    public GameObject CupPrefab;
    public GameObject BallPrefab;
    public Text ScoreText;
    public GameObject[] Hearts; 
    public GameObject GameOverPanel;
    public GameObject PauseMenu;
    private int _score = 0;
    private int _lives = 3;
    private List<Cup> _cups = new List<Cup>();
    private GameObject _ball;
    private Cup _cupWithBall;
    private bool _isAcceptingInput = false;
    private void Start()
    {
        if (GameOverPanel != null) GameOverPanel.SetActive(false);
        if (PauseMenu != null) PauseMenu.SetActive(false);
        UpdateUI();
        StartCoroutine(StartRound(true));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenu != null && PauseMenu.activeSelf)
                ResumeGame();
            else
                PauseGame();
        }
    }
    private IEnumerator StartRound(bool initialSetup)
    {
        _isAcceptingInput = false;
        if (initialSetup)
        {
            foreach (var c in _cups) Destroy(c.gameObject);
            _cups.Clear();
            if (_ball != null) Destroy(_ball);
            int numCups = GameSettings.Difficulty + 2; 
            float spacing = numCups == 4 ? 3.2f : 4.5f;
            float startX = -((numCups - 1) * spacing) / 2.0f; 
            for(int i = 0; i < numCups; i++)
            {
                var go = Instantiate(CupPrefab, new Vector3(startX + i * spacing, 0, 0), Quaternion.identity);
                var cup = go.GetComponent<Cup>();
                cup.GameManager = this;
                _cups.Add(cup);
            }
            _ball = Instantiate(BallPrefab);
            _cupWithBall = _cups[Random.Range(0, _cups.Count)];
            foreach (var c in _cups)
            {
                var sr = c.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    if (c == _cupWithBall && GameSettings.SAMDebugMode)
                        sr.color = Color.green;
                    else
                        sr.color = Color.white;
                }
            }
            _ball.transform.position = new Vector3(_cupWithBall.transform.position.x, -1.1f, _cupWithBall.transform.position.z);
            _ball.transform.SetParent(null);
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(_cupWithBall.LiftUp());
            yield return new WaitForSeconds(1.0f);
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
        }
        foreach (var c in _cups)
        {
            if (c.transform.position.y > 0.1f)
            {
                StartCoroutine(c.MoveDown());
            }
        }
        yield return new WaitForSeconds(0.3f); 
        _ball.transform.SetParent(_cupWithBall.transform, true);
        int shuffleCount = 5 + _score * 2; 
        float shuffleSpeed = Mathf.Max(0.15f, 0.5f - (_score * 0.05f));
        for (int i = 0; i < shuffleCount; i++)
        {
            int idx1 = Random.Range(0, _cups.Count);
            int idx2 = Random.Range(0, _cups.Count);
            while (idx1 == idx2) idx2 = Random.Range(0, _cups.Count);
            Cup c1 = _cups[idx1];
            Cup c2 = _cups[idx2];
            Vector3 pos1 = c1.transform.position;
            Vector3 pos2 = c2.transform.position;
            Coroutine m1 = StartCoroutine(c1.MoveTo(pos2, shuffleSpeed));
            Coroutine m2 = StartCoroutine(c2.MoveTo(pos1, shuffleSpeed));
            yield return m1;
            yield return m2;
        }
        _isAcceptingInput = true;
    }
    public void OnCupClicked(Cup clickedCup)
    {
        if (!_isAcceptingInput) return;
        _isAcceptingInput = false;
        StartCoroutine(RevealResult(clickedCup));
    }
    private IEnumerator RevealResult(Cup clickedCup)
    {
        _ball.transform.SetParent(null, true);
        _ball.transform.position = new Vector3(_cupWithBall.transform.position.x, -1.1f, _cupWithBall.transform.position.z);
        yield return StartCoroutine(clickedCup.LiftUp());
        if (clickedCup == _cupWithBall)
        {
            _score++;
            UpdateUI();
            StartCoroutine(StartRound(false));
        }
        else
        {
            _lives--;
            UpdateUI();
            yield return StartCoroutine(_cupWithBall.LiftUp());
            if (_lives <= 0)
            {
                yield return new WaitForSeconds(2.0f);
                if (GameOverPanel != null) GameOverPanel.SetActive(true);
            }
            else
            {
                StartCoroutine(StartRound(false));
            }
        }
    }
    private void UpdateUI()
    {
        if(ScoreText != null) ScoreText.text = "Score: " + _score;
        for(int i=0; i<Hearts.Length; i++)
        {
            if (Hearts[i] != null)
                Hearts[i].SetActive(i < _lives);
        }
    }
    public void ResumeGame() { if(PauseMenu) PauseMenu.SetActive(false); Time.timeScale = 1; }
    public void PauseGame() { if(PauseMenu) PauseMenu.SetActive(true); Time.timeScale = 0; }
    public void RestartGame() { Time.timeScale = 1; SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    public void QuitToMenu() { Time.timeScale = 1; SceneManager.LoadScene("MainMenu"); }
}

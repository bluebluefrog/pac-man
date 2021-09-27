using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get {
            return _instance;
        }
    }

    public GameObject pacman;
    public GameObject goman1;
    public GameObject goman2;
    public GameObject goman3;
    public GameObject goman4;

    public bool isSuperPacman = false;

    public List<int> usingIndex = new List<int>();
    public List<int> rawIndex = new List<int> {0,1,2,3};
    private List<GameObject> pacdotGos = new List<GameObject>();
    private int pacdotNum = 0;
    private int nowEat = 0;
    public int score = 0;
    public Text remainText;
    public Text nowText;
    public Text scoreText;

    public GameObject gamestartPanel;
    public GameObject gameoverPrefab;
    public GameObject gamePanel;
    public GameObject winPrefab;
    public AudioClip startClip;



    private void Awake()
    {
        _instance = this;
        int tempCount = rawIndex.Count;
        for (int i = 0; i < tempCount; i++) {
           int tempIndex= Random.Range(0, rawIndex.Count);
            usingIndex.Add(rawIndex[tempIndex]);
            rawIndex.RemoveAt(tempIndex);
        }

        foreach (Transform t in GameObject.Find("Maze").transform)
        {
            pacdotGos.Add(t.gameObject);
        }
        pacdotNum = GameObject.Find("Maze").transform.childCount;
    }

    private void Start()
    {
        SetGameState(false);
    }

    private void Update()
    {
        if (nowEat == pacdotNum && pacman.GetComponent<PacmanMove>().enabled != false) 
        {
            gamePanel.SetActive(false);
            Instantiate(winPrefab);
            StopAllCoroutines();
            SetGameState(false);
        }
        if (nowEat == pacdotNum)
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(0);
            }
        }
        if (gamePanel.activeInHierarchy)
        {
            remainText.text = "Remain:" + (pacdotNum - nowEat);
            nowText.text = "Eaten:" + (nowEat);
            scoreText.text = "Score:" + (score);
        }
    }

    public void OnStartButton()
    {
        SetGameState(true);
        Invoke("CreateSuperpacdot", 5f);
        AudioSource.PlayClipAtPoint(startClip, new Vector3(0,0,-5));
        gamestartPanel.SetActive(false);
        gamePanel.SetActive(true);
        Invoke("PlayAudio", 5f);
    }

    public void PlayAudio() {
        GetComponent<AudioSource>().Play();
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void OnEatPacdot(GameObject go)
    {
        nowEat++;
        score += 100;
        pacdotGos.Remove(go);  
    }

    public void OnEatSuperPacdot() {
        score += 200;
        Invoke("CreateSuperpacdot", 20f);
        ChangeEnemy();
        isSuperPacman = true;
        StartCoroutine(RecoverEnemy());
    }

    IEnumerator RecoverEnemy() {
        yield return new WaitForSeconds(10f);
        UnFreezeEnemy();
        UnChangeEnemy();
        isSuperPacman = false;

    }

    private void CreateSuperpacdot() 
    {
        if (pacdotGos.Count < 5) 
        {
            return;
        }

       int tempIndex= Random.Range(0, pacdotGos.Count);
        pacdotGos[tempIndex].transform.localScale = new Vector3(3, 3, 3);
        pacdotGos[tempIndex].GetComponent<Pacdot>().isSuperPacdot = true;
    }

    private void ChangeEnemy()
    {
        goman1.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        goman2.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        goman3.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        goman4.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
    }

    private void UnChangeEnemy()
        {
        goman1.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        goman2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        goman3.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        goman4.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }

    public void FreezeEnemy(GameObject go) {

        go.GetComponent<GhostMove>().enabled = false;

    }

    public void UnFreezeEnemy() {
        goman1.GetComponent<GhostMove>().enabled = true;
        goman2.GetComponent<GhostMove>().enabled = true;
        goman3.GetComponent<GhostMove>().enabled = true;
        goman4.GetComponent<GhostMove>().enabled = true;
    }

    private void SetGameState(bool state) {

        pacman.GetComponent<PacmanMove>().enabled = state;
        goman1.GetComponent<GhostMove>().enabled = state;
        goman2.GetComponent<GhostMove>().enabled = state;
        goman3.GetComponent<GhostMove>().enabled = state;
        goman4.GetComponent<GhostMove>().enabled = state;
    }
}

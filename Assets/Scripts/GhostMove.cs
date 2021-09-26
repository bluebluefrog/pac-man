using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GhostMove : MonoBehaviour
{
    public GameObject[] wayPointsGos;
    public float speed = 0.2f;

    private List<Vector3> wayPoints = new List<Vector3>();
    private int index=0;
    private Vector3 headPos;

    private void Start()
    {
        headPos = transform.position+new Vector3(0,3,0);
        LoadAPath(wayPointsGos[GameManager.Instance.usingIndex[GetComponent<SpriteRenderer>().sortingOrder - 2]]);
    }

    private void FixedUpdate()
    {
        if (transform.position != wayPoints[index])
        {
            Vector2 temp = Vector2.MoveTowards(transform.position, wayPoints[index], speed);
            GetComponent<Rigidbody2D>().MovePosition(temp);
        }
        else 
        {
            index++;
            if (index >= wayPoints.Count)
            {
                index = 0;
                LoadAPath(wayPointsGos[Random.Range(0, wayPointsGos.Length)]);
            }
            
        }

        Vector2 dir = wayPoints[index] - transform.position;
        GetComponent<Animator>().SetFloat("Dirx", dir.x);
        GetComponent<Animator>().SetFloat("Diry", dir.y);
    }

    private void LoadAPath(GameObject go) {
        wayPoints.Clear();
        foreach (Transform t in go.transform)
        {
            wayPoints.Add(t.position);
        }
        wayPoints.Insert(0, headPos);
        wayPoints.Add(headPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Pacman")
        {

            if (GameManager.Instance.isSuperPacman)
            {
                transform.position = headPos - new Vector3(0, 3, 0);
                index = 0;
                GameManager.Instance.FreezeEnemy(this.gameObject);
                GameManager.Instance.score +=300;
            }
            else
            {
                collision.gameObject.SetActive(false);
                GameManager.Instance.gamePanel.SetActive(false);
                Instantiate(GameManager.Instance.gameoverPrefab);
                Invoke("Restart", 3f);
            }
        }

    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }
}

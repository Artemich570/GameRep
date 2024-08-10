using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public string teamTurn;
    public bool isMoving;
    public List<string> teams;
    private int firstindex;
    public bool newTurn;
    public bool firstMove;
    public bool endTurn;
    public int movesCounter;
    public Dictionary<string, int> piretesNum;

    public GameObject currentPirate;
    public GameObject currentShip;
    void Start()
    {
        movesCounter = 0;
        firstMove = true;
        teams = DataHolder.teams;
        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i] != null)
            {
                firstindex = i;
                break;
            }
        }
        teamTurn = teams[firstindex];

        isMoving = true;
        newTurn = false;
        endTurn = false;

        piretesNum = new Dictionary<string, int>()
        {
            { "Black", 3 },
            { "White", 3 },
            { "Red", 3 },
            { "Yellow", 3 }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (movesCounter >= 10)
        {
            currentPirate.GetComponent<Pirate>().Kill();
            movesCounter = 0;
            isMoving = false;
            RemoveGlowing();
        }
    }

    //функция для смены хода
    public void NextTurn()
    {
        CheckEnd();
        movesCounter = 0;
        int teamIndex = teams.IndexOf(teamTurn);
        for (int i = 1; i < teams.Count; i++)
        {
            if (teamIndex + i < teams.Count)
            {
                if (teams[teamIndex + i] != null)
                {
                    teamTurn = teams[teamIndex + i];
                    break;
                }
            }
            else if (teamIndex + i >= teams.Count)
            {
                teamTurn = teams[firstindex];
                break;
            }
        }
        firstMove = true;

        int pirates_num = 0;
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(500, 500, 500), Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Pirate>() != null)
            {
                collider.GetComponent<Pirate>().underAttack = false;
                if (collider.GetComponent<Pirate>().skipTurn > 0 && collider.GetComponent<Pirate>().team == teamTurn)
                collider.GetComponent<Pirate>().skipTurn--;

                if (collider.GetComponent<Pirate>().team == teamTurn)
                {
                    if (!collider.GetComponent<Pirate>().isTrapped && collider.GetComponent<Pirate>().skipTurn == 0)
                    {
                        pirates_num++;
                    }
                }
            }
        }
        endTurn = false;
        if (pirates_num == 0)
        {
            NextTurn();
        }
    }

    public void RemoveGlowing()
    {
        GameObject tile;
        Collider[] collidersToDel = Physics.OverlapBox(transform.position, new Vector3(500, 500, 500), Quaternion.identity);
        foreach (Collider collider in collidersToDel)
        {
            tile = collider.gameObject;
            if (tile.GetComponent<Tile>() != null)
            {
                tile.GetComponent<Tile>().isReady = false;
            }
        }
    }
    public void CheckEnd()
    {
        bool allTiles = true;
        bool allCoins = true;
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(500, 500, 500), Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Tile>() != null)
            {
                if (!collider.GetComponent<Tile>().isOpened && collider.GetComponent<Tile>().tileType != "Water")
                {
                    allTiles = false;
                    break;
                }
            }
            if (collider.GetComponent<Pirate>() != null)
            {
                if (collider.GetComponent<Pirate>().withCoin)
                {
                    allCoins = false;
                    break;
                }
            }
            if (collider.name == "CoinPrefab(Clone)")
            {
                allCoins = false;
                break;
            }
        }

        if (allTiles && allCoins)
        {
            print("Game Over");
            SceneManager.LoadScene("Menu");
        }
    }
}
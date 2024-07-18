using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraGUI : MonoBehaviour
{
    private SceneController scene;
    public bool canTakeCoin;
    public bool candropCoin;
    public bool canRevive;
    public GameObject activePlayer;
    public TMP_Text black, white, red, yellow, team, movesLeft;
    public Transform nextMove, move, revive, takeCoin, dropCoin;
    GUIStyle style;
    [SerializeField] private GameObject redPiratPrefab, blackPiratPrefab, whitePiratPrefab, yellowPiratPrefab;
    private string teamName;
    // Start is called before the first frame update
    void Start()
    {
        scene = GameObject.Find("SceneController").GetComponent<SceneController>();
        black = transform.GetChild(4).GetComponent<TMP_Text>();
        white = transform.GetChild(5).GetComponent<TMP_Text>();
        red = transform.GetChild(6).GetComponent<TMP_Text>();
        yellow = transform.GetChild(7).GetComponent<TMP_Text>();
        team = transform.GetChild(8).GetComponent<TMP_Text>();
        movesLeft = transform.GetChild(9).GetComponent<TMP_Text>();
        nextMove = transform.GetChild(10);
        move = transform.GetChild(11);
        revive = transform.GetChild(12);
        takeCoin = transform.GetChild(13);
        dropCoin = transform.GetChild(14);
        style = new GUIStyle();
        style.fontSize = 30;
    }

    // Update is called once per frame
    void Update()
    {
        black.text = DataHolder.TeamsCoins["Black"].ToString();
        white.text = DataHolder.TeamsCoins["White"].ToString();
        red.text = DataHolder.TeamsCoins["Red"].ToString();
        yellow.text = DataHolder.TeamsCoins["Yellow"].ToString();
        
        switch (scene.teamTurn)
        {
            case "Black":
                teamName = "Чёрных"; break;
            case "White":
                teamName = "Белых"; break;
            case "Red":
                teamName = "Красных"; break;
            case "Yellow":
                teamName = "Жёлтых"; break;
        }
        team.text = "Ход команды "+ teamName;
        if (scene.currentPirate != null)
        {
            if (scene.currentPirate.GetComponent<Pirate>().onTurntable > 0)
            {
                movesLeft.text = "Осталось ходов: "+scene.currentPirate.GetComponent<Pirate>().onTurntable.ToString();
            }
            else
            {
                movesLeft.text = "";
            }
        }
        else { movesLeft.text = ""; }

        if (!scene.isMoving)
        {
            nextMove.gameObject.SetActive(true);
        }
        else { nextMove.gameObject.SetActive(false); }

        if (scene.currentPirate != null)
        {
            if (canTakeCoin)
            {
                takeCoin.gameObject.SetActive(true);
            }
            else { takeCoin.gameObject.SetActive(false); }

            if (scene.currentPirate.GetComponent<Pirate>().withCoin && candropCoin)
            {
                dropCoin.gameObject.SetActive(true);
            }
            else { dropCoin.gameObject.SetActive(false); }

            if (scene.currentPirate.GetComponent<Pirate>().onTurntable > 0)
            {
                move.gameObject.SetActive(true);
            }
            else { move.gameObject.SetActive(false); }

            if (scene.currentPirate.GetComponent<Pirate>().CheckCurrentTile().GetComponent<Tile>() != null)
            {
                if (scene.currentPirate.GetComponent<Pirate>().CheckCurrentTile().GetComponent<Tile>().tileType == "Revive Fort" && scene.piretesNum[scene.currentPirate.GetComponent<Pirate>().team] < 3)
                {
                    revive.gameObject.SetActive(true);
                }
                else { revive.gameObject.SetActive(false); }
            }
            else { revive.gameObject.SetActive(false); }
        }
        else
        {
            takeCoin.gameObject.SetActive(false);
            dropCoin.gameObject.SetActive(false);
            move.gameObject.SetActive(false);
            revive.gameObject.SetActive(false);
        }
    }

    public void NextMove()
    {
        scene.NextTurn();
        scene.isMoving = true;
    }
    public void TakeCoin()
    {
        scene.currentPirate.GetComponent<Pirate>().TakeCoin();
    }
    public void DropCoin()
    {
        scene.currentPirate.GetComponent<Pirate>().DropCoin();
    }
    public void Move()
    {
        scene.currentPirate.GetComponent<Pirate>().onTurntable--;
        scene.currentPirate = null;
        scene.isMoving = false;
        scene.endTurn = true;
    }
    public void Revive()
    {
        GameObject cur_tile = scene.currentPirate.GetComponent<Pirate>().CheckCurrentTile();
        GameObject newPirate = null;
        if (scene.currentPirate.GetComponent<Pirate>().team == "Black")
        {

            newPirate = Instantiate(blackPiratPrefab, new Vector3(47, -100, 13), Quaternion.identity, GameObject.Find("Board").transform);
        }
        else if (scene.currentPirate.GetComponent<Pirate>().team == "White")
        {
            newPirate = Instantiate(whitePiratPrefab, new Vector3(47, -100, 13), Quaternion.identity, GameObject.Find("Board").transform);
        }
        else if (scene.currentPirate.GetComponent<Pirate>().team == "Red")
        {
            newPirate = Instantiate(redPiratPrefab, new Vector3(47, -100, 13), Quaternion.identity, GameObject.Find("Board").transform);
        }
        else if (scene.currentPirate.GetComponent<Pirate>().team == "Yellow")
        {
            newPirate = Instantiate(yellowPiratPrefab, new Vector3(47, -100, 13), Quaternion.identity, GameObject.Find("Board").transform);
        }
        //cur_tile.GetComponent<Tile>().TileClicked();
        scene.piretesNum[scene.currentPirate.GetComponent<Pirate>().team] += 1;
        scene.currentPirate = newPirate;
        cur_tile.GetComponent<Tile>().TileClicked();
    }

}

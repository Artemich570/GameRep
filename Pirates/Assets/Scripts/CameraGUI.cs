using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraGUI : MonoBehaviour
{
    private SceneController scene;
    public bool canTakeCoin;
    public bool canRevive;
    public GameObject activePlayer;
    public TMP_Text black, white, red, yellow, team, movesLeft;
    GUIStyle style;
    [SerializeField] private GameObject redPiratPrefab, blackPiratPrefab, whitePiratPrefab, yellowPiratPrefab;
    // Start is called before the first frame update
    void Start()
    {
        scene = GameObject.Find("SceneController").GetComponent<SceneController>();
        black = GameObject.Find("Canvas").transform.GetChild(4).GetComponent<TMP_Text>();
        white = GameObject.Find("Canvas").transform.GetChild(5).GetComponent<TMP_Text>();
        red = GameObject.Find("Canvas").transform.GetChild(6).GetComponent<TMP_Text>();
        yellow = GameObject.Find("Canvas").transform.GetChild(7).GetComponent<TMP_Text>();
        team = GameObject.Find("Canvas").transform.GetChild(8).GetComponent<TMP_Text>();
        movesLeft = GameObject.Find("Canvas").transform.GetChild(9).GetComponent<TMP_Text>();
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
        team.text = "Turn of "+scene.teamTurn+" Team";
        if (scene.currentPirate != null)
        {
            if (scene.currentPirate.GetComponent<Pirate>().onTurntable > 0)
            {
                movesLeft.text = scene.currentPirate.GetComponent<Pirate>().onTurntable.ToString()+" Moves left";
            }
            else
            {
                movesLeft.text = "";
            }
        }
        else { movesLeft.text = ""; }
    }

    private void OnGUI()
    {
        //проверяю, все ли пираты стоят, а не двигаются, если все стоят, то отображаю кнопку, в противном случае - нет
        if (!GameObject.Find("SceneController").GetComponent<SceneController>().isMoving)
        {
            if (GUI.Button(new Rect(Screen.width - 480, Screen.height - 160, 460, 140), "<size=30>Next Move</size>"))
            {
                scene.NextTurn();
                scene.isMoving = true;
            }
        }
        
        if (scene.currentPirate != null)
        {
            if (canTakeCoin)
            {
                if (GUI.Button(new Rect(Screen.width - 480, Screen.height - 320, 460, 140), "<size=30>Take coin</size>"))
                {
                    //print("You took a coin");
                    scene.currentPirate.GetComponent<Pirate>().TakeCoin();
                }
            }

            if (scene.currentPirate.GetComponent<Pirate>().withCoin)
            {
                if (GUI.Button(new Rect(Screen.width - 480, Screen.height - 320, 460, 140), "<size=30>Drop coin</size>"))
                {
                    scene.currentPirate.GetComponent<Pirate>().DropCoin();
                }
            }

            if (scene.currentPirate.GetComponent<Pirate>().onTurntable > 0)
            {
                if (GUI.Button(new Rect(Screen.width - 480, Screen.height - 160, 460, 140), "<size=30>Move</size>"))
                {
                    scene.currentPirate.GetComponent<Pirate>().onTurntable--;
                    scene.currentPirate = null;
                    scene.isMoving = false;
                    scene.endTurn = true;
                }
            }

            if (scene.currentPirate.GetComponent<Pirate>().CheckCurrentTile().GetComponent<Tile>() != null)
            {
                if (scene.currentPirate.GetComponent<Pirate>().CheckCurrentTile().GetComponent<Tile>().tileType == "Revive Fort" && scene.piretesNum[scene.currentPirate.GetComponent<Pirate>().team] < 3)
                {
                    if (GUI.Button(new Rect(Screen.width - 960, Screen.height - 160, 460, 140), "<size=30>Revive</size>"))
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
            }
        }
    }
}

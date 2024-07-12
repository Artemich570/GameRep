using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class Pirate : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string team;
    public bool isTrapped;
    public bool inWater;
    //public bool isDead;
    public string onPos;
    public GameObject lastTile;
    public int skipTurn;
    public bool withCoin;
    public int onTurntable;
    SceneController scene;
    public bool underAttack;

    // Start is called before the first frame update
    void Start()
    {
        scene = GameObject.Find("SceneController").GetComponent<SceneController>();
        isTrapped = false;
        inWater = false;
        withCoin = false;
        underAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject CheckCurrentTile()
    {
        GameObject cur_tile = null;
        Collider[] colls = Physics.OverlapBox(transform.position, new Vector3(1, 1, 1), Quaternion.identity);
        foreach (Collider el in colls)
        {
            if (el.GetComponent<Tile>() != null || el.GetComponent<Ship>() != null)
            {
                cur_tile = el.gameObject;
                if (el.GetComponent<Ship>() != null)
                {
                    break;
                }
            }
        }
        return cur_tile;
    }

    public void CheckTiles(Vector3 size = new Vector3())
    {
        if (size == new Vector3(0, 0, 0))
        {
            size = new Vector3(5, 5, 5);
        }
        GameObject cur_tile = CheckCurrentTile();

        Collider[] colliders = Physics.OverlapBox(cur_tile.transform.position, size, Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if ((collider.GetComponent<Tile>() != null || collider.GetComponent<Ship>() != null) && collider.gameObject != cur_tile)
            {
                if (collider.GetComponent<Tile>() != null)
                {
                    if (!inWater && collider.GetComponent<Tile>().tileType != "Water")
                    {
                        if (withCoin)
                        {
                            if (collider.GetComponent<Tile>().isOpened)
                            {
                                collider.gameObject.GetComponent<Tile>().isReady = true;
                            }
                        }
                        else
                        {
                            collider.gameObject.GetComponent<Tile>().isReady = true;
                        }
                        Collider[] colliders2 = Physics.OverlapBox(collider.transform.position, new Vector3(5, 5, 5), Quaternion.identity);
                        foreach (Collider collider2 in colliders2)
                        {
                            if (collider2.GetComponent<Pirate>() != null)
                            {
                                if (collider2.GetComponent<Pirate>().team != team)
                                {
                                    collider.gameObject.GetComponent<Tile>().isReady = false;
                                }
                            }
                        }
                    }
                    else if (inWater && collider.GetComponent<Tile>().tileType == "Water")
                    {
                        collider.gameObject.GetComponent<Tile>().isReady = true;
                    }
                }
                else if (collider.GetComponent<Ship>() != null && team == collider.GetComponent<Ship>().team)
                {
                    collider.GetComponent<Ship>().isReady = true;
                    collider.GetComponent<Ship>().isTile = true;
                }
            }
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
            if (tile.GetComponent<Pirate>() != null)
            {
                tile.GetComponent<Pirate>().underAttack = false;
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if ((scene.teamTurn == team && skipTurn == 0 && !isTrapped && !scene.endTurn) || underAttack)
        {
            if (scene.isMoving && scene.firstMove || !scene.isMoving)
            {
                if (onTurntable > 0)
                {
                    JustClicked();
                }
                else
                {
                    if (underAttack) { Clicked(true, true); }
                    else { Clicked(); }
                }
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if ((scene.teamTurn == team && skipTurn == 0 && !isTrapped && !scene.endTurn) || underAttack)
        {
            if (scene.isMoving && scene.firstMove || !scene.isMoving)
            {
                if (underAttack)
                {
                    GetComponent<Outline>().OutlineColor = Color.red;
                    GetComponent<Outline>().OutlineWidth = 2f;
                }
                else
                {
                    GetComponent<Outline>().OutlineColor = Color.white;
                    GetComponent<Outline>().OutlineWidth = 2f;
                }
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if ((scene.teamTurn == team && skipTurn == 0 && !isTrapped && !scene.endTurn) || underAttack)
        {
            if (scene.isMoving && scene.firstMove || !scene.isMoving)
            {
                //string dark_name = GetComponent<MeshRenderer>().material.name;
                //print(dark_name);
                //string name = dark_name.Substring(5, dark_name.Length - 16);
                //GetComponent<MeshRenderer>().material = Resources.Load<Material>($"Tiles/{name}");
                GetComponent<Outline>().OutlineWidth = 0;
            }
        }
    }
    public void Clicked(bool flag = true, bool victim = false)
    {
        if (victim)
        {
            GameObject cur_tile = CheckCurrentTile();
            GameObject last_pirate = scene.currentPirate;
            GameObject teamShip = GameObject.Find($"{team}" + "Ship(Clone)");

            Collider[] colliders = Physics.OverlapBox(cur_tile.transform.position, new Vector3(4, 4, 4), Quaternion.identity);
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<Pirate>() != null)
                {
                    if (collider.GetComponent<Pirate>().team != last_pirate.GetComponent<Pirate>().team)
                    {
                        if (onPos == "pos1") { cur_tile.GetComponent<Tile>().pos1 = null; }
                        else if (onPos == "pos2") { cur_tile.GetComponent<Tile>().pos2 = null; }
                        else if (onPos == "pos3") { cur_tile.GetComponent<Tile>().pos3 = null; }

                        if (cur_tile.GetComponent<Tile>().tileType == "Water")
                        {
                            collider.GetComponent<Pirate>().Kill();
                        }
                        if (collider.GetComponent<Pirate>().withCoin)
                        {
                            collider.GetComponent<Pirate>().DropCoin(flag = true);
                        }
                        
                        scene.currentPirate = collider.gameObject;
                        teamShip.GetComponent<Ship>().isTile = true;
                        teamShip.GetComponent<Ship>().isReady = true;
                        teamShip.GetComponent<Ship>().Clicked();
                        underAttack = false;
                    }
                }
            }
            scene.currentPirate = last_pirate;
            cur_tile.GetComponent<Tile>().TileClicked();
        }
        else
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(500, 10, 500), Quaternion.identity);
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<Ship>() != null)
                {
                    collider.GetComponent<Ship>().isTile = false;
                    collider.GetComponent<Ship>().isReady = false;
                }
            }

            scene.currentShip = null;
            RemoveGlowing();
            scene.currentPirate = gameObject;
            GameObject cur_tile = CheckCurrentTile();
            if (flag)
            {
                CheckTiles();
                CheckCoins();
                lastTile = cur_tile;
            }

            //print($"{onPos}, {cur_tile}");
            /*
            if (onPos == "pos1")
            {
                if (cur_tile.GetComponent<Ship>() != null)
                {
                    cur_tile.GetComponent<Ship>().pos1 = null;
                }
                else
                {
                    cur_tile.GetComponent<Tile>().pos1 = null;
                }
            }
            else if (onPos == "pos2")
            {
                if (cur_tile.GetComponent<Ship>() != null)
                {
                    cur_tile.GetComponent<Ship>().pos2 = null;
                }
                else
                {
                    cur_tile.GetComponent<Tile>().pos2 = null;
                }
            }
            else if (onPos == "pos3")
            {
                if (cur_tile.GetComponent<Ship>() != null)
                {
                    cur_tile.GetComponent<Ship>().pos3 = null;
                }
                else
                {
                    cur_tile.GetComponent<Tile>().pos3 = null;
                }
            }
            */
            CheckPirates();
            //onPos = null;
        }
        
    }
    public void CheckCoins()
    {
        GameObject cur_tile = CheckCurrentTile();
        Collider[] colliders = Physics.OverlapBox(cur_tile.transform.position, new Vector3(4, 10, 4), Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Coin") && !withCoin)
            {
                GameObject.Find("Canvas").GetComponent<CameraGUI>().canTakeCoin = true;
                break;
            }
            else
            {
                GameObject.Find("Canvas").GetComponent<CameraGUI>().canTakeCoin = false;
            }
        }
    }
    public void TakeCoin()
    {
        GameObject cur_tile = CheckCurrentTile();
        Collider[] colliders = Physics.OverlapBox(cur_tile.transform.position, new Vector3(4, 4, 4), Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Coin"))
            {
                withCoin = true;
                Destroy(collider.gameObject);
                GameObject.Find("Canvas").GetComponent<CameraGUI>().canTakeCoin = false;
                //RemoveGlowing();
                break;
            }
        }
        CheckCoins();
        RemoveGlowing();
        CheckTiles();
    }
    public void DropCoin(bool flag = false)
    {
        int coinsNum = 0;
        GameObject cur_tile = CheckCurrentTile();
        Collider[] colliders = Physics.OverlapBox(cur_tile.transform.position, new Vector3(4, 4, 4), Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Coin"))
            {
                coinsNum++;
            }
        }
        withCoin = false;
        GameObject.Find("Canvas").GetComponent<CameraGUI>().canTakeCoin = true;
        Instantiate(Resources.Load<GameObject>("Prefabs/CoinPrefab"), new Vector3(cur_tile.transform.position.x - 3, cur_tile.transform.position.y + coinsNum / 5f, cur_tile.transform.position.z - 3), Quaternion.identity);
        RemoveGlowing();
        if (!flag) { CheckTiles(); }
        
    }

    public void JustClicked()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(500, 10, 500), Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Ship>() != null)
            {
                collider.GetComponent<Ship>().isTile = false;
                collider.GetComponent<Ship>().isReady = false;
            }
        }

        scene.currentShip = null;
        RemoveGlowing();
        scene.currentPirate = gameObject;
        GameObject.Find("Canvas").GetComponent<CameraGUI>().canTakeCoin = false;
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public void CheckPirates()
    {
        GameObject cur_tile = CheckCurrentTile();
        Collider[] colliders = Physics.OverlapBox(cur_tile.transform.position, new Vector3(10, 10, 10), Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Pirate>() != null && collider.gameObject != gameObject)
            {
                if (collider.GetComponent<Pirate>().team != team && !collider.GetComponent<Pirate>().isTrapped && collider.GetComponent<Pirate>().CheckCurrentTile().GetComponent<Tile>().tileType != "Fort" && collider.GetComponent<Pirate>().CheckCurrentTile().GetComponent<Tile>().tileType != "Revive Fort" && collider.GetComponent<Pirate>().CheckCurrentTile().GetComponent<Ship>() == null)
                {
                    if ((inWater && collider.GetComponent<Pirate>().CheckCurrentTile().GetComponent<Tile>().tileType == "Water") || (!inWater && collider.GetComponent<Pirate>().onTurntable == 0 && !collider.GetComponent<Pirate>().inWater))
                    {
                        collider.GetComponent<Pirate>().underAttack = true;
                    }
                }
            }
        }
    }
}

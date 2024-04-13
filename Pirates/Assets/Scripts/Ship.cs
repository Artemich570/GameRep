using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ship : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string team;
    public GameObject pos1;
    public GameObject pos2;
    public GameObject pos3;
    public bool isReady;
    public bool isTile;
    public GameObject lastTile;
    SceneController scene;
    public string defaultMaterial;
    // Start is called before the first frame update
    void Start()
    {
        scene = GameObject.Find("SceneController").GetComponent<SceneController>();
        isReady = false;
        isTile = false;
    }

    public GameObject CheckCurrentTile()
    {
        GameObject cur_tile = null;
        Collider[] colls = Physics.OverlapBox(transform.position, new Vector3(1, 1, 1), Quaternion.identity);
        foreach (Collider el in colls)
        {
            if (el.GetComponent<Tile>() != null)
            {
                cur_tile = el.gameObject;
            }
        }
        return cur_tile;
    }

    public void CheckTiles()
    {
        GameObject cur_tile = CheckCurrentTile();
        lastTile = cur_tile;

        Collider[] colliders = Physics.OverlapBox(cur_tile.transform.position, new Vector3(5, 5, 5), Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Tile>() != null && collider.gameObject != cur_tile)
            {
                if (collider.GetComponent<Tile>().tileType == "Water" && collider.gameObject.CompareTag($"{team} Water"))
                {
                    collider.GetComponent<Tile>().isReady = true;
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
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (scene.teamTurn == team)
        {
            if (!isTile && scene.isMoving && !scene.firstMove) { }
            else
            {
                Clicked();
            }
            
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (scene.teamTurn == team && (isReady || (pos1 != null || pos2 != null || pos3 != null)) && !scene.endTurn)
        {
            if (!isTile && !scene.firstMove) { }
            else
            {
                //string full_name = GetComponent<MeshRenderer>().material.name;
                //string name = full_name.Substring(0, full_name.Length - 11);
                //print(name);
                //GetComponent<MeshRenderer>().material = Resources.Load<Material>($"Tiles/dark {name}");
                GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/" + DataHolder.darkMaterials[DataHolder.materials.IndexOf(defaultMaterial)]);
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (scene.teamTurn == team && (isReady || (pos1 != null || pos2 != null || pos3 != null)) && !scene.endTurn)
        {
            if (!isTile && scene.isMoving && !scene.firstMove) { }
            else
            {
                //string dark_name = GetComponent<MeshRenderer>().material.name;
                //print(dark_name);
                //string name = dark_name.Substring(5, dark_name.Length - 16);
                //GetComponent<MeshRenderer>().material = Resources.Load<Material>($"Tiles/{name}");
                GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/" + defaultMaterial);
                
            }
        }
    }
    public void Clicked()
    {
        if (isTile && isReady)
        {
            if (pos1 == null)
            {
                print(scene.currentPirate);
                pos1 = scene.currentPirate;
                scene.currentPirate.GetComponent<Pirate>().onPos = "pos1";
                scene.currentPirate.transform.position = new Vector3(transform.position.x, transform.position.y+1, transform.position.z);
            }
            else if (pos2 == null)
            {
                pos2 = scene.currentPirate;
                scene.currentPirate.GetComponent<Pirate>().onPos = "pos2";
                scene.currentPirate.transform.position = new Vector3(transform.position.x + 3, transform.position.y+1, transform.position.z - 3);
            }
            else if (pos3 == null)
            {
                pos3 = scene.currentPirate;
                scene.currentPirate.GetComponent<Pirate>().onPos = "pos3";
                scene.currentPirate.transform.position = new Vector3(transform.position.x - 3, transform.position.y+1, transform.position.z + 3);
            }
            if (scene.currentPirate.GetComponent<Pirate>().inWater)
            {
                scene.currentPirate.GetComponent<Pirate>().inWater = false;
            }
            if (scene.currentPirate.GetComponent<Pirate>().withCoin)
            {
                DataHolder.TeamsCoins[team]++;
                scene.currentPirate.GetComponent<Pirate>().withCoin = false;
                //print(DataHolder.TeamsCoins[team]);
            }
            isTile = false;
            isReady = false;
            scene.isMoving = false;
            scene.currentPirate = null;
            RemoveGlowing();
        }
        else
        {
            if (pos1 != null || pos2 != null || pos3 != null)
            {
                scene.currentPirate = null;
                RemoveGlowing();
                scene.currentShip = gameObject;
                CheckTiles();
            }
        }
    }
}

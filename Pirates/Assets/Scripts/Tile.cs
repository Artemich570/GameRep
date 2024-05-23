using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject pos1;
    public GameObject pos2;
    public GameObject pos3;
    public string tileType;
    public bool isOpened;
    public bool isReady;
    SceneController scene;
    public bool isActive;
    public string defaultMaterial;
    // Start is called before the first frame update
    void Start()
    {
        pos1 = null;
        pos2 = null;
        pos3 = null;
        isOpened = false;
        isReady = false;
        tileType = GameObject.Find("Board").GetComponent<FillMap>().tiles[gameObject];
        scene = GameObject.Find("SceneController").GetComponent<SceneController>();

        //Присваиваю дефолтный материал
        if (tileType == "Water")
        {
            defaultMaterial = "water";
        }
        else
        {
            defaultMaterial = "grass";
        }

        //Делаю некоторые клетки неигровыми
        if ((transform.position.x == 110 && transform.position.z == -50) || (transform.position.x == 50 && transform.position.z == -110) || (transform.position.x == -10 && transform.position.z == -50) || (transform.position.x == 50 && transform.position.z == 10))
        {
            isActive = false;
        }
        else { isActive = true; }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isReady)
        {
            TileClicked();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isReady)
        {
            //string full_name = GetComponent<MeshRenderer>().material.name;
            //string name = full_name.Substring(0, full_name.Length - 11);
            //print(name);
            //GetComponent<MeshRenderer>().material = Resources.Load<Material>($"Tiles/dark {name}");
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/" + DataHolder.darkMaterials[DataHolder.materials.IndexOf(defaultMaterial)]);
        }
        
        if (tileType == "Horse" || tileType == "4 Sides")
        {
            print(tileType);
        }
        else
        {
            //print("");
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isReady)
        {
            //print(GetComponent<MeshRenderer>().material.name);
            //string dark_name = GetComponent<MeshRenderer>().material.name;
            //print(dark_name);
            //string name = dark_name.Substring(5, dark_name.Length - 16);
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/" + defaultMaterial);
        }
    }

    void Update()
    {
        
    }

    public void TileClicked()
    {
        //Проверка на то, сколько пиратов уже стоит на клетке
        if (scene.currentPirate != null)
        {
            if (pos1 == null)
            {
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
        }
        //Для кораблей
        else if (scene.currentShip != null)
        {
            //Считаю пиратов на корабле
            GameObject[] pirates = new GameObject[3];
            Collider[] colliders = Physics.OverlapBox(scene.currentShip.transform.position, new Vector3(4, 4, 4), Quaternion.identity);
            int i = 0;
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<Pirate>() != null)
                {
                    if (collider.GetComponent<Pirate>().team == scene.currentShip.GetComponent<Ship>().team)
                    {
                        pirates[i] = collider.gameObject;
                        collider.GetComponent<Pirate>().inWater = false;
                        i++;
                    }
                }
            }
            scene.currentShip.GetComponent<Ship>().pos1 = null;
            scene.currentShip.GetComponent<Ship>().pos2 = null;
            scene.currentShip.GetComponent<Ship>().pos3 = null;
            if (pirates[0] != null)
            {
                pirates[0].transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                scene.currentShip.GetComponent<Ship>().pos1 = pirates[0];
                pirates[0].GetComponent<Pirate>().onPos = "pos1";
            }
            if (pirates[1] != null)
            {
                pirates[1].transform.position = new Vector3(transform.position.x + 3, transform.position.y+1, transform.position.z - 3);
                scene.currentShip.GetComponent<Ship>().pos2 = pirates[1];
                pirates[1].GetComponent<Pirate>().onPos = "pos2";
            }
            if (pirates[2] != null)
            {
                pirates[2].transform.position = new Vector3(transform.position.x - 3, transform.position.y+1, transform.position.z + 3);
                scene.currentShip.GetComponent<Ship>().pos3 = pirates[2];
                pirates[2].GetComponent<Pirate>().onPos = "pos3";
            }
            scene.currentShip.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);

            Collider[] colliders2 = Physics.OverlapBox(scene.currentShip.transform.position, new Vector3(4, 4, 4), Quaternion.identity);
            foreach (Collider collider in colliders2)
            {
                if (collider.GetComponent<Pirate>() != null)
                {
                    scene.piretesNum[collider.GetComponent<Pirate>().team] -= 1;
                    collider.GetComponent<Pirate>().Kill();
                    /*
                    if (collider.GetComponent<Pirate>().team != scene.currentShip.GetComponent<Ship>().team)
                    {
                        scene.piretesNum[collider.GetComponent<Pirate>().team] -= 1;
                        collider.GetComponent<Pirate>().Kill();
                    }
                    */
                }
            }

            isActive = false;
            scene.currentShip.GetComponent<Ship>().lastTile.GetComponent<Tile>().isActive = true;
            scene.currentShip.GetComponent<Ship>().lastTile.GetComponent<Tile>().pos1 = null;
            scene.currentShip.GetComponent<Ship>().lastTile.GetComponent<Tile>().pos2 = null;
            scene.currentShip.GetComponent<Ship>().lastTile.GetComponent<Tile>().pos3 = null;
        }

        if (scene.currentPirate != null)
        {
            scene.currentPirate.GetComponent<Pirate>().CheckCoins(); //Проверка на монеты вокруг
        }
        
        scene.RemoveGlowing();

        if (tileType == "Free")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/free");
            defaultMaterial = "free";
        }
        else if (tileType == "Ice")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/ice");
            defaultMaterial = "ice";
        }
        else if (tileType == "Trap")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/trap");
            defaultMaterial = "trap";
            Trap();
        }
        else if (tileType == "Crocodile")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/crocodile");
            defaultMaterial = "crocodile";
            Crocodile();
        }
        else if (tileType == "Cannibal")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/cannibal");
            defaultMaterial = "cannibal";
            Cannibal();
        }
        else if (tileType == "Fort")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/fort");
            defaultMaterial = "fort";
        }
        else if (tileType == "Revive Fort")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/revive fort");
            defaultMaterial = "revive fort";
            Revive();
        }
        else if (tileType == "Chest1")
        {
            CoinSpawner(1);
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/chest 1");
            defaultMaterial = "chest 1";
        }
        else if (tileType == "Chest2")
        {
            CoinSpawner(2);
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/chest 2");
            defaultMaterial = "chest 2";
        }
        else if (tileType == "Chest3")
        {
            CoinSpawner(3);
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/chest 3");
            defaultMaterial = "chest 3";
        }
        else if (tileType == "Chest4")
        {
            CoinSpawner(4);
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/chest 4");
            defaultMaterial = "chest 4";
        }
        else if (tileType == "Chest5")
        {
            CoinSpawner(5);
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/chest 5");
            defaultMaterial = "chest 5";
        }
        else if (tileType == "Plane")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/plane");
            defaultMaterial = "plane";
            Plane();
        }
        else if (tileType == "Air balloon")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/air balloon");
            defaultMaterial = "air balloon";
            AirBalloon();
        }
        else if (tileType == "Cannon Left")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/cannon left");
            defaultMaterial = "cannon left";
            Cannon("left");
        }
        else if (tileType == "Cannon Up")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/cannon up");
            defaultMaterial = "cannon up";
            Cannon("up");
        }
        else if (tileType == "Cannon Right")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/cannon right");
            defaultMaterial = "cannon right";
            Cannon("right");
        }
        else if (tileType == "Cannon Down")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/cannon down");
            defaultMaterial = "cannon down";
            Cannon("down");
        }
        else if (tileType == "Rum")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/rum");
            defaultMaterial = "rum";
            Rum();
        }
        else if (tileType == "Left")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/left");
            defaultMaterial = "left";
            Left();
        }
        else if (tileType == "Right")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/right");
            defaultMaterial = "right";
            Right();
        }
        else if (tileType == "Up")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/up");
            defaultMaterial = "up";
            Up();
        }
        else if (tileType == "Down")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/down");
            defaultMaterial = "down";
            Down();
        }
        else if (tileType == "4 Sides")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/4 sides");
            defaultMaterial = "4 sides";
            FourSides();
        }
        else if (tileType == "Up or Down")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/up or down");
            defaultMaterial = "up or down";
            UpOrDown();
        }
        else if (tileType == "Up or Left")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/up or left");
            defaultMaterial = "up or left";
            UpOrLeft();
        }
        else if (tileType == "Up or Right")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/up or right");
            defaultMaterial = "up or right";
            UpOrRight();
        }
        else if (tileType == "Down or Left")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/down or left");
            defaultMaterial = "down or left";
            DownOrLeft();
        }
        else if (tileType == "Down or Right")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/down or right");
            defaultMaterial = "down or right";
            DownOrRight();
        }
        else if (tileType == "Left or Right")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/left or right");
            defaultMaterial = "left or right";
            LeftOrRight();
        }
        else if (tileType == "2 Turns")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/2 turns");
            defaultMaterial = "2 turns";
            TurnTable(1);
        }
        else if (tileType == "3 Turns")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/3 turns");
            defaultMaterial = "3 turns";
            TurnTable(2);
        }
        else if (tileType == "4 Turns")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/4 turns");
            defaultMaterial = "4 turns";
            TurnTable(3);
        }
        else if (tileType == "5 Turns")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/5 turns");
            defaultMaterial = "5 turns";
            TurnTable(4);
        }
        else if (tileType == "Horse")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/horse");
            defaultMaterial = "horse";
            Horse();
        }
        else if (tileType == "Water")
        {
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Tiles/water");
            defaultMaterial = "water";
        }
        //Если это первый ход
        if (!scene.newTurn)
        {
            scene.currentPirate = null;
            scene.isMoving = false;
            scene.endTurn = true;
        }
        else
        {
            scene.newTurn = false;
            scene.endTurn = true;
        }
        isOpened = true;
        isReady = false;
        scene.currentShip = null;
        scene.firstMove = false;
        //Делаю все корабли неактивными
        Collider[] collidersToDel = Physics.OverlapBox(transform.position, new Vector3(500, 500, 500), Quaternion.identity);
        foreach (Collider collider in collidersToDel)
        {
            if (collider.GetComponent<Ship>() != null && !scene.isMoving)
            {
                collider.GetComponent<Ship>().isReady = false;
                collider.GetComponent<Ship>().isTile = false;
            }
        }
    }

    public void Plane()
    {
        if (!isOpened)
        {
            scene.newTurn = true;
            scene.isMoving = true;
            scene.currentPirate.GetComponent<Pirate>().Clicked(false);
            scene.currentPirate.GetComponent<Pirate>().CheckTiles(new Vector3(500, 500, 500));
        }
        
    }
    public void AirBalloon()
    {
        GameObject teamShip = GameObject.Find($"{scene.currentPirate.GetComponent<Pirate>().team}" + "Ship(Clone)");
        teamShip.GetComponent<Ship>().isTile = true;
        teamShip.GetComponent<Ship>().isReady = true;
        teamShip.GetComponent<Ship>().Clicked();
    }

    public void Left()
    {
        scene.newTurn = true;
        scene.isMoving = true;
        scene.currentPirate.GetComponent<Pirate>().Clicked(false);
        StartCoroutine(Waiter("left"));  
    }
    public void Right()
    {
        scene.newTurn = true;
        scene.isMoving = true;
        scene.currentPirate.GetComponent<Pirate>().Clicked(false);
        StartCoroutine(Waiter("right"));
    }
    public void Up()
    {
        scene.newTurn = true;
        scene.isMoving = true;
        scene.currentPirate.GetComponent<Pirate>().Clicked(false);
        StartCoroutine(Waiter("up"));
    }
    public void Down()
    {
        scene.newTurn = true;
        scene.isMoving = true;
        scene.currentPirate.GetComponent<Pirate>().Clicked(false);
        StartCoroutine(Waiter("down"));
    }

    public void Rum()
    {
        scene.currentPirate.GetComponent<Pirate>().skipTurn = 2;
    }

    public void Trap()
    {
        int piratesNum = 0;
        if (pos1 != null) { piratesNum++; }
        if (pos2 != null) { piratesNum++; }
        if (pos3 != null) { piratesNum++; }
        if (piratesNum == 1)
        {
            scene.currentPirate.GetComponent<Pirate>().isTrapped = true;
        }
        else
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(4, 4, 4), Quaternion.identity);
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<Pirate>() != null)
                {
                    collider.GetComponent<Pirate>().isTrapped = false;
                }
            }
        }
    }

    public void Crocodile()
    {
        if (scene.currentPirate.GetComponent<Pirate>().lastTile.GetComponent<Tile>() != null)
        {
            scene.newTurn = true;
            scene.isMoving = true;
            scene.currentPirate.GetComponent<Pirate>().Clicked(false);
            StartCoroutine(Waiter("crocodile1"));
        }
        else
        {
            scene.newTurn = true;
            scene.isMoving = true;
            scene.currentPirate.GetComponent<Pirate>().Clicked(false);
            StartCoroutine(Waiter("crocodile2"));
        }
        
    }

    public void Cannibal()
    {
        scene.piretesNum[scene.currentPirate.GetComponent<Pirate>().team] -= 1;
        scene.currentPirate.GetComponent<Pirate>().Kill();
    }

    public void Revive()
    {
        //GameObject.Find("Main Camera").GetComponent<CameraGUI>().canRevive = true;
    }

    public void FourSides()
    {
        scene.newTurn = true;
        scene.isMoving = true; //говорю, что игрок ходит, а не стоит на месте
        scene.currentPirate.GetComponent<Pirate>().Clicked(false);
        GameObject tile;
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(5, 5, 5), Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            tile = collider.gameObject;
            if ((tile.GetComponent<Tile>() != null && tile.GetComponent<Tile>().tileType != "Water") || tile.GetComponent<Ship>() != null)
            {
                float tp_x = tile.transform.position.x;
                float tp_z = tile.transform.position.z;
                float p_x = transform.position.x;
                float p_z = transform.position.z;
                if ((tp_x == p_x + 10 && tp_z == p_z) || (tp_x == p_x - 10 && tp_z == p_z) || (tp_z == p_z + 10 && tp_x == p_x) || (tp_z == p_z - 10 && tp_x == p_x)) // тут просто огромнейшая проверка на то, где именно эта клетка, а именно либо сверху, сниху, справа или слева
                {
                    if (tile.GetComponent<Tile>() != null)
                    {
                        if (scene.currentPirate.GetComponent<Pirate>().withCoin)
                        {
                            if (tile.GetComponent<Tile>().isOpened)
                            {
                                tile.GetComponent<Tile>().isReady = true;
                            }
                        }
                        else
                        {
                            tile.GetComponent<Tile>().isReady = true;
                        }
                    }
                    else if (tile.GetComponent<Ship>() != null)
                    {
                        print("Here is ship");
                        tile.GetComponent<Ship>().isTile = true;
                        tile.GetComponent<Ship>().isReady = true;
                    }
                }

            }
        }
    }

    public void LeftOrRight()
    {
        scene.newTurn = true;
        scene.isMoving = true;
        scene.currentPirate.GetComponent<Pirate>().Clicked(false);
        GameObject tile;
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(5, 5, 5), Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            tile = collider.gameObject;
            if ((tile.GetComponent<Tile>() != null && tile.GetComponent<Tile>().tileType != "Water") || tile.GetComponent<Ship>() != null)
            {
                if ((tile.transform.position.x == transform.position.x + 10 && tile.transform.position.z == transform.position.z) || (tile.transform.position.x == transform.position.x - 10 && tile.transform.position.z == transform.position.z))
                {
                    if (tile.GetComponent<Tile>() != null)
                    {
                        if (scene.currentPirate.GetComponent<Pirate>().withCoin)
                        {
                            if (tile.GetComponent<Tile>().isOpened)
                            {
                                tile.GetComponent<Tile>().isReady = true;
                            }
                        }
                        else
                        {
                            tile.GetComponent<Tile>().isReady = true;
                        }
                    }
                    else
                    {
                        tile.GetComponent<Ship>().isTile = true;
                        tile.GetComponent<Ship>().isReady = true;
                    }
                }

            }
        }
    }

    public void UpOrDown()
    {
        scene.newTurn = true;
        scene.isMoving = true;
        scene.currentPirate.GetComponent<Pirate>().Clicked(false);
        GameObject tile;
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(5, 5, 5), Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            tile = collider.gameObject;
            if ((tile.GetComponent<Tile>() != null && tile.GetComponent<Tile>().tileType != "Water") || tile.GetComponent<Ship>() != null)
            {
                if ((tile.transform.position.z == transform.position.z + 10 && tile.transform.position.x == transform.position.x) || (tile.transform.position.z == transform.position.z - 10 && tile.transform.position.x == transform.position.x))
                {
                    if (tile.GetComponent<Tile>() != null)
                    {
                        if (scene.currentPirate.GetComponent<Pirate>().withCoin)
                        {
                            if (tile.GetComponent<Tile>().isOpened)
                            {
                                tile.GetComponent<Tile>().isReady = true;
                            }
                        }
                        else
                        {
                            tile.GetComponent<Tile>().isReady = true;
                        }
                    }
                    else
                    {
                        tile.GetComponent<Ship>().isTile = true;
                        tile.GetComponent<Ship>().isReady = true;
                    }
                }

            }
        }
    }

    public void UpOrLeft()
    {
        scene.newTurn = true;
        scene.isMoving = true;
        scene.currentPirate.GetComponent<Pirate>().Clicked(false);
        GameObject tile;
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(5, 5, 5), Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            tile = collider.gameObject;
            if ((tile.GetComponent<Tile>() != null && tile.GetComponent<Tile>().tileType != "Water") || tile.GetComponent<Ship>() != null)
            {
                if ((tile.transform.position.x == transform.position.x + 10 && tile.transform.position.z == transform.position.z) || (tile.transform.position.z == transform.position.z - 10 && tile.transform.position.x == transform.position.x))
                {
                    if (tile.GetComponent<Tile>() != null)
                    {
                        if (scene.currentPirate.GetComponent<Pirate>().withCoin)
                        {
                            if (tile.GetComponent<Tile>().isOpened)
                            {
                                tile.GetComponent<Tile>().isReady = true;
                            }
                        }
                        else
                        {
                            tile.GetComponent<Tile>().isReady = true;
                        }
                    }
                    else
                    {
                        tile.GetComponent<Ship>().isTile = true;
                        tile.GetComponent<Ship>().isReady = true;
                    }
                }
            }
        }
    }

    public void UpOrRight()
    {
        scene.newTurn = true;
        scene.isMoving = true;
        scene.currentPirate.GetComponent<Pirate>().Clicked(false);
        GameObject tile;
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(5, 5, 5), Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            tile = collider.gameObject;
            if ((tile.GetComponent<Tile>() != null && tile.GetComponent<Tile>().tileType != "Water") || tile.GetComponent<Ship>() != null)
            {
                if ((tile.transform.position.x == transform.position.x - 10 && tile.transform.position.z == transform.position.z) || (tile.transform.position.z == transform.position.z - 10 && tile.transform.position.x == transform.position.x))
                {
                    if (tile.GetComponent<Tile>() != null)
                    {
                        if (scene.currentPirate.GetComponent<Pirate>().withCoin)
                        {
                            if (tile.GetComponent<Tile>().isOpened)
                            {
                                tile.GetComponent<Tile>().isReady = true;
                            }
                        }
                        else
                        {
                            tile.GetComponent<Tile>().isReady = true;
                        }
                    }
                    else
                    {
                        tile.GetComponent<Ship>().isTile = true;
                        tile.GetComponent<Ship>().isReady = true;
                    }
                }
            }
        }
    }

    public void DownOrLeft()
    {
        scene.newTurn = true;
        scene.isMoving = true;
        scene.currentPirate.GetComponent<Pirate>().Clicked(false);
        GameObject tile;
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(5, 5, 5), Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            tile = collider.gameObject;
            if ((tile.GetComponent<Tile>() != null && tile.GetComponent<Tile>().tileType != "Water") || tile.GetComponent<Ship>() != null)
            {
                if ((tile.transform.position.x == transform.position.x + 10 && tile.transform.position.z == transform.position.z) || (tile.transform.position.z == transform.position.z + 10 && tile.transform.position.x == transform.position.x))
                {
                    if (tile.GetComponent<Tile>() != null)
                    {
                        if (scene.currentPirate.GetComponent<Pirate>().withCoin)
                        {
                            if (tile.GetComponent<Tile>().isOpened)
                            {
                                tile.GetComponent<Tile>().isReady = true;
                            }
                        }
                        else
                        {
                            tile.GetComponent<Tile>().isReady = true;
                        }
                    }
                    else
                    {
                        tile.GetComponent<Ship>().isTile = true;
                        tile.GetComponent<Ship>().isReady = true;
                    }
                }
            }
        }
    }

    public void DownOrRight()
    {
        scene.newTurn = true;
        scene.isMoving = true;
        scene.currentPirate.GetComponent<Pirate>().Clicked(false);
        GameObject tile;
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(5, 5, 5), Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            tile = collider.gameObject;
            if ((tile.GetComponent<Tile>() != null && tile.GetComponent<Tile>().tileType != "Water") || tile.GetComponent<Ship>() != null)
            {
                if ((tile.transform.position.x == transform.position.x - 10 && tile.transform.position.z == transform.position.z) || (tile.transform.position.z == transform.position.z + 10 && tile.transform.position.x == transform.position.x))
                {
                    if (tile.GetComponent<Tile>() != null)
                    {
                        //tile.GetComponent<Tile>().isReady = true;
                        if (scene.currentPirate.GetComponent<Pirate>().withCoin)
                        {
                            if (tile.GetComponent<Tile>().isOpened)
                            {
                                tile.GetComponent<Tile>().isReady = true;
                            }
                        }
                        else
                        {
                            tile.GetComponent<Tile>().isReady = true;
                        }
                    }
                    else
                    {
                        tile.GetComponent<Ship>().isTile = true;
                        tile.GetComponent<Ship>().isReady = true;
                    }
                }
            }
        }
    }

    public void Horse()
    {
        scene.newTurn = true;
        scene.isMoving = true; 
        scene.currentPirate.GetComponent<Pirate>().Clicked(false);
        GameObject tile;
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(20, 10, 20), Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            tile = collider.gameObject;
            if ((tile.GetComponent<Tile>() != null && tile.GetComponent<Tile>().tileType != "Water") || tile.GetComponent<Ship>() != null)
            {
                float tp_x = tile.transform.position.x;
                float tp_z = tile.transform.position.z;
                float p_x = transform.position.x;
                float p_z = transform.position.z;
                if ((tp_x == p_x - 10 && tp_z == p_z - 20) || (tp_x == p_x - 20 && tp_z == p_z - 10) || (tp_x == p_x - 20 && tp_z == p_z + 10) || (tp_x == p_x - 10 && tp_z == p_z + 20) || (tp_x == p_x + 10 && tp_z == p_z + 20) || (tp_x == p_x + 20 && tp_z == p_z + 10) || (tp_x == p_x + 20 && tp_z == p_z - 10) || (tp_x == p_x + 10 && tp_z == p_z - 20))
                {
                    if (tile.GetComponent<Tile>() != null)
                    {
                        if (scene.currentPirate.GetComponent<Pirate>().withCoin)
                        {
                            if (tile.GetComponent<Tile>().isOpened)
                            {
                                tile.GetComponent<Tile>().isReady = true;
                            }
                        }
                        else
                        {
                            tile.GetComponent<Tile>().isReady = true;
                        }
                    }
                    else if (tile.GetComponent<Ship>() != null)
                    {
                        tile.GetComponent<Ship>().isTile = true;
                        tile.GetComponent<Ship>().isReady = true;
                    }
                }

            }
        }
    }

    public void TurnTable(int number)
    {
        scene.currentPirate.GetComponent<Pirate>().onTurntable = number;
    }

    public void Cannon(string direction)
    {
        for (int i = 10; i < 10000; i+=10)
        {
            Vector3 newPos = transform.position;
            if (direction == "up") { newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - i); }
            else if (direction == "down") { newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + i); }
            else if (direction == "left") { newPos = new Vector3(transform.position.x + i, transform.position.y, transform.position.z); }
            else if (direction == "right") { newPos = new Vector3(transform.position.x - i, transform.position.y, transform.position.z); }

            bool onShip = false;
            Collider[] colliders = Physics.OverlapBox(newPos, new Vector3(4, 4, 4), Quaternion.identity);
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<Ship>() != null)
                {
                    if (collider.GetComponent<Ship>().team == scene.currentPirate.GetComponent<Pirate>().team)
                    {
                        collider.GetComponent<Ship>().isTile = true;
                        collider.GetComponent<Ship>().isReady = true;
                        collider.GetComponent<Ship>().Clicked();
                    }
                    else
                    {
                        scene.piretesNum[scene.currentPirate.GetComponent<Pirate>().team] -= 1;
                        scene.currentPirate.GetComponent<Pirate>().Kill();
                    }
                    onShip = true;
                    break;
                }
            }

            if (!onShip)
            {
                foreach (Collider collider in colliders)
                {
                    if (collider.GetComponent<Tile>() != null && collider.GetComponent<Tile>().tileType == "Water")
                    {
                        scene.currentPirate.GetComponent<Pirate>().inWater = true;
                        collider.GetComponent<Tile>().TileClicked();
                        break;
                    }
                }
            }
        }
    }

    public void GetCurrentTile(int x, int y, int z)
    {
        Collider[] colliders = Physics.OverlapBox(new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z), new Vector3(1, 1, 1), Quaternion.identity);
        bool onShip = false;

        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Ship>() != null)
            {
                if (scene.currentPirate.GetComponent<Pirate>().team == collider.GetComponent<Ship>().team)
                {
                    collider.GetComponent<Ship>().isTile = true;
                    collider.GetComponent<Ship>().isReady = true;
                    collider.GetComponent<Ship>().Clicked();
                }
                else
                {
                    scene.currentPirate.GetComponent<Pirate>().Kill();
                }
                onShip = true;
                break;
            }
        }
        if (!onShip)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<Tile>() != null)
                {
                    if (collider.GetComponent<Tile>().tileType == "Water")
                    {
                        if (collider.GetComponent<Tile>().isActive)
                        {
                            scene.currentPirate.GetComponent<Pirate>().inWater = true;
                        }
                    }
                    collider.GetComponent<Tile>().isReady = true;
                    collider.GetComponent<Tile>().TileClicked();
                }
            }
        }
    }

    public void CoinSpawner(int amount)
    {
        if (!isOpened)
        {
            for (int i = 0; i < amount; i++)
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/CoinPrefab"), new Vector3(transform.position.x - 3, transform.position.y + i/5f, transform.position.z - 3), Quaternion.identity);
            }
        }
    }

    public IEnumerator Waiter(string direction)
    {
        yield return new WaitForSeconds(0.1f);
        switch (direction)
        {
            case "up":
                GetCurrentTile(0, 0, -10);
                break;
            case "down":
                GetCurrentTile(0, 0, 10);
                break;
            case "right":
                GetCurrentTile(-10, 0, 0);
                break;
            case "left":
                GetCurrentTile(10, 0, 0);
                break;
            case "crocodile1":
                scene.currentPirate.GetComponent<Pirate>().lastTile.GetComponent<Tile>().TileClicked();
                break;
            case "crocodile2":
                scene.currentPirate.GetComponent<Pirate>().lastTile.GetComponent<Ship>().isTile = true;
                scene.currentPirate.GetComponent<Pirate>().lastTile.GetComponent<Ship>().isReady = true;
                scene.currentPirate.GetComponent<Pirate>().lastTile.GetComponent<Ship>().Clicked();
                break;
        }
    }
}
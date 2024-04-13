using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillMap : MonoBehaviour
{
    [SerializeField] private GameObject waterPrefab;
    [SerializeField] private GameObject grassPrefab;
    private GameObject tile;
    [SerializeField] private GameObject redShipPrefab, blackShipPrefab, whiteShipPrefab, yellowShipPrefab;
    private GameObject redShip, blackShip, whiteShip, yellowShip;
    [SerializeField] private GameObject redPiratPrefab, blackPiratPrefab, whitePiratPrefab, yellowPiratPrefab;
    private GameObject redPirat1, redPirat2, redPirat3, blackPirat1, blackPirat2, blackPirat3, whitePirat1, whitePirat2, whitePirat3, yellowPirat1, yellowPirat2, yellowPirat3;


    public Dictionary<GameObject, string> tiles = new Dictionary<GameObject, string>(); // создаю словарь, где ключ - клетка, а значение - строка с название типа клетки

    private List<string> tileTypes = new List<string>() { "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Ice", "Ice", "Ice", "Ice", "Ice", "Ice", "Trap", "Trap", "Trap", "Crocodile", "Crocodile", "Crocodile", "Cannibal", "Fort", "Fort", "Revive Fort", "Chest1", "Chest1", "Chest1", "Chest1", "Chest1", "Chest2", "Chest2", "Chest2", "Chest2", "Chest2", "Chest3", "Chest3", "Chest3", "Chest4", "Chest4", "Chest5", "Plane", "Air balloon", "Air balloon", "Cannon Right", "Cannon Left", "Rum", "Rum", "Rum", "Rum", "Left", "Left", "Left", "Left", "Right", "Right", "Right", "Right", "Up", "Up", "Up", "Up", "Down", "Down", "Down", "Down", "4 Sides", "4 Sides", "4 Sides", "4 Sides", "Up or Down", "Up or Down", "Up or Left", "Up or Right", "Down or Left", "Down or Right", "Left or Right", "Left or Right", "2 Turns", "2 Turns", "2 Turns", "2 Turns", "2 Turns", "3 Turns", "3 Turns", "3 Turns", "3 Turns", "4 Turns", "4 Turns", "5 Turns", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Free", "Cannon Up", "Cannon Down" }; // список возможных клеток

    void Start()
    {
        // заполнение сверху вниз, справа налево (тк камера в игре перевернута, то наоборот)
        for (int x = -10; x < 120; x += 10)
        {
            for (int z = 10; z > -120; z -= 10)
            {
                if ((x > -10 && x < 110) && (z < 10 && z > -110))
                {
                    // проверки на воду и сушу
                    if ((x == 0 && z == 0) || (x == 0 && z == -100) || (x == 100 && z == 0) || (x == 100 && z == -100))
                    {
                        tile = Instantiate(waterPrefab, new Vector3(x, 0, z), Quaternion.identity, gameObject.transform);
                        //tile.tag = "Water";
                        tiles.Add(tile, "Water");
                    }
                    else
                    {
                        tile = Instantiate(grassPrefab, new Vector3(x, 0, z), Quaternion.identity, gameObject.transform);
                        string tileType = tileTypes[Random.Range(0, tileTypes.Count)]; // присваиваю этой клетке случайный тип, а потом удаляю его из массива tileTypes
                        tiles.Add(tile, tileType);
                        tileTypes.Remove(tileType);

                    }
                }
                else
                {
                    tile = Instantiate(waterPrefab, new Vector3(x, 0, z), Quaternion.identity, gameObject.transform);
                    tiles.Add(tile, "Water");
                }

                // Меняю теги воды, чтобы корабль определенного цвета мог ходить только по воде своего цвета
                if (x == -10 && z <= 0 && z >= -90)
                {
                    tile.tag = "Red Water";
                }
                else if (x == 110 && z <= 0 && z >= -90)
                {
                    tile.tag = "Black Water";
                }
                else if (z == -110 && x <= 90 && x >= 10)
                {
                    tile.tag = "White Water";
                }
                else if (z == 10 && x <= 90 && x >= 10)
                {
                    tile.tag = "Yellow Water";
                }

                //Удаляю ненужные клетки воды
                if ((x == 110 && z == -110) || (x == 100 && z == -110) || (x == 110 && z == -100) || (x == -10 && z == -110) || (x == 0 && z == -110) || (x == -10 && z == -100) || (x == -10 && z == 10) || (x == -10 && z == 0) || (x == 0 && z == 10) || (x == 110 && z == 10) || (x == 110 && z == 0) || (x == 100 && z == 10))
                {
                    Destroy(tile.GetComponent<Tile>());
                }
            }
        }

        // спавню игроков и корабли

        if (DataHolder.teams.Contains("Black"))
        {
            blackShip = Instantiate(blackShipPrefab, new Vector3(110, 0.1f, -50), Quaternion.identity, gameObject.transform);
            blackPirat1 = Instantiate(blackPiratPrefab, new Vector3(110, 1f, -50), Quaternion.identity, gameObject.transform);
            blackPirat2 = Instantiate(blackPiratPrefab, new Vector3(113, 1f, -53), Quaternion.identity, gameObject.transform);
            blackPirat3 = Instantiate(blackPiratPrefab, new Vector3(107, 1f, -47), Quaternion.identity, gameObject.transform);

            blackShip.GetComponent<Ship>().pos1 = blackPirat1;
            blackShip.GetComponent<Ship>().pos2 = blackPirat2;
            blackShip.GetComponent<Ship>().pos3 = blackPirat3;

            blackPirat1.GetComponent<Pirate>().onPos = "pos1";
            blackPirat2.GetComponent<Pirate>().onPos = "pos2";
            blackPirat3.GetComponent<Pirate>().onPos = "pos3";
        }

        if (DataHolder.teams.Contains("White"))
        {
            whiteShip = Instantiate(whiteShipPrefab, new Vector3(50, 0.1f, -110), Quaternion.identity, gameObject.transform);
            whitePirat1 = Instantiate(whitePiratPrefab, new Vector3(50, 1f, -110), Quaternion.identity, gameObject.transform);
            whitePirat2 = Instantiate(whitePiratPrefab, new Vector3(53, 1f, -113), Quaternion.identity, gameObject.transform);
            whitePirat3 = Instantiate(whitePiratPrefab, new Vector3(47, 1f, -107), Quaternion.identity, gameObject.transform);

            whiteShip.GetComponent<Ship>().pos1 = whitePirat1;
            whiteShip.GetComponent<Ship>().pos2 = whitePirat2;
            whiteShip.GetComponent<Ship>().pos3 = whitePirat3;

            whitePirat1.GetComponent<Pirate>().onPos = "pos1";
            whitePirat2.GetComponent<Pirate>().onPos = "pos2";
            whitePirat3.GetComponent<Pirate>().onPos = "pos3";
        }

        if (DataHolder.teams.Contains("Red"))
        {
            redShip = Instantiate(redShipPrefab, new Vector3(-10, 0.1f, -50), Quaternion.identity, gameObject.transform);
            redPirat1 = Instantiate(redPiratPrefab, new Vector3(-10, 1f, -50), Quaternion.identity, gameObject.transform);
            redPirat2 = Instantiate(redPiratPrefab, new Vector3(-7, 1f, -53), Quaternion.identity, gameObject.transform);
            redPirat3 = Instantiate(redPiratPrefab, new Vector3(-13, 1f, -47), Quaternion.identity, gameObject.transform);

            redShip.GetComponent<Ship>().pos1 = redPirat1;
            redShip.GetComponent<Ship>().pos2 = redPirat2;
            redShip.GetComponent<Ship>().pos3 = redPirat3;

            redPirat1.GetComponent<Pirate>().onPos = "pos1";
            redPirat2.GetComponent<Pirate>().onPos = "pos2";
            redPirat3.GetComponent<Pirate>().onPos = "pos3";
        }

        if (DataHolder.teams.Contains("Yellow"))
        {
            yellowShip = Instantiate(yellowShipPrefab, new Vector3(50, 0.1f, 10), Quaternion.identity, gameObject.transform);
            yellowPirat1 = Instantiate(yellowPiratPrefab, new Vector3(50, 1f, 10), Quaternion.identity, gameObject.transform);
            yellowPirat2 = Instantiate(yellowPiratPrefab, new Vector3(53, 1f, 7), Quaternion.identity, gameObject.transform);
            yellowPirat3 = Instantiate(yellowPiratPrefab, new Vector3(47, 1f, 13), Quaternion.identity, gameObject.transform);

            yellowShip.GetComponent<Ship>().pos1 = yellowPirat1;
            yellowShip.GetComponent<Ship>().pos2 = yellowPirat2;
            yellowShip.GetComponent<Ship>().pos3 = yellowPirat3;

            yellowPirat1.GetComponent<Pirate>().onPos = "pos1";
            yellowPirat2.GetComponent<Pirate>().onPos = "pos2";
            yellowPirat3.GetComponent<Pirate>().onPos = "pos3";
        }
    }
}

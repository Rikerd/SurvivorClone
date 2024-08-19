using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePositionChecker : MonoBehaviour
{
    public enum TilePosition { TopLeft,    TopMiddle,   TopRight,
                               MiddleLeft, TrueMiddle,  MiddleRight,
                               BotLeft,    BotMiddle,   BotRight};

    public TilePosition tilePosition;

    private int tileScaleSize = 48;
    private GameObject background;

    private Dictionary<TilePosition, Vector3> tilePositionDictionary = new Dictionary<TilePosition, Vector3>
    {
        { TilePosition.TopLeft, new Vector3(1, -1, 0) },
        { TilePosition.TopMiddle, new Vector3(0, -1, 0) },
        { TilePosition.TopRight, new Vector3(-1, -1, 0) },
        { TilePosition.MiddleLeft, new Vector3(1, 0, 0) },
        { TilePosition.TrueMiddle, new Vector3(0, 0, 0) },
        { TilePosition.MiddleRight, new Vector3(-1, 0, 0) },
        { TilePosition.BotLeft, new Vector3(1, 1, 0) },
        { TilePosition.BotMiddle, new Vector3(0, 1, 0) },
        { TilePosition.BotRight, new Vector3(-1, 1, 0) },
    };

    private void Start()
    {
        background = GameObject.Find("Background");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            background.transform.position += tilePositionDictionary[tilePosition] * (tileScaleSize * 3);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "New Random Tile", menuName = "Tile/Random")]
public class RandomTile : Tile
    {
        /// <summary>
        /// The Sprites used for randomizing output.
        /// </summary>
        [SerializeField]
        public Sprite[] m_Sprites;

        /// <summary>
        /// Retrieves any tile rendering data from the scripted tile.
        /// </summary>
        /// <param name="position">Position of the Tile on the Tilemap.</param>
        /// <param name="tilemap">The Tilemap the tile is present on.</param>
        /// <param name="tileData">Data to render the tile.</param>
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            if ((m_Sprites != null) && (m_Sprites.Length > 0))
            {
                long hash = position.x;
                hash = (hash + 0xabcd1234) + (hash << 15);
                hash = (hash + 0x0987efab) ^ (hash >> 11);
                hash ^= position.y;
                hash = (hash + 0x46ac12fd) + (hash << 7);
                hash = (hash + 0xbe9730af) ^ (hash << 11);
                var oldState = Random.state;
                Random.InitState((int)hash);
                tileData.sprite = m_Sprites[(int) (m_Sprites.Length * Random.value)];
                Random.state = oldState;
            }
        }
    }

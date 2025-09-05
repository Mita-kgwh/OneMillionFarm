using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTileManager : MonoSingleton<FarmTileManager>
{
    [SerializeField] private int row, col;
    public List<FarmTile> tiles;


}

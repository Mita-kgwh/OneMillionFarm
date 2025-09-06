using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plant or Animal on Plant Tile
/// </summary>
public class BaseCreatureItem : MonoBehaviour
{
    [SerializeField] protected ItemType creatureType;
    private int creatureID;
    protected BaseCreatureBehaviour creatureBehaviour;

    public ItemType CreatureType => creatureType;
    public int CreatureID => creatureID;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use for object need timing
/// </summary>
public interface IUpdateable
{
    void OnUpdate(float dt);
}

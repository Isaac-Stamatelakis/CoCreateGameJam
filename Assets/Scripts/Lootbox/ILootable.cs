using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDisplayable {
    public Sprite getSprite();
}

public abstract class Lootable : ScriptableObject, IDisplayable {
    public abstract Sprite getSprite();
}

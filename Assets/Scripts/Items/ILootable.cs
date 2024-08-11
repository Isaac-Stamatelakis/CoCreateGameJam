using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;


public interface IDisplayable {
    public Sprite getSprite();
    public string getName();
}

public interface ISecondaryHeaderDisplayable {
    public string getSecondaryHeader();
}

public interface INumberHeaderDisplayable {
    public string getNumberHeader();
}
public interface IIdedObject {
    public string getId();
}

public abstract class Lootable : ScriptableObject, IDisplayable, IIdedObject {
    [SerializeField] protected string id;
    public string getId()
    {
        return id;
    }

    public string getName()
    {
        return name;
    }
    public abstract Sprite getSprite();
    public abstract ItemSlotType getItemSlotType();
}


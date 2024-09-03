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
public interface ILootable : IDisplayable {
    public string getId();
}

public abstract class Lootable : ScriptableObject, IDisplayable, ILootable {
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

public abstract class LootableAggregate : ILootable {
    public string getId()
    {
        Lootable lootable = getLootable();
        if (lootable == null) {
            return null;
        }
        return lootable.getId();
    }

    public abstract Lootable getLootable();

    public string getName()
    {
        Lootable lootable = getLootable();
        if (lootable == null) {
            return null;
        }
        return lootable.getName();
    }

    public Sprite getSprite()
    {
        Lootable lootable = getLootable();
        if (lootable == null) {
            return null;
        }
        return lootable.getSprite();
    }
}


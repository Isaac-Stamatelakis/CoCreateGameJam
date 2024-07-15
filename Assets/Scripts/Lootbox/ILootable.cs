using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
}

public interface ILootableCount {
    public float getAmount();
    public void addAmount(float amount);
    public Lootable getLootable();
}

public static class LootableCountUtils {
    public static void giveCountable<T>(T toGive, List<T> list) where T : ILootableCount {
        T matched = matchCountable<T>(toGive,list);
        if (matched == null) {
            list.Add(toGive);
        } else {
            matched.addAmount(toGive.getAmount());
        }
    }

    public static T matchCountable<T>(T value, List<T> elements) where T : ILootableCount {
        if (value == null || value.getLootable() == null) {
            return default(T);
        }
        string toGiveId = value.getLootable().getId();
        if (toGiveId == null) {
            return default(T);
        }
        foreach (T element in elements) {
            if (element != null && element.getLootable().getId().Equals(toGiveId)) {
                return element;
            }
        }
        return default(T);
    }
}

public class LootableCount : IDisplayable, INumberHeaderDisplayable {
    public Lootable lootable;
    public float count;

    public LootableCount(Lootable lootable, float count)
    {
        this.lootable = lootable;
        this.count = count;
    }

    public string getName()
    {
        return lootable.name;
    }

    public string getNumberHeader()
    {
        return $"{count:F1}";
    }

    public Sprite getSprite()
    {
        return lootable.getSprite();
    }
}
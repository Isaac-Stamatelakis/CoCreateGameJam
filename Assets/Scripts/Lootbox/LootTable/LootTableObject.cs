using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;


public interface ILootTable {
    public List<LootFrequency> getLootFrequencies();
    public int getRolls();
    public bool getAllowRepitions();
}
[CreateAssetMenu(fileName = "New LootTable", menuName = "LootTable")]
public class LootTableObject : ScriptableObject, ILootTable
{
    public List<LootFrequency> lootFrequencies;
    public int rolls = 1;
    public bool repetitions = true;

    public bool getAllowRepitions()
    {
        return repetitions;
    }

    public List<LootFrequency> getLootFrequencies()
    {
        return lootFrequencies;
    }

    public int getRolls()
    {
        return rolls;
    }
}

public class LootTable : ILootTable {
    private List<LootFrequency> lootFrequencies;
    private int rolls = 1;
    private bool repetitions = true;

    public LootTable(List<LootFrequency> lootFrequencies, int rolls, bool repetitions)
    {
        this.lootFrequencies = lootFrequencies;
        this.rolls = rolls;
        this.repetitions = repetitions;
    }

    public bool getAllowRepitions()
    {
        return repetitions;
    }

    public List<LootFrequency> getLootFrequencies()
    {
        return lootFrequencies;
    }

    public int getRolls()
    {
        return rolls;
    }
}   

[System.Serializable]
public class LootFrequency {
    public Lootable val;
    public int frequency = 1;
    public int amount = 1;
}

public static class LootTableUtils {
    public static List<IntItemSlot> openLootTable(ILootTable lootTable) {
        if (lootTable == null) {
            return new List<IntItemSlot>();
        }
        int totalFrequency = 0;
        List<LootFrequency> loot = lootTable.getLootFrequencies();
        int rolls = lootTable.getRolls();
        bool repetitions = lootTable.getAllowRepitions();
        foreach (LootFrequency lootable in loot) {
            totalFrequency += lootable.frequency;
        }
        
        List<IntItemSlot> lootedItemSlots = new List<IntItemSlot>();
        List<LootFrequency> temp = new List<LootFrequency>();
        foreach (LootFrequency lootFrequency in loot) {
            temp.Add(lootFrequency);
        }
        while (temp.Count > 0 && lootedItemSlots.Count < rolls) {
            int ran = Random.Range(0,totalFrequency);
            totalFrequency = 0;
            for (int i = 0; i < temp.Count; i++) {
                LootFrequency lootFrequency = temp[i];
                totalFrequency += lootFrequency.frequency;
                if (totalFrequency > ran) {
                    lootedItemSlots.Add(ItemSlotUtils.fromLootFrequency(lootFrequency));
                    if (!repetitions) {
                        temp.RemoveAt(i);
                    }
                    break;
                }
                
            }
        }
        return lootedItemSlots;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreatureModule;
using InventoryModule;

namespace LevelModule.Combat {
    public static class CreetureSpriteLoader 
    {
        public static GameObject loadCreature(EquipedCreeture equipedCreeture, int order,  Side side) {
            GameObject creature = new GameObject();
            creature.name = equipedCreeture.creeture.name;
            SpriteRenderer spriteRenderer = creature.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = equipedCreeture.creeture.sprite;
            creature.layer = LayerMask.NameToLayer("Creature");
            creature.AddComponent<BoxCollider2D>();
            creature.transform.position = getPosition(order,side);
            return creature;
        }

        private static Vector2 getPosition(int order, Side side) {
            switch (side) {
                case Side.Left:
                    switch (order) {
                        case 0:
                            return new Vector2(-14f,8f);
                        case 1:
                            return new Vector2(-10,6.5f);
                        case 2:
                            return new Vector2(-14,5f);
                        default:
                            Debug.LogError("Order should be in range 0-2");
                            return Vector2.zero;
                    }
                case Side.Right:
                    switch (order) {
                        case 0:
                            return new Vector2(3,8f);
                        case 1:
                            return new Vector2(-1,6.5f);
                        case 2:
                            return new Vector2(3,5f);
                        default:
                            Debug.LogError("Order should be in range 0-2");
                            return Vector2.zero;
                    }
            }
            return Vector2.zero;
        }

        
    }

    public enum Side {
            Left,
            Right
        }
}


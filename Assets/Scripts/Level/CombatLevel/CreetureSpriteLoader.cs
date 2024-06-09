using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Items;
using TMPro;

namespace Levels.Combat {
    public static class CreetureSpriteLoader 
    {
        public static GameObject loadCreature(EquipedCreeture equipedCreeture, int order,  Side side) {
            GameObject creature = new GameObject();
            creature.name = equipedCreeture.Creeture.name;
            SpriteRenderer spriteRenderer = creature.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = equipedCreeture.Creeture.Sprite;
            GameObject health = new GameObject();
            health.name = "Health";
            health.transform.SetParent(creature.transform,false);
            health.transform.position = new Vector3(-1,-spriteRenderer.bounds.extents.y-0.3f,1);
            health.transform.localScale = new Vector3(0.3f,0.3f);
            SpriteRenderer healthRenderer = health.AddComponent<SpriteRenderer>();
            healthRenderer.sprite = Resources.Load<Sprite>("Sprites/Health_Points");

            GameObject healthText = new GameObject();
            TextMeshPro textMeshPro = healthText.AddComponent<TextMeshPro>();
            textMeshPro.text = equipedCreeture.Creeture.Health.ToString();
            textMeshPro.color = Color.black;
            textMeshPro.fontSize = 4;
            textMeshPro.alignment = TextAlignmentOptions.Center;

            healthText.transform.SetParent(creature.transform,false);
            healthText.transform.position = new Vector3(-1,-spriteRenderer.bounds.extents.y-0.3f,0);
            healthText.name = "HealthText";
            
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


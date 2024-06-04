using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels.Combat {
    public class CombatCreatureUIContainer : MonoBehaviour
    {
        [SerializeField] private CreatureCombatUI creatureCombatUIPrefab;
        public void addCreature(CreatureCombatObject creatureCombatObject) {
            CreatureCombatUI creatureCombatUI = GameObject.Instantiate(creatureCombatUIPrefab);
            creatureCombatObject.syncCombatUI(creatureCombatUI);
            Canvas canvas = transform.parent.GetComponent<Canvas>();
            SpriteRenderer spriteRenderer = creatureCombatObject.GetComponent<SpriteRenderer>();
            Vector3 SizeY = new Vector3(0,spriteRenderer.size.y/2f*1.25f,0);
            Vector3 creatureTopPosition = creatureCombatObject.transform.position + SizeY;
            RectTransform canvasTransform = canvas.transform as RectTransform;
            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main,creatureTopPosition);
            creatureCombatUI.name = $"{creatureCombatObject.CreatureInCombat.EquipedCreeture.creeture.name} UI";
            creatureCombatUI.transform.SetParent(transform,false);
            creatureCombatUI.transform.position = screenPosition;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI.Lists;
using Creatures;
using Actions.Script;

namespace Levels.Combat {
    public class EnemyTurnUI : MonoBehaviour
    {
        [SerializeField] private Image creatureImage;
        [SerializeField] private TextMeshProUGUI text;

        public void display(EquipedCreature element, ScriptedAction scriptedAction)
        {
            creatureImage.sprite = element.getSprite();
            text.text = CombatLevelUtils.getEnemyTurnText(element.getName(), scriptedAction);
        }
    }
}


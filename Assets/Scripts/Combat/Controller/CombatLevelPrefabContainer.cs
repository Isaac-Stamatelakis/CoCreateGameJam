using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels.Combat {
    public class CombatLevelPrefabContainer : MonoBehaviour
    {
        private static CombatLevelPrefabContainer instance;
        public void Awake() {
            instance = this;
        }
        [SerializeField] private DamageIndicatorUI damageIndicatorUIPrefab;
        public DamageIndicatorUI getDamageIndicator() {
            return GameObject.Instantiate(damageIndicatorUIPrefab);
        }
        public static CombatLevelPrefabContainer Instance { get => instance; }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels.Combat {
    public class CombatLevelUIController : MonoBehaviour
    {
        [SerializeField] private CombatLevelController combatLevelController;
        [SerializeField] private CombatLevelDisplayedCreatureUI displayedCreatureUI;
        [SerializeField] private CombatCreatureUIContainer combatCreatureUIContainer;
        [SerializeField] private CombatLevelActionUIController actionUIController;
        public CombatLevelDisplayedCreatureUI DisplayedCreatureUI { get => displayedCreatureUI; }
        public CombatCreatureUIContainer CombatCreatureUIContainer { get => combatCreatureUIContainer; }
        public CombatLevelActionUIController ActionUIController { get => actionUIController; }
        public CombatLevelController CombatLevelController { get => combatLevelController; }
    }

}

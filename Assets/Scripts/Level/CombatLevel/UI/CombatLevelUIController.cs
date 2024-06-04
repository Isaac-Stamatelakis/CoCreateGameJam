using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels.Combat {
    public class CombatLevelUIController : MonoBehaviour
    {
        [SerializeField] private CombatLevelDisplayedCreatureUI displayedCreatureUI;
        [SerializeField] private CombatCreatureUIContainer combatCreatureUIContainer;
        public CombatLevelDisplayedCreatureUI DisplayedCreatureUI { get => displayedCreatureUI; }
        public CombatCreatureUIContainer CombatCreatureUIContainer { get => combatCreatureUIContainer; }
    }

}

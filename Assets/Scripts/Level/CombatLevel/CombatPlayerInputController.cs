using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Levels.Combat {
    public class CombatPlayerInputController : MonoBehaviour
    {
        private CombatLevelController levelController;
        public CombatLevelController LevelController { get => levelController; set => levelController = value; }

        // Start is called before the first frame update
        void Start()
        {
            /*
            mAttackButton = transform.Find("AttackButton").GetComponent<Button>();
            mAttackButton.onClick.AddListener(attackClick);

            mHealButton = transform.Find("HealButton").GetComponent<Button>();
            mHealButton.onClick.AddListener(healClick);

            mDefendButton = transform.Find("DefendButton").GetComponent<Button>();
            mDefendButton.onClick.AddListener(defendClick);
            */
        }

    }
}


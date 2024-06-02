using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using System.Linq;
using TMPro;
using Player;

namespace Levels.Combat {
    public class CombatLevelController : MonoBehaviour
    {
        [SerializeField] private Camera mCamera;
        [SerializeField] private CreatureCombatObject creatureCombatObjectPrefab;
        [SerializeField] private CombatCreatureContainer playerCreaturesContainer;
        [SerializeField] private CombatCreatureContainer aiCreaturesContainer;
        private CreatureInCombat selectedPlayerCreature;
        private CombatPlayer humanPlayer;
        private CombatPlayer aiPlayer;
        private List<CreatureInCombat> combatOrder;
        
        private CombatLevel level;
        private int currentTurn;
        private bool gameOver;
        // Start is called before the first frame update
        
        public void init(CombatPlayer humanPlayer, CombatPlayer aiPlayer, CombatLevel level) {
            this.humanPlayer = humanPlayer;
            playerCreaturesContainer.displayCreatures(humanPlayer.Creatures);

            this.aiPlayer = aiPlayer;
            aiCreaturesContainer.displayCreatures(aiPlayer.Creatures);

            this.level = level;
            combatOrder = CombatLevelFactory.generateTurns(new List<CombatPlayer>{humanPlayer,aiPlayer});

        }

        public void Update() {
            bool isHumanTurn = humanPlayer.HasCreature(combatOrder[currentTurn]);
            if (!isHumanTurn) {
                return;
            }
            if (Input.GetMouseButton(0)) {
                /*
                Vector2 mousePosition = Input.mousePosition;
                Vector2 mouseWorldPosition = mCamera.ScreenToWorldPoint(mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition,Vector2.zero,Mathf.Infinity,1 << LayerMask.NameToLayer("Creature"));
                if (hit.collider != null) {
                    Transform transform = hit.collider.transform;
                    foreach (KeyValuePair<EquipedCreeture,Transform> kvp in creatures) {
                        if (kvp.Value == transform && aiPlayer.Contains(kvp.Key)) {
                            playerSelectCreature(kvp.Key);
                            break;
                        }
                    }
                }
                */
            }
            
        }

        private IEnumerator moveAI() {
            yield return new WaitForSeconds(0.5f);
            int move = Random.Range(0,2);
            List<CreatureInCombat> targets = humanPlayer.Creatures.ToList();
            int n = targets.Count;
            System.Random rng = new System.Random();
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                CreatureInCombat value = targets[k];
                targets[k] = targets[n];
                targets[n] = value;
            }
            /*
            switch (move) {
                case 0: // attack
                    foreach (EquipedCreeture target in targets) {
                        if (target.CurrentHealth <= 0) {
                            continue;
                        }
                        Debug.Log("ai attack");
                        target.attack(currentCreatureTurn.getAttacks());
                        updateHealth(target);
                        break;
                    }
                    break;
                case 1: // heal
                    currentCreatureTurn.heal();
                    updateHealth(currentCreatureTurn);
                    break;
                case 2: // defend
                    break;
            }
            yield return new WaitForSeconds(0.5f);
            iterateTurn();
            */
        }

        private void iterateTurn() {
            if (gameOver) {
                return;
            }
            if (humanPlayer.IsDead()) {
                gameOver = true;
                showLossScreen();
                return;
            }
            if (aiPlayer.IsDead()) {
                gameOver = true;
                showWinScreen();
                return;
            }
            currentTurn++;
            if (currentTurn >= combatOrder.Count) {
                currentTurn = 0;
            }

            CreatureInCombat currentCreation = combatOrder[currentTurn]; 
            if (aiPlayer.HasCreature(combatOrder[currentTurn])) {
                Debug.Log("Moving ai");
                StartCoroutine(moveAI());
                return;
            }
        }

        private void showWinScreen() {
            GameObject over = GameObject.Instantiate(Resources.Load<GameObject>("UI/GameWinScreen"));
            GameObject.Destroy(this);
            GameObject.Destroy(GameObject.Find("CombatUI(Clone)"));
            over.transform.SetParent(GameObject.Find("MainCanvas").transform,false);
            GameObject player = new GameObject();
            PlayerIO playerIO = player.AddComponent<PlayerIO>();
            foreach (Lootable lootable in level.lootables) {
                playerIO.give(lootable);
            }
            Debug.Log("Player Win");
        }

        private void showLossScreen() {
            GameObject over = GameObject.Instantiate(Resources.Load<GameObject>("UI/GameOverScreen"));
            GameObject.Destroy(this);
            GameObject.Destroy(GameObject.Find("CombatUI(Clone)"));
            over.transform.SetParent(GameObject.Find("MainCanvas").transform,false);
            Debug.Log("AI Win");
        }

       
        

        private void playerSelectCreature(CreatureInCombat creeture) {
            /*
            if (selectedPlayerCreature == creeture) {
                return;
            }
            if (selectedPlayerCreature != null) {
                Transform playerSelect = creatures[selectedPlayerCreature].Find("PlayerHighlight");
                GameObject.Destroy(playerSelect.gameObject);
            }
            selectedPlayerCreature = creeture;
            GameObject highlighter = new GameObject();
            highlighter.name = "PlayerHighlight";
            SpriteRenderer spriteRenderer = highlighter.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/turn_halo");
            highlighter.transform.localScale = new Vector3(4.5f,4.5f,1);
            spriteRenderer.color = Color.red;
            highlighter.transform.position = new Vector3(0,0,1);
            highlighter.transform.SetParent(creatures[selectedPlayerCreature],false);
            */
        }
        private void setCreatureTurn(CreatureInCombat creeture) {
            /*
            if (currentCreatureTurn != null && currentCreatureTurn.Health > 0) {
                Transform highligher = creatures[currentCreatureTurn].Find("TurnHighlight");
                GameObject.Destroy(highligher.gameObject);
            }
            currentCreatureTurn = creeture;
            if (currentCreatureTurn.Health <= 0) {
                iterateTurn();
                return;
            }
            GameObject highlighter = new GameObject();
            highlighter.name = "TurnHighlight";
            SpriteRenderer spriteRenderer = highlighter.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/turn_halo");
            spriteRenderer.color = Color.yellow;
            highlighter.transform.localScale = new Vector3(4,4,1);
            highlighter.transform.position = new Vector3(0,0,1);
            highlighter.transform.SetParent(creatures[currentCreatureTurn],false);
            */
            
        }


    }
}


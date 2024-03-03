using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreatureModule;
using System.Linq;
using TMPro;

namespace LevelModule.Combat {
    public class CombatLevelController : MonoBehaviour
    {
        private Camera mCamera;
        private HashSet<EquipedCreeture> player;
        private HashSet<EquipedCreeture> ai;
        private Dictionary<EquipedCreeture, Transform> creatures;
        private List<EquipedCreeture> turns;
        private EquipedCreeture currentCreatureTurn;
        private EquipedCreeture selectedPlayerCreature;
        private int currentTurn;
        private bool gameOver;
        
        public bool IsPlayerTurn {get => player.Contains(currentCreatureTurn);}
        // Start is called before the first frame update
        
        public void init(Dictionary<EquipedCreeture, Transform> playerD, Dictionary<EquipedCreeture, Transform> aiD) {
            creatures = new Dictionary<EquipedCreeture, Transform>();
            player = new HashSet<EquipedCreeture>();
            ai = new HashSet<EquipedCreeture>();
            foreach (KeyValuePair<EquipedCreeture, Transform> kvp in playerD) {
                creatures[kvp.Key] = kvp.Value;
                kvp.Key.initForBattle();
                player.Add(kvp.Key);
            }
            foreach (KeyValuePair<EquipedCreeture, Transform> kvp in aiD) {
                creatures[kvp.Key] = kvp.Value;
                kvp.Key.initForBattle();
                ai.Add(kvp.Key);
            }
            generateTurns();
        }

        public void Start() {
            mCamera = GameObject.Find("Camera").GetComponent<Camera>();
        }

        private void updateHealth(EquipedCreeture equipedCreeture) {
            Transform creatureTransform = creatures[equipedCreeture];
            Transform textTransform = creatureTransform.Find("HealthText");
            TextMeshPro textMeshPro = textTransform.GetComponent<TextMeshPro>();
            textMeshPro.text = equipedCreeture.CurrentHealth.ToString();
        }

        public void Update() {
            if (!IsPlayerTurn) {
                return;
            }
            if (Input.GetMouseButton(0)) {
                Vector2 mousePosition = Input.mousePosition;
                Vector2 mouseWorldPosition = mCamera.ScreenToWorldPoint(mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition,Vector2.zero,Mathf.Infinity,1 << LayerMask.NameToLayer("Creature"));
                if (hit.collider != null) {
                    Transform transform = hit.collider.transform;
                    foreach (KeyValuePair<EquipedCreeture,Transform> kvp in creatures) {
                        if (kvp.Value == transform && ai.Contains(kvp.Key)) {
                            playerSelectCreature(kvp.Key);
                            break;
                        }
                    }
                }
            }
            
        }

        private void generateTurns() {
            turns = new List<EquipedCreeture>();
            turns.AddRange(creatures.Keys.ToList());
            turns = turns.OrderBy(creature => creature.getSpeed()).ToList();
            setCreatureTurn(turns[0]);
        }

        private void awaitMove() {
            Debug.Log("Player Turn");
        }

    
        private IEnumerator moveAI() {
            yield return new WaitForSeconds(0.5f);
            int move = Random.Range(0,2);
            List<EquipedCreeture> targets = player.ToList();
            int n = targets.Count;
            System.Random rng = new System.Random();
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                EquipedCreeture value = targets[k];
                targets[k] = targets[n];
                targets[n] = value;
            }
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
        }

        public void playerAttack() {
            if (!IsPlayerTurn) {
                return;
            }
            if (selectedPlayerCreature == null) {
                return;
            }
            selectedPlayerCreature.attack(currentCreatureTurn.getAttacks());
            updateHealth(selectedPlayerCreature);
            iterateTurn();
        }

        public void playerHeal() {
            if (!IsPlayerTurn) {
                return;
            }
            currentCreatureTurn.heal();
            updateHealth(currentCreatureTurn);
            iterateTurn();
        }

        public void playerDefend() {
            if (!IsPlayerTurn) {
                return;
            }
            if (selectedPlayerCreature == null) {
                return;
            }
            iterateTurn();
        }

        private void iterateTurn() {
            if (gameOver) {
                return;
            }
            if (playerDead()) {
                gameOver = true;
                showLossScreen();
                return;
            }
            if (aiDead()) {
                gameOver = true;
                showWinScreen();
                return;
            }
            currentTurn++;
            if (currentTurn >= turns.Count) {
                currentTurn = 0;
            }
            setCreatureTurn(turns[currentTurn]);
            if (ai.Contains(currentCreatureTurn)) {
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
            Debug.Log("Player Win");
        }

        private void showLossScreen() {
            GameObject over = GameObject.Instantiate(Resources.Load<GameObject>("UI/GameOverScreen"));
            GameObject.Destroy(this);
            GameObject.Destroy(GameObject.Find("CombatUI(Clone)"));
            over.transform.SetParent(GameObject.Find("MainCanvas").transform,false);
            Debug.Log("AI Win");
        }

        private bool playerDead() {
            foreach (EquipedCreeture equipedCreeture in player) {
                if (equipedCreeture.CurrentHealth > 0) {
                    return false;
                }
            }
            return true;
        }
        private bool aiDead() {
            foreach (EquipedCreeture equipedCreeture in ai) {
                if (equipedCreeture.CurrentHealth > 0) {
                    return false;
                }
            }
            return true;
        }

        private void playerSelectCreature(EquipedCreeture creeture) {
            
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
        }
        private void setCreatureTurn(EquipedCreeture creeture) {
            if (currentCreatureTurn != null && currentCreatureTurn.CurrentHealth > 0) {
                Transform highligher = creatures[currentCreatureTurn].Find("TurnHighlight");
                GameObject.Destroy(highligher.gameObject);
            }
            currentCreatureTurn = creeture;
            if (currentCreatureTurn.CurrentHealth <= 0) {
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
            
        }


    }
}


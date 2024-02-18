using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreatureModule;
using System.Linq;

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
        
        public bool IsPlayerTurn {get => player.Contains(currentCreatureTurn);}
        // Start is called before the first frame update
        
        public void init(Dictionary<EquipedCreeture, Transform> playerD, Dictionary<EquipedCreeture, Transform> aiD) {
            creatures = new Dictionary<EquipedCreeture, Transform>();
            player = new HashSet<EquipedCreeture>();
            ai = new HashSet<EquipedCreeture>();
            foreach (KeyValuePair<EquipedCreeture, Transform> kvp in playerD) {
                creatures[kvp.Key] = kvp.Value;
                player.Add(kvp.Key);
            }
            foreach (KeyValuePair<EquipedCreeture, Transform> kvp in aiD) {
                creatures[kvp.Key] = kvp.Value;
                ai.Add(kvp.Key);
            }
            generateTurns();
        }

        public void Start() {
            mCamera = GameObject.Find("Camera").GetComponent<Camera>();
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
            yield return new WaitForSeconds(0.25f);
            int move = Random.Range(0,3);
            switch (move) {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
            yield return new WaitForSeconds(0.25f);
            iterateTurn();
        }

        public void playerAttack() {
            if (!IsPlayerTurn) {
                return;
            }
            if (selectedPlayerCreature == null) {
                return;
            }
            iterateTurn();
        }

        public void playerHeal() {
            if (!IsPlayerTurn) {
                return;
            }
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
            if (currentCreatureTurn != null) {
                Transform highligher = creatures[currentCreatureTurn].Find("TurnHighlight");
                GameObject.Destroy(highligher.gameObject);
            }

            currentCreatureTurn = creeture;
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


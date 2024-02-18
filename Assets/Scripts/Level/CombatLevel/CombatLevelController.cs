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
            Debug.Log(ai.Count);
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
                        if (kvp.Value == transform) {
                            Debug.Log(transform.name);
                            break;
                        } 
                        
                        /*&& ai.Contains(kvp.Key)) {
                            playerSelectCreature(kvp.Key);
                        }
                        */
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

        }

        private void playerSelectCreature(EquipedCreeture creeture) {
            if (selectedPlayerCreature != null) {
                Transform playerSelect = creatures[creeture].Find("PlayerHighlight");
                GameObject.Destroy(playerSelect);
            }
            Debug.Log("1");
            selectedPlayerCreature = creeture;
            GameObject highlighter = new GameObject();
            highlighter.name = "PlayerHighlight";
            SpriteRenderer spriteRenderer = highlighter.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Borders/Common_border");
            highlighter.transform.position = new Vector3(0,0,1);
            highlighter.transform.SetParent(creatures[currentCreatureTurn],false);
        }
        private void setCreatureTurn(EquipedCreeture creeture) {
            if (currentCreatureTurn != null) {
                Transform highligher = creatures[creeture].Find("TurnHighlight");
                GameObject.Destroy(highligher);
            }

            currentCreatureTurn = creeture;
            GameObject highlighter = new GameObject();
            highlighter.name = "TurnHighlight";
            SpriteRenderer spriteRenderer = highlighter.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Borders/Common_border");
            highlighter.transform.position = new Vector3(0,0,1);
            highlighter.transform.SetParent(creatures[currentCreatureTurn],false);
            
        }


    }
}


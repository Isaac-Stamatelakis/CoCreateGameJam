using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using Items.Equipment;

namespace Levels.Combat {
    public enum HighlightType {
        View,
        Turn,
        Enemy,
        Ally
    }
    public interface ISyncedMovementObject {
        public void move(Vector3 vector);
    }
    public class CreatureCombatObject : MonoBehaviour, ISyncedMovementObject, ICombatCreature
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer highlightSprite;
        [SerializeField] private AudioSource audioSource;
        private CreatureInCombat creatureInCombat;
        private CreatureCombatUI combatUI;
        public CreatureInCombat CreatureInCombat { get => creatureInCombat; }
        public CreatureCombatUI CombatUI { get => combatUI; }
        public Animator Animator { get => animator; }
        public AudioSource AudioSource { get => audioSource; }
        private Vector3 originPosition;
        private List<StatusEffect> statusEffects;
        private bool dead;

        public void Start() {
            this.originPosition = transform.position;
        }

        public void display(CreatureInCombat creatureInCombat) {
            this.creatureInCombat = creatureInCombat;
            animator.runtimeAnimatorController = creatureInCombat.EquipedCreeture.Creeture.AnimationController;
        }

        public void syncCombatUI(CreatureCombatUI creatureCombatUI) {
            this.combatUI = creatureCombatUI;
            this.originPosition = transform.position;
            resetUIPosition();
        }

        public void resetUIPosition() {
            if (combatUI==null) {
                return;
            }
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Vector3 SizeY = new Vector3(0,spriteRenderer.size.y/2f*1.25f,0);
            Vector3 creatureTopPosition = originPosition + SizeY;
            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main,creatureTopPosition);
            CombatUI.transform.position = screenPosition;
        }

        public void highlight(HighlightType? highlightType) {
            if (highlightSprite==null) {
                return;
            }
            switch (highlightType) {
                case HighlightType.Turn:
                    changeHighlightColor(Color.cyan);
                    break;
                case HighlightType.Enemy:
                    changeHighlightColor(Color.red);
                    break;
                case HighlightType.Ally:
                    changeHighlightColor(Color.green);
                    break;
                case HighlightType.View:
                    changeHighlightColor(Color.yellow);
                    break;
                default:
                    highlightSprite.gameObject.SetActive(false);
                    break;
            }
        }


        private void changeHighlightColor(Color color) {
            highlightSprite.gameObject.SetActive(true);
            highlightSprite.color = color;
        }

        public void move(Vector3 vector)
        {
            transform.position += vector;
            if (CombatUI == null) {
                return;
            }
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(CombatUI.transform.position);
            worldPosition += vector;
            CombatUI.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main,worldPosition);
        }

        public void setPosition(Vector3 vector) {
            transform.position = vector;
            resetUIPosition();
        }

        public IEnumerator resetPosition() {
            (Vector3,int) tuple = GlobalUtils.speedAndIterationsToMove(originPosition,transform.position,1);
            Vector3 speed = tuple.Item1;
            int iterations = tuple.Item2;
            while (iterations > 0) {
                iterations--;
                move(speed);
                yield return new WaitForFixedUpdate();
            }
            setPosition(originPosition);
        }

        public void kill() {
            if (dead) {
                return;
            }
            dead = true;
            GameObject.Destroy(highlightSprite.gameObject);
            GameObject.Destroy(combatUI.gameObject);
            StartCoroutine(killAnimation());
        }

        private IEnumerator killAnimation() {
            int speed = 3;
            Quaternion rotationDegrees = Quaternion.Euler(speed, 0, 0);
            for (int i = 0; i < 90/speed; i++) {
                Vector3 position = transform.position;
                position.y -= 0.025f;
                transform.position = position;
                Quaternion rotation = transform.rotation;
                rotation = rotation * rotationDegrees;
                transform.rotation = rotation;
                yield return new WaitForFixedUpdate();
            }
            GameObject.Destroy(gameObject);
        }

        public void hit(float damage, DamageType damageType)
        {
            if (dead) {
                return;
            }
            DamageIndicatorUI damageIndicatorUI = CombatLevelPrefabContainer.Instance.getDamageIndicator();
            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main,transform.position);
            damageIndicatorUI.display(damage,damageType,screenPosition);
            damageIndicatorUI.transform.SetParent(CombatLevelController.Instance.CanvasTransform);
            creatureInCombat.hit(damage,damageType);
            
            if (creatureInCombat.Health <= 0) {
                kill();
            }
            combatUI.display();
        }

        public void heal(float amount)
        {
            creatureInCombat.heal(amount);
            combatUI.display();
        }

        public void addStatus(StatusEffect statusEffect)
        {
            creatureInCombat.addStatus(statusEffect);
        }

        public bool hasStatus(string statusName)
        {
            return creatureInCombat.hasStatus(statusName);
        }

        public List<EnchantedEquipment> getEquipment()
        {
            return creatureInCombat.getEquipment();
        }

        public float getHealth()
        {
            return creatureInCombat.getHealth();
        }

        public float getHealthPercent()
        {
            return creatureInCombat.getHealthPercent();
        }

        public float getMana()
        {
            return creatureInCombat.getMana();
        }

        public float getManaPercent()
        {
            return creatureInCombat.getManaPercent();
        }

        public float getStat(CreatureStat creatureStat)
        {
            return creatureInCombat.getStat(creatureStat);
        }
    }
}


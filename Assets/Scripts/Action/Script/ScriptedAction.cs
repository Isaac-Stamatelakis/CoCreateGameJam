using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Levels.Combat;

namespace Actions.Script {
    [CreateAssetMenu(fileName = "New Action Script", menuName = "Action/Script")]
    public class ScriptedAction : ScriptableObject, ICombatAction
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private List<Declaration<RuntimeAnimatorController>> animations;
        [SerializeField] private List<Declaration<GameObject>> prefabs;
        [SerializeField] private List<Declaration<AudioClip>> sounds;
        [SerializeField] private string actionScript;

        public string ActionScript { get => actionScript;}
        public Dictionary<string,GameObject> PrefabDict {get => DeclarationFactory.ToDict<GameObject>(prefabs);}
        public Dictionary<string,RuntimeAnimatorController> AnimationDict {get => DeclarationFactory.ToDict<RuntimeAnimatorController>(animations);}
        public Dictionary<string,AudioClip> SoundDict {get => DeclarationFactory.ToDict<AudioClip>(sounds);}
        [System.Serializable]
        private class Declaration<T> {
            [SerializeField] private string key;
            [SerializeField] private T value;
            public string Key { get => key; }
            public T Value { get => value; }
        }
        private static class DeclarationFactory {
            public static Dictionary<string,T> ToDict<T>(List<Declaration<T>> declarations) {
                Dictionary<string,T> dict = new Dictionary<string, T>();
                foreach (Declaration<T> declaration in declarations) {
                    dict[declaration.Key] = declaration.Value;
                }
                return dict;
            }
        }

        public string getTitle()
        {
            return name;
        }

        public string getDescription()
        {
            return ActionScriptInterpretorUtils.getDescription(actionScript);
        }

        public Sprite getSprite()
        {
            return sprite;
        }
    }
}


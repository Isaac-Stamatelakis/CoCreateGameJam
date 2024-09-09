using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player;
using Levels.Combat;
using System;
using System.Linq;

namespace Levels {
    public interface ILevelLoader {
        public void load(ILevel level);
    }
    public abstract class LevelLoader<T> : MonoBehaviour, ILevelLoader where T : LevelObject
    {
        [SerializeField] private T testLevel;
        private bool loaded = false;
        public void Awake() {
            StartCoroutine(loadTest());
        }

        private IEnumerator loadTest() {
            yield return new WaitForFixedUpdate();
            if (loaded) {
                yield return null;
            }
            Debug.Log($"Loading Test Level '{testLevel.name}'");
            load(testLevel);
        }
        public void load(ILevel level) {
            string levelType = level.GetType().ToString().Split(".").Last();
            string sceneName = SceneManager.GetActiveScene().name;
            Debug.Log($"{levelType} Loaded In Scene '{sceneName}'");
            PrepareToLoad();
            loadLevel((T)level);
            loaded = true;
            GameObject.Destroy(this);
        }

        public void PrepareToLoad() {
            GameObject player = new GameObject();
            player.name = "Player";
            player.AddComponent<PlayerIO>();
        }
        protected abstract void loadLevel(T level);
    }
    
    public class LevelManager {
        public static ILevel currentLevel;
        public static void changeLevel(ILevel level) {
            SceneManager.LoadScene(level.getSceneName());
            currentLevel = level;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameObject[] objects = scene.GetRootGameObjects();

            foreach (GameObject gameObject in objects) {
                ILevelLoader levelLoader = gameObject.GetComponent<ILevelLoader>();
                if (levelLoader != null) {
                    levelLoader.load(currentLevel);
                }
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}


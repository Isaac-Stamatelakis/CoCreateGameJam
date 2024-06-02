using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player;
using Levels.Combat;

namespace Levels {
    public interface ILevelLoader {
        public void load(Level level);
    }
    public abstract class LevelLoader<T> : MonoBehaviour, ILevelLoader where T : Level
    {
        private T level;
        public void initalize(Level level) {
            this.level = (T) level;
        }
        public void load(Level level) {
            PrepareToLoad();
            loadLevel((T)level);
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
        public static Level currentLevel;
        public static void changeLevel(Level level) {
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


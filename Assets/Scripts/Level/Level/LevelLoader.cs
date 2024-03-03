using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player;
using LevelModule.Combat;

namespace LevelModule {
    public class LevelLoader : MonoBehaviour
    {
        public void load(Level level) {
            GameObject levelPrefab = GameObject.Instantiate(level.levelPrefab);
            level.load();
            //GameObject playerPrefab = Resources.Load<GameObject>("Player");
            //GameObject player = GameObject.Instantiate(playerPrefab);
            GameObject.Destroy(this);
        }
    }
    
    public class LevelManager {
        public static Level currentLevel;
        public static void changeLevel(Level level) {
            SceneManager.LoadScene("LevelScene");
            if (level is CombatLevel combatLevel) {
                combatLevel.initalize(GameObject.Find("Player").GetComponent<PlayerIO>().EquipedCreetures);
            }
            currentLevel = level;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            LevelLoader levelLoader = GameObject.Find("Loader").GetComponent<LevelLoader>();
            levelLoader.load(currentLevel);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Trading {
    public class TradingCreatureContainer : MonoBehaviour
    {
        private TradingCreatureObject[] creatures;
        public void Start() {
            creatures = GetComponentsInChildren<TradingCreatureObject>();
        }


    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trading {
    public class TradingCreatureObject : MonoBehaviour
    {
        private TradingCreature tradingCreature;
        public void display(TradingCreature tradingCreature) {
            GetComponent<Animator>().runtimeAnimatorController = tradingCreature.Creature.AnimationController;
            this.tradingCreature = tradingCreature;
        }
    }

}

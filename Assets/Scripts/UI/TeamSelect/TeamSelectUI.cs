using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using UnityEngine.UI;
using Player;

namespace UI.TeamSelect {
    public class TeamSelectUI : MonoBehaviour
    {
        [SerializeField] private TeamSelectCreatureListUI teamSelectCreatureList;
        public void Start() {
            teamSelectCreatureList.display(PlayerIO.Instance.getPlayerTeam());
        }
    }
}


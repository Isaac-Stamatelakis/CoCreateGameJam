using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreatureModule;
using UnityEngine.UI;
using TMPro;

namespace InventoryModule.Creetures {
    public class CreaterViewer : MonoBehaviour
    {
        private Creeture creeture;
        public Creeture Creeture { get => creeture; set => creeture = value; }
        public void initalize(Creeture creeture) {
            this.creeture = creeture;
            Image image = transform.Find("Image").GetComponent<Image>();
            image.sprite = creeture.sprite;
            TextMeshProUGUI text = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            text.text = creeture.name;
        }
    }
}


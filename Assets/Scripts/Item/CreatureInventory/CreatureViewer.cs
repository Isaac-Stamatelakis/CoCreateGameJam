using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;
using UnityEngine.UI;
using TMPro;

namespace Items.Creetures {
    public class CreaterViewer : MonoBehaviour
    {
        private Creature creeture;
        public Creature Creeture { get => creeture; set => creeture = value; }
        public void initalize(Creature creeture) {
            this.creeture = creeture;
            Image image = transform.Find("Image").GetComponent<Image>();
            image.sprite = creeture.Sprite;
            TextMeshProUGUI text = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            text.text = creeture.name;
        }
    }
}


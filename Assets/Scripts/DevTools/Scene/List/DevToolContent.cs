using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevTools {
    public class DevToolContent : IDisplayable
    {
        private string content;
        public string getName()
        {
            return content;
        }

        public Sprite getSprite()
        {
            return null;
        }
    }
}


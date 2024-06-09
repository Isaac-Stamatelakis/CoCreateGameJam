using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels {
    public static class LineFactory
    {
        /// <summary>
        /// Doesn't work with URP
        /// </summary>
        public static GameObject createRender(WorldNode node, WorldNode connection, Transform lineContainer) {
            GameObject line = new GameObject();
            line.name = node.name + " To " + connection.name;
            line.transform.SetParent(lineContainer);
            LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
            Material defaultMaterial = Resources.Load<Material>("Materials/Default.mat");
            Shader shader = lineRenderer.GetComponent<Shader>();
            //shader = defaultMaterial;
            //lineRenderer.material = defaultMaterial;
            lineRenderer.materials[0] = defaultMaterial;
            lineRenderer.startWidth = 0.25f;
            lineRenderer.endWidth = 0.25f;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, node.transform.position);
            lineRenderer.SetPosition(1, connection.transform.position);
            return line;
        }


        public static GameObject create(WorldNode node, WorldNode connection, Transform lineContainer) {
            GameObject line = new GameObject();

            line.name = node.name + " To " + connection.name;
            line.transform.SetParent(lineContainer,false);
            SpriteRenderer spriteRenderer = line.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/white_box");
            line.transform.position = (node.transform.position + connection.transform.position)/2;
            Vector2 dif = node.transform.position - connection.transform.position;
            float dist = Vector2.Distance(node.transform.position,connection.transform.position);
            float angle = Mathf.Atan2(dif.y,dif.x) * Mathf.Rad2Deg;
            line.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            line.transform.localScale = new Vector3(dist/(spriteRenderer.bounds.size.x)*1.2f,0.5f);
            return line;
        }
    }
}


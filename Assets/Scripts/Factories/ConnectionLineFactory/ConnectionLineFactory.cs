using Models;
using UnityEngine;
using Views;

namespace Factories.ConnectionLineFactory
{
    public class ConnectionLineFactory : IConnectionLineFactory
    {
        private const float LINE_WIDTH = 0.07f;

        private readonly Transform m_root;
        private readonly Material m_lineMaterial;

        public ConnectionLineFactory(Transform _root)
        {
            m_root = _root;
            m_lineMaterial = CreateLineMaterial();
        }

        public ConnectionLineView Create(ConnectionModel _model)
        {
            var lineObject = new GameObject("Connection Line");
            lineObject.transform.SetParent(m_root, false);

            var lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.useWorldSpace = true;
            lineRenderer.startWidth = LINE_WIDTH;
            lineRenderer.endWidth = LINE_WIDTH;
            lineRenderer.material = m_lineMaterial;
            lineRenderer.numCapVertices = 4;

            var view = lineObject.AddComponent<ConnectionLineView>();
            view.Initialize(_model, lineRenderer);

            return view;
        }

        private static Material CreateLineMaterial()
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit");

            if (shader == null)
                shader = Shader.Find("Unlit/Color");

            if (shader == null)
                shader = Shader.Find("Sprites/Default");

            var material = new Material(shader);
            material.color = Color.white;

            return material;
        }
    }
}
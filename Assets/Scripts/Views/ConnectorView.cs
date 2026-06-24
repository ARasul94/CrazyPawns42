using UnityEngine;

namespace Views
{
    public class ConnectorView : MonoBehaviour
    {
        [SerializeField] private Renderer m_renderer;
        [SerializeField] private Collider m_collider;

        private Material m_defaultMaterial;

        public PawnView owner { get; private set; }
        public Transform cachedTransform => transform;
        public Collider collider => m_collider;

        public void Initialize(PawnView _owner)
        {
            owner = _owner;

            if (m_renderer != null)
                m_defaultMaterial = m_renderer.sharedMaterial;
        }

        public void SetMaterial(Material _material)
        {
            if (m_renderer != null && _material != null)
                m_renderer.sharedMaterial = _material;
        }

        public void RestoreMaterial()
        {
            if (m_renderer != null && m_defaultMaterial != null)
                m_renderer.sharedMaterial = m_defaultMaterial;
        }

        private void Reset()
        {
            m_renderer = GetComponent<Renderer>();
            m_collider = GetComponent<Collider>();
        }
    }
}
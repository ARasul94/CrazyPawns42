using Models;
using UnityEngine;

namespace Views
{
    public class ConnectionLineView : MonoBehaviour
    {
        private ConnectionModel m_model;
        private LineRenderer m_lineRenderer;

        public ConnectionModel model => m_model;

        public void Initialize(ConnectionModel _model, LineRenderer _lineRenderer)
        {
            m_model = _model;
            m_lineRenderer = _lineRenderer;

            m_lineRenderer.positionCount = 2;
            UpdatePositions();
        }

        public void UpdatePositions()
        {
            if (m_model == null || m_lineRenderer == null)
                return;

            m_lineRenderer.SetPosition(0, m_model.from.cachedTransform.position);
            m_lineRenderer.SetPosition(1, m_model.to.cachedTransform.position);
        }

        public void DestroyView()
        {
            Destroy(gameObject);
        }
    }
}
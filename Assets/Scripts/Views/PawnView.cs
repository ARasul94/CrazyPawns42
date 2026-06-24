using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using ViewModels;

namespace Views
{
    public class PawnView : MonoBehaviour
    {
        [SerializeField] private Collider m_bodyCollider;
        [SerializeField] private Renderer[] m_bodyRenderers;
        [SerializeField] private ConnectorView[] m_connectors;

        private readonly CompositeDisposable m_disposables = new();
        private Material[] m_defaultBodyMaterials;
        private PawnViewModel m_viewModel;
        private Material m_deleteMaterial;

        public event Action<PawnView> Destroying;
        
        public bool isDestroyed { get; private set; }
        public int id => m_viewModel.id;
        public PawnViewModel viewModel => m_viewModel;
        public IReadOnlyList<ConnectorView> connectors => m_connectors;
        public Transform cachedTransform => transform;

        public void Initialize(PawnViewModel _viewModel, Material _deleteMaterial)
        {
            m_viewModel = _viewModel;
            m_deleteMaterial = _deleteMaterial;

            CacheDefaultMaterials();

            foreach (var connector in m_connectors)
                connector.Initialize(this);
            
            m_viewModel.position
                .Subscribe(SetPosition)
                .AddTo(m_disposables);

            m_viewModel.isOutsideBoard
                .Subscribe(SetDeleteState)
                .AddTo(m_disposables);
        }

        public bool IsBodyCollider(Collider _target)
        {
            return _target != null && _target == m_bodyCollider;
        }

        public void SetPosition(Vector3 _position)
        {
            transform.position = _position;
        }

        public void DestroyView()
        {
            if (isDestroyed)
                return;

            isDestroyed = true;
            Destroying?.Invoke(this);
            Destroy(gameObject);
        }

        private void SetDeleteState(bool _isOutsideBoard)
        {
            if (_isOutsideBoard)
                ApplyDeleteMaterial();
            else
                RestoreDefaultMaterials();
        }

        private void ApplyDeleteMaterial()
        {
            if (m_deleteMaterial == null)
                return;

            foreach (var bodyRenderer in m_bodyRenderers)
            {
                if (bodyRenderer != null)
                    bodyRenderer.sharedMaterial = m_deleteMaterial;
            }

            foreach (var connector in m_connectors)
                connector.SetMaterial(m_deleteMaterial);
        }

        private void RestoreDefaultMaterials()
        {
            for (var i = 0; i < m_bodyRenderers.Length; i++)
            {
                if (m_bodyRenderers[i] != null && m_defaultBodyMaterials[i] != null)
                    m_bodyRenderers[i].sharedMaterial = m_defaultBodyMaterials[i];
            }

            foreach (var connector in m_connectors)
                connector.RestoreMaterial();
        }

        private void CacheDefaultMaterials()
        {
            m_defaultBodyMaterials = new Material[m_bodyRenderers.Length];

            for (var i = 0; i < m_bodyRenderers.Length; i++)
            {
                if (m_bodyRenderers[i] != null)
                    m_defaultBodyMaterials[i] = m_bodyRenderers[i].sharedMaterial;
            }
        }

        private void OnDestroy()
        {
            m_disposables.Dispose();
            m_viewModel?.Dispose();
        }

        private void Reset()
        {
            m_bodyCollider = GetComponentInChildren<BoxCollider>();
            m_bodyRenderers = GetComponentsInChildren<Renderer>();
            m_connectors = GetComponentsInChildren<ConnectorView>();
        }
    }
}
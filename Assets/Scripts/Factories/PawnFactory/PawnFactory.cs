using Models;
using Services.PawnRegistryService;
using UnityEngine;
using ViewModels;
using Views;
using Zenject;

namespace Factories.PawnFactory
{
    public class PawnFactory : IPawnFactory
    {
        private readonly IPawnRegistryService m_pawnRegistry;
        private readonly DiContainer m_container;
        private readonly PawnView m_prefab;
        private readonly Transform m_root;
        private readonly CrazyPawn.CrazyPawnSettings m_settings;

        private int m_nextId;

        public PawnFactory(
            DiContainer _container,
            PawnView _prefab,
            Transform _root,
            CrazyPawn.CrazyPawnSettings _settings,
            IPawnRegistryService _pawnRegistryService)
        {
            m_container = _container;
            m_prefab = _prefab;
            m_root = _root;
            m_settings = _settings;
            m_pawnRegistry = _pawnRegistryService;
        }

        public PawnView Create(Vector3 _position)
        {
            var view = m_container.InstantiatePrefabForComponent<PawnView>(m_prefab, _position, Quaternion.identity, m_root);
            var model = new PawnModel(m_nextId++, _position);
            var viewModel = new PawnViewModel(model);

            view.Initialize(viewModel, m_settings.DeleteMaterial);
            m_pawnRegistry.Register(view);

            return view;
        }
    }
}
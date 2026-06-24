using EntryPoints;
using Factories.PawnFactory;
using Services;
using Services.BoardBoundsService;
using Services.InputRaycastService;
using UnityEngine;
using Views;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Scene")]
        [SerializeField] private Camera m_camera;
        [SerializeField] private Transform m_pawnsRoot;

        [Header("Templates")]
        [SerializeField] private PawnView m_pawnPrefab;
        [SerializeField] private CrazyPawn.CrazyPawnSettings m_settings;

        public override void InstallBindings()
        {
            Container.BindInstance(m_settings).AsSingle();
            Container.BindInstance(m_camera).AsSingle();

            Container.Bind<IInputRaycastService>()
                .To<InputRaycastService>()
                .AsSingle();

            Container.Bind<IBoardBoundsService>()
                .To<BoardBoundsService>()
                .AsSingle();

            Container.Bind<IPawnFactory>()
                .To<PawnFactory>()
                .AsSingle()
                .WithArguments(m_pawnPrefab, m_pawnsRoot);

            Container.Bind<PawnSpawnService>()
                .AsSingle();

            Container.BindInterfacesTo<PawnDragService>()
                .AsSingle();

            Container.BindInterfacesTo<GameEntryPoint>()
                .AsSingle();
        }

        private void Reset()
        {
            m_camera = Camera.main;
        }
    }
}
using EntryPoints;
using Factories.ConnectionLineFactory;
using Factories.PawnFactory;
using Services;
using Services.BoardBoundsService;
using Services.ConnectionPreviewService;
using Services.ConnectionService;
using Services.InputRaycastService;
using Services.InteractionService;
using Services.PawnRegistryService;
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
        [SerializeField] private Transform m_connectionsRoot;
        [SerializeField] private BoardView m_boardView;

        [Header("Templates")]
        [SerializeField] private PawnView m_pawnPrefab;
        [SerializeField] private CrazyPawn.CrazyPawnSettings m_settings;

        public override void InstallBindings()
        {
            Container.BindInstance(m_settings).AsSingle();
            Container.BindInstance(m_camera).AsSingle();
            Container.BindInstance(m_boardView).AsSingle();

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
            
            Container.Bind<IPawnRegistryService>()
                .To<PawnRegistryService>()
                .AsSingle();

            Container.Bind<IConnectionLineFactory>()
                .To<ConnectionLineFactory>()
                .AsSingle()
                .WithArguments(m_connectionsRoot);
            
            Container.Bind<IConnectionPreviewService>()
                .To<ConnectionPreviewService>()
                .AsSingle()
                .WithArguments(m_connectionsRoot);

            Container.Bind<IConnectionService>()
                .To<ConnectionService>()
                .AsSingle();
            
            Container.Bind<IInteractionStateService>()
                .To<InteractionStateService>()
                .AsSingle();

            Container.Bind<PawnSpawnService>()
                .AsSingle();
            
            Container.Bind<BoardSpawnService>()
                .AsSingle();

            Container.BindInterfacesTo<PawnDragService>()
                .AsSingle();

            Container.BindInterfacesTo<GameEntryPoint>()
                .AsSingle();
            
            Container.BindInterfacesTo<ConnectionInputService>()
                .AsSingle();
            
            Container.BindInterfacesTo<CameraMoveService>()
                .AsSingle();
            
            Container.BindExecutionOrder<ConnectionInputService>(-30);
            Container.BindExecutionOrder<PawnDragService>(-20);
            Container.BindExecutionOrder<CameraMoveService>(-10);
        }

        private void Reset()
        {
            m_camera = Camera.main;
        }
    }
}
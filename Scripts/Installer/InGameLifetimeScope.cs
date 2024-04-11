using Unity1week202403.Data;
using Unity1week202403.Domain;
using Unity1week202403.Domain.Capture;
using Unity1week202403.Presentation;
using Unity1week202403.Structure;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1week202403.Installer
{
    public class InGameLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private MonsterMasterDataStoreSource _monsterMasterDataStoreSource;

        [SerializeField]
        private StageSceneDataStoreSource _stageSceneDataStoreSource;

        [SerializeField]
        private PlacePointView _placePointViewPrefab;

        [SerializeField]
        private Camera _captureCamera;

        [SerializeField]
        private RenderTexture _captureRenderTexture;

        [Header("Help")]
        [SerializeField]
        private Transform _helpRoot;

        [SerializeField]
        private HelpView _helpViewPrefab;

        [Header("MonsterDetail")]
        [SerializeField]
        private Transform _monsterDetailRoot;

        [SerializeField]
        private MonsterDetailView _monsterDetailViewPrefab;

        [Header("Performer")]
        [SerializeField]
        private Transform _battlePerformRoot;

        [SerializeField]
        private BattleReadyPerformView _battleReadyPerformViewPrefab;

        [SerializeField]
        private BattleStartPerformView _battleStartPerformViewPrefab;

        [SerializeField]
        private BattleResultFailedPerformView _battleResultFailedPerformViewPrefab;

        [SerializeField]
        private BattleResultVictoryPerformView _battleResultVictoryPerformViewPrefab;

        [Header("Settings")]
        [SerializeField]
        private int _startStageId;

        [SerializeField]
        private int _initialPlayerOwnCost;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<InGameLoop>();
            builder.Register<BattleInitializer>(Lifetime.Scoped);
            builder.Register<BattleShutdownUseCase>(Lifetime.Scoped);
            builder.Register<HelpAutoDisplayUseCase>(Lifetime.Singleton);

            builder.RegisterComponentOnNewGameObject<GizmoDrawer>(Lifetime.Singleton)
                .DontDestroyOnLoad();

            builder.RegisterBuildCallback(resolver => resolver.Resolve<GizmoDrawer>());

            BuildBattleReadyPerform(builder);
            BuildMonster(builder);
            BuildBattleMonster(builder);
            BuildStage(builder);
            BuildBattlePerform(builder);
            BuildMonsterSelect(builder);
            BuildPlayerStatus(builder);
            BuildSkill(builder);
            BuildTimeControl(builder);
            BuildHistory(builder);
            BuildCapture(builder);
            BuildDebug(builder);
            BuildHelp(builder);
        }

        private void BuildBattleReadyPerform(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab<BattleReadyPerformView>(_battleReadyPerformViewPrefab, Lifetime.Scoped)
                .UnderTransform(_battlePerformRoot);
            builder.RegisterEntryPoint<BattleReadyPerformPresenter>(Lifetime.Scoped).AsSelf();
        }

        private void BuildMonster(IContainerBuilder builder)
        {
            builder.Register<BattleMonsterPresenterContainer>(Lifetime.Scoped);
            builder.Register<MonsterMasterDataRepository>(Lifetime.Scoped);
            builder.Register<MonsterMasterDataService>(Lifetime.Scoped);
            builder.RegisterInstance<MonsterMasterDataStoreSource>(_monsterMasterDataStoreSource);
            builder.RegisterFactory<GameObject, Vector3, BattleMonsterPresenter>(resolver =>
                {
                    return (prefab, position) =>
                    {
                        var monster = Instantiate(prefab, position, Quaternion.identity)
                            .GetComponent<BattleMonsterPresenter>();
                        resolver.Inject(monster);
                        return monster;
                    };
                },
                Lifetime.Scoped);
        }

        private void BuildBattleMonster(IContainerBuilder builder)
        {
            builder.Register<BattleMonsterAttackUseCase>(Lifetime.Scoped);
            builder.Register<BattleMonsterContainer>(Lifetime.Scoped);
            builder.RegisterEntryPoint<BattleMonsterContainerService>(Lifetime.Scoped).AsSelf();
            builder.Register<BattleMonsterFactory>(Lifetime.Scoped);
            builder.Register<BattleMonsterPresenterFactory>(Lifetime.Scoped);
            builder.Register<CreateAllyBattleMonsterUseCase>(Lifetime.Scoped);
            builder.Register<CreateEnemyBattleMonsterUseCase>(Lifetime.Scoped);
            builder.Register<AllyPlacedMonsterUseCase>(Lifetime.Scoped);
        }

        private void BuildStage(IContainerBuilder builder)
        {
            builder.Register<GetStageInfoUseCase>(Lifetime.Scoped);
            builder.Register<GetStageCostUseCase>(Lifetime.Scoped);

            builder.Register<JudgeMonsterPlaceableUseCase>(Lifetime.Scoped)
                .AsImplementedInterfaces()
                .AsSelf();

            builder.Register<StageSituation>(Lifetime.Scoped)
                .WithParameter(new StageId(_startStageId));
            builder.Register<NextStageUseCase>(Lifetime.Scoped);

            builder.Register<StageSceneRepository>(Lifetime.Scoped);
            builder.RegisterInstance<StageSceneDataStoreSource>(_stageSceneDataStoreSource);
            builder.RegisterEntryPoint<StageSceneService>(Lifetime.Scoped).AsSelf();
            builder.Register<LoadStageSceneUseCase>(Lifetime.Scoped);
        }

        private void BuildBattlePerform(IContainerBuilder builder)
        {
            builder.Register<EnemyGeneratePresenter>(Lifetime.Scoped);
            builder.Register<PlayerGeneratePresenter>(Lifetime.Scoped)
                .AsImplementedInterfaces()
                .AsSelf();

            builder.Register<BattleStartPerformPresenter>(Lifetime.Scoped);
            builder.RegisterComponentInNewPrefab<BattleStartPerformView>(_battleStartPerformViewPrefab, Lifetime.Scoped)
                .UnderTransform(_battlePerformRoot);

            builder.RegisterComponentInNewPrefab<BattleResultFailedPerformView>(_battleResultFailedPerformViewPrefab,
                    Lifetime.Scoped)
                .UnderTransform(_battlePerformRoot);
            builder.RegisterComponentInNewPrefab<BattleResultVictoryPerformView>(_battleResultVictoryPerformViewPrefab,
                    Lifetime.Scoped)
                .UnderTransform(_battlePerformRoot);

            builder.RegisterComponentInHierarchy<InBattleUIView>();

            builder.RegisterEntryPoint<BattlePerformPresenter>(Lifetime.Scoped).AsSelf();
            builder.Register<BattleTerminationCalculator>(Lifetime.Scoped);
            builder.Register<BattleResultCalculator>(Lifetime.Scoped);
            builder.Register<BattleResultPresenter>(Lifetime.Scoped);

            builder.RegisterComponentInHierarchy<ResultVirtualCamera>();
            builder.RegisterComponentInHierarchy<CameraController>();
        }

        private void BuildMonsterSelect(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<MonsterSelectView>();
            builder.Register<MonsterSelectPresenter>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<CreateMonsterSelectViewModelUseCase>(Lifetime.Scoped);

            // PlacePoint
            builder.RegisterComponentInNewPrefab<PlacePointView>(_placePointViewPrefab, Lifetime.Scoped);
            builder.Register<PlacePointPresenter>(Lifetime.Scoped);

            // MonsterDetail
            builder.RegisterEntryPoint<MonsterDetailPresenter>().AsSelf();
            builder.RegisterComponentInNewPrefab<MonsterDetailView>(_monsterDetailViewPrefab, Lifetime.Scoped)
                .UnderTransform(_monsterDetailRoot);
            builder.Register<CreateMonsterDetailViewModelUseCase>(Lifetime.Scoped);
        }

        private void BuildPlayerStatus(IContainerBuilder builder)
        {
            builder.Register<PlayerStatus>(Lifetime.Scoped)
                .WithParameter(new Cost(_initialPlayerOwnCost));

            // Cost
            builder.RegisterComponentInHierarchy<CurrentCostView>();
            builder.RegisterEntryPoint<CurrentCostPresenter>();
            builder.Register<PlayerConsumeCostUseCase>(Lifetime.Scoped);
        }

        private void BuildSkill(IContainerBuilder builder)
        {
            builder.Register<SkillTargetCalculator>(Lifetime.Scoped);
            builder.Register<KnockbackService>(Lifetime.Scoped);
            builder.Register<AbnormalityEffectCalculator>(Lifetime.Scoped);
        }

        private static void BuildTimeControl(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<InBattleTimeControlView>();
            builder.RegisterEntryPoint<TimeControlPresenter>().AsSelf();
            builder.Register<TimeControlUseCase>(Lifetime.Scoped);
        }

        private static void BuildHistory(IContainerBuilder builder)
        {
            builder.Register<PlaceRecordUseCase>(Lifetime.Scoped);
            builder.Register<PlaceRecordFactory>(Lifetime.Scoped);
            builder.Register<BattleMonsterPlaceHistory>(Lifetime.Scoped);
        }

        private void BuildCapture(IContainerBuilder builder)
        {
            builder.RegisterInstance(_captureCamera);
            builder.Register<CaptureUseCase>(Lifetime.Scoped)
                .WithParameter(_captureCamera)
                .WithParameter(_captureRenderTexture);
        }

        private void BuildDebug(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<BattleDebug>(Lifetime.Scoped);
        }

        private void BuildHelp(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab<HelpView>(_helpViewPrefab, Lifetime.Scoped)
                .UnderTransform(_helpRoot);
            builder.RegisterEntryPoint<HelpPresenter>(Lifetime.Scoped).AsSelf();
        }
    }
}
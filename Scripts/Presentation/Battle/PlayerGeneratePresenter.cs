using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Unity1week202403.Domain;
using Unity1week202403.Extensions;
using Unity1week202403.Structure;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer.Unity;

namespace Unity1week202403.Presentation
{
    public class PlayerGeneratePresenter : Presenter, IInitializable, ITickable
    {
        private readonly JudgeMonsterPlaceableUseCase _judgeMonsterPlaceableUseCase;
        private readonly FindPlacedMonsterUseCase _findPlacedMonsterUseCase;
        private readonly BattleMonsterPresenterFactory _battleMonsterPresenterFactory;
        private readonly MonsterMasterDataService _monsterMasterDataService;
        private readonly BattleMonsterPresenterContainer _battleMonsterPresenterContainer;
        private readonly CreateAllyBattleMonsterUseCase _createAllyBattleMonsterUseCase;
        private readonly BattleMonsterContainer _battleMonsterContainer;
        private readonly MonsterSelectPresenter _monsterSelectPresenter;
        private readonly PlayerConsumeCostUseCase _playerConsumeCostUseCase;
        private readonly AllyPlacedMonsterUseCase _allyPlacedMonsterUseCase;
        private readonly PlacePointPresenter _placePointPresenter;
        private readonly PlaceRecordUseCase _placeRecordUseCase;
        private readonly CameraController _cameraController;
        private readonly HelpAutoDisplayUseCase _helpAutoDisplayUseCase;

        private readonly Plane _plane = new(Vector3.up, Vector3.zero);

        private readonly Subject<Unit> _onSubmit = new();
        private bool _isRunning;
        private MonsterId? _selectedMonsterId;

        public PlayerGeneratePresenter(
            JudgeMonsterPlaceableUseCase judgeMonsterPlaceableUseCase,
            BattleMonsterPresenterFactory battleMonsterPresenterFactory,
            MonsterMasterDataService monsterMasterDataService,
            BattleMonsterPresenterContainer battleMonsterPresenterContainer,
            CreateAllyBattleMonsterUseCase createAllyBattleMonsterUseCase,
            BattleMonsterContainer battleMonsterContainer,
            MonsterSelectPresenter monsterSelectPresenter,
            PlayerConsumeCostUseCase playerConsumeCostUseCase,
            AllyPlacedMonsterUseCase allyPlacedMonsterUseCase,
            PlacePointPresenter placePointPresenter,
            PlaceRecordUseCase placeRecordUseCase,
            CameraController cameraController,
            HelpAutoDisplayUseCase helpAutoDisplayUseCase)
        {
            _judgeMonsterPlaceableUseCase = judgeMonsterPlaceableUseCase;
            _battleMonsterPresenterFactory = battleMonsterPresenterFactory;
            _monsterMasterDataService = monsterMasterDataService;
            _battleMonsterPresenterContainer = battleMonsterPresenterContainer;
            _createAllyBattleMonsterUseCase = createAllyBattleMonsterUseCase;
            _battleMonsterContainer = battleMonsterContainer;
            _monsterSelectPresenter = monsterSelectPresenter;
            _playerConsumeCostUseCase = playerConsumeCostUseCase;
            _allyPlacedMonsterUseCase = allyPlacedMonsterUseCase;
            _placePointPresenter = placePointPresenter;
            _findPlacedMonsterUseCase = new FindPlacedMonsterUseCase(battleMonsterContainer);
            _placeRecordUseCase = placeRecordUseCase;
            _cameraController = cameraController;
            _helpAutoDisplayUseCase = helpAutoDisplayUseCase;
        }

        public void Initialize()
        {
            _monsterSelectPresenter.OnSubmitAsObservable()
                .Subscribe(_onSubmit.OnNext)
                .AddTo(this);

            _monsterSelectPresenter.OnResetAsObservable()
                .Subscribe(_ => _allyPlacedMonsterUseCase.ResetAll())
                .AddTo(this);

            _monsterSelectPresenter.OnSelectAsObservable()
                .Subscribe(monsterId =>
                {
                    _selectedMonsterId = monsterId;
                    _monsterSelectPresenter.Select(monsterId);
                })
                .AddTo(this);

            _monsterSelectPresenter.OnBigSwitchAsObservable()
                .Subscribe(_ =>
                {
                    _monsterSelectPresenter.Deselect();
                    _monsterSelectPresenter.SwitchBig();
                })
                .AddTo(this);
        }

        public async UniTask GenerateAsync(StageId stageId, CancellationToken cancellationToken)
        {
            // 選択をリセットしておく
            _selectedMonsterId = null;
            _monsterSelectPresenter.Deselect();

            // 強制表示
            await _helpAutoDisplayUseCase.TryShowHelpAsync();

            _cameraController.SetPlayable(true);

            if (_placeRecordUseCase.HasRecord())
            {
                Debug.Log("前回の配置がある履歴がある");
                _placeRecordUseCase.Restore();
            }

            // すぐ配置に移りたいため、Forget
            _monsterSelectPresenter.ShowAsync(stageId, cancellationToken).Forget();

            _isRunning = true;

            await _onSubmit.FirstAsync(cancellationToken);

            _isRunning = false;

            _monsterSelectPresenter.Deselect();

            // 配置を記録
            _placeRecordUseCase.Record();

            await _monsterSelectPresenter.HideAsync(cancellationToken);
        }

        public void Tick()
        {
            if (!_isRunning)
            {
                _placePointPresenter.Hide();
                return;
            }

            // UIと被っていたら無効にする
            if (IsPointOverUI())
            {
                _placePointPresenter.Hide();
                return;
            }

            var isFocus = TryGetFocusPoint(out var focusWorldPoint);
            if (!isFocus)
            {
                _placePointPresenter.Hide();
                return;
            }

            var isPlaced = _findPlacedMonsterUseCase.TryAllyFind(focusWorldPoint, out var placedBattleMonster);
            // 右クリックで削除
            if (isPlaced && Input.GetMouseButton(1))
            {
                _allyPlacedMonsterUseCase.Remove(placedBattleMonster.BattleMonsterId);
                return;
            }

            if (_selectedMonsterId is null)
            {
                _placePointPresenter.Hide();
                return;
            }

            if (!_judgeMonsterPlaceableUseCase.Judge(_selectedMonsterId.Value, focusWorldPoint, true))
            {
                _placePointPresenter.Show(focusWorldPoint,
                    _monsterMasterDataService.GetColliderRadius(_selectedMonsterId.Value), false);
                return;
            }

            _placePointPresenter.Show(focusWorldPoint,
                _monsterMasterDataService.GetColliderRadius(_selectedMonsterId.Value), true);

            if (Input.GetMouseButton(0))
            {
                if (_playerConsumeCostUseCase.CanConsume(_selectedMonsterId.Value))
                {
                    Create(_selectedMonsterId.Value, focusWorldPoint);
                }
                else
                {
                    Debug.Log("コストが足りません");
                }
            }
        }

        private void Create(MonsterId monsterId, Vector3 worldPosition)
        {
            var monster = _createAllyBattleMonsterUseCase.Create(monsterId);
            _battleMonsterContainer.Add(monster);
            var monsterPresenter = _battleMonsterPresenterFactory.Create(monster, worldPosition);
            _battleMonsterPresenterContainer.Add(monsterPresenter);

            // 手持ちコストの消費
            _playerConsumeCostUseCase.Consume(monsterId);
        }

        private bool TryGetFocusPoint(out Vector3 focusWorldPoint)
        {
            focusWorldPoint = default;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (_plane.Raycast(ray, out var distance))
            {
                focusWorldPoint = ray.GetPoint(distance);
                return true;
            }

            return false;
        }

        private bool IsPointOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}
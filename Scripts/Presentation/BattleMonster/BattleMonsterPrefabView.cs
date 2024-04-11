using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Unity1week202403.Presentation
{
    public class BattleMonsterPrefabView : MonoBehaviour
    {
        [SerializeField] private SpriteLibrary _spriteLibrary;
        [SerializeField] private SpriteLibraryAsset _allySpriteLibraryAsset;
        [SerializeField] private SpriteLibraryAsset _enemySpriteLibraryAsset;
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationClip _moveClip;
        [SerializeField] private AnimationClip _attackClip;
        [SerializeField] private AnimationClip _deathClip;

        [SerializeField] private SpriteResolver _faceResolver;
        
        private float? _autoToMoveTime;
        private AnimationClip _currentClip;

        private const string CategoryName = "Face";
        private const string FaceNormalName = "Normal";
        private const string FaceMetojiName = "Metoji";
        private const string FaceYarareName = "Yarare";
        
        private static readonly Vector2 _faceMetojiTimeRange = new(0.2f, 0.5f);

        private float _faceCloseLeftTime = 0f;
        
        public void SetUp(bool isEnemy)
        {
            transform.localPosition = new Vector3(0,0,Random.Range(-0.01f, 0.01f));
            
            if(_spriteLibrary != null)
            {
                _spriteLibrary.spriteLibraryAsset = isEnemy ? _enemySpriteLibraryAsset : _allySpriteLibraryAsset;
            }
        }

        public void PlayMove()
        {
            if (_currentClip == _moveClip)
            {
                return;
            }
            
            PlayClip(_moveClip, false, false);
        }
        
        public void PlayAttack()
        {
            PlayClip(_attackClip, true, true);
        }
        
        public void PlayDeath()
        {
            PlayClip(_deathClip, true, true);
            _faceResolver?.SetCategoryAndLabel(CategoryName, FaceYarareName);
            _faceCloseLeftTime = 0f;
        }

        public void PlayDamaged()
        {
            _faceResolver?.SetCategoryAndLabel(CategoryName, FaceMetojiName);
            _faceCloseLeftTime = Random.Range(_faceMetojiTimeRange.x, _faceMetojiTimeRange.y);
        }
        
        private void PlayClip(AnimationClip animationClip, bool isImmediate, bool isAutoToMove)
        {
            if(_animator == null)
            {
                return;
            }
            
            _currentClip = animationClip;
            _animator.CrossFadeInFixedTime(_currentClip.name, isImmediate ? 0 :0.1f);
            _autoToMoveTime = isAutoToMove ? Time.time + _currentClip.length : null;
        }

        private void Update()
        {
            transform.rotation = Camera.main.transform.rotation;
            
            if (_autoToMoveTime.HasValue)
            {
                if (Time.time > _autoToMoveTime.Value)
                {
                    _autoToMoveTime = null;
                    PlayMove();
                }
            }
            
            if(_faceCloseLeftTime > 0)
            {
                _faceCloseLeftTime -= Time.deltaTime;
                if(_faceCloseLeftTime <= 0)
                {
                    _faceResolver?.SetCategoryAndLabel(CategoryName, FaceNormalName);
                }
            }
        }
    }
}
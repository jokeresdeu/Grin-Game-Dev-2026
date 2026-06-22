using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// Handles slot selection and the build/upgrade bar. Slot picking is physics-free:
    /// on a primary click not over UI, the nearest active slot within its pick radius is
    /// selected. The bottom bar shows the 3 race towers (empty slot) or upgrade/sell (built).
    /// </summary>
    public class BuildManager : MonoBehaviour
    {
        public static BuildManager Instance { get; private set; }

        [Header("World")]
        [SerializeField] private Transform _towerContainer;
        [SerializeField] private Tower _towerPrefab;
        [SerializeField] private SpriteRenderer _rangeRing;
        [SerializeField] private SpriteRenderer _slotHighlight;

        [Header("Build bar")]
        [SerializeField] private GameObject _buildBar;
        [SerializeField] private GameObject _buildGroup;
        [SerializeField] private GameObject _upgradeGroup;
        [SerializeField] private Button[] _buildButtons;
        [SerializeField] private TMP_Text[] _buildLabels;
        [SerializeField] private TMP_Text _towerInfo;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private TMP_Text _upgradeLabel;
        [SerializeField] private Button _sellButton;
        [SerializeField] private TMP_Text _sellLabel;

        private TowerSlot _selected;
        private TowerConfig[] _roleConfigs;
        private float _ringNative = 1f;
        private float _highlightNative = 1f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            RaceId race = RaceSelection.Selected;
            _roleConfigs = new TowerConfig[TowerConfig.Roles.Length];
            for (int i = 0; i < _roleConfigs.Length; i++)
                _roleConfigs[i] = TowerConfig.Resolve(race, TowerConfig.Roles[i]);

            if (_buildButtons != null)
            {
                for (int i = 0; i < _buildButtons.Length; i++)
                {
                    int index = i;
                    if (_buildButtons[i] != null)
                        _buildButtons[i].onClick.AddListener(() => Build(index));
                }
            }
            if (_upgradeButton != null) _upgradeButton.onClick.AddListener(UpgradeSelected);
            if (_sellButton != null) _sellButton.onClick.AddListener(SellSelected);

            if (_rangeRing != null && _rangeRing.sprite != null) _ringNative = _rangeRing.sprite.bounds.size.x;
            if (_slotHighlight != null && _slotHighlight.sprite != null) _highlightNative = _slotHighlight.sprite.bounds.size.x;

            Deselect();
        }

        private void Update()
        {
            if (GameManager.Instance == null)
                return;

            GameState state = GameManager.Instance.State;
            if (state == GameState.Won || state == GameState.Lost)
            {
                if (_selected != null)
                    Deselect();
                return;
            }
            if (state != GameState.Playing)
                return;

            HandleClick();
            RefreshAffordability();
            UpdateIndicators();
        }

        private void HandleClick()
        {
            if (!InputReader.PrimaryDown || InputReader.IsPointerOverUI())
                return;

            TowerSlot slot = PickSlot();
            if (slot != null)
                Select(slot);
            else
                Deselect();
        }

        private TowerSlot PickSlot()
        {
            Camera cam = Camera.main;
            if (cam == null || LevelManager.Instance == null)
                return null;

            Vector3 world = cam.ScreenToWorldPoint(Input.mousePosition);
            world.z = 0f;

            TowerSlot best = null;
            float bestDist = float.MaxValue;
            var slots = LevelManager.Instance.ActiveSlots;
            for (int i = 0; i < slots.Count; i++)
            {
                TowerSlot slot = slots[i];
                if (slot == null)
                    continue;
                float d = (slot.Position - world).magnitude;
                if (d <= slot.PickRadius && d < bestDist)
                {
                    best = slot;
                    bestDist = d;
                }
            }
            return best;
        }

        private void Select(TowerSlot slot)
        {
            _selected = slot;
            RefreshBar();
        }

        public void Deselect()
        {
            _selected = null;
            if (_buildBar != null) _buildBar.SetActive(false);
            if (_rangeRing != null) _rangeRing.enabled = false;
            if (_slotHighlight != null) _slotHighlight.enabled = false;
        }

        private void RefreshBar()
        {
            if (_selected == null)
            {
                if (_buildBar != null) _buildBar.SetActive(false);
                return;
            }

            if (_buildBar != null) _buildBar.SetActive(true);

            bool occupied = _selected.Occupied;
            if (_buildGroup != null) _buildGroup.SetActive(!occupied);
            if (_upgradeGroup != null) _upgradeGroup.SetActive(occupied);

            if (!occupied)
            {
                if (_buildLabels != null && _roleConfigs != null)
                {
                    for (int i = 0; i < _buildLabels.Length && i < _roleConfigs.Length; i++)
                    {
                        if (_buildLabels[i] != null)
                            _buildLabels[i].text = $"{_roleConfigs[i].DisplayName}\n{_roleConfigs[i].BuildCost} зол.";
                    }
                }
            }
            else
            {
                Tower tower = _selected.Tower;
                if (_towerInfo != null)
                    _towerInfo.text = $"{tower.Config.DisplayName} · рівень {tower.Level}";
                if (_upgradeLabel != null)
                    _upgradeLabel.text = tower.CanUpgrade ? $"Покращити\n{tower.UpgradeCost} зол." : "Макс.\nрівень";
                if (_sellLabel != null)
                    _sellLabel.text = $"Продати\n+{tower.SellValue} зол.";
            }
        }

        private void RefreshAffordability()
        {
            if (_selected == null || ResourceManager.Instance == null)
                return;

            if (!_selected.Occupied)
            {
                if (_buildButtons != null && _roleConfigs != null)
                {
                    for (int i = 0; i < _buildButtons.Length && i < _roleConfigs.Length; i++)
                    {
                        if (_buildButtons[i] != null)
                            _buildButtons[i].interactable = ResourceManager.Instance.CanAfford(_roleConfigs[i].BuildCost);
                    }
                }
            }
            else
            {
                Tower tower = _selected.Tower;
                if (_upgradeButton != null)
                    _upgradeButton.interactable = tower.CanUpgrade && ResourceManager.Instance.CanAfford(tower.UpgradeCost);
                if (_sellButton != null)
                    _sellButton.interactable = true;
            }
        }

        private void UpdateIndicators()
        {
            if (_selected == null)
                return;

            if (_slotHighlight != null)
            {
                Vector3 p = _selected.Position;
                p.z = 0.1f;
                _slotHighlight.transform.position = p;
                _slotHighlight.enabled = true;
                SizeTo(_slotHighlight.transform, _highlightNative, _selected.PickRadius * 2f);
            }

            if (_rangeRing != null)
            {
                if (_selected.Occupied)
                {
                    Tower tower = _selected.Tower;
                    float range = tower.Config.RangeAt(tower.Level);
                    Vector3 p = _selected.Position;
                    p.z = 0.2f;
                    _rangeRing.transform.position = p;
                    SizeTo(_rangeRing.transform, _ringNative, range * 2f);
                    _rangeRing.enabled = true;
                }
                else
                {
                    _rangeRing.enabled = false;
                }
            }
        }

        private static void SizeTo(Transform t, float native, float diameter)
        {
            float s = native > 0.0001f ? diameter / native : diameter;
            t.localScale = new Vector3(s, s, 1f);
        }

        private void Build(int roleIndex)
        {
            if (_selected == null || _selected.Occupied || _roleConfigs == null)
                return;
            if (roleIndex < 0 || roleIndex >= _roleConfigs.Length)
                return;

            TowerConfig config = _roleConfigs[roleIndex];
            if (ResourceManager.Instance == null || !ResourceManager.Instance.TrySpend(config.BuildCost))
                return;

            Tower tower = Instantiate(_towerPrefab, _selected.Position, Quaternion.identity, _towerContainer);
            tower.Setup(config, config.BuildCost);
            _selected.SetTower(tower);
            RefreshBar();
        }

        private void UpgradeSelected()
        {
            if (_selected == null || !_selected.Occupied)
                return;

            Tower tower = _selected.Tower;
            if (!tower.CanUpgrade)
                return;

            int cost = tower.UpgradeCost;
            if (ResourceManager.Instance == null || !ResourceManager.Instance.TrySpend(cost))
                return;

            tower.Upgrade(cost);
            RefreshBar();
        }

        private void SellSelected()
        {
            if (_selected == null || !_selected.Occupied)
                return;

            Tower tower = _selected.Tower;
            if (ResourceManager.Instance != null)
                ResourceManager.Instance.AddGold(tower.SellValue);

            Destroy(tower.gameObject);
            _selected.ClearTower();
            RefreshBar();
        }

        public void ClearAllTowers()
        {
            Deselect();
            if (_towerContainer == null)
                return;

            for (int i = _towerContainer.childCount - 1; i >= 0; i--)
                Destroy(_towerContainer.GetChild(i).gameObject);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}

using ItemSystem;
using ResourceSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingManage
{
    public class ConveyorBelt : Building, IResourceInput, IResourceOutput
    {
        [SerializeField] private ConveyorBeltResource _beltResource;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private float _speed = 2f;
        [SerializeField] private Resource _container;
        [SerializeField] private List<CustomRuleTile> _rules;
        public LinkedDirection LinkedDirection;

        private float _process = 0;

        protected override void Awake()
        {
            base.Awake();

            MapManager.Instance.BuildController.OnBuildingChange += RotationUpdate;
        }

        private void OnDisable()
        {
            if(!MapManager.IsDestroyed)
                MapManager.Instance.BuildController.OnBuildingChange -= RotationUpdate;
        }

        private void Update()
        {
            if (_container.type == ResourceType.None)
            {
                _process = 0;
                return;
            }

            if (_process >= 1)
            {
                _process = 1;
                TransferResource();

                if (_container.type == ResourceType.None)
                    _beltResource.DisableResource();
            }
            else
            {
                _process += Time.deltaTime * _speed;
                _beltResource?.Move(_process);
            }
        }

        public void TransferResource()
        {
            //다음 위치를 가져오는 부분 1x1 사이즈 일때만 유효한 부분임
            Vector2Int nextPosition = Position.min + Direction.GetTileDirection(direction);

            //그 부분에 건물이 있고 그 건물이 IResourceInput을 가지고 있다면
            bool buildingExsist =
                MapManager.Instance.TryGetBuilding(nextPosition, out Building connectedBuilding);

            if (!buildingExsist)
            {
                if (_container.type != ResourceType.None)
                {
                    Vector2 offset = Vector2.up * 0.5f;
                    Vector2 position =
                        Position.center
                        + (Vector2)Direction.GetTileDirection(direction) / 2f
                        + offset;

                    ItemDropManager.Instance.GenerateDropItem(
                        (int)_container.type, _container.amount, position);
                    _container = new Resource(ResourceType.None, 0);

                    _beltResource.DisableResource();
                }

                return;
            }

            if (!connectedBuilding.TryGetComponent(out IResourceInput input)) return;

            //방향을 반대로 돌려서 input에 TryInsertResource를 호출해줘
            DirectionEnum opposite = Direction.GetOpposite(direction);

            input.TryInsertResource(_container, opposite, out _container);
            if (_container.type == ResourceType.None) _process = 0;
        }

        public bool TryInsertResource(Resource resource, DirectionEnum direction, out Resource remain)
        {
            //리소스가 이미 존재할 때, 반대 방향에서 올 때
            if (direction == base.direction || _container.type != ResourceType.None)
            {
                remain = resource;
                return false;
            }

            _container = resource;
            _beltResource.gameObject.SetActive(true);

            //시작 부분, 끝 부분
            Vector2 offset = Vector2.up * 0.5f;
            Vector2 from = Position.center + (Vector2)Direction.GetTileDirection(direction) / 2f + offset;
            Vector2 to = Position.center + (Vector2)Direction.GetTileDirection(base.direction) / 2f + offset;
            Vector2 center = Position.center + offset;

            _beltResource.Init(center, from, to, resource);
            remain = new Resource(ResourceType.None, 0);

            return true;
        }


        public override void Build(Vector2Int position, DirectionEnum direction, bool save = false)
        {
            base.Build(position, direction, save);

            Building building;
            Vector2Int connected = position + Direction.GetTileDirection(direction);
            
            bool buildingExsist =
                MapManager.Instance.TryGetBuilding(connected, out building);

            if (buildingExsist && building is ConveyorBelt belt)
            {
                belt.LinkedDirection |=
                    (LinkedDirection)(1 << (int)Direction.GetOpposite(direction));
            }

            for (int i = 0; i < 4; i++)
            {
                connected = position + Direction.directionsInt[i];
                MapManager.Instance.TryGetBuilding(position, out building);
                ConveyorBelt beltInstance = building as ConveyorBelt;

                buildingExsist =
                    MapManager.Instance.TryGetBuilding(connected, out building);

                if (buildingExsist && building is ConveyorBelt lbelt)
                {
                    if (lbelt.direction == Direction.GetOpposite((DirectionEnum)i))
                    {
                        beltInstance.LinkedDirection |= (LinkedDirection)(1 << i);
                    }
                }
            }

        }

        public override void Destroy()
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2Int connected = Position.min + Direction.directionsInt[i];
                MapManager.Instance.TryGetBuilding(connected, out Building building);

                if (building != null && building is ConveyorBelt belt)
                {
                    belt.LinkedDirection &=
                        ~(LinkedDirection)(1 << (i + 2) % 4);
                }
            }

            base.Destroy();
        }

        public override void SetRotation(DirectionEnum direction)
        {
            base.direction = direction;
            CustomRuleTile selectedRule = null;

            foreach (var r in _rules)
            {
                if (r.direction == direction &&
                    (r.linkedDirection & LinkedDirection) == r.linkedDirection)
                {
                    selectedRule = r;
                    break;
                }
            }


            if ((int)direction % 2 == 0) _spriteRenderer.material.SetInt("_IsVertical", 1);
            else _spriteRenderer.material.SetInt("_IsVertical", 0);

            if ((int)direction < 2) _spriteRenderer.material.SetInt("_IsReverse", 0);
            else _spriteRenderer.material.SetInt("_IsReverse", 1);


            if (selectedRule != null)
            {
                _spriteRenderer.sprite = selectedRule.sprite;
            }
            else
            {
                if (_rules.Count > 0)
                    _spriteRenderer.sprite = _rules[0].sprite;
            }
        }

        private void RotationUpdate()
        {
            CustomRuleTile selectedRule = null;

            foreach (var r in _rules)
            {
                if (r.direction == direction &&
                    (r.linkedDirection & LinkedDirection) == r.linkedDirection)
                {
                    selectedRule = r;
                    break;
                }
            }

            if (selectedRule != null)
            {
                _spriteRenderer.sprite = selectedRule.sprite;
            }
            else
            {
                if (_rules.Count > 0)
                    _spriteRenderer.sprite = _rules[0].sprite;
            }
        }
    }

    [Serializable]
    public class CustomRuleTile
    {
        public DirectionEnum direction;
        public LinkedDirection linkedDirection;
        public Sprite sprite;
    }

}

[Flags]
public enum LinkedDirection
{
    Down = 1,
    Right = 2,
    Up = 4,
    Left = 8
}
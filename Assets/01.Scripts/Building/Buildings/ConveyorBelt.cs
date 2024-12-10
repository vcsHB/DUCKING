using ItemSystem;
using ResourceSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingManage
{
    public class ConveyorBelt : Transfortation
    {
        [SerializeField] private ConveyorBeltResource _beltResource;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Resource _container = new Resource(ResourceType.None, 0);
        private float _process = 0;

        [SerializeField] private float _speed = 2f;
        [SerializeField] private List<CustomRuleTile> _rules;

        private void Update()
        {
            if (_container.type == ResourceType.None || _container.amount <= 0) return;

            if (_process >= 1)
            {
                _process = 1;
                TransferResource();
            }
            else
            {
                _process += Time.deltaTime * _speed;
                _beltResource.Move(_process);
            }
        }

        public override void TransferResource()
        {
            _outputDirection.ForEach(dir =>
            {
                //다음 위치를 가져오는 부분 1x1 사이즈 일때만 유효한 부분임
                Vector2Int nextPosition = Position.min + Direction.GetTileDirection(dir);

                //그 부분에 건물이 있고 그 건물이 IResourceInput을 가지고 있다면
                bool buildingExsist =
                    MapManager.Instance.TryGetBuilding(nextPosition, out Building connectedBuilding);

                if (!buildingExsist)
                {
                    _process = 0;
                    _beltResource.DisableResource();
                    Vector2 dropItemPosition = (Vector2)_beltResource.transform.position;
                    dropItemPosition += (Vector2)Direction.GetTileDirection(dir) * 0.1f;
                    if (dir != DirectionEnum.Up) dropItemPosition.y -= 0.1f;

                    ItemDropManager.Instance.GenerateDropItem((int)_container.type, _container.amount, dropItemPosition);
                    _container = new Resource(ResourceType.None, 0);
                    return;
                }

                if (!connectedBuilding.TryGetComponent(out IResourceInput input)) return;

                //방향을 반대로 돌려서 input에 TryInsertResource를 호출해줘
                DirectionEnum opposite = Direction.GetOpposite(dir);

                input.TryInsertResource(_container, opposite, out Resource remain);
                if (remain.type == ResourceType.None)
                {
                    _process = 0;
                    _container = new Resource(ResourceType.None, 0);
                    _beltResource.DisableResource();
                }
            });
        }

        public override bool TryInsertResource(Resource resource, DirectionEnum direction, out Resource remain)
        {
            if (!_inputDirection.Contains(direction)
                || _container.type != ResourceType.None || _container.amount > 0)
            {
                remain = resource;
                return false;
            }

            _process = 0;
            _container = new Resource(resource.type, 1);

            if (resource.amount > 1)
            {
                remain = new Resource(resource.type, resource.amount - 1);
                CreateConveyorResource(resource, direction);
                return false;
            }

            remain = new Resource(ResourceType.None, 0);
            CreateConveyorResource(resource, direction);
            return true;
        }

        public override void SetRotation(DirectionEnum direction)
        {
            base._outputDirection.Clear();
            base._outputDirection.Add(direction);

            base._direction = direction;
            CustomRuleTile selectedRule = null;

            foreach (var r in _rules)
            {
                if (r.outputDirection == direction)
                {
                    bool isSelected = true;

                    //컨베이어 벨트는 들어오는 곳이 하나라서...
                    //들어오는 곳이 없는 경우
                    if ((_inputDirection.Count <= 0 && r.inputDirection.Count <= 0))
                    {
                        selectedRule = r;
                        break;
                    }

                    for (int i = 0; i < _inputDirection.Count; i++)
                    {
                        if (!r.inputDirection.Contains(_inputDirection[i])) isSelected = false;
                    }

                    if (isSelected)
                    {
                        selectedRule = r;
                        break;
                    }
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

            MapManager.Instance.RotateBuilding(Position.min, direction);
        }

        protected override void UpdateInputOutput()
        {
            base.UpdateInputOutput();
            CustomRuleTile selectedRule = null;

            foreach (var r in _rules)
            {
                if (r.outputDirection == _direction)
                {
                    bool isSelected = true;

                    //컨베이어 벨트는 들어오는 곳이 하나라서...
                    //들어오는 곳이 없는 경우
                    if ((_inputDirection.Count <= 0 && r.inputDirection.Count <= 0))
                    {
                        selectedRule = r;
                        break;
                    }

                    for (int i = 0; i < _inputDirection.Count; i++)
                    {
                        if (!r.inputDirection.Contains(_inputDirection[i])) isSelected = false;
                    }

                    if (isSelected)
                    {
                        selectedRule = r;
                        break;
                    }
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

        protected override void CheckNeighbor(Vector2Int position)
        {
            _inputDirection.Clear();

            for (int i = 0; i < 4; i++)
            {
                Vector2Int connected = position + Direction.directionsInt[i];

                bool buildingExsist =
                    MapManager.Instance.TryGetBuilding(connected, out Building building);

                if (!buildingExsist) continue;

                if (building is Transfortation transfortation)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        DirectionEnum direction = (DirectionEnum)j;
                        Debug.Log(direction.ToString() + " " + transfortation.ContainOutput(direction));
                    }


                    //연결된놈, 현재 컨베이어 벨트의 방향확인
                    if (!transfortation.ContainOutput(Direction.GetOpposite((DirectionEnum)i))
                        || ContainOutput((DirectionEnum)i)) continue;

                    SetInputDirection((DirectionEnum)i);
                }
                else if (building.TryGetComponent(out IResourceOutput output))
                {
                    if (ContainOutput((DirectionEnum)i)) continue;
                    SetInputDirection((DirectionEnum)i);
                }
            }
        }

        private void CreateConveyorResource(Resource resource, DirectionEnum inputDir)
        {
            //시작 부분, 끝 부분
            Vector2 offset = Vector2.up * 0.5f;
            Vector2 from = Position.center + (Vector2)Direction.GetTileDirection(inputDir) / 2f + offset;
            Vector2 to = Position.center + (Vector2)Direction.GetTileDirection(_direction) / 2f + offset;
            Vector2 center = Position.center + offset;

            _beltResource.gameObject.SetActive(true);
            _beltResource.Init(center, from, to, resource, _speed);
        }
    }

    [Serializable]
    public class CustomRuleTile
    {
        public DirectionEnum outputDirection;
        public List<DirectionEnum> inputDirection;
        public Sprite sprite;
    }

}
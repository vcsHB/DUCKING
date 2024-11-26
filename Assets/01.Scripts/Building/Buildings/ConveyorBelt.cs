using ItemSystem;
using ResourceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

namespace BuildingManage
{
    public class ConveyorBelt : Transfortation
    {
        [SerializeField] private ConveyorBeltResource _beltResourcePf;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private float _speed = 2f;
        [SerializeField] private List<CustomRuleTile> _rules;

        private void Update()
        {
            for (int i = 0; i < _processes.Count; i++)
            {
                if (_processes[i] >= 1)
                {
                    _processes[i] = 1;
                    TransferResource();
                }
                else
                {
                    _processes[i] += Time.deltaTime * _speed;
                }
            }
        }

        public override void TransferResource()
        {
            base.TransferResource();
        }

        public override bool TryInsertResource(Resource resource, DirectionEnum direction, out Resource remain)
        {
            bool result = base.TryInsertResource(resource, direction, out remain);

            if (result)
            {
                _container[^1] = new Resource(resource.type, 1);
                if (resource.amount > 1)
                    remain = new Resource(resource.type, resource.amount - 1);

                ConveyorBeltResource beltResource = Instantiate(_beltResourcePf);

                //시작 부분, 끝 부분
                Vector2 offset = Vector2.up * 0.5f;
                Vector2 from = Position.center + (Vector2)Direction.GetTileDirection(direction) / 2f + offset;
                Vector2 to = Position.center + (Vector2)Direction.GetTileDirection(base.direction) / 2f + offset;
                Vector2 center = Position.center + offset;
                bool isLastBelt = true;

                _outputDirection.ForEach(dir =>
                {
                    Vector2Int nextPosition = Position.min + Direction.GetTileDirection(dir);

                    if (MapManager.Instance.TryGetBuilding(nextPosition, out Building building)
                        && building.TryGetComponent(out IResourceInput input)) isLastBelt = false;
                });

                beltResource.Init(center, from, to, resource, _speed, isLastBelt);
            }
            return result;
        }

        public override void SetRotation(DirectionEnum direction)
        {
            base._outputDirection.Clear();
            base._outputDirection.Add(direction);

            base.direction = direction;
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

        protected override void OnGenerateDropItem()
        {
            //Vector2 offset = Vector2.up * 0.5f;
            //Vector2 position =
            //    Position.center
            //    + (Vector2)Direction.GetTileDirection(direction) / 2f
            //    + offset;

            //for (int i = 0; i < _processes.Count; i++)
            //{
            //    if (_processes[i] < 1) continue;

            //    ItemDropManager.Instance.GenerateDropItem(
            //       (int)_container[i].type, _container[i].amount, position);

            //    _processes.RemoveAt(i);
            //    _container.RemoveAt(i);
            //    i--;
            //}
        }

        protected override void UpdateInputs()
        {
            base.UpdateInputs();
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
            for (int i = 0; i < 4; i++)
            {
                Vector2Int connected = position + Direction.directionsInt[i];

                bool buildingExsist =
                    MapManager.Instance.TryGetBuilding(connected, out Building building);

                if (!buildingExsist) continue;

                if (building is Transfortation transfortation)
                {
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
    }

    [Serializable]
    public class CustomRuleTile
    {
        public DirectionEnum outputDirection;
        public List<DirectionEnum> inputDirection;
        public Sprite sprite;
    }

}
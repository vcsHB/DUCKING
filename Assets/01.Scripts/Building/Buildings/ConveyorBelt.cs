using ItemSystem;
using ResourceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace BuildingManage
{
    public class ConveyorBelt : Transfortation
    {
        [SerializeField] private ConveyorBeltResource _beltResource;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private float _speed = 2f;
        [SerializeField] private List<CustomRuleTile> _rules;

        private float _process = 0;

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

        public override void TransferResource()
        {
            base.TransferResource();
            if (_container.type == ResourceType.None) _process = 0;
        }

        public override bool TryInsertResource(Resource resource, DirectionEnum direction, out Resource remain)
        {
            bool result = base.TryInsertResource(resource, direction, out remain);

            if (result)
            {
                _container = new Resource(resource.type, 1);
                if (resource.amount > 1) 
                    remain = new Resource(resource.type, resource.amount - 1);

                _beltResource.gameObject.SetActive(true);

                //시작 부분, 끝 부분
                Vector2 offset = Vector2.up * 0.5f;
                Vector2 from = Position.center + (Vector2)Direction.GetTileDirection(direction) / 2f + offset;
                Vector2 to = Position.center + (Vector2)Direction.GetTileDirection(base.direction) / 2f + offset;
                Vector2 center = Position.center + offset;

                _beltResource.Init(center, from, to, resource);
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
        }

        protected override void OnGenerateDropItem()
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

                    for(int i = 0; i < _inputDirection.Count; i++)
                    {
                        if (!r.inputDirection.Contains(_inputDirection[i])) isSelected = false;
                    }

                    if(isSelected)
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
                MapManager.Instance.TryGetBuilding(position, out Building belt);
                ConveyorBelt beltInstance = belt as ConveyorBelt;

                bool buildingExsist =
                    MapManager.Instance.TryGetBuilding(connected, out Building building);

                if (buildingExsist)
                {
                    if (building is Transfortation transfortation)
                    {
                        if (!transfortation.ContainOutput(Direction.GetOpposite((DirectionEnum)i))
                            || ContainOutput((DirectionEnum)i)) continue;

                        SetInputDirection((DirectionEnum)i);
                        break;
                    }
                    else if (building.TryGetComponent(out IResourceOutput output))
                    {
                        beltInstance.SetInputDirection((DirectionEnum)i);
                        break;
                    }
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
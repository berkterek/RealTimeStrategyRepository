using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using RealTimeStrategy.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RealTimeStrategy.Commands
{
    public class UnitSelectionHandler : MonoBehaviour
    {
        [SerializeField] RectTransform _unitSelectionArea;
        [SerializeField] LayerMask _layerMask;

        Camera _camera;
        PlayerController _playerController;
        Vector2 _startPosition;

        public List<UnitController> SelectionUnits { get; } = new List<UnitController>();

        private void Awake()
        {
            _camera = Camera.main;
        }

        private IEnumerator Start()
        {
            //temp solution
            while (_playerController == null)
            {
                yield return new WaitForSeconds(1f);
                _playerController = NetworkClient.connection.identity.GetComponent<PlayerController>();
                Debug.Log(_playerController == null);
            }
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                StartSelectionArea();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                ClearSelectionArea();
            }
            else if (Mouse.current.leftButton.IsPressed())
            {
                UpdateSelectionArea();
            }
        }

        private void UpdateSelectionArea()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            float areaWidth = mousePosition.x - _startPosition.x;
            float areaHeight = mousePosition.y - _startPosition.y;

            _unitSelectionArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
            _unitSelectionArea.anchoredPosition = _startPosition + new Vector2(areaWidth / 2f, areaHeight / 2f);
        }

        private void StartSelectionArea()
        {
            if (!Keyboard.current.leftShiftKey.IsPressed())
            {
                foreach (UnitController unit in SelectionUnits)
                {
                    unit.Selection(false);
                }
                
                SelectionUnits.Clear();
            }

            _unitSelectionArea.gameObject.SetActive(true);
            _startPosition = Mouse.current.position.ReadValue();
        }

        private void ClearSelectionArea()
        {
            _unitSelectionArea.gameObject.SetActive(false);

            if (_unitSelectionArea.sizeDelta.magnitude == 0f)
            {
                Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask)) return;

                if (!hit.collider.TryGetComponent<UnitController>(out UnitController unitController)) return;

                if (!unitController.hasAuthority) return;

                SelectionUnits.Add(unitController);

                foreach (UnitController unit in SelectionUnits)
                {
                    unit.Selection(true);
                }
            }
            else
            {
                Vector2 min = _unitSelectionArea.anchoredPosition - (_unitSelectionArea.sizeDelta / 2);
                Vector2 max = _unitSelectionArea.anchoredPosition + (_unitSelectionArea.sizeDelta / 2);

                foreach (UnitController unitController in _playerController.Units)
                {
                    if(SelectionUnits.Contains(unitController)) continue;
                    
                    Vector3 screenPosition = _camera.WorldToScreenPoint(unitController.transform.position);

                    if (screenPosition.x > min.x && screenPosition.x < max.x && screenPosition.y > min.y && screenPosition.y < max.y)
                    {
                        SelectionUnits.Add(unitController);
                        unitController.Selection(true);
                    }
                }
            }
        }
    }
}
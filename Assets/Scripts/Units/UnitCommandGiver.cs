using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField] private UnitSelectionHandler _unitSelectionHandler;
    [SerializeField] private LayerMask _layerMask;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if(!Mouse.current.rightButton.wasPressedThisFrame) return;

        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if(!Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, _layerMask)) return;
        
        TryMove(raycastHit.point);
    }

    private void TryMove(Vector3 destinationPoint)
    {
        foreach (Unit selectedUnit in _unitSelectionHandler.SelectedUnits)
        {
            selectedUnit.GetUnitMovement().CmdMove(destinationPoint);
        }
    }
}
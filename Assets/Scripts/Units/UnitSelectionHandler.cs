using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    
    private Camera _mainCamera;

    private List<Unit> _selectedUnits = new List<Unit>();

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            AreaDeselection();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            AreaSelection();
        }
    }

    private void AreaDeselection()
    {
        foreach (Unit selectedUnit in _selectedUnits)
            selectedUnit.Deselect();
        
        _selectedUnits.Clear();
    }

    private void AreaSelection()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity,_layerMask)) return;
        
        if(!raycastHit.collider.TryGetComponent(out Unit unit)) return;
        
        if(!unit.hasAuthority) return;

        _selectedUnits.Add(unit);

        foreach (Unit selectedUnit in _selectedUnits)
            selectedUnit.Select();

    }
}

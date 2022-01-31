using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;


public class UnitSelectionHandler : MonoBehaviour
{
    public List<Unit> SelectedUnits { get; } = new List<Unit>();
    
    [SerializeField] private RectTransform _unitSelectionArea;
    [SerializeField] private LayerMask _layerMask;
    
    private Vector2 _unitSelectionAreaStartPosition;
    private Camera _mainCamera;
    private RTSPlayer _rtsPlayer;

    private void Awake()
    {
        _mainCamera = Camera.main;

        Unit.OnAuthorityUnitDespawned += AuthorityHandleUnitDespawned;
    }


    private void OnDestroy()
    {
        Unit.OnAuthorityUnitDespawned -= AuthorityHandleUnitDespawned;
    }

    private void Update()
    {
        if (_rtsPlayer == null)
            if (NetworkClient.connection.identity.TryGetComponent(out RTSPlayer rtsPlayer))
                _rtsPlayer = rtsPlayer;
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartSelectionArea();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
        else if (Mouse.current.leftButton.isPressed)
        {
            UpdateSelectionArea();
        }
    }
    
    private void StartSelectionArea()
    {
        if (!Keyboard.current.leftShiftKey.isPressed)
        {
            foreach (Unit selectedUnit in SelectedUnits)
                selectedUnit.Deselect();
        
            SelectedUnits.Clear();
        }
        
        _unitSelectionArea.gameObject.SetActive(true);
        _unitSelectionAreaStartPosition = Mouse.current.position.ReadValue();
        UpdateSelectionArea();
    }
    
    private void UpdateSelectionArea()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float areaWidth = mousePosition.x - _unitSelectionAreaStartPosition.x;
        float areaHeight = mousePosition.y - _unitSelectionAreaStartPosition.y;

        _unitSelectionArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
        _unitSelectionArea.anchoredPosition =
            _unitSelectionAreaStartPosition + new Vector2(areaWidth / 2f, areaHeight / 2f);
    }
    
    private void ClearSelectionArea()
    {
        _unitSelectionArea.gameObject.SetActive(false);

        if (_unitSelectionArea.sizeDelta.sqrMagnitude == 0)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity,_layerMask)) return;
        
            if(!raycastHit.collider.TryGetComponent(out Unit unit)) return;
        
            if(!unit.hasAuthority) return;

            SelectedUnits.Add(unit);

            foreach (Unit selectedUnit in SelectedUnits)
                selectedUnit.Select();
            
            return;
        }

        Vector2 min = _unitSelectionArea.anchoredPosition - _unitSelectionArea.sizeDelta / 2;
        Vector2 max = _unitSelectionArea.anchoredPosition + _unitSelectionArea.sizeDelta / 2;

        foreach (Unit myUnit in _rtsPlayer.GetMyUnits())
        {
            if(SelectedUnits.Contains(myUnit)) continue;
            
            Vector2 screenPosition = _mainCamera.WorldToScreenPoint(myUnit.transform.position);

            if (screenPosition.x > min.x && screenPosition.x < max.x && screenPosition.y > min.y &&
                screenPosition.y < max.y)
            {
                SelectedUnits.Add(myUnit);
                
                myUnit.Select();
            }
        }

    }
    
    private void AuthorityHandleUnitDespawned(Unit unit)
    {
        SelectedUnits.Remove(unit);
    }
}

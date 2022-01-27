using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.Mathematics;
using UnityEngine;

public class UnitFiring : NetworkBehaviour
{
    [SerializeField] private Targeter _targeter;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private float _fireRange = 5f;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private float _rotationSpeed = 20f;

    private float _lastFireTime;

    [ServerCallback]
    private void Update()
    {
        Targetable target = _targeter.GetTarget();
        
        if(target == null) return;
        
        if(!CanFireAtTarget(target)) return;

        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        if (Time.time > (1f / _fireRate) + _lastFireTime)
        {
            _lastFireTime = Time.time;

            Quaternion projectileRotation = 
                Quaternion.LookRotation(target.GetAimAtPoint().position - _projectileSpawnPoint.position);

            GameObject projectileInstance =
                Instantiate(_projectilePrefab, _projectileSpawnPoint.position, projectileRotation);
            
            NetworkServer.Spawn(projectileInstance, connectionToClient);


        }
    }

    [Server]
    private bool CanFireAtTarget(Targetable target)
    {
        float distance = (target.transform.position - transform.position).sqrMagnitude;
        
        return distance <= _fireRange * _fireRange;
    }
}

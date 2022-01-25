using Mirror;
using UnityEngine;


public class Targetable : NetworkBehaviour
{
    [SerializeField] private Transform _aimAtPoint;

    public Transform GetAimAtPoint()
    {
        return _aimAtPoint;
    }
}

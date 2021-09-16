using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private float _arenaMaxX;
    [SerializeField] private float _arenaMinX;
    [SerializeField] private float _arenaMaxZ;
    [SerializeField] private float _arenaMinZ;

    [SerializeField] private GameObject _player;
    private int _rotationAngle = 80;

    [SerializeField] private Health _bossHealth;

    private enum movementStates { ROTATION, TO_PLAYER, PILLAR, WAIT};
    [SerializeField] private movementStates _movementState = movementStates.ROTATION;
    [SerializeField] private Vector3 _pillarSpot;
    [SerializeField] private GameObject _pillar; // this is sloppy, but I'm running out of time and just want it to work
    private float _tolerance; // the distance moved in one fixed update
    private void Start()
    {
        _tolerance = 3;
    }

    protected override void Move()
    {
        if (_movementState != movementStates.WAIT && _bossHealth.CurrentHealth < _bossHealth.MaxHealth / 2)
        {
            _movementState = movementStates.PILLAR;
        }

        switch (_movementState)
        {
            case movementStates.ROTATION:
                RotationMovement();
                break;
            case movementStates.TO_PLAYER:
                MoveTowardsPlayer();
                break;
            case movementStates.PILLAR:
                PillarTime();
                break;
            case movementStates.WAIT:
                break;
        }   
    }

    private void RotationMovement()
    {
        // get heading
        Vector3 heading = _player.transform.position - transform.position;
        heading = heading / heading.magnitude;

        //rotate
        _rb.MoveRotation(Quaternion.Lerp(_rb.rotation, Quaternion.LookRotation(heading) * Quaternion.Euler(0, _rotationAngle, 0), 0.5f));

        //don't run into wall
        Vector3 move = _rb.position + (transform.forward * MoveSpeed * 5);
        if (move.x > _arenaMaxX || move.x < _arenaMinX || move.z > _arenaMaxZ || move.z < _arenaMinZ)
        {
            _movementState = movementStates.TO_PLAYER;
            StartCoroutine(TurnAwayFromWall());
        }
        else
        // move
        {
            _rb.MovePosition(_rb.position + (transform.forward * MoveSpeed));
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 heading = _player.transform.position - transform.position;
        heading = heading / heading.magnitude;
        _rb.MoveRotation(Quaternion.Lerp(_rb.rotation, Quaternion.LookRotation(heading), 0.1f));
        _rb.MovePosition(_rb.position + (transform.forward * MoveSpeed));
    }

    private void PillarTime()
    {
        Vector3 heading = _pillarSpot - transform.position;
        Debug.Log(heading.magnitude);
        if (heading.magnitude > _tolerance)
        {
            heading = heading / heading.magnitude;
            _rb.MoveRotation(Quaternion.Lerp(_rb.rotation, Quaternion.LookRotation(heading), 0.1f));
            _rb.MovePosition(_rb.position + (transform.forward * MoveSpeed));
        }
        else
        {
            _rb.MovePosition(_pillarSpot);
            _pillar.GetComponent<BossPillar>().ActivatePillar();
            _movementState = movementStates.WAIT;
        }
    }

    IEnumerator TurnAwayFromWall()
    {
        yield return new WaitForSeconds(0.5f);
        _movementState = movementStates.ROTATION;
    }
}

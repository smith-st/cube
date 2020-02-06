using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class PlayerController : ControllerWithListener<IPlayerListener>
{
    private const float Size = 1f;
    public const float HalfSize = Size / 2f;
    private const int RotateIteration = 20;
    private const float RotationStep = 90f / RotateIteration;


    [SerializeField]
    private GameObject parts;
    private Vector3 _pivot;
    private bool _rotatingNow;
    private bool _reachedFinish;
    private bool _canMove = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            _reachedFinish = true;
        }else if (other.CompareTag("Bonus"))
        {
            _listener?.PlayerCollectBonus(other.gameObject.GetComponent<BonusController>());
        }
    }

    public void FinishLevel()
    {
        _canMove = false;
    }
    public void MoveToDirection(Direction direction)
    {
        if (!_canMove || _rotatingNow || !CanRotateOnDirection(direction))
        {
            return;
        }

        _rotatingNow = true;
        var (axis, pivot) = PointsDependenceDirection(direction);
        StartCoroutine(RotateObject(pivot, axis, FinishRotation));
    }

    public void MoveOnPath(List<Vector3> path)
    {
        _canMove = false;
        StartCoroutine(StartMoveOnPath(path));
    }

    private IEnumerator StartMoveOnPath(List<Vector3> path)
    {
        yield return new WaitUntil(()=> !_rotatingNow);
        var sequence = DOTween.Sequence();
        const float speed = 0.3f;
        foreach (var position in path)
        {
            var to = new Vector3(position.x, HalfSize, position.z);
            sequence.Append(transform.DOMove(to, speed).SetEase(Ease.Linear));
        }

        sequence.AppendCallback(() =>
        {
            _canMove = true;
            if (_reachedFinish)
            {
                _listener?.PlayerReachedFinish();
                FinishLevel();
            }
        });
        sequence.Play();
    }

    public void DestroyPlayer()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        parts.SetActive(true);
    }
    private void FinishRotation()
    {
        _rotatingNow = false;
        if (_reachedFinish)
        {
            _listener?.PlayerReachedFinish();
            FinishLevel();
        }
        else
        {
            _listener?.PlayerMoved();
        }
    }

    private (Vector3 axis, Vector3 pivot) PointsDependenceDirection(Direction direction)
    {
        var position = transform.position;
        switch (direction)
        {
            case Direction.Top:
                return (
                    axis:Vector3.right,
                    pivot:new Vector3(position.x, position.y - HalfSize, position.z + HalfSize));
            case Direction.Bottom:
                return (
                    axis:Vector3.left,
                    pivot:new Vector3(position.x, position.y - HalfSize, position.z - HalfSize));
            case Direction.Right:
                return (
                    axis:Vector3.back,
                    pivot:new Vector3(position.x + HalfSize,position.y - HalfSize, position.z ));
            case Direction.Left:
                return (
                    axis:Vector3.forward,
                    pivot:new Vector3(position.x - HalfSize,position.y - HalfSize, position.z ));
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    private IEnumerator RotateObject(Vector3 pivotPoint, Vector3 rotationAxis, Action callback)
    {
        for (var i = 0; i < RotateIteration; i++)
        {
            transform.RotateAround(pivotPoint, rotationAxis, RotationStep);
            yield return new WaitForFixedUpdate();
        }
        callback?.Invoke();
    }

    private bool CanRotateOnDirection(Direction direction)
    {
        Vector3 rayDirection;
        switch (direction)
        {
            case Direction.Top:
                rayDirection = Vector3.forward;
                break;
            case Direction.Bottom:
                rayDirection = Vector3.back;
                break;
            case Direction.Right:
                rayDirection = Vector3.right;
                break;
            case Direction.Left:
                rayDirection = Vector3.left;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        var position = transform.position;
        var from = new Vector3(
            position.x,
            position.y-HalfSize,
            position.z
        );
        return !Physics.Raycast(from, rayDirection,1f,LayerMask.GetMask("Wall"));
    }
}

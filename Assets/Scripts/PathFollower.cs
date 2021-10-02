using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathFollower : MonoBehaviour
{
    public PathController pathController;
    public float moveSpeed = 3;
    public float rotationSpeed = 180;
    public Transform[] nodes;
    public int currentTargetId = -1;
    public bool completed = false;
    public UnityEvent onCompletedPath;

    public void SetPath(PathController newController)
    {
        pathController = newController;
        // find all nodes in the path
        nodes = pathController.GetNodes();

        // find the closest node to use as the first node
        currentTargetId = GetClosestPosition(nodes, transform.position);
    }

    private void Start()
    {
        if (pathController != null)
            SetPath(pathController);
    }

    private int GetClosestPosition(Transform[] transforms, Vector3 from)
    {
        int closest = -1;
        for (int i = 0; i < transforms.Length; ++i)
        {
            if (closest == -1)
            {
                closest = i;
            }
            else if (Vector3.Distance(transforms[closest].position, from) > Vector3.Distance(transforms[i].position, from))
            {
                closest = i;
            }
        }
        return closest;
    }

    private void Update()
    {
        if (completed || pathController == null) return;

        Vector3 target = nodes[currentTargetId].position;

        // move towards current target node
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        // rotate towards target node
        var direction = target - transform.position;
        if (direction != Vector3.zero)
        {
            var lookat = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookat, rotationSpeed * Time.deltaTime);
        }

        UpdateTarget(target);
    }

    private void UpdateTarget(Vector3 currentTarget)
    {
        // if other is a path node then get the next node in path and set it as current target
        if (Vector3.Distance(transform.position, currentTarget) < 0.1f)
        {
            currentTargetId += 1;
            if (currentTargetId >= nodes.Length)
            {
                if (pathController.isLooping)
                    currentTargetId = 0;
                else
                {
                    currentTargetId = -1;
                    completed = true;
                    onCompletedPath.Invoke();
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public bool isLooping = true;

    public Transform[] GetNodes()
    {
        var nodesAndParent = new List<Transform>(GetComponentsInChildren<Transform>());
        nodesAndParent.RemoveAt(0);

        Transform[] nodes;
        nodes = nodesAndParent.ToArray();

        if (nodes == null || nodes.Length == 0)
        {
            Debug.LogError("We got no path nodes, yo.", this);
        }
        return nodes;
    }

    private void OnDrawGizmosSelected()
    {
        Transform[] nodes = GetNodes();
        if (nodes.Length == 1)
        {
            return;
        }

        for (int i = 0; i < nodes.Length; ++i)
        {
            if (i == nodes.Length -1)
            {
                if (isLooping)
                    Gizmos.DrawLine(nodes[i].position, nodes[0].position);
            }
            else
            {
                Gizmos.DrawLine(nodes[i].position, nodes[i+1].position);

            }

        }
    }

}

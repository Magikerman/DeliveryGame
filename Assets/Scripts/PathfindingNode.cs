using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode : MonoBehaviour
{
    public List<PathfindingNode> neightbourds;


    //Profe, puse esto para llamarlo desde el editor y no tener que poner de uno a uno. Lo borraria pero lo dejo para que no creas que estoy loco y fui de uno en uno
    [SerializeField] private LayerMask layer;
    [ContextMenu("Set Nodes")]
    public void SetNeighbours()
    {
        neightbourds = new List<PathfindingNode>();

        CastRay(transform.forward);
        CastRay(transform.right);
        CastRay(-transform.right);
        CastRay(-transform.forward);
    }

    private void CastRay(Vector3 dir)
    {
        Debug.DrawRay(transform.position, dir * 40f, Color.red, 3f);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 40f, layer))
        {
            var node = hit.collider.GetComponent<PathfindingNode>();
            if (node != null)
                neightbourds.Add(node);
        }
    }
}

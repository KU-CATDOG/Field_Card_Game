using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEntity : MonoBehaviour
{
    [SerializeField]
    private float generateProbability;
    public float GenerateProbability => generateProbability;
    private MeshRenderer meshRenderer;
    private Coordinate pos;
    public Coordinate position
    {
        get
        {
            if (pos == null)
                position = new Coordinate((int)transform.position.x, (int)transform.position.z);
            return pos;
        }
        set
        {
            if (!meshRenderer)
                meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.enabled = false;
            if (!GameManager.Instance.Map[value.X, value.Y]) return;
            else if (GameManager.Instance.Map[value.X, value.Y].Discovered) OnSightRoutine();
            pos = value;
            transform.position = new Vector3(pos.X, transform.position.y, pos.Y);
        }
    }
    private void Awake()
    {
    }
    private void OnSightRoutine()
    {
        meshRenderer.enabled = true;
    }
    private void Update()
    {
        if (position == null) return;
        
    }
}

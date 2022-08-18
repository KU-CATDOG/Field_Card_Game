using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour, IInteractable
{
    private Coordinate pos;
    public Coordinate position
    {
        get
        {
            return pos;
        }
        set
        {
            transform.position = new Vector3(value.X, transform.position.y, value.Y);
            GameManager.Instance.Map[value.X, value.Y].EntityOnTile.Add(this);
            pos = value;
        }
    }
    private void Awake()
    {
        GameManager.Instance.NeutralList.Add(gameObject);
    }
    public Coordinate GetPosition()
    {
        return position;
    }
    public void Interaction()
    {
        Debug.Log("INTERACT");
    }

}

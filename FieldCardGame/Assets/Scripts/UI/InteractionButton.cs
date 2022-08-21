using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class InteractionButton : PlayerButton, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        if(data.button == 0)
        {
            Player player = GameManager.Instance.CurPlayer as Player;
            if(player.Interactables.Count > 1)
            {
                StartCoroutine(SelectInteraction());
            }
            else
            {
                player.Interactables[0].Interaction();
            }
        }
    }

    public IEnumerator SelectInteraction()
    {
        Player player = GameManager.Instance.CurPlayer as Player;
        List<Coordinate> availables = new();
        foreach (var i in player.Interactables)
        {
            availables.Add(i.GetPosition());
        }
        yield return StartCoroutine(PlayerUIManager.Instance.TileSelect(1, availables));
        if(PlayerUIManager.Instance.SelectedTile != null)
        {
            foreach(var i in player.Interactables)
            {
                if(i.GetPosition().X == PlayerUIManager.Instance.SelectedTile.X && i.GetPosition().Y == PlayerUIManager.Instance.SelectedTile.Y)
                {
                    i.Interaction();
                    yield break;
                }
            }
        }
    }
}

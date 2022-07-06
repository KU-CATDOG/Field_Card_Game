using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool Discovered { get; set; }
    public bool Onsight { get; set; }
    public Character CharacterOnTile { get; set; }
    public List<object> EntityOnTile { get; set; } = new List<object>();
    public Color TileColor { get; set; }
    public List<IEnumerator> OnCharacterEnterRoutine { get; private set; }
    public List<IEnumerator> OnCharacterStayRoutine { get; private set; }
    public List<IEnumerator> OnCharacterExitRoutine { get; private set; }

    public List<int> RoutineID { get; private set; }
    public List<IEnumerator> TileRoutine { get; private set; }
}

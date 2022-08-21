using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCardObject : MonoBehaviour
{
    public static bool OnReward { get; set; }
    private Coordinate pos;
    public Coordinate position
    {
        get
        {
            return pos;
        }
        set
        {
            pos = value;
            transform.position = new Vector3(pos.X, 1.5f, pos.Y);
        }
    }
    public bool DestroyOffer { get; set; }
    float period = 2f;
    float curTime = 0f;
    float amplitude = 0.3f;
    float angleSpeed = Mathf.PI / 3;
    void Update()
    {
        if (DestroyOffer)
        {
            Destroy(gameObject);
            return;
        }
        transform.Rotate(Vector3.up, 60f * Time.deltaTime, Space.World);
        if(curTime < period / 2)
        {
            curTime += Time.deltaTime;
            transform.position += Vector3.down * amplitude * Time.deltaTime / period * 2;
        }
        else if(curTime < period)
        {
            curTime += Time.deltaTime;
            transform.position += Vector3.up * amplitude * Time.deltaTime / period * 2;
        }
        else
        {
            curTime = 0f;
            Vector3 tmp = transform.position;
            tmp.y = 1.5f;
            transform.position = tmp;
        }
    }
    public IEnumerator GiveReward()
    {
        while(GameManager.Instance.Map[position.X, position.Y].CharacterOnTile is not Player)
        {
            yield return null;
        }
        OnReward = true;
        GameManager.Instance.GetCardReward(3);
        DestroyOffer = true;
        yield return GameManager.Instance.StartCoroutine(wait());
    }
    private IEnumerator wait()
    {
        yield return new WaitUntil(() => {  return !OnReward; });
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private static int _deckSize = 12;
    [SerializeField] private int _handSize = 5;
    [SerializeField] private GameObject[] _cards;
    [SerializeField] private GameObject[] _cardPrefabs;
    private Vector3 startPosition;

    public void CardSetup(Engine engine)
    {
        //generate deck of random cards
        for (int i = 0; i < _deckSize; i++)
        {
            GameObject card = Instantiate(_cardPrefabs[Random.Range(0, _cardPrefabs.Length)], transform);
            card.transform.gameObject.SetActive(false);
            card.GetComponent<DraggableImage>().GameEngine = engine;
            _cards[i] = card;
        }
        DeckUpdate();
    }

    public void DeckUpdate()
    {
        List<GameObject> temp = new List<GameObject>(_cards);

        Vector3 startPosition = GetStartPosition(temp, transform.position);

        for (int i = 0; i < temp.Count; i++)
        {
            GameObject card = temp[i];
            if (card.GetComponent<DraggableImage>().IsPlayed)
            {
                temp.RemoveAt(i);
                card.SetActive(false);
            }
        }

        int handSize;
        if (temp.Count >= 5)
            handSize = _handSize;
        else
            handSize = temp.Count;

        for (int i = 0; i < handSize; i++)
        {
            GameObject card = temp[i];
            card.SetActive(true);
            card.transform.position = startPosition;

            startPosition += new Vector3(120, 0);
        }

        _cards = temp.ToArray();
    }

    //card placement
    private Vector3 GetStartPosition(List<GameObject> temp, Vector3 position)
    {
        if (temp.Count >= 5)
            startPosition = transform.position + new Vector3(-240, 0, 0);
        else if (temp.Count == 4)
            startPosition = transform.position + new Vector3(-180, 0, 0);
        else if (temp.Count == 3)
            startPosition = transform.position + new Vector3(-120, 0, 0);
        else if (temp.Count == 2)
            startPosition = transform.position + new Vector3(-60, 0, 0);
        else if (temp.Count == 1)
            startPosition = transform.position;

        return startPosition;
    }
}

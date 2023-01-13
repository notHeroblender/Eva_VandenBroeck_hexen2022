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

    private void DeckUpdate()
    {
        List<GameObject> temp = new List<GameObject>(_cards);


    }
}

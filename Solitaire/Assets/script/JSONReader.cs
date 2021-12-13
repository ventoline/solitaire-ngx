using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{

    public TextAsset cardJSON;

    [System.Serializable]
    public class Card {
        public string suit;
        public string value;
        public int strength;
        public string faceImg;
    }


    [System.Serializable]

    public class CardDeck {
        public Card[] cards;
     
    
    }

    public CardDeck myDeck = new CardDeck();

    void Awake()
    {
        myDeck = JsonUtility.FromJson<CardDeck>(cardJSON.text);

        //Debug.Log(myDeck.cards.Length);
        foreach (Card i in myDeck.cards) {
            string s;
            int val;
            switch (i.value)
            {

                case "A":
                    s = "ace";
                    val = 14;
                    break;

                case "K":
                    s = "king"; 
                    val = 13;
                    break;

                case "Q":
                    s = "queen";
                    val = 12;
                    break;

                case "J":
                    s = "jack";
                    val = 11;
                    break;

                default:  
                    s = i.value;
                    val = int.Parse(i.value);
                    break; 

            }

            string img = "/cardsFace/" + s + "_of_" + i.suit.ToString() + ".png";
            i.faceImg = img;
            i.strength = val; 
            
        }


    }

   
}

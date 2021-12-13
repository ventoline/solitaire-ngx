using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createCards : MonoBehaviour
{
    public GameObject cardPrefab;
    public Material faceMat;
    public Material backMat;
    private JSONReader comp;
    private List<Texture> cardFaceTex;
    private List<GameObject> cardDeck;
    public Vector3[] placeHoldersPositions;
    public Vector3[] columnsPositions;
    public GameObject deckPos;
    private GameObject goCol; 
    GameObject goTop;



    void shuffleDeck<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }

    }

    void dealFirstHand() {

        Debug.Log("DEAL");
  
        for (int i = 0; i < 7; i++)
        {
            

            for (int j = i; j < 7; j++)
            {
                //Debug.Log(columnsPositions[j]);
              Vector3 deltPos = new Vector3(
                                                          columnsPositions[j].x,
                                                          columnsPositions[j].y - i*.35f,
                                                          i * -0.025f + columnsPositions[j].z -.1f
                );
                    
               // Debug.Log(deltPos + " "+ j +"x" +i + "pos be: "+ deltPos);
                cardDeck[(i * 7) + j].transform.position = deltPos;
                cardDeck[(i * 7) + j].transform.eulerAngles = new Vector3(0,180,0);
                cardDeck[(i * 7) + j].transform.parent = goCol.transform.GetChild(j).transform;
                //flip last
                if ( j == i) { cardDeck[(i * 7) + j].transform.eulerAngles = new Vector3(0, 0, 0); 
                   // Debug.Log("flip last");
                }


            }  
          

        }

    }

    void generateCards() {

        
        for (int i = 0; i < comp.myDeck.cards.Length; i++)
        {


            //material
            Material cardFaceMat = new Material(faceMat);// new Material(Shader.Find("Universal Render Pipeline/Lit"));
            cardFaceMat.CopyPropertiesFromMaterial(faceMat);
            cardFaceMat.SetTexture("_BaseMap", cardFaceTex[i]);
        
            //card GO
            GameObject goCard = new GameObject();
            goCard.AddComponent<cardFunction>();
            goCard.GetComponent<cardFunction>().suit = comp.myDeck.cards[i].suit;
            goCard.GetComponent<cardFunction>().strength = comp.myDeck.cards[i].strength;
            goCard.GetComponent<cardFunction>().value = comp.myDeck.cards[i].value;
            goCard.name = string.Concat(comp.myDeck.cards[i].suit, comp.myDeck.cards[i].value);
            goCard.AddComponent<BoxCollider>();
            goCard.GetComponent<BoxCollider>().size = new Vector3(1.67f, 2.65f, 0.01f);


            //Card faces
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);
            go.transform.localScale = new Vector3(0.15f, 1, 0.25f);
            go.transform.rotation = Quaternion.Euler(-90, 0, 0);
            go.GetComponent<MeshRenderer>().material = cardFaceMat;
            go.transform.parent = goCard.transform;
            go.GetComponent<MeshCollider>().enabled = false;
            GameObject back = GameObject.CreatePrimitive(PrimitiveType.Plane);
            back.transform.localScale = new Vector3(0.15f, 1, 0.25f);
            back.transform.rotation = Quaternion.Euler(90, 0, 0);
            back.GetComponent<MeshRenderer>().material = backMat;
            back.transform.parent = goCard.transform;
            back.GetComponent<MeshCollider>().enabled = false;
            cardDeck.Add(goCard);
            
            
            //display in deck 
            Vector3 goCardPos = new Vector3(deckPos.transform.position.x + Random.Range(.2f, -.2f),
                                            deckPos.transform.position.y + Random.Range(.3f, -.3f),
                                            deckPos.transform.position.z /*- i * 0.0001f*/);
            goCard.transform.position = goCardPos;
            goCard.transform.eulerAngles = new Vector3(0, 180, Random.Range(-50.0f, 50.0f) / 10);
            
           
        }

        // shuffle cards
        shuffleDeck(cardDeck);



        //shuffle physical deck
         for ( int i = 0; i < cardDeck.Count; i++) {
           
             Vector3 goCardPos = new Vector3(cardDeck[i].transform.position.x ,
                                               cardDeck[i].transform.position.y ,
                                                deckPos.transform.position.z -i * 0.001f);

            cardDeck[i].transform.position = goCardPos;
            //nest into Deck
            cardDeck[i].transform.parent = deckPos.transform;


         }  


        dealFirstHand();

    }


    void Awake()
    {
        comp = gameObject.GetComponent(typeof(JSONReader)) as JSONReader;
        Debug.Log(comp.myDeck.cards.Length);
        placeHoldersPositions = new Vector3[4];
        columnsPositions = new Vector3[7];
        cardFaceTex = new List<Texture>();
        cardDeck = new List<GameObject>();

         goCol = GameObject.Find("columns");
         goTop = GameObject.Find("topPos");
         deckPos = GameObject.Find("posDeck");


        for (int i = 0; i < comp.myDeck.cards.Length; i++)
        {

             string path = Application.dataPath + "/Resources" + comp.myDeck.cards[i].faceImg;
             byte[] byteArray = System.IO.File.ReadAllBytes(path);  
             Texture2D tex = new Texture2D(512,512);
             tex.LoadImage(byteArray); 
            // Texture2D tex = Resources.Load<Texture2D>("/cardsFace/2_of_hearts", typeof(Texture2D)); //Resources.Load<Texture2D>(comp.myDeck.cards[i].faceImg);
             cardFaceTex.Add(tex);
        
        }

        //Debug.Log(cardFaceTex[0].name);


        foreach (Transform child in goCol.transform) { columnsPositions[child.GetSiblingIndex()] = child.transform.position;// Debug.Log("looping through columns go " + child.GetSiblingIndex() + columnsPositions[child.GetSiblingIndex()]);
        }
        foreach (Transform child in goTop.transform) { placeHoldersPositions[child.GetSiblingIndex()] = child.transform.position; }




    }
    void Start()
    {
        generateCards();
    }




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cardFunction : MonoBehaviour
{
     public int strength;
    public string suit;
    public string value;
    private float pileDepth;
    private bool dragging;
    private Vector3 originalPos;
    private Vector3 offsetDrag;
    private float mouseZ;


    void flip() { }

    void OnMouseDrag()
    {
        dragging = true;
          Vector3 mouse3DPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mouseZ));
        mouse3DPos -= new Vector3(0, 0, .25f);
        if ((transform.root.name == "pioche" || transform.parent.root.name == "topPos" || transform.root.name == "columns") && transform.rotation.y == 0)
        {
            transform.position = mouse3DPos + offsetDrag;

        }
    }
        

void OnEndDrag(GameObject go) {

          Debug.Log("dropped "+ go.name);
        GameObject target;
        dragging = false;

        //check location hover
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 100.0F);

        {
            // Debug.Log("hovering:  " + hits[1].transform.parent.parent.name + " 1 called " + hits[1].transform.name + "  " + hits.Length + " hits");
            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    //columns
                    Debug.Log("parents parents " + hits[i].transform.root);

                    if (hits[i].transform.root.name == "columns")
                    {
                        if (hits[i].transform.rotation.eulerAngles.y == 0 && hits[i].transform != go.transform && hits[i].transform.childCount == 2)
                        {
                            // Debug.Log("hits are: " + i + hits[i].transform.name + "of rot x " + hits[i].transform.rotation.eulerAngles);
                            target = hits[i].transform.gameObject;
                            Debug.Log("target :" + target);
                            Debug.Log("checkElegibility " + checkElegibility(go, target));

                            //  check legal 
                            if (checkElegibility(go, target))
                            {
                                // move to origianl or new position, and nest appropriately
                                //flip cards if needed
                                if (go.transform.parent.childCount > 1) go.transform.parent.GetChild(go.transform.parent.childCount - 2).transform.eulerAngles = new Vector3(0, 0, 0);
                                go.transform.parent = target.transform;
                                go.transform.position = new Vector3(target.transform.position.x, target.transform.position.y - .2f, target.transform.position.z - 0.025f);
                                // position
                            }

                            else { go.transform.position = originalPos; }
                        }


                    }


                    //top cards
                    if (hits[i].transform.root.name == "topPos")
                    {
                        if (go.GetComponent<cardFunction>().strength == 14 && hits[i].transform.childCount == 0)
                        {  // nest in top pos 
                           // Debug.Log(go.transform.parent.GetChild(go.transform.parent.childCount - 2));
                            if (go.transform.parent.childCount > 1) go.transform.parent.GetChild(go.transform.parent.childCount - 2).transform.eulerAngles = new Vector3(0, 0, 0);
                            go.transform.parent = hits[i].transform;
                            go.transform.position = hits[i].transform.position;// new Vector3(target.transform.position.x, target.transform.position.y - .2f, target.transform.position.z - 0.25f);
                            // position
                        }

                        else { go.transform.position = originalPos; }



                        //pioche
                        if (hits[i].transform.root.name == "pioche")
                        {
                            go.transform.position = originalPos;
                        }


                    }
 } }
                    //no hovering
                    else { go.transform.position = originalPos; }

               
           
        }

         

        }



    
    void OnMouseDown()
    { 
      originalPos = transform.position;
        mouseZ = originalPos.z -55;// Camera.main.WorldToScreenPoint(originalPos).z;
        Vector3 mouse3DPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mouseZ));
        offsetDrag = originalPos - mouse3DPos;
            
        Debug.Log("mouse down..");

       

        }


    void OnMouseUp()
    {


        if (dragging) { OnEndDrag(gameObject); 
        Debug.Log("released " + transform.name); }


      
            // picking from the deck
            if (transform.parent.name == "posDeck")
            {
                Debug.Log("flipping from deck top at" + pileDepth);
                transform.parent = GameObject.Find("pioche").transform;
                pileDepth = GameObject.Find("posDeck").transform.position.z - (0.01f * GameObject.Find("pioche").transform.childCount);

                // flip from deck
                transform.position = new Vector3(-3.5f + Random.Range(.2f, -.2f), transform.position.y + Random.Range(.3f, -.3f), pileDepth);
                transform.eulerAngles = new Vector3(0, 0, Random.Range(-20.0f, 20.0f) / 10);
            

        }


        }

    
    bool checkElegibility(GameObject go, GameObject target) {
        if (go.GetComponent<cardFunction>().strength == target.GetComponent<cardFunction>().strength + 1)
        {
            bool ok = false;
            switch (go.GetComponent<cardFunction>().suit)
            {

                case "hearts":
                    if (target.GetComponent<cardFunction>().suit == "clubs" || target.GetComponent<cardFunction>().suit == "spades")
                    { ok = true; }
                    break;

                     case "diamonds":
                    if (target.GetComponent<cardFunction>().suit == "clubs" || target.GetComponent<cardFunction>().suit == "spades")
                    { ok = true; }
                    break;
                     case "clubs":
                    if (target.GetComponent<cardFunction>().suit == "hearts" || target.GetComponent<cardFunction>().suit == "diamonds")
                    { ok = true; }
                    break;
                     case "spades":
                    if (target.GetComponent<cardFunction>().suit == "hearts" || target.GetComponent<cardFunction>().suit == "diamonds")
                    { ok = true; }
                    break;
                default: ok = false;
                    break;

            }
            return ok;
        }

        else { return false; }

    }

    
    /*
    void GetMouseClick() {



        if (Input.GetMouseButton(0))
        {
            // Debug.Log("clickinn..");
            //Vector3 MousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;// = Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("inside " + hit.transform.parent + " clicked on " + hit.transform.name);
                originalPos = hit.transform.position;
                offsetDrag = hit.transform.position - Input.mousePosition;


                if (hit.transform.parent.name == "topPos")
                {
                    Debug.Log("card from the top");
                    hit.transform.position = Input.mousePosition new Vector3(-2.5f, hit.transform.position.y, pileDepth);
                    hit.transform.rotation = Quaternion.Euler(0, 0, 0);

                }


                else if (hit.transform.parent.name == "columns")
                {
                    Debug.Log("card from the game");
                    //check for eligibility 

                    hit.transform.position = new Vector3(-2.5f, hit.transform.position.y, pileDepth);
                    hit.transform.rotation = Quaternion.Euler(0, 0, 0);

                }
            }


            if (Input.GetMouseButtonUp(0))
            {

                Debug.Log("released..");
                //Vector3 MousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("inside " + hit.transform.parent + " clicked on " + hit.transform.name);

                    if (hit.transform.parent.name == "posDeck")
                    {
                        pileDepth -= 0.0001f;
                        Debug.Log("flipping from deck top");
                        // flip from deck
                        hit.transform.position = new Vector3(-2.5f, hit.transform.position.y, pileDepth);
                        hit.transform.rotation = Quaternion.Euler(0, 0, 0);



                    }
                }


            }

       



                }
    }     */
    void Start()
    {

       
    }

    void Awake()
    {
        // Debug.Log("awake");
    }

    // Update is called once per frame
    void Update()
    {
       // GetMouseClick();
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    GameController gameController;
    void Start()
    {
        gameController = this.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetMouseButtonDown(0) )
        {
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;
            
            if( Physics.Raycast( ray, out hit, 100 ) )
            {
                if(hit.transform.gameObject.layer == 6)
                {
                    gameController.Play(hit.transform.GetComponent<Space>().x, hit.transform.GetComponent<Space>().y, hit.transform.position);
                }
            }
        }
    }
}

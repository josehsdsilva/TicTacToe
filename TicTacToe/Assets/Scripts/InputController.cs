using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    int x, y;
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
                    x = hit.transform.GetComponent<Space>().x;
                    y = hit.transform.GetComponent<Space>().y;
                    if(GameController.instance.IsValidMove(x, y))
                    {
                        GameController.instance.Play(x, y);
                        hit.transform.GetComponent<Space>().OnClick(true);
                    }
                    else hit.transform.GetComponent<Space>().OnClick(false);
                }
            }
        }
    }
}

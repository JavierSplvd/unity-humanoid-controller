using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTextDialog : MonoBehaviour
{
    [SerializeField]
    private TextManager textManager;
    [SerializeField]
    private TextManager.Dialog dialog;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag.Equals("Player"))
        {
            textManager.ShowText(dialog);
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if(other.tag.Equals("Player"))
        {
            textManager.HideText();
        }
    }
}

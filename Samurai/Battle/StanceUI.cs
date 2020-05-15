using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Numian
{
    public class StanceUI : MonoBehaviour
    {
        public BattleCharacterController controller;
        private Image air, mountain, sea;
        // Start is called before the first frame update
        void Start()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                if(transform.GetChild(i).name.Equals("Air"))
                    air = transform.GetChild(i).GetComponent<Image>();
                if(transform.GetChild(i).name.Equals("Mountain"))
                    mountain = transform.GetChild(i).GetComponent<Image>();
                if(transform.GetChild(i).name.Equals("Sea"))
                    sea = transform.GetChild(i).GetComponent<Image>();
            }
            
        }

        private Image GetImageForStance()
        {
            switch(controller.GetStance())
            {
                case Stances.HighStance:
                    return air;
                case Stances.MiddleStance:
                    return mountain;
                case Stances.LowStance:
                    return sea;
                default:
                    return null;
            }
        }

        // Update is called once per frame
        void Update()
        {
            air.color = new Color(1, 1, 1, 0.5f);
            mountain.color = new Color(1, 1, 1, 0.5f);
            sea.color = new Color(1, 1, 1, 0.5f);
            GetImageForStance().color = new Color(1, 1, 1, 1);
        }
    }
}
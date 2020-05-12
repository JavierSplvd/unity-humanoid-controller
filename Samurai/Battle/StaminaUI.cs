using UnityEngine;
using UnityEngine.UI;

namespace Numian
{
    public class StaminaUI : MonoBehaviour
    {
        [SerializeField]
        private BattleCharacterController controller;
        [SerializeField]
        private Image[] allStaminaBoxes;
        [SerializeField]
        private Color on, off;
        // Start is called before the first frame update
        void Start()
        {
            allStaminaBoxes = new Image[transform.childCount];
            for(int i = 0; i < transform.childCount; i++)
            {
                allStaminaBoxes[i] = transform.GetChild(i).gameObject.GetComponent<Image>();
            }

        }

        // Update is called once per frame
        void Update()
        {
            for(int i = 0; i < controller.GetData().currentStamina; i++)
            {
                allStaminaBoxes[i].color = on;
            }
            for(int i = controller.GetData().currentStamina; i < controller.GetData().maxStamina; i++)
            {
                allStaminaBoxes[i].color = off;
            }
        }
    }
}

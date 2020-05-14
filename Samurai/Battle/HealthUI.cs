using UnityEngine;
using UnityEngine.UI;

namespace Numian
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField]
        private BattleCharacterController controller;
        [SerializeField]
        private Image current, blood;
        private float bloodSpeed;
        void Start()
        {
            current.fillAmount = 1f;
            blood.fillAmount = current.fillAmount;

        }

        void Update()
        {
            current.fillAmount = GetCurrentPercentageHealth();
            blood.fillAmount = Mathf.Lerp(blood.fillAmount, current.fillAmount, Time.deltaTime * 3f);
        }

        private float GetCurrentPercentageHealth()
        {
            return (float) controller.GetData().currentHealth / controller.GetData().maxHealth;
        }
    }
}

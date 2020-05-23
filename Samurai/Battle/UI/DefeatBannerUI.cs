using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Numian
{
    public class DefeatBannerUI : MonoBehaviour
    {
        private TurnBasedBattleController controller;
        private float minValue = 0f;
        private float maxValue = 1f;
        private bool animate = false;
        private Image banner;
        // Start is called before the first frame update
        void Start()
        {
            banner = GetComponent<Image>();
            controller = GameObject
                .FindGameObjectWithTag(GameObjectTags.BattleController.ToString())
            .GetComponent<TurnBasedBattleController>();
            controller.OnDefeat += Animate;
        }

        // Update is called once per frame
        void Update()
        {
            if (animate)
            {
                banner.fillAmount = Mathf.Lerp(banner.fillAmount, maxValue, 4f * Time.deltaTime);
            }
            else
            {
                banner.fillAmount = Mathf.Lerp(banner.fillAmount, minValue, 4f * Time.deltaTime);
            }
        }

        private void Animate()
        {
            animate = true;
        }
    }
}
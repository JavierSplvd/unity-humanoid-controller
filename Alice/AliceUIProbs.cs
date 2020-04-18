using UnityEngine.UI;
using UnityEngine;

public class AliceUIProbs : MonoBehaviour
{
    public GameObject alice;

    private Image noiseSlider;
    private Image stamina;
    private AliceEmitSound aliceEmitSound;
    private BaseHumanController baseHumanController;
    // Start is called before the first frame update
    void Start()
    {
        GameObject temp;
        for(int i = 0; i<transform.childCount; i++)
        {
            temp = transform.GetChild(i).gameObject;
            switch(temp.name)
            {
                case "Noise":
                    noiseSlider = temp.GetComponent<Image>();
                    break;
                case "Stamina":
                    stamina = temp.GetComponent<Image>();
                    break;
            }
        }

        aliceEmitSound = alice.GetComponent<AliceEmitSound>();
        baseHumanController = alice.GetComponent<BaseHumanController>();
    }

    // Update is called once per frame
    void Update()
    {
        noiseSlider.fillAmount = aliceEmitSound.runningAverage / 0.3f;
        stamina.fillAmount = baseHumanController.GetPercentageCurrentStamina();
    }
}

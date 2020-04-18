using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManagerCooldown : Cooldown
{
    private float currentTime;
    private float maxTime;

    public TextManagerCooldown(float maxTime)
    {
        this.maxTime = maxTime;
        currentTime = 0f;
    }

    public override void Cool()
    {
        currentTime = maxTime;
    }

    public override bool IsAvailable()
    {
        return currentTime == 0f;
    }

    public override void Update()
    {
        currentTime -= Time.deltaTime;
    }
}

[RequireComponent(typeof(Text))]
public class TextManager : MonoBehaviour
{
    private Text text;

    public enum Dialog { Intro, Climbing, Boxes, Objective, Key, Door }
    private float currentAlpha = 0f;
    private float targetAlpha = 1f;
    private Color color;
    private Cooldown cooldown;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        color = text.color;
        cooldown = new TextManagerCooldown(2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (targetAlpha > currentAlpha)
        {
            currentAlpha += Time.deltaTime;
        }
        else
        {
            currentAlpha -= Time.deltaTime;
        }
        color.a = Mathf.Clamp(currentAlpha, 0f, 1f);
        text.color = color;
        cooldown.Update();
    }

    public void ShowText(Dialog d)
    {
        if (cooldown.IsAvailable())
        {
            text.text = GetText(d);
            currentAlpha = 0f;
            targetAlpha = 1f;
        }

    }

    public void HideText()
    {
        if (cooldown.IsAvailable())
        {
            targetAlpha = 0f;
        }
    }

    private string GetText(Dialog d)
    {
        switch (d)
        {
            case Dialog.Intro:
                return "Hello again Alice. I didn't expect you to come back to Wonderland... Especially in these dark times.";
            case Dialog.Boxes:
                return "You can push the boxes pushing them. Try moving them towards the mushrooms.";
            case Dialog.Climbing:
                return "You can climb the wall if you walk towards it.";
            case Dialog.Objective:
                return "You need to get away from here. Get the key and open the door. Take caution with the teapot monster.";
            case Dialog.Door:
                return "This is the door... Where is it supposed to take me?";
            case Dialog.Key:
                return "This must be the key.";
            default:
                return "";
        }
    }
}

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

    public override void Heat()
    {
        currentTime = maxTime;
    }

    public override bool IsAvailable()
    {
        return currentTime == 0f;
    }

    public override void Update()
    {
        currentTime = Mathf.Clamp(currentTime - Time.deltaTime, 0, maxTime);
    }
    public override float GetCurrentTime() => currentTime;
}

[RequireComponent(typeof(Text))]
public class TextManager : MonoBehaviour
{
    private Text text;

    public enum Dialog { None, Intro, Climbing, Boxes, Objective, Key, Door, Flower }
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
            cooldown.Heat();
            Debug.Log(cooldown);
        }

    }

    public void HideText()
    {
        text.text = GetText(Dialog.None);
        targetAlpha = 0f;
    }

    private string GetText(Dialog d)
    {
        switch (d)
        {
            case Dialog.Intro:
                return "Hello again Alice. I didn't expect you to come back to Wonderland... Especially in these dark times. Find the key and exit by the door.";
            case Dialog.Boxes:
                return "You can push the boxes pushing them. Try moving them towards the mushrooms.";
            case Dialog.Climbing:
                return "You can climb the wall if you walk towards it.";
            case Dialog.Objective:
                return "Take caution making noises with the teapot monster. You should walk or crouch.";
            case Dialog.Door:
                return "This is the door... Where is it supposed to take me?";
            case Dialog.Key:
                return "This must be the key for the door.";            
            case Dialog.None:
                return "";
            case Dialog.Flower:
                return "I wonder if the flowers still sing.";
            default:
                return "";
        }
    }
}

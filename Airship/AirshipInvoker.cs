using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipInvoker : MonoBehaviour
{
    private List<Command> commands = new List<Command>();
    public float scale = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach(Command c in commands)
        {
            c.Execute(gameObject, scale);
        }
    }

    public void Add(Command c)
    {
        commands.Add(c);
    }

    public void Remove(Command c)
    {
        commands.Remove(c);
    }

    public void ClearAllCommands()
    {
        commands.Clear();
    }

    public void SetScaleMultiplier(float scale)
    {
        this.scale = scale;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipInvoker : MonoBehaviour
{
    private List<Command> commands = new List<Command>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach(Command c in commands)
        {
            c.Execute(gameObject);
        }
    }

    public void Add(Command c)
    {
        commands.Add(c);
    }
}

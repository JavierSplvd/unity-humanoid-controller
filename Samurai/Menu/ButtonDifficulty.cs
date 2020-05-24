using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Numian
{
public class ButtonDifficulty : MonoBehaviour
{
    [SerializeField]
    private int level;
    [SerializeField]
    private DifficultyController controller;


    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject
            .FindGameObjectWithTag(GameObjectTags.DifficultyController.ToString())
        .GetComponent<DifficultyController>();
        Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void TaskOnClick(){
		controller.SetLevel(level);
	}
}
}
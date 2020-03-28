using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FootIKSmooth : MonoBehaviour
{

    public float characterMaxHeightToCollider;
    public float characterMinHeightToCollider;

    public bool IkActive= true;
    [Range(0f, 1f)]
    public float WeightPositionRight= 1f;
	[Range(0f, 1f)]
	public float WeightRotationRight= 0f;
    [Range(0f, 1f)]
    public float WeightPositionLeft = 1f;
	[Range(0f, 1f)]
	public float WeightRotationLeft = 0f;

    Animator anim;
    [Tooltip("Offset para ajustar posicion de pie")]
    public Vector3 offset;
    [Tooltip("Capa de los objetos donde se puede ajustar el pie")]
    public LayerMask RayMask;

    public string idleAnimatorStateName = "Idle";

    void Start()
    {
        anim = GetComponent<Animator>();
        SetupColliderHeights();
    }

    void SetupColliderHeights() {
        GetComponent<CharacterController>().height = characterMaxHeightToCollider;
    }

    RaycastHit hit;
    void CharacterControllerAdjustment() {
        GetComponent<CharacterController>().height = characterMinHeightToCollider + (characterMaxHeightToCollider - characterMinHeightToCollider) * Vector3.Dot(hit.normal, Vector3.up);
    }


    void OnAnimatorIK()
    {
        if(IkActive && anim.GetCurrentAnimatorStateInfo(0).IsName(idleAnimatorStateName))
        {
			Vector3 FootPos = anim.GetIKPosition(AvatarIKGoal.RightFoot); //Obtenemos posicion del Pie
            if (Physics.Raycast(FootPos + Vector3.up, Vector3.down, out hit, 1.2f, RayMask)) //Lanzamos raycast hacia abajo
            {
                CharacterControllerAdjustment();
				anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, WeightPositionRight);
				anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, WeightRotationRight);
				anim.SetIKPosition(AvatarIKGoal.RightFoot, hit.point + offset); //Posocionamos el pie segun dio el Raycast

				Debug.DrawLine(hit.point, Vector3.ProjectOnPlane(hit.normal, Vector3.right), Color.blue);
				Debug.DrawLine(FootPos, FootPos + Vector3.right, Color.yellow);

                if (WeightRotationRight > 0f) //Ajustamos rotacion si se tiene asignado
                {
                    //Formula para determina que rotacion requiere el pie (Esto puede mejorarse aun)
                    Quaternion footRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
                    anim.SetIKRotation(AvatarIKGoal.RightFoot, footRotation);
                }
            }
            else //No dio a nada, mejor que conserve poisicion y rotacion de la animacion
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0f);
                anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0f);
            }

			FootPos = anim.GetIKPosition(AvatarIKGoal.LeftFoot); //Obtenemos posicion del Pie
            if (Physics.Raycast(FootPos + Vector3.up, Vector3.down, out hit, 1.2f, RayMask)) //Lanzamos raycast hacia abajo
            {
				anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, WeightPositionLeft);
				anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, WeightRotationLeft);
				anim.SetIKPosition(AvatarIKGoal.LeftFoot, hit.point + offset);

                Debug.DrawLine(hit.point, Vector3.ProjectOnPlane(hit.normal, Vector3.right), Color.blue);
				Debug.DrawLine(FootPos, FootPos + Vector3.right, Color.yellow);

                if (WeightRotationLeft > 0f)//Ajustamos rotacion si se tiene asignado
                {
                    //Formula para determina que rotacion requiere el pie (Esto puede mejorarse aun)
                    Quaternion footRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
                    anim.SetIKRotation(AvatarIKGoal.LeftFoot, footRotation);
                }
            }
            else //No dio a nada, mejor que conserve poisicion y rotacion de la animacion
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0f);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0f);
            }
            

        }
        else //IK Apagado, no hacemos nada
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0f);
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0f);
        }
    }
}
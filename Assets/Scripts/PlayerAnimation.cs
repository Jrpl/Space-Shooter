using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        if (!_anim)
        {
            Debug.LogError("Failed to find Animation Controller on Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Bug with animation getting canceled by immediatley pressing A after having held down D and vice versa
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.J))
        {
            _anim.SetBool("onTurnLeft", true);
            _anim.SetBool("onTurnRight", false);
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.J))
        {
            _anim.SetBool("onTurnLeft", false);
            _anim.SetBool("onTurnRight", false);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.L))
        {
            _anim.SetBool("onTurnLeft", false);
            _anim.SetBool("onTurnRight", true);
        }
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.L))
        {
            _anim.SetBool("onTurnLeft", false);
            _anim.SetBool("onTurnRight", false);
        }
    }
}

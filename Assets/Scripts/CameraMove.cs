using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Animator _anim;
    private AnimationClip _currentClip;


    private void Start()
    {
        _anim = GetComponent<Animator>(); 
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object

                if (hit.transform.CompareTag("MiniGame"))
                {
                    //get the camera animation from the collider
                    AnimationClip camMove = hit.transform.GetComponent<FocusBox>().CameraFocusAnimation;

                    MoveToGame(camMove);
                }
            }
        }


        if (Input.GetMouseButtonDown(1))
        {
            ResetCamera();
        }
    }


    private void MoveToGame(AnimationClip clip)
    {
        _anim.SetFloat("Direction", 1f);

        _currentClip = clip;

        _anim.Play(clip.name);
    }


    private void ResetCamera()
    {
        _anim.SetFloat("Direction", -1f);
        
        _anim.Play(_currentClip.name);
    }


}

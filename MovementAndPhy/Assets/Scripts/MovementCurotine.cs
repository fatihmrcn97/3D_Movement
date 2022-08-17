using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCurotine : MonoBehaviour
{
    [SerializeField] private Transform _firstTargetPos, _secondTargetPos,_endPosition;

    public bool _isRight;

    private Animator anim;

    [SerializeField] TrailRenderer trailerRenderer1, trailerRenderer2;


    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        StartCoroutine(FirstCarMovement_Back(_firstTargetPos,_secondTargetPos, _endPosition, _isRight));
        TrailerRendererFalse();
    }

    private void TrailerRendererFalse()
    {
        trailerRenderer1.emitting = false;
        trailerRenderer2.emitting = false;
    }   private void TrailerRendererTrue()
    {
        trailerRenderer1.emitting = true;
        trailerRenderer2.emitting = true;
    }

    private IEnumerator FirstCarMovement_Back(Transform firstTargetPos, Transform _secondTargetPos, Transform endPosition, bool isRight)
    {
        var origPos = transform.position;
        var origRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(0, 90, 0);
        float elapsedTime = 0;

        while (elapsedTime < .5f)
        {
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / .5f);
            transform.position = new Vector3(origPos.x, origPos.y, Mathf.Lerp(origPos.z, firstTargetPos.position.z, progress));
            yield return null;
        }
        transform.position = new Vector3(origPos.x, origPos.y, firstTargetPos.position.z);
        StartCoroutine(FirstCarMovement_BackRotate(firstTargetPos, _secondTargetPos,endPosition,isRight));
    }
    private IEnumerator FirstCarMovement_BackRotate(Transform firstTargetPos, Transform _secondTargetPos,Transform endPosition,bool isRight)
    {
        // Araba geri geri yanaþýyor bu kýsýmda

        var origPos = transform.position;
        var origRot = transform.rotation;
        float denem;
        Quaternion targetRot = Quaternion.Euler(0, 90, 0);

        if (isRight)
        {
            denem = -3;
            targetRot = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            denem = 3;
            targetRot = Quaternion.Euler(0, -90, -0);
        }


        float elapsedTime = 0;
        while (elapsedTime < .25f)
        {
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / .25f);
            transform.position = new Vector3(Mathf.Lerp(origPos.x, origPos.x+ denem, progress), origPos.y, Mathf.Lerp(origPos.z, origPos.z -3f, progress));
            transform.rotation = Quaternion.Slerp(origRot, targetRot, progress);
            yield return null;

        }
        transform.position = new Vector3(origPos.x + denem, origPos.y, origPos.z - 3f);
      


        StartCoroutine(
                FirstCarMovement_ForwardOnly(_secondTargetPos, endPosition, isRight));
    }

    private IEnumerator FirstCarMovement_ForwardOnly(Transform secondTargetPos,Transform endPosition,bool isRight)
    {
        var origPos = transform.position;
        var origRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(0, 0, 0);
        float elapsedTime = 0;
        while (elapsedTime < .35f)
        {
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / .35f);
            transform.position = new Vector3(Mathf.Lerp(origPos.x, secondTargetPos.position.x, progress), origPos.y, origPos.z);      
            yield return null;

        }
        transform.position = new Vector3(secondTargetPos.position.x, origPos.y, origPos.z);
        StartCoroutine(FirstCarMovement_RotateForward(endPosition, isRight));
    }


    private IEnumerator FirstCarMovement_RotateForward(Transform endPosition,bool isRight)
    {
        var origPos = transform.position;
        var origRot = transform.rotation;
        TrailerRendererTrue();
        // Drift atarken

        Quaternion targetRot;
        float denem;
        if (isRight)
        {
            denem = 6f;
            anim.SetBool("sideupother", true);
            targetRot = Quaternion.Euler(0, -30, 0);
        }
        else
        {
            targetRot = Quaternion.Euler(0, 30, 0);
            anim.SetBool("sideup", true);
            denem = -6f;
        } 
        float elapsedTime = 0;
     
        while (elapsedTime < .3f)
        { 
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / .3f);
            transform.position = new Vector3(Mathf.Lerp(origPos.x, origPos.x + denem, progress), origPos.y, Mathf.Lerp(origPos.z, origPos.z + 3f, progress));
            transform.rotation = Quaternion.Slerp(origRot, targetRot, progress * 1f);
            yield return null;

        }
        transform.rotation = targetRot;
        transform.position = new Vector3(origPos.x + denem, origPos.y, origPos.z + 3f);
        anim.SetBool("sideup", false);
        anim.SetBool("sideupother", false);

        

        StartCoroutine(EndPosition(endPosition));
    }

    private IEnumerator EndPosition(Transform endPosition)
    {
        bool oneTime = true;
        var origPos = transform.position;
        var origRot = transform.rotation;
        float elapsedTime = 0;
     
        while (elapsedTime < .6f)
        {
            elapsedTime += Time.deltaTime;
            if (oneTime && elapsedTime> .3f)
            {   
                TrailerRendererFalse();
                anim.SetTrigger("park");
                oneTime = false;
            }
            var progress = Mathf.Clamp01(elapsedTime / .6f);
            transform.position = new Vector3(origPos.x, origPos.y, Mathf.Lerp(origPos.z, endPosition.position.z, progress));
            transform.rotation = Quaternion.Slerp(origRot, Quaternion.identity, progress * 1.95f);
       
            yield return null;
        }
     
        transform.rotation = Quaternion.identity;
    }


     

}

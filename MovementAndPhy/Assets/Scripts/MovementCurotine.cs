using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCurotine : MonoBehaviour
{
    [SerializeField] private Transform _firstTargetPos, _secondTargetPos,_endPosition;

    public bool _isRight;
    

    private void Start()
    {
        StartCoroutine(FirstCarMovement_Back(_firstTargetPos,_secondTargetPos, _endPosition, _isRight));
    }

    private IEnumerator FirstCarMovement_Back(Transform firstTargetPos, Transform _secondTargetPos, Transform endPosition, bool isRight)
    {
        var origPos = transform.position;
        var origRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(0, 90, 0);
        float elapsedTime = 0;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / 1f);
            transform.position = new Vector3(origPos.x, origPos.y, Mathf.Lerp(origPos.z, firstTargetPos.position.z, progress));
            yield return null;
        }
        transform.position = new Vector3(origPos.x, origPos.y, firstTargetPos.position.z);
        StartCoroutine(FirstCarMovement_BackRotate(firstTargetPos, _secondTargetPos,endPosition,isRight));
    }
    private IEnumerator FirstCarMovement_BackRotate(Transform firstTargetPos, Transform _secondTargetPos,Transform endPosition,bool isRight)
    {
        var origPos = transform.position;
        var origRot = transform.rotation;
        float denem;
        Quaternion targetRot = Quaternion.Euler(0, 90, 0);

        if (isRight)
        {
            denem = 4;
            targetRot = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            denem = -4;
            targetRot = Quaternion.Euler(0, -90, 0);
        }


        float elapsedTime = 0;
        while (elapsedTime < .45f)
        {
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / .45f);
            transform.position = new Vector3(Mathf.Lerp(origPos.x, origPos.x+ denem, progress), origPos.y, Mathf.Lerp(origPos.z, origPos.z + 3f, progress));
            transform.rotation = Quaternion.Slerp(origRot, targetRot, progress);
            yield return null;

        }
        transform.position = new Vector3(origPos.x + denem, origPos.y, origPos.z + 3f);
        StartCoroutine(
                FirstCarMovement_ForwardOnly(_secondTargetPos, endPosition, isRight));
    }

    private IEnumerator FirstCarMovement_ForwardOnly(Transform secondTargetPos,Transform endPosition,bool isRight)
    {
        var origPos = transform.position;
        var origRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(0, 0, 0);
        float elapsedTime = 0;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / 1f);
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
        float denem;
        if (isRight)
        {
            denem = -2f;
        }
        else
        {
            denem = 2f;
        }
        Quaternion targetRot = Quaternion.Euler(0, 0, 0);
        float elapsedTime = 0;
        while (elapsedTime < .3f)
        {
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / .3f);
            transform.position = new Vector3(Mathf.Lerp(origPos.x, origPos.x+ denem, progress), origPos.y, Mathf.Lerp(origPos.z, origPos.z - 3f, progress));
            transform.rotation = Quaternion.Slerp(origRot, targetRot, progress);
            yield return null;

        }
        transform.position = new Vector3(origPos.x + denem, origPos.y, origPos.z - 3f);
        StartCoroutine(EndPosition(endPosition));
    }

    private IEnumerator EndPosition(Transform endPosition)
    {
        var origPos = transform.position;
        var origRot = transform.rotation; 
        float elapsedTime = 0;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / 1f);
            transform.position = new Vector3(origPos.x, origPos.y, Mathf.Lerp(origPos.z, endPosition.position.z, progress)); 
            yield return null;

        }
     
    }


     

}

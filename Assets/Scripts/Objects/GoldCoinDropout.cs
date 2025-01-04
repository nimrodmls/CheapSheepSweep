using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoldCoinDropout : MonoBehaviour
{
    [SerializeField] private Transform visual;
    [SerializeField] private float explosionDistance = 2f;
    [SerializeField] private float explosionDuration = 1f;
    [SerializeField] private float explosionPower = 2f;
    [SerializeField] private float fadeOutTime = 1f;

    private void Start()
    {
        Vector3 translation = new Vector3(
            Random.Range(-explosionDistance, explosionDistance),
            Random.Range(-explosionDistance, explosionDistance));
        int maxJump = 1;
        Sequence animation = transform.DOJump(
            transform.position + translation,
            explosionPower,
            maxJump,
            explosionDuration);
        animation.onComplete = OnCoinJumpCompletion;
    }

    private void OnCoinJumpCompletion()
    {
        float fadeToValue = 0; // Fading out completely
        visual.GetComponent<SpriteRenderer>().DOFade(fadeToValue, fadeOutTime).onComplete = () => Destroy(gameObject);
    }

}

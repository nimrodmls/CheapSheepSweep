using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsCollectedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsCollectedText;
    [SerializeField] private RectTransform collectionPoint;
    [SerializeField] private Transform staticCoin;
    [SerializeField] private float collectionSpeed = 2f;
    [SerializeField] private float collectionImageScaleUp = 1.1f;
    [SerializeField] private float collectionTextScaleUp = 1.05f;
    [SerializeField] private float collectionScaleUpSpeed = 0.2f;

    private int coinsCollected = 0;

    public void CollectCoin(GoldCoin goldCoin)
    {
        Transform collectedCoin = Instantiate(staticCoin, transform.parent);
        collectedCoin.position = Camera.main.WorldToScreenPoint(
            goldCoin.GetComponent<Transform>().position);
        Tweener move_anim = collectedCoin.DOMove(collectionPoint.position, collectionSpeed);
        move_anim.OnStart(() => move_anim.SetEase(Ease.Linear));
        move_anim.onComplete = () => OnCollectionAnimationComplete(collectedCoin.gameObject);
    }

    public void DecrementCounter(int amount)
    {
        coinsCollected -= amount;
        SetCounter();
    }

    public void GetCollectionPoint()
    {

    }

    private void Awake()
    {
        SetCounter();
    }

    private void SetCounter()
    {
        coinsCollectedText.text = coinsCollected.ToString();
    }

    private void OnCollectionAnimationComplete(GameObject collectedCoin)
    {
        Destroy(collectedCoin);
        coinsCollected++;
        SetCounter();

        // Peforming the scale up and down animation of the coin collection (on the image & text)
        collectionPoint.transform.DOScale(
            Vector3.one * collectionImageScaleUp, collectionScaleUpSpeed
        ).onComplete = () => collectionPoint.transform.DOScale(Vector3.one, collectionScaleUpSpeed);
        coinsCollectedText.transform.DOScale(
            Vector3.one * collectionTextScaleUp, collectionScaleUpSpeed
        ).onComplete = () => coinsCollectedText.transform.DOScale(Vector3.one, collectionScaleUpSpeed);
    }
}

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnCoinCollectedEventArgs> OnCoinCollected;
    public class OnCoinCollectedEventArgs : EventArgs
    {
        public GoldCoin collectedCoin;
    }

    public event EventHandler OnBombCollided;
    public event EventHandler OnBombInVicinity;

    public bool IsBombInVicinity { get; private set; }

    [Header("Player Properties")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 400f;
    [SerializeField] private float bombDetectionRadius = 2f;

    private void Awake()
    {
        if (null != Instance)
        {
            Debug.LogError("Only one instance of Player is allowed");
        }

        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnInteraction += GameInput_OnInteraction;
    }

    private void GameInput_OnInteraction(object sender, EventArgs e)
    {
        RaycastHit2D hit = Physics2D.CircleCast(
            transform.position, 2, transform.up, 2, LayerMask.GetMask("Interactables"));
        if (!hit)
        {
            return;
        }

        InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
        if (null != interactable)
        {
            interactable.Interact(this);
        }
    }

    private void Update()
    {
        Vector2 movement = GameInput.Instance.GetMovementVector();
        if (Vector2.zero != movement)
        {
            transform.position += new Vector3(movement.x, movement.y) * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.forward, movement), rotateSpeed * Time.deltaTime);
        }

        CheckVicinity();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (null != collision.gameObject.GetComponent<GoldCoin>())
        {
            GoldCoin collected = collision.gameObject.GetComponent<GoldCoin>();
            collected.Collect(this);
            OnCoinCollected?.Invoke(
                this, new OnCoinCollectedEventArgs { collectedCoin = collected });
        }
        else if (null != collision.gameObject.GetComponent<Bomb>())
        {
            collision.gameObject.GetComponent<Bomb>().Collect(this);
            OnBombCollided?.Invoke(this, EventArgs.Empty);
        }
    }

    private void CheckVicinity()
    {
        List<Collider2D> results = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = LayerMask.GetMask("Collectibles");
        int res_count = Physics2D.OverlapCircle(
            transform.position, bombDetectionRadius, contactFilter, results);
        if (0 == res_count)
        {
            IsBombInVicinity = false;
            return;
        }

        foreach (Collider2D hit in results)
        {
            if (null != hit.gameObject.GetComponent<Bomb>())
            {
                OnBombInVicinity?.Invoke(this, EventArgs.Empty);
                IsBombInVicinity = true;
                return;
            }
        }

        // Reaching here means no bomb in vicinity
        IsBombInVicinity = false;
    }
}

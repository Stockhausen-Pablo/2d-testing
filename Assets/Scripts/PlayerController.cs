using Assets.Scripts;
using Assets.Scripts.DataPersistence;
using Assets.Scripts.DataPersistence.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    public float moveSpeed = 2;

    private bool isMoving;

    private Vector2 input; // 2D Input Vector

    private Vector3 previousInput;

    private Animator animator;

    public LayerMask solidObjectsLayer;

    public LayerMask interactableLayer;

    public LayerMask sceneLoaderLayer;

    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one PlayerController in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        this.animator = GetComponent<Animator>();
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Move(Vector3.zero));
    }

    // Update is called once per frame
    // gets executed on every frame rendered due to MonoBehaviour
    public void HandleUpdate()
    {
        // below code just used to test exiting the scene,
        // you probably wouldn't want to actually do this as part of your character controller script.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // save the game anytime before loading a new scene
            DataPersistenceManager.Instance.SaveGame();
            // load the main menu scene
            SceneManager.LoadSceneAsync("MainMenu");
        }

        if (!isMoving)
        {
            // if not moving, wait and check for input
            input.x = Input.GetAxisRaw("Horizontal"); // Users type left or right key
            input.y = Input.GetAxisRaw("Vertical"); // Users type up or down

            //Debug.Log("This is input.x" + input.x);
            //Debug.Log("This is input.y" + input.y);

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                // Player moved from original spawn
                Vector3 targetPos = transform.position; // store new player position
                targetPos.x += input.x;
                targetPos.y += input.y;

                previousInput = new Vector3(input.x, input.y);

                if (IsWalkable(targetPos))
                    StartCoroutine(Move(targetPos));

                var sceneLoader = IsSceneLoader(targetPos);
                if (sceneLoader.isSceneLoader)
                    LoadScene(sceneLoader.collider);
            }
        }

        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.T))
            Interact();
    }

    void Interact()
    {
        // Determine facing direction while not moving (if not moving the input case is x 0, y 0) 
        var facingDirection = isMoving ? new Vector3(input.x, input.y) : previousInput;
        var interactPos = transform.position + facingDirection;

        // Draws direction line
        Debug.DrawLine(transform.position, interactPos, Color.red, 1f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);

        if (collider != null)
        {
            collider.GetComponent<IInteractable>()?.Interact();
        }
    }

    void LoadScene(Collider2D collider)
    {
        collider.GetComponent<ISceneLoader>()?.Load();
    }

    // Runs in a Coroutine manner
    // https://docs.unity3d.com/Manual/Coroutines.html
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        //animator.SetBool("isMoving", isMoving);
        // if we move to anything and we minus the original move and there a movement is still going on
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            // the player is currently moving
            // Time.deltaTime ensures that frame rate is consistent
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }

    private (bool isSceneLoader, Collider2D collider) IsSceneLoader(Vector3 targetPos)
    {
        var collider = Physics2D.OverlapCircle(targetPos, 0.2f, sceneLoaderLayer);
        if (collider != null)
        {
            // player is standing on sceneLoaderLayer and new scene should be loaded
            return (isSceneLoader: true, collider: collider);
        }
        return (isSceneLoader: false, collider: collider);
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        // Smooth the targetPos because of mid centered pivot of the character sprite
        // moving up will result in collision +1 y before real collision
        // TODO do the smoothing based on the facing direction
        //var facingDirection = new Vector3(input.x, input.y);
        //var interactPos = transform.position + facingDirection;
        //Vector3 smoothedPivotPos = targetPos - new Vector3(0.5f, 1, 0);

        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactableLayer) != null)
        {
            // player is overlapping with solid object and movement should be cancelled
            return false;
        }
        return true;
    }

    public void LoadData(GameData gameData)
    {
        this.transform.position = gameData.playerPos;
    }

    public void SaveData(GameData gameData)
    {
        gameData.playerPos = this.transform.position;
    }
}


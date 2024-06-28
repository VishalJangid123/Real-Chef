using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

   

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    float movingSpeed = 7f;
    float rotationSpeed = 10f;

    bool isWalking = false;

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private LayerMask npcInteractableLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private Transform chatBubbleTransform;

    float playerRadius = .7f;
    float playerHeight = 2;
    float interactionDistance = 2f;

    Vector3 lastInterectDirection;

    BaseCounter selectedCounter;
    BaseInteract baseInteract;
    private KitchenObject kitchenObject;

    public List<RestaurantTable> tables;


    // ----
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer = true;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    // ----

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There is more player instacnce ");

        }
        Instance = this;
       
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
        gameInput.OnInteractNpcAction += GameInput_OnInteractNpcAction;

    }

    private void GameInput_OnInteractNpcAction(object sender, EventArgs e)
    {
        print("I button clicked");

        if (baseInteract != null)
        {
            Say("Hello, how can I help you?");
            
            baseInteract.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            print("F button clicked");
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            print("E button clicked");
            selectedCounter.Interact(this);
        }
        
    }

    float rotationX = 0f;
    float rotationY = 0f;
    float sensitivity = 15f;
    [SerializeField] private Transform cam;

    void Update()
    {
        HandleMovement();
        //HandleMovement__CharacterController();
        HandleInteraction();

        rotationX += Input.GetAxis("Mouse X") * sensitivity;
        rotationY += Input.GetAxis("Mouse Y") * -1f * sensitivity;
        Camera.main.transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
    }

    private void HandleMovement__CharacterController()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void HandleInteraction()
    {
        Vector2 inputVector = gameInput.GetNormalizedMovements();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if(moveDir != Vector3.zero)
        {
            lastInterectDirection = moveDir;
        }

        if(Physics.Raycast(transform.position, lastInterectDirection, out RaycastHit raycastHit, interactionDistance, counterLayerMask))
        {
            // we hit something
            if(raycastHit.transform.TryGetComponent(out BaseCounter basecounter)){

                if (basecounter != selectedCounter)
                {
                    SetSelectedCounter(basecounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }

        }
        else
        {
            SetSelectedCounter(null);
        }

        HandleNPCInteraction();
        
    }

    private void HandleNPCInteraction()
    {
        if (Physics.Raycast(transform.position, lastInterectDirection, out RaycastHit raycastHit, interactionDistance, npcInteractableLayerMask))
        {
            // we hit something

            if (raycastHit.transform.TryGetComponent(out BaseInteract baseInteract))
            {

               this.baseInteract = baseInteract;
               
            }
            else
            {
                this.baseInteract = null;
            }

        }
        else
        {
        }
    }

    private void SetSelectedCounter(BaseCounter basecounter)
    {
        this.selectedCounter = basecounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetNormalizedMovements();
        Vector3 movDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = movingSpeed * Time.deltaTime;
        bool playerCanMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
            playerRadius, movDir, moveDistance);

        if (!playerCanMove)
        {
            // if player cannot move while two keys are pressed eg. WD, WA

            // check if player can move in x direction
            //Vector3 movX = new Vector3(movDir.x, 0, 0).normalized;
            //playerCanMove = movDir.x !=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
            //playerRadius, movX, moveDistance);

            //if (playerCanMove)
            //{
            //    movDir = movX;
            //}
            //else
            //{
            //    // check if player can move in z direction
            //    Vector3 movZ = new Vector3(0, 0, movDir.z).normalized;
            //    playerCanMove = movDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
            //    playerRadius, movZ, moveDistance);

            //    if (playerCanMove)
            //    {
            //        movDir = movZ;
            //    }
            //    else
            //    {
            //        // player cannot move
            //    }
            //}

        }

        if (playerCanMove)
        {
            transform.position += movDir * Time.deltaTime * movingSpeed;
        }

        isWalking = movDir != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, movDir, rotationSpeed * Time.deltaTime);
    }


    public bool IsWalking()
    {
        return isWalking;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject ko)
    {
        kitchenObject = ko;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public void SeatCustomer(Customer customer, RestaurantTable table)
    {
        customer.isSeated = true;
        table.AssignCustomer(customer);
    }

    public void DeclineRequest(Customer customer)
    {
        print("Sorry, we don't have a table available for your party size.");
        //conversationPanel.SetActive(true);
        //conversationText.text = "Sorry, we don't have a table available for your party size.";
        //foreach (Button button in optionButtons)
        //{
        //    button.gameObject.SetActive(false);
        //}
        //StartCoroutine(WaitForConversationEnd());
    }

    //private IEnumerator WaitForConversationEnd()
    //{
    //    while (conversationPanel.activeSelf)
    //    {
    //        yield return null;
    //    }
    //    interactingCustomer = null;
    //}


    public RestaurantTable FindAvailableTable(int partySize)
    {
        foreach (RestaurantTable table in tables)
        {
            if (table.IsAvailable && table.SeatingCapacity >= partySize)
            {
                return table;
            }
        }
        return null;
    }

    public void Say(string text)
    {
        ChatBubble.Create(this.transform, chatBubbleTransform.localPosition, text);
    }
}

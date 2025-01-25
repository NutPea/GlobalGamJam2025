using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Grid.Content
{
    public class EmptyContent : AGridContent
    {
        private static PlayerInput playerInput;
        [SerializeField] private InputActionReference clickAction;

        private void OnEnable()
        {
            if(playerInput == null)
            {
                playerInput = new PlayerInput();
                playerInput.Mouse.Enable();
                playerInput.Enable();
            }

            playerInput.Mouse.LeftClick.performed += OnClick;
        }

        private void OnDisable()
        {
            playerInput.Mouse.LeftClick.performed -= OnClick;
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    GridPresenter.Instance.InvokeGridPressed(Vector2Int.RoundToInt(new Vector2(transform.position.x, transform.position.z)));
                }
            }
        }
    }
}
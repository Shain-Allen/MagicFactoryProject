// GENERATED AUTOMATICALLY FROM 'Assets/InputManagerFiles/GameControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameControls"",
    ""maps"": [
        {
            ""name"": ""GeneralControls"",
            ""id"": ""7a02c4ba-4f01-4eb8-934d-eb029619f802"",
            ""actions"": [
                {
                    ""name"": ""Left Click"",
                    ""type"": ""Button"",
                    ""id"": ""349bf245-e803-445e-9fa0-40da262582b9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right Click"",
                    ""type"": ""Button"",
                    ""id"": ""52658700-929c-409c-898d-a225baec6d39"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerMovement"",
                    ""type"": ""Value"",
                    ""id"": ""b4111608-31e8-46b0-913f-e4c946aff0d9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Button"",
                    ""id"": ""5142d694-8da5-4c36-ad05-7661c0325710"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ClearBrush"",
                    ""type"": ""Button"",
                    ""id"": ""dcb5ec2e-f122-40be-ae28-ea1f72c481a4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e11482a3-a057-407e-b5d8-e822dbbf68cf"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""c957580f-2c58-4c0b-845a-d9f3ab010e9a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""90623648-f3f7-4c6e-b30a-f2ade4c3c453"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""90104ff1-66d3-418b-99dc-408f7e4b7bcb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f5c979af-1ffa-4fa3-9aee-fc7a68ade6d3"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a879511d-7c1a-417a-99a0-741f815be7dc"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d65b097b-6973-480e-a31b-b73bce150912"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ece3315d-f02c-4043-8c8a-ce74e5130b41"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ClearBrush"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""245a7c24-1f15-468c-8b50-8697a6a7976b"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GeneralControls
        m_GeneralControls = asset.FindActionMap("GeneralControls", throwIfNotFound: true);
        m_GeneralControls_LeftClick = m_GeneralControls.FindAction("Left Click", throwIfNotFound: true);
        m_GeneralControls_RightClick = m_GeneralControls.FindAction("Right Click", throwIfNotFound: true);
        m_GeneralControls_PlayerMovement = m_GeneralControls.FindAction("PlayerMovement", throwIfNotFound: true);
        m_GeneralControls_Rotate = m_GeneralControls.FindAction("Rotate", throwIfNotFound: true);
        m_GeneralControls_ClearBrush = m_GeneralControls.FindAction("ClearBrush", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // GeneralControls
    private readonly InputActionMap m_GeneralControls;
    private IGeneralControlsActions m_GeneralControlsActionsCallbackInterface;
    private readonly InputAction m_GeneralControls_LeftClick;
    private readonly InputAction m_GeneralControls_RightClick;
    private readonly InputAction m_GeneralControls_PlayerMovement;
    private readonly InputAction m_GeneralControls_Rotate;
    private readonly InputAction m_GeneralControls_ClearBrush;
    public struct GeneralControlsActions
    {
        private @GameControls m_Wrapper;
        public GeneralControlsActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftClick => m_Wrapper.m_GeneralControls_LeftClick;
        public InputAction @RightClick => m_Wrapper.m_GeneralControls_RightClick;
        public InputAction @PlayerMovement => m_Wrapper.m_GeneralControls_PlayerMovement;
        public InputAction @Rotate => m_Wrapper.m_GeneralControls_Rotate;
        public InputAction @ClearBrush => m_Wrapper.m_GeneralControls_ClearBrush;
        public InputActionMap Get() { return m_Wrapper.m_GeneralControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GeneralControlsActions set) { return set.Get(); }
        public void SetCallbacks(IGeneralControlsActions instance)
        {
            if (m_Wrapper.m_GeneralControlsActionsCallbackInterface != null)
            {
                @LeftClick.started -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnLeftClick;
                @LeftClick.performed -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnLeftClick;
                @LeftClick.canceled -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnLeftClick;
                @RightClick.started -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnRightClick;
                @PlayerMovement.started -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnPlayerMovement;
                @PlayerMovement.performed -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnPlayerMovement;
                @PlayerMovement.canceled -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnPlayerMovement;
                @Rotate.started -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnRotate;
                @ClearBrush.started -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnClearBrush;
                @ClearBrush.performed -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnClearBrush;
                @ClearBrush.canceled -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnClearBrush;
            }
            m_Wrapper.m_GeneralControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LeftClick.started += instance.OnLeftClick;
                @LeftClick.performed += instance.OnLeftClick;
                @LeftClick.canceled += instance.OnLeftClick;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @PlayerMovement.started += instance.OnPlayerMovement;
                @PlayerMovement.performed += instance.OnPlayerMovement;
                @PlayerMovement.canceled += instance.OnPlayerMovement;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @ClearBrush.started += instance.OnClearBrush;
                @ClearBrush.performed += instance.OnClearBrush;
                @ClearBrush.canceled += instance.OnClearBrush;
            }
        }
    }
    public GeneralControlsActions @GeneralControls => new GeneralControlsActions(this);
    public interface IGeneralControlsActions
    {
        void OnLeftClick(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnPlayerMovement(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnClearBrush(InputAction.CallbackContext context);
    }
}

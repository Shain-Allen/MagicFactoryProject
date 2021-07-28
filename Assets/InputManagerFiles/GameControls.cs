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
                    ""name"": ""MouseClicks"",
                    ""type"": ""Button"",
                    ""id"": ""349bf245-e803-445e-9fa0-40da262582b9"",
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
                    ""action"": ""MouseClicks"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8cccea26-2451-4a8e-9130-5b59b489514e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseClicks"",
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
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GeneralControls
        m_GeneralControls = asset.FindActionMap("GeneralControls", throwIfNotFound: true);
        m_GeneralControls_MouseClicks = m_GeneralControls.FindAction("MouseClicks", throwIfNotFound: true);
        m_GeneralControls_PlayerMovement = m_GeneralControls.FindAction("PlayerMovement", throwIfNotFound: true);
        m_GeneralControls_Rotate = m_GeneralControls.FindAction("Rotate", throwIfNotFound: true);
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
    private readonly InputAction m_GeneralControls_MouseClicks;
    private readonly InputAction m_GeneralControls_PlayerMovement;
    private readonly InputAction m_GeneralControls_Rotate;
    public struct GeneralControlsActions
    {
        private @GameControls m_Wrapper;
        public GeneralControlsActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouseClicks => m_Wrapper.m_GeneralControls_MouseClicks;
        public InputAction @PlayerMovement => m_Wrapper.m_GeneralControls_PlayerMovement;
        public InputAction @Rotate => m_Wrapper.m_GeneralControls_Rotate;
        public InputActionMap Get() { return m_Wrapper.m_GeneralControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GeneralControlsActions set) { return set.Get(); }
        public void SetCallbacks(IGeneralControlsActions instance)
        {
            if (m_Wrapper.m_GeneralControlsActionsCallbackInterface != null)
            {
                @MouseClicks.started -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnMouseClicks;
                @MouseClicks.performed -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnMouseClicks;
                @MouseClicks.canceled -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnMouseClicks;
                @PlayerMovement.started -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnPlayerMovement;
                @PlayerMovement.performed -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnPlayerMovement;
                @PlayerMovement.canceled -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnPlayerMovement;
                @Rotate.started -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_GeneralControlsActionsCallbackInterface.OnRotate;
            }
            m_Wrapper.m_GeneralControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MouseClicks.started += instance.OnMouseClicks;
                @MouseClicks.performed += instance.OnMouseClicks;
                @MouseClicks.canceled += instance.OnMouseClicks;
                @PlayerMovement.started += instance.OnPlayerMovement;
                @PlayerMovement.performed += instance.OnPlayerMovement;
                @PlayerMovement.canceled += instance.OnPlayerMovement;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
            }
        }
    }
    public GeneralControlsActions @GeneralControls => new GeneralControlsActions(this);
    public interface IGeneralControlsActions
    {
        void OnMouseClicks(InputAction.CallbackContext context);
        void OnPlayerMovement(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
    }
}

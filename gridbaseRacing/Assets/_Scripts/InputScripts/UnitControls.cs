//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/_Scripts/InputScripts/UnitControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @UnitControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @UnitControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""UnitControls"",
    ""maps"": [
        {
            ""name"": ""BasicMovement"",
            ""id"": ""4c2d207a-a64c-4a7f-8548-2548aa959a0b"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""eac05877-e2a8-47bf-812c-4dbb1af627ae"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Reverse"",
                    ""type"": ""Button"",
                    ""id"": ""a67c0626-7d71-4fab-a780-b081d86d44bb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""2498f537-a3c0-488b-be0d-da29db9de15c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8d6cc230-311d-4348-a2a9-db002d5a5d84"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7619720d-daf5-404a-a999-c34dbe472bd1"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4ed5fe4a-882e-4612-924c-b0a9c971df66"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""604a2016-b272-4fe3-95c0-bad25440921a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector Controller"",
                    ""id"": ""5ead46cf-4660-4e00-abf8-4a659462e822"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b61ba2f0-7447-42d6-896f-b53a35f427b5"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d40fe4fb-09ef-4d04-b419-ed57d13e2ac1"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3f165590-749f-4e07-aa08-1a08ac75dfcf"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""77760748-f276-4ef8-b877-8e036e3bfd69"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""18ac3457-d6e6-4447-8384-849b03df51d6"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reverse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GeneralKeys"",
            ""id"": ""b03e4cfa-981d-4303-8826-3927fcf59c61"",
            ""actions"": [
                {
                    ""name"": ""Restart"",
                    ""type"": ""Button"",
                    ""id"": ""4c5fc31b-8caa-46a3-9c04-cab4fa49e29b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CameraMovement"",
                    ""type"": ""Value"",
                    ""id"": ""7b1d4a1e-ecc5-422d-9c05-c67326ff90c8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Next"",
                    ""type"": ""Button"",
                    ""id"": ""94372c20-8f4e-41cb-86a1-8f989d4f73ef"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b2a9d93f-f7e6-4f69-818a-7f0c8e25b0c7"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Restart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""25ace25d-5981-4cef-9f87-8dfbe721f754"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7dc54caf-7865-4a63-9487-be61a7969afd"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b1e18f4e-a3b9-489d-bb5c-b6e927928915"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""23bb5ec9-167b-45c1-b6f2-0a5a466bbb50"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c8afea07-37ec-4606-9da0-ffab636d2adb"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector Controller"",
                    ""id"": ""7035cdad-e220-42f2-b757-02ee943a2093"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""07a9d9c4-0ef1-45cb-af16-6b6b32a4f9ef"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4a4d2eb6-c0ee-4015-ab8f-56944d9f7938"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0ba7738c-6a46-46e1-a01b-486b8c0a8ce0"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""cffee71d-4490-4500-90bf-bdedc9b9cf4f"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3f3ec6dd-f588-456a-acf7-acbb0baf1c5c"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Next"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // BasicMovement
        m_BasicMovement = asset.FindActionMap("BasicMovement", throwIfNotFound: true);
        m_BasicMovement_Move = m_BasicMovement.FindAction("Move", throwIfNotFound: true);
        m_BasicMovement_Reverse = m_BasicMovement.FindAction("Reverse", throwIfNotFound: true);
        // GeneralKeys
        m_GeneralKeys = asset.FindActionMap("GeneralKeys", throwIfNotFound: true);
        m_GeneralKeys_Restart = m_GeneralKeys.FindAction("Restart", throwIfNotFound: true);
        m_GeneralKeys_CameraMovement = m_GeneralKeys.FindAction("CameraMovement", throwIfNotFound: true);
        m_GeneralKeys_Next = m_GeneralKeys.FindAction("Next", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // BasicMovement
    private readonly InputActionMap m_BasicMovement;
    private List<IBasicMovementActions> m_BasicMovementActionsCallbackInterfaces = new List<IBasicMovementActions>();
    private readonly InputAction m_BasicMovement_Move;
    private readonly InputAction m_BasicMovement_Reverse;
    public struct BasicMovementActions
    {
        private @UnitControls m_Wrapper;
        public BasicMovementActions(@UnitControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_BasicMovement_Move;
        public InputAction @Reverse => m_Wrapper.m_BasicMovement_Reverse;
        public InputActionMap Get() { return m_Wrapper.m_BasicMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BasicMovementActions set) { return set.Get(); }
        public void AddCallbacks(IBasicMovementActions instance)
        {
            if (instance == null || m_Wrapper.m_BasicMovementActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_BasicMovementActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Reverse.started += instance.OnReverse;
            @Reverse.performed += instance.OnReverse;
            @Reverse.canceled += instance.OnReverse;
        }

        private void UnregisterCallbacks(IBasicMovementActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Reverse.started -= instance.OnReverse;
            @Reverse.performed -= instance.OnReverse;
            @Reverse.canceled -= instance.OnReverse;
        }

        public void RemoveCallbacks(IBasicMovementActions instance)
        {
            if (m_Wrapper.m_BasicMovementActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IBasicMovementActions instance)
        {
            foreach (var item in m_Wrapper.m_BasicMovementActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_BasicMovementActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public BasicMovementActions @BasicMovement => new BasicMovementActions(this);

    // GeneralKeys
    private readonly InputActionMap m_GeneralKeys;
    private List<IGeneralKeysActions> m_GeneralKeysActionsCallbackInterfaces = new List<IGeneralKeysActions>();
    private readonly InputAction m_GeneralKeys_Restart;
    private readonly InputAction m_GeneralKeys_CameraMovement;
    private readonly InputAction m_GeneralKeys_Next;
    public struct GeneralKeysActions
    {
        private @UnitControls m_Wrapper;
        public GeneralKeysActions(@UnitControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Restart => m_Wrapper.m_GeneralKeys_Restart;
        public InputAction @CameraMovement => m_Wrapper.m_GeneralKeys_CameraMovement;
        public InputAction @Next => m_Wrapper.m_GeneralKeys_Next;
        public InputActionMap Get() { return m_Wrapper.m_GeneralKeys; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GeneralKeysActions set) { return set.Get(); }
        public void AddCallbacks(IGeneralKeysActions instance)
        {
            if (instance == null || m_Wrapper.m_GeneralKeysActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GeneralKeysActionsCallbackInterfaces.Add(instance);
            @Restart.started += instance.OnRestart;
            @Restart.performed += instance.OnRestart;
            @Restart.canceled += instance.OnRestart;
            @CameraMovement.started += instance.OnCameraMovement;
            @CameraMovement.performed += instance.OnCameraMovement;
            @CameraMovement.canceled += instance.OnCameraMovement;
            @Next.started += instance.OnNext;
            @Next.performed += instance.OnNext;
            @Next.canceled += instance.OnNext;
        }

        private void UnregisterCallbacks(IGeneralKeysActions instance)
        {
            @Restart.started -= instance.OnRestart;
            @Restart.performed -= instance.OnRestart;
            @Restart.canceled -= instance.OnRestart;
            @CameraMovement.started -= instance.OnCameraMovement;
            @CameraMovement.performed -= instance.OnCameraMovement;
            @CameraMovement.canceled -= instance.OnCameraMovement;
            @Next.started -= instance.OnNext;
            @Next.performed -= instance.OnNext;
            @Next.canceled -= instance.OnNext;
        }

        public void RemoveCallbacks(IGeneralKeysActions instance)
        {
            if (m_Wrapper.m_GeneralKeysActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGeneralKeysActions instance)
        {
            foreach (var item in m_Wrapper.m_GeneralKeysActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GeneralKeysActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GeneralKeysActions @GeneralKeys => new GeneralKeysActions(this);
    public interface IBasicMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnReverse(InputAction.CallbackContext context);
    }
    public interface IGeneralKeysActions
    {
        void OnRestart(InputAction.CallbackContext context);
        void OnCameraMovement(InputAction.CallbackContext context);
        void OnNext(InputAction.CallbackContext context);
    }
}

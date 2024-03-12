//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/_Scripts/UnitControls.inputactions
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
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // BasicMovement
        m_BasicMovement = asset.FindActionMap("BasicMovement", throwIfNotFound: true);
        m_BasicMovement_Move = m_BasicMovement.FindAction("Move", throwIfNotFound: true);
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
    public struct BasicMovementActions
    {
        private @UnitControls m_Wrapper;
        public BasicMovementActions(@UnitControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_BasicMovement_Move;
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
        }

        private void UnregisterCallbacks(IBasicMovementActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
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
    public interface IBasicMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBinder : MonoBehaviour
{

    // Variables
    /*---------------------------------------------------------------------------*/


    //Mapping from gameAction identifiers to bound gameActions.
    private static Dictionary<string, GameAction> gameActionBindings;

    //Set of all analog buttons.
    private static HashSet<AnalogButton> analogButtons;

    //Mapping from axis identifiers to bound axes.
    private static Dictionary<string, GameAxis> gameAxisBindings;

    //Mapping from 2d axis identifiers to bound 2d axes.
    private static Dictionary<string, GameAxis2D> gameAxis2DBindings;



    // Unity : Start/Update
    /*---------------------------------------------------------------------------*/


    void Start()
    {
        gameActionBindings = new Dictionary<string, GameAction>();
        analogButtons = new HashSet<AnalogButton>();
        gameAxisBindings = new Dictionary<string, GameAxis>();
    }

    void Update()
    {
        foreach (AnalogButton analogButton in analogButtons)
        {
            analogButton.UpdateButtonState();
        }
    }



    // Methods
    /*---------------------------------------------------------------------------*/


    /*************************************************************************
     * Binding                                              *
     *************************************************************************/

    // Button-Action

    /**
     * Binds the button with the specified button ID (such as "joysyick 1 button 
     * 2") and description (such as "RB") to an gameAction with the specified gameAction 
     * ID (such as "interact") and description (e.g. "Interact with 
     * Buttons").
     * 
     * Returns true if this was the first input to be bound to the specified gameAction.
     */
    public static bool BindKey(string gameActionID, string gameActionDescr, string KeyID, string keyDescr)
    {
        return BindButton(gameActionID, gameActionDescr, new Key(KeyID, keyDescr));
    }

    /**
     * Binds the mouse button with the specified button ID (such as 0) and 
     * description (such as "Left Mouse Button") with to an gameAction with the 
     * specified gameAction ID (such as "interact") and description (e.g. "Interact 
     * with Buttons").
     * 
     * Returns true if this was the first input to be bound to the specified gameAction.
     */
    public static bool BindMouseButton(string gameActionID, string gameActionDescr, int buttonID, string buttonDescr)
    {
        return BindButton(gameActionID, gameActionDescr, new MouseButton(buttonID, buttonDescr));
    }

    /**
     * Binds the analog axis with the specified analog axis ID (such as "joystick 1 
     * axis0") and description (such as "Left Mouse Button") to a game action with 
     * the specified game action ID (such as "interact") and description (e.g. 
     * "Interact with Buttons").
     * 
     * The analog axis will be act as a button and activate when it has a value 
     * greater than triggerPoint (if triggerPoint > 0) or a value less than 
     * triggerPoint (if triggerPoint < 0).
     * 
     * Returns true if this was the first input to be bound to the specified 
     * gameAction.
     */
    public static bool BindAnalogButton(string gameActionID, string gameActionDescr, string analogAxisID, string buttonDescr, float triggerPoint)
    {
        AnalogButton analogButton = new AnalogButton(analogAxisID, buttonDescr, triggerPoint);
        analogButtons.Add(analogButton);
        return BindButton(gameActionID, gameActionDescr, analogButton);
    }

    /**
     * Unbinds the input with the specified description to the gameAction with the 
     * specified identifier.
     * 
     * Returns true if such an input was unbound; else returns false.
     */
    public static bool UnbindButton(string gameActionID, string buttonDescr)
    {
        if (gameActionBindings.ContainsKey(gameActionID)) //GameAction exists
        {
            GameAction gameAction = gameActionBindings[gameActionID];
            bool success = gameAction.UnbindButton(buttonDescr);
            if (success && !gameAction.HasBoundButtons())
            {
                gameActionBindings.Remove(gameActionID);
            }
            return success;
        }

        return false;
    }


    // InputAxis-GameAxis

    public static bool BindKeyAxis(string gameAxisID, string gameAxisDescr, string posKeyID, string negKeyID, string keyAxisDescr)
    {
        return BindAxis(gameAxisID, gameAxisDescr, new KeyAxis(posKeyID, negKeyID, keyAxisDescr));
    }

    public static bool BindMouseButtonAxis(string gameAxisID, string gameAxisDescr, int posButtonID, int negButtonID, string buttonAxisDescr)
    {
        return BindAxis(gameAxisID, gameAxisDescr, new MouseButtonAxis(posButtonID, negButtonID, buttonAxisDescr));
    }

    public static bool BindAnalogAxis(string gameAxisID, string gameAxisDescr, string analogAxisID, string analogAxisDescr)
    {
        return BindAxis(gameAxisID, gameAxisDescr, new AnalogAxis(analogAxisID, analogAxisDescr));
    }

    public static bool UnbindAxis(string gameAxisID, string inputAxisDescr)
    {
        if (gameAxisBindings.ContainsKey(gameAxisID)) //Game action exists
        {
            GameAxis gameAxis = gameAxisBindings[gameAxisID];
            bool success = gameAxis.UnbindInputAxis(inputAxisDescr);
            if (success && !gameAxis.HasBoundInputAxes())
            {
                gameAxisBindings.Remove(gameAxisID);
            }
            return success;
        }

        return false;
    }


    // InputAxis2D-GameAxis2D

    public static bool BindKeyAxis2D(string gameAxisID, string gameAxisDescr, string posXKeyID, string negXKeyID, string posYKeyID, string negYKeyID, string keyAxisDescr)
    {
        return BindAxis2D(gameAxisID, gameAxisDescr, new KeyAxis2D(posXKeyID, negXKeyID, posYKeyID, negYKeyID, keyAxisDescr));
    }

    public static bool BindMouseButtonAxis2D(string gameAxisID, string gameAxisDescr, int posXButtonID, int negXButtonID, int posYButtonID, int negYButtonID, string buttonAxisDescr)
    {
        return BindAxis2D(gameAxisID, gameAxisDescr, new MouseButtonAxis2D(posXButtonID, negXButtonID, posYButtonID, negYButtonID, buttonAxisDescr));
    }

    public static bool BindAnalogAxis2D(string gameAxisID, string gameAxisDescr, string analogXAxisID, string analogYAxisID, string analogAxisDescr)
    {
        return BindAxis2D(gameAxisID, gameAxisDescr, new AnalogAxis2D(analogXAxisID, analogYAxisID, analogAxisDescr));
    }

    public static bool UnbindAxis2D(string gameAxisID, string inputAxisDescr)
    {
        if (gameAxis2DBindings.ContainsKey(gameAxisID)) //Game action exists
        {
            GameAxis2D gameAxis2D = gameAxis2DBindings[gameAxisID];
            bool success = gameAxis2D.UnbindInputAxis2D(inputAxisDescr);
            if (success && !gameAxis2D.HasBoundInputAxes2D())
            {
                gameAxis2DBindings.Remove(gameAxisID);
            }
            return success;
        }

        return false;
    }


    /*************************************************************************
     * Getting input                                                         *
     *************************************************************************/

    // Actions/Buttons

    /**
     * Returns true while any input bound to the specified gameAction is held down.
     */
    public static bool GetHeld(string gameActionID)
    {
        return GetStatus(gameActionID, ButtonInput.ButtonState.Held);
    }

    /**
     * Returns false while any input bound to the specified gameAction is released.
     */
    public static bool GetReleased(string gameActionID)
    {
        return GetStatus(gameActionID, ButtonInput.ButtonState.Released);
    }

    /**
     * Returns true during the first frame the user triggers an input bound to 
     * the specified gameAction.
     */
    public static bool GetDown(string gameActionID)
    {
        return GetStatus(gameActionID, ButtonInput.ButtonState.Down);
    }

    /**
     * Returns true the first frame the user releases an input bound to the 
     * specified gameAction.
     */
    public static bool GetUp(string gameActionID)
    {
        return GetStatus(gameActionID, ButtonInput.ButtonState.Up);
    }

    // Axes

    public static float GetAxis(string gameAxisID)
    {
        GameAxis gameAxis;
        if (gameAxisBindings.TryGetValue(gameAxisID, out gameAxis))
        {
            return gameAxis.GetAxis();
        }
        else
        {
            Debug.LogError("A game axis with the identification " + gameAxisID + " has not been bound.");
            return 0.0f;
        }
    }

    // 2D Axes

    public static void GetAxis2D(string gameAxisID, out float axisX, out float axisY)
    {
        GameAxis2D gameAxis2D;
        if (gameAxis2DBindings.TryGetValue(gameAxisID, out gameAxis2D))
        {
            gameAxis2D.GetAxis2D(out axisX, out axisY);
        }
        else
        {
            Debug.LogError("A 2D game axis with the identification " + gameAxisID + " has not been bound.");
            axisX = 0.0f;
            axisY = 0.0f;
        }
    }

    public static float GetAxis2DX(string gameAxisID)
    {
        GameAxis2D gameAxis2D;
        if (gameAxis2DBindings.TryGetValue(gameAxisID, out gameAxis2D))
        {
            return gameAxis2D.GetAxisX();
        }
        else
        {
            Debug.LogError("A 2D game axis with the identification " + gameAxisID + " has not been bound.");
            return 0.0f;
        }
    }

    public static float GetAxis2DY(string gameAxisID)
    {
        GameAxis2D gameAxis2D;
        if (gameAxis2DBindings.TryGetValue(gameAxisID, out gameAxis2D))
        {
            return gameAxis2D.GetAxisY();
        }
        else
        {
            Debug.LogError("A 2D game axis with the identification " + gameAxisID + " has not been bound.");
            return 0.0f;
        }
    }
    

    /*************************************************************************
     * Input descriptions                                                    *
     *************************************************************************/

    /**
     * Get a lists of all descriptory names of inputs boudn to the gameActions the 
     * the specifed identifier.
     * 
     * Returns true if any gameAction/inputs were found; else returns false and the 
     * list is set to null.
     */
    public static bool GetButtonDescriptions(string gameActionID, out ArrayList list)
    {
        GameAction gameAction;
        if (gameActionBindings.TryGetValue(gameActionID, out gameAction))
        {
            gameAction.GetInputDescriptions(out list);
            return true;
        }
        else
        {
            list = null;
            return false;
        }

    }

    public static bool GetInputAxisDescriptions(string gameAxisID, out ArrayList list)
    {
        GameAxis gameAxis;
        if (gameAxisBindings.TryGetValue(gameAxisID, out gameAxis))
        {
            gameAxis.GetInputDescriptions(out list);
            return true;
        }
        else
        {
            list = null;
            return false;
        }

    }

    public static bool GetInputAxis2DDescriptions(string gameAxisID, out ArrayList list)
    {
        GameAxis2D gameAxis2D;
        if (gameAxis2DBindings.TryGetValue(gameAxisID, out gameAxis2D))
        {
            gameAxis2D.GetInputDescriptions(out list);
            return true;
        }
        else
        {
            list = null;
            return false;
        }

    }



    // Private 
    /*-----------------------------------------------------------------------*/


    /*************************************************************************
     * Helper functions                                                      *
     *************************************************************************/

    /**
     * Binds the specified input to the an gameAction with the specified input ID 
     * (such as "interact") and description (e.g. "Interact with Buttons").
     * 
     * If no such gameAction exists, creates one.
     * 
     * If an gameAction was created, returns true; if this input was added to a 
     * already bound gameAction, returns false.
     */
    private static bool BindButton(string gameActionID, string gameActionDescr, ButtonInput button)
    {
        if (gameActionBindings.ContainsKey(gameActionID)) //Already bound gameAction
        {
            gameActionBindings[gameActionID].BindButton(button);
            return false;
        }
        else //New gameAction
        {
            gameActionBindings.Add(gameActionID, new GameAction(button, gameActionDescr));
            return true;
        }
    }

    private static bool BindAxis(string gameAxisID, string gameAxisDescr, AxisInput inputAxis)
    {
        if (gameAxisBindings.ContainsKey(gameAxisID)) //Already bound game axis
        {
            gameAxisBindings[gameAxisID].BindInputAxis(inputAxis);
            return false;
        }
        else //New game axis
        {
            gameAxisBindings.Add(gameAxisID, new GameAxis(inputAxis, gameAxisDescr));
            return true;
        }
    }

    private static bool BindAxis2D(string gameAxisID, string gameAxisDescr, AxisInput2D inputAxis2D)
    {
        if (gameAxis2DBindings.ContainsKey(gameAxisID)) //Already bound 2D game axis 
        {
            gameAxis2DBindings[gameAxisID].BindInputAxis2D(inputAxis2D);
            return false;
        }
        else //New 2D game axis
        {
            gameAxis2DBindings.Add(gameAxisID, new GameAxis2D(inputAxis2D, gameAxisDescr));
            return true;
        }
    }

    /**
     * Returns true if any button bound to the specified gameAction has the specified status.
     */
    private static bool GetStatus(string gameActionID, ButtonInput.ButtonState inputStatus)
    {
        GameAction gameAction;
        if (gameActionBindings.TryGetValue(gameActionID, out gameAction))
        {
            return gameAction.GetStatus(inputStatus);
        }
        else
        {
            Debug.LogError("A game action with the identification " + gameActionID + " has not been bound.");
            return false;
        }
    }


    /*************************************************************************
     * Internal Button/Action classes                                        *
     *************************************************************************/

    /**
     * An gameAction that inputs can be bound to.
     */
    private class GameAction
    {
        private string description;
        private ArrayList inputList;

        public GameAction(string description)
        {
            this.description = description;
            inputList = new ArrayList();
        }

        public GameAction(ButtonInput button, string description) : this(description)
        {
            BindButton(button);
        }

        public string GetDescription()
        {
            return description;
        }

        public void GetInputDescriptions(out ArrayList list)
        {
            list = new ArrayList();
            foreach (ButtonInput button in inputList)
            {
                list.Add(button.GetDescription());
            }
        }

        public void BindButton(ButtonInput button)
        {
            inputList.Add(button);
        }

        public bool UnbindButton(string buttonDescr)
        {
            foreach (ButtonInput button in inputList)
            {
                if (String.Equals(button.GetDescription(), buttonDescr))
                {
                    inputList.Remove(button);
                    return true;
                }
            }

            return false;
        }

        public bool HasBoundButtons()
        {
            return inputList.Count != 0;
        }

        public bool GetStatus(ButtonInput.ButtonState inputStatus)
        {
            foreach (ButtonInput button in inputList)
            {
                if (button.IsStatus(inputStatus))
                {
                    return true;
                }
            }
            return false;
        }
    }

    /**
     * A generic button input.
     */
    private abstract class ButtonInput
    {
        private string description; //Decriptive name of the input, such as "RT" (right trigger)

        public enum ButtonState
        {
            Released,
            Down,
            Held,
            Up,
        }

        protected ButtonInput(string description)
        {
            this.description = description;
        }

        public string GetDescription()
        {
            return description;
        }

        public bool IsStatus(ButtonState inputStatus)
        {
            switch (inputStatus)
            {
                case ButtonState.Held:
                    return IsHeld();

                case ButtonState.Released:
                    return IsReleased();

                case ButtonState.Down:
                    return IsDown();

                case ButtonState.Up:
                    return IsUp();
            }
            return false;
        }

        public abstract bool IsHeld();
        public abstract bool IsReleased();
        public abstract bool IsDown();
        public abstract bool IsUp();
    }

    /**
     * A button input.
     */
    private class Key : ButtonInput
    {
        private string keyID;

        public Key(string keyID, string keyDescr) : base(keyDescr)
        {
            this.keyID = keyID;
        }

        public override bool IsHeld()
        {
            return Input.GetButton(keyID);
        }

        public override bool IsReleased()
        {
            return ! IsHeld();
        }

        public override bool IsDown()
        {
            return Input.GetButtonDown(keyID);
        }

        public override bool IsUp()
        {
            return Input.GetButtonUp(keyID);
        }
    }

    /**
     * A mouse button input.
     */
    private class MouseButton : ButtonInput
    {
        private int buttonID;

        public MouseButton(int buttonID, string buttonDescr) : base(buttonDescr)
        {
            this.buttonID = buttonID;
        }

        public override bool IsHeld()
        {
            return Input.GetMouseButton(buttonID);
        }

        public override bool IsReleased()
        {
            return ! IsHeld();
        }

        public override bool IsDown()
        {
            return Input.GetMouseButtonDown(buttonID);
        }

        public override bool IsUp()
        {
            return Input.GetMouseButtonUp(buttonID);
        }
    }

    /**
     * An analog axis input (such as a mouse movement or analog stick direction) acting as a button.
     */
    private class AnalogButton : ButtonInput
    {

        private string analogAxisID;
        private ButtonState buttonStatus;

        private float triggerPoint;
        private bool triggerPositive;

        public AnalogButton(string analogAxisID, string analogAxisDescr, float triggerPoint) : base(analogAxisDescr)
        {
            this.analogAxisID = analogAxisID;
            buttonStatus = ButtonState.Released;

            this.triggerPoint = triggerPoint;
            if (triggerPoint > 0)
            {
                triggerPositive = true;
            }
            else
            {
                triggerPositive = false;
            }
        }

        public void UpdateButtonState()
        {
            switch (buttonStatus)
            {
                case ButtonInput.ButtonState.Released:
                    if (IsHeld())
                    {
                        buttonStatus = ButtonInput.ButtonState.Down;
                    }
                    break;

                case ButtonInput.ButtonState.Down:
                    buttonStatus = ButtonInput.ButtonState.Held;
                    break;

                case ButtonInput.ButtonState.Held:
                    if (IsReleased())
                    {
                        buttonStatus = ButtonInput.ButtonState.Up;
                    }
                    break;

                case ButtonInput.ButtonState.Up:
                    buttonStatus = ButtonInput.ButtonState.Released;
                    break;
            }
        }

        public override bool IsHeld()
        {
            if (triggerPositive)
            {
                return Input.GetAxis(analogAxisID) > triggerPoint;
            }
            else
            {
                return Input.GetAxis(analogAxisID) < triggerPoint;
            }
        }

        public override bool IsReleased()
        {
            return Input.GetAxis(analogAxisID) == 0;
        }


        public override bool IsDown()
        {
            return buttonStatus == ButtonState.Down;
        }

        public override bool IsUp()
        {
            return buttonStatus == ButtonState.Up;
        }
    }


    /*************************************************************************
     * Internal Axis classes                                                 *
     *************************************************************************/

    private class GameAxis
    {
        private string description;
        private ArrayList inputList;

        public GameAxis(string description)
        {
            this.description = description;
            inputList = new ArrayList();
        }

        public GameAxis(AxisInput inputAxis, string description) : this(description)
        {
            BindInputAxis(inputAxis);
        }

        public string GetDescription()
        {
            return description;
        }

        public void GetInputDescriptions(out ArrayList list)
        {
            list = new ArrayList();
            foreach (AxisInput inputAxis in inputList)
            {
                list.Add(inputAxis.GetDescription());
            }
        }

        public void BindInputAxis(AxisInput inputAxis)
        {
            inputList.Add(inputAxis);
        }

        public bool UnbindInputAxis(string inputAxisDescr)
        {
            foreach (AxisInput inputAxis in inputList)
            {
                if (String.Equals(inputAxis.GetDescription(), inputAxisDescr))
                {
                    inputList.Remove(inputAxis);
                    return true;
                }
            }

            return false;
        }

        public bool HasBoundInputAxes()
        {
            return inputList.Count != 0;
        }

        public float GetAxis()
        {
            float axis;
            foreach (AxisInput inputAxis in inputList)
            {
                axis = inputAxis.GetAxis();
                if (axis != 0.0f) 
                {
                    return axis;
                }
            }
            return 0.0f;
        }
    }

    /**
     * A generic axis input.
     */
    private abstract class AxisInput
    {
        private string description; //Decriptive name of the input, such as "RT" (right trigger)

        protected AxisInput(string description)
        {
            this.description = description;
        }

        public string GetDescription()
        {
            return description;
        }

        public abstract float GetAxis();
    }

    private class KeyAxis : AxisInput
    {
        private string posKeyID;
        private string negKeyID;

        public KeyAxis(string posKeyID, string negKeyID, string KeyAxisDescr) : base(KeyAxisDescr)
        {
            this.posKeyID = posKeyID;
            this.negKeyID = negKeyID;
        }

        public override float GetAxis()
        {
            float posDown = Input.GetKey(posKeyID) ? 1.0f : 0.0f;
            float negDown = Input.GetKey(negKeyID) ? 1.0f : 0.0f;
            return posDown - negDown;
        }
    }

    private class MouseButtonAxis : AxisInput
    {
        private int posButtonID;
        private int negButtonID;

        public MouseButtonAxis(int posButtonID, int negButtonID, string buttonAxisDescr) : base(buttonAxisDescr)
        {
            this.posButtonID = posButtonID;
            this.negButtonID = negButtonID;
        }

        public override float GetAxis()
        {
            float posDown = Input.GetMouseButton(posButtonID) ? 1.0f : 0.0f;
            float negDown = Input.GetMouseButton(negButtonID) ? 1.0f : 0.0f;
            return posDown - negDown;
        }
    }

    private class AnalogAxis : AxisInput
    {
        public string analogAxisID;

        public AnalogAxis(string analogAxisID, string analogAxisDescr) : base(analogAxisDescr)
        {
            this.analogAxisID = analogAxisID;
        }

        public override float GetAxis()
        {
            return Input.GetAxis(analogAxisID);
        }
    }


    /*************************************************************************
     * Internal 2D Axis classes                                                 *
     *************************************************************************/

    private class GameAxis2D
    {
        private string description;
        private ArrayList inputList;

        public GameAxis2D(string description)
        {
            this.description = description;
            inputList = new ArrayList();
        }

        public GameAxis2D(AxisInput2D inputAxis2D, string description) : this(description)
        {
            BindInputAxis2D(inputAxis2D);
        }

        public string GetDescription()
        {
            return description;
        }

        public void GetInputDescriptions(out ArrayList list)
        {
            list = new ArrayList();
            foreach (AxisInput2D inputAxis2D in inputList)
            {
                list.Add(inputAxis2D.GetDescription());
            }
        }

        public void BindInputAxis2D(AxisInput2D inputAxis2D)
        {
            inputList.Add(inputAxis2D);
        }

        public bool UnbindInputAxis2D(string inputAxisDescr)
        {
            foreach (AxisInput2D inputAxis2D in inputList)
            {
                if (String.Equals(inputAxis2D.GetDescription(), inputAxisDescr))
                {
                    inputList.Remove(inputAxis2D);
                    return true;
                }
            }

            return false;
        }

        public bool HasBoundInputAxes2D()
        {
            return inputList.Count != 0;
        }

        public void GetAxis2D(out float axisX, out float axisY)
        {
            float tempAxisX;
            float tempAxisY;
            axisX = 0.0f;
            axisY = 0.0f;

            foreach (AxisInput2D inputAxis in inputList)
            {
                inputAxis.GetAxis2D(out tempAxisX, out tempAxisY);
                if (tempAxisX != 0.0f && axisX == 0.0f)
                {
                    axisX = tempAxisX;
                    if (axisY != 0.0f)
                    {
                        return;
                    }
                }
                if (tempAxisY != 0.0f && axisY == 0.0f)
                {
                    axisY = tempAxisY;
                    if (axisX != 0.0f)
                    {
                        return;
                    }
                }
            }
        }

        public float GetAxisX()
        {
            float axis;
            foreach (AxisInput2D inputAxis in inputList)
            {
                axis = inputAxis.GetAxisX();
                if (axis != 0.0f)
                {
                    return axis;
                }
            }
            return 0.0f;
        }

        public float GetAxisY()
        {
            float axis;
            foreach (AxisInput2D inputAxis in inputList)
            {
                axis = inputAxis.GetAxisY();
                if (axis != 0.0f)
                {
                    return axis;
                }
            }
            return 0.0f;
        }
    }

    /**
     * A generic 2D axis input.
     */
    private abstract class AxisInput2D
    {
        private string description; //Decriptive name of the input, such as "RT" (right trigger)

        protected AxisInput2D(string description)
        {
            this.description = description;
        }

        public string GetDescription()
        {
            return description;
        }

        public abstract void GetAxis2D(out float axisX, out float axisY);
        public abstract float GetAxisX();
        public abstract float GetAxisY();
    }

    private abstract class ButtonAxis2D : AxisInput2D
    {
        private static float diagFactor = (float)Math.Sqrt(0.5);

        protected ButtonAxis2D(string buttonAxisDescr) : base(buttonAxisDescr) 
        {
        }

        protected float GetAxis(bool posButton, bool negButton, bool orthogonalButton)
        {
            if (orthogonalButton) //Diagional
            {
                return (posButton ? diagFactor : 0.0f) - (negButton ? diagFactor : 0.0f);
            }
            else //Straight
            {
                return (posButton ? 1.0f : 0.0f) - (negButton ? 1.0f : 0.0f);
            }
        }

        protected void GetAxis2D(bool posXButton, bool negXButton, bool posYButton, bool negYButton, out float axisX, out float axisY)
        {
            if ((posXButton || negXButton) && (posYButton || negYButton)) //Diagional
            {
                axisX = (posXButton ? diagFactor : 0.0f) - (negXButton ? diagFactor : 0.0f);
                axisY = (posYButton ? diagFactor : 0.0f) - (negYButton ? diagFactor : 0.0f);
            }
            else //Straight
            {
                axisX = (posXButton ? 1.0f : 0.0f) - (negXButton ? 1.0f : 0.0f);
                axisY = (posYButton ? 1.0f : 0.0f) - (negYButton ? 1.0f : 0.0f);
            }
        }
    }

    private class KeyAxis2D : ButtonAxis2D
    {
        private string posXKeyID;
        private string negXKeyID;

        private string posYKeyID;
        private string negYKeyID;

        public KeyAxis2D(string posXKeyID, string negXKeyID, string posYKeyID, string negYKeyID, string keyAxisDescr) : base(keyAxisDescr)
        {
            this.posXKeyID = posXKeyID;
            this.negXKeyID = negXKeyID;

            this.posYKeyID = posYKeyID;
            this.negYKeyID = negYKeyID;
        }

        public override void GetAxis2D(out float axisX, out float axisY)
        {
            GetAxis2D(Input.GetKey(posXKeyID), Input.GetKey(negXKeyID), Input.GetKey(posYKeyID), Input.GetKey(negYKeyID), out axisX, out axisY);
        }

        public override float GetAxisX()
        {
            return GetAxis(Input.GetKey(posXKeyID), Input.GetKey(negXKeyID), Input.GetKey(posYKeyID) || Input.GetKey(negYKeyID));
        }

        public override float GetAxisY()
        {
            return GetAxis(Input.GetKey(posYKeyID), Input.GetKey(negYKeyID), Input.GetKey(posXKeyID) || Input.GetKey(negXKeyID));
        }
    }

    private class MouseButtonAxis2D : ButtonAxis2D
    {
        private int posXButtonID;
        private int negXButtonID;

        private int posYButtonID;
        private int negYButtonID;

        public MouseButtonAxis2D(int posXButtonID, int negXButtonID, int posYButtonID, int negYButtonID, string buttonAxisDescr) : base(buttonAxisDescr)
        {
            this.posXButtonID = posXButtonID;
            this.negXButtonID = negXButtonID;

            this.posYButtonID = posYButtonID;
            this.negYButtonID = negYButtonID;
        }

        public override void GetAxis2D(out float axisX, out float axisY)
        {
            GetAxis2D(Input.GetMouseButton(posXButtonID), Input.GetMouseButton(negXButtonID), Input.GetMouseButton(posYButtonID), Input.GetMouseButton(negYButtonID), out axisX, out axisY);
        }

        public override float GetAxisX()
        {
            return GetAxis(Input.GetMouseButton(posXButtonID), Input.GetMouseButton(negXButtonID), Input.GetMouseButton(posYButtonID) || Input.GetMouseButton(negYButtonID));
        }

        public override float GetAxisY()
        {
            return GetAxis(Input.GetMouseButton(posYButtonID), Input.GetMouseButton(negYButtonID), Input.GetMouseButton(posXButtonID) || Input.GetMouseButton(negXButtonID));
        }
    }

    private class AnalogAxis2D : AxisInput2D
    {
        private string analogXAxisID;
        private string analogYAxisID;

        public AnalogAxis2D(string analogXAxisID, string analogYAxisID, string analogAxisDescr) : base(analogAxisDescr)
        {
            this.analogXAxisID = analogXAxisID;
            this.analogYAxisID = analogYAxisID;
        }

        public override void GetAxis2D(out float axisX, out float axisY)
        {
            axisX = Input.GetAxis(analogXAxisID);
            axisY = Input.GetAxis(analogXAxisID);
        }

        public override float GetAxisX()
        {
            return Input.GetAxis(analogXAxisID);
        }

        public override float GetAxisY()
        {
            return Input.GetAxis(analogYAxisID);
        }
    }
}

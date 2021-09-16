using Godot;
using Godot.Collections;

public class VirtualControls : CanvasLayer
{
    [Export]
    public bool Visible {
        set => _SetVisibilityStatus(value);
        get => _GetVisibilityStatus();
    }
    [Export]
    public Dictionary<string, string> ActionMap = new Dictionary<string, string>();

    private MarginContainer _Main;
    private VirtualButton _ButtonA;
    private VirtualButton _ButtonB;
    private VirtualButton _ButtonX;
    private VirtualJoystick _Joystick;
    private bool _Visible;
    private Logging _Logger;

    public VirtualControls() {
        _Logger = Logging.GetLogger("VirtualControls");
    }

    public override void _Ready() {
        _Main = GetNode<MarginContainer>("Main");

        _ButtonA = GetNode<VirtualButton>("Main/Rows/BottomRow/RightColumn/A");
        _ButtonA.Connect(nameof(VirtualButton.Touched), this, nameof(_ButtonEvent), new Array() { _ButtonA, true });
        _ButtonA.Connect(nameof(VirtualButton.Released), this, nameof(_ButtonEvent), new Array() { _ButtonA, false });

        _ButtonB = GetNode<VirtualButton>("Main/Rows/BottomRow/RightColumn/B");
        _ButtonB.Connect(nameof(VirtualButton.Touched), this, nameof(_ButtonEvent), new Array() { _ButtonB, true });
        _ButtonB.Connect(nameof(VirtualButton.Released), this, nameof(_ButtonEvent), new Array() { _ButtonB, false });

        _ButtonX = GetNode<VirtualButton>("Main/Rows/BottomRow/RightColumn/X");
        _ButtonX.Connect(nameof(VirtualButton.Touched), this, nameof(_ButtonEvent), new Array() { _ButtonX, true });
        _ButtonX.Connect(nameof(VirtualButton.Released), this, nameof(_ButtonEvent), new Array() { _ButtonX, false });

        _Joystick = GetNode<VirtualJoystick>("Main/Rows/BottomRow/LeftColumn/Joystick");
        _Joystick.Connect(nameof(VirtualJoystick.Moved), this, nameof(_JoystickMoved));
        _Joystick.Connect(nameof(VirtualJoystick.Released), this, nameof(_JoystickReleased));

        _SetVisibilityStatus(_Visible);
    }

    private void _SetVisibilityStatus(bool visibility) {
        _Visible = visibility;
        if (_Main != null) {
            _Main.Visible = visibility;
        }
    }

    private bool _GetVisibilityStatus() {
        if (_Main != null) {
            return _Main.Visible;
        } else {
            return _Visible;
        }
    }

    private void _ButtonEvent(VirtualButton button, bool pressed) {
        if (button == _ButtonA) {
            _SendButtonEvent("a", pressed);
        }

        else if (button == _ButtonB) {
            _SendButtonEvent("b", pressed);
        }

        else if (button == _ButtonX) {
            _SendButtonEvent("x", pressed);
        }
    }

    private void _JoystickMoved(Vector3 force) {
        if (force.x < 0) {
            _SendJoystickEvent("left", -force.x);
            _SendJoystickEvent("right", 0);
        } else if (force.x > 0) {
            _SendJoystickEvent("right", force.x);
        } else {
            _SendJoystickEvent("left", 0);
            _SendJoystickEvent("right", 0);
        }

        if (force.y < 0) {
            _SendJoystickEvent("up", -force.y);
            _SendJoystickEvent("down", 0);
        } else if (force.y > 0) {
            _SendJoystickEvent("up", 0);
            _SendJoystickEvent("down", force.y);
        } else {
            _SendJoystickEvent("up", 0);
            _SendJoystickEvent("down", 0);
        }
    }

    private void _JoystickReleased() {
        _SendJoystickEvent("left", 0);
        _SendJoystickEvent("right", 0);
        _SendJoystickEvent("up", 0);
        _SendJoystickEvent("down", 0);
    }

    private void _SendJoystickEvent(string name, float value) {
        if (ActionMap.ContainsKey(name)) {
            var action = ActionMap[name];
            if (value > 0) {
                Input.ActionPress(action, value);
            } else {
                Input.ActionRelease(action);
            }
        }
    }

    private void _SendButtonEvent(string name, bool pressed) {
        if (ActionMap.ContainsKey(name)) {
            var action = ActionMap[name];
            if (pressed) {
                Input.ActionPress(action);
            } else {
                Input.ActionRelease(action);
            }
        }
    }
}

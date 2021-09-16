using Godot;

public class VirtualJoystick : MarginContainer
{
    [Signal]
    public delegate void Moved(Vector3 movement);
    [Signal]
    public delegate void Touched();
    [Signal]
    public delegate void Released();

    [Export]
    public float InitialOpacity = 0.5f;
    [Export]
    public float TouchedOpacity = 1.0f;

    public float Deadzone = 0.3f;

    private TextureRect _Base;
    private TextureRect _Head;
    private int _JoystickTouchIndex = -1;

    public override void _Ready()
    {
        _Base = GetNode<TextureRect>("Base");
        _Head = GetNode<TextureRect>("Head");

        _Base.Modulate = Colors.White.WithAlphaf(InitialOpacity);
        _Head.Modulate = Colors.White.WithAlphaf(InitialOpacity);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touchEvent) {
            if (!touchEvent.Pressed && touchEvent.Index == _JoystickTouchIndex) {
                _JoystickTouchIndex = -1;
                _Head.RectPosition = Vector2.Zero;
                _Base.Modulate = Colors.White.WithAlphaf(InitialOpacity);
                _Head.Modulate = Colors.White.WithAlphaf(InitialOpacity);
                EmitSignal(nameof(Released));
            }

            else if (_JoystickTouchIndex == -1 && touchEvent.Pressed && _Base.GetGlobalRect().HasPoint(touchEvent.Position)) {
                _JoystickTouchIndex = touchEvent.Index;
                _Base.Modulate = Colors.White.WithAlphaf(TouchedOpacity);
                _Head.Modulate = Colors.White.WithAlphaf(TouchedOpacity);
                EmitSignal(nameof(Touched));
            }
        }

        else if (@event is InputEventScreenDrag dragEvent) {
            var baseRectDrag = _Base.GetGlobalRect().Grow(2);

            if (_JoystickTouchIndex != -1 && baseRectDrag.HasPoint(dragEvent.Position)) {
                var basePosition = _Base.RectGlobalPosition + _Base.RectSize / 2;
                var mouseBaseVec = dragEvent.Position - basePosition;
                var force = mouseBaseVec / (_Base.RectSize / 2);

                // Move head
                _Head.RectPosition = (_Base.RectSize / 2 - _Head.RectSize / 2) + (force * _Base.RectSize / 2);

                if (Mathf.Abs(force.x) < Deadzone) {
                    force.x = 0;
                }

                if (Mathf.Abs(force.y) < Deadzone) {
                    force.y = 0;
                }

                EmitSignal(nameof(Moved), force);
            }
        }
    }
}

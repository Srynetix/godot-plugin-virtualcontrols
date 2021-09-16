using Godot;

[Tool]
public class VirtualButton : MarginContainer
{
    [Signal]
    public delegate void Touched();
    [Signal]
    public delegate void Released();

    [Export]
    public Texture ButtonTexture {
        get => _GetTexture();
        set => _SetTexture(value);
    }

    [Export]
    public float InitialOpacity = 0.5f;
    [Export]
    public float TouchedOpacity = 1.0f;

    private Texture _ButtonTexture;
    private Texture _DefaultTexture;
    private TextureRect _TextureRect;
    private int _ButtonTouchIndex = -1;

    public override void _Ready()
    {
        _TextureRect = GetNode<TextureRect>("TextureRect");
        _DefaultTexture = _TextureRect.Texture;

        if (_ButtonTexture != null) {
            _TextureRect.Texture = _ButtonTexture;
        }

        _TextureRect.Modulate = Colors.White.WithAlphaf(InitialOpacity);
    }

    private void _SetTexture(Texture tex) {
        _ButtonTexture = tex;

        if (_TextureRect != null) {
            if (_ButtonTexture == null) {
                _TextureRect.Texture = _DefaultTexture;
            } else {
                _TextureRect.Texture = _ButtonTexture;
            }
        }
    }

    public Texture _GetTexture() {
        return _ButtonTexture;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touchEvent) {
            if (!touchEvent.Pressed && touchEvent.Index == _ButtonTouchIndex) {
                _ButtonTouchIndex = -1;
                _TextureRect.Modulate = Colors.White.WithAlphaf(InitialOpacity);
                EmitSignal(nameof(Released));
            }

            else if (_ButtonTouchIndex == -1 && touchEvent.Pressed && _TextureRect.GetGlobalRect().HasPoint(touchEvent.Position)) {
                _ButtonTouchIndex = touchEvent.Index;
                _TextureRect.Modulate = Colors.White.WithAlphaf(TouchedOpacity);
                EmitSignal(nameof(Touched));
            }
        }
    }
}

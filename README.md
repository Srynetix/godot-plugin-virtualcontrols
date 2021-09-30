# Simple Virtual Controls for Godot Engine (3.3.3, C#)

Contains a simple virtual controls interface with one joystick and three buttons: A, B and X.

## How to install

Use `git` submodules: open a command prompt in your project folder and then:

```
git submodule add https://github.com/Srynetix/godot-plugin-virtualcontrols addons/virtualcontrols
```

This plugin depends on [godot-plugin-nodeext](https://github.com/Srynetix/godot-plugin-nodeext).

## How to use

- Simply spawn the `VirtualControls` node, trigger visibility status with the `Visible` property.  
- Map joystick movements and key presses with the `ActionMap` property.

Available keys:
- `left`: Joystick left movement
- `right`: Joystick right movement
- `up`: Joystick up movement
- `down`: Joystick down movement
- `a`: 'A' button
- `b`: 'B' button
- `x`: 'X' button

_Example_:

```cs
VirtualControls vc = GetNode<VirtualControls>("VirtualControls");
vc.ActionMap["left"] = "move_left";
vc.ActionMap["a"] = "jump";
```

## Licenses

- Ostrich Sans - SIL Open Font License - https://github.com/theleagueof/ostrich-sans
- On-Screen Controls - CC0 1.0 Universal - https://www.kenney.nl/assets/onscreen-controls
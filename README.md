# Money Pop (Unity Edition)

A Unity-based reimplementation of the Money Pop prototype. The project keeps the
same learning goals—helping kids practise counting cash and making change—but all
runtime code now targets the Unity engine.

## Getting Started

1. Install **Unity 2021 LTS or newer** (the project only uses built-in UI
   components, so any current LTS release works).
2. Clone this repository and open it from the Unity Hub.
3. Ensure the scene that contains the `MainGameController`, `MoneyHUD`,
   `CashierDropZone`, and a `GameState` object is loaded (you can wire them up in
   any layout that suits your platform).
4. Enter Play mode to try the drag-and-drop denomination splitting gameplay.

### Scene Wiring Overview

The original Godot scenes cannot be imported, so you will need to rebuild the UI
layout inside Unity. Use the following guide when constructing your own scene:

- Add a **Canvas** configured for `Screen Space - Overlay`. The canvas will host
  two panels: a play area (`RectTransform`) for coins/notes and a cashier drop
  zone.
- Attach the **MoneyHUD** script to a HUD panel and assign the required
  references (cashier/board labels, start panel, etc.). The `StartGameRequested`
  UnityEvent should invoke `MainGameController.OnStartGameRequested`.
- Place a **GameState** MonoBehaviour in the scene. This acts as the singleton
  equivalent of the old Godot autoload.
- Create a **MoneyPiece** prefab (a simple UI element with an Image background
  and Text label works well). Assign it to the `piecePrefab` slot on
  `MainGameController`.
- Add a **CashierDropZone** to the drop area so dragged `MoneyPiece` instances
  can be destroyed and tallied.

With those references configured, pressing the start button in the HUD will
spawn a money piece in the play area. Players can split pieces, drag them to the
cashier zone, and see running totals in the HUD—all powered by the Unity C#
components found under `Assets/Scripts`.

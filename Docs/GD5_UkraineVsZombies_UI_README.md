# GD5 UI - Ukraine vs Zombies

Branch: `labs/GD-5-novikov-adding_ui_to_ukraine_vs_zombie`

## Added UI elements

- Score text shows the number of destroyed enemies.
- Base HP text shows current base health.
- Base HP slider gives a visual health indicator.
- Game Over panel appears when base HP reaches zero.
- Restart button reloads the current scene.
- Pause menu supports `Resume`, `Restart`, and `Quit`.
- Main menu controller supports `Start Game`, `Credits`, `Back`, and `Quit`.

## UI controller classes

- `GameManager` updates score, base HP, health slider, restart logic, and game over panel.
- `PauseMenuUI` controls the in-game pause menu and pauses gameplay with `Time.timeScale`.
- `MainMenuUI` controls the main menu scene and loads the `UkraineVsZombies` scene.

## Unity setup

1. In `UkraineVsZombies`, create a `PausePanel` inside `Canvas`.
2. Add three buttons to `PausePanel`: `Resume`, `Restart`, `Quit`.
3. Add `PauseMenuUI` to any scene object and assign `PausePanel`.
4. Connect button events:
   - `Resume` -> `PauseMenuUI.Resume`
   - `Restart` -> `PauseMenuUI.Restart`
   - `Quit` -> `PauseMenuUI.Quit`
5. Create a `MainMenu` scene with a Canvas and buttons:
   - `Start` -> `MainMenuUI.StartGame`
   - `Credits` -> `MainMenuUI.ShowCredits`
   - `Back` -> `MainMenuUI.ShowMainPanel`
   - `Quit` -> `MainMenuUI.Quit`
6. Add `MainMenu` first in Build Settings, then `UkraineVsZombies`.

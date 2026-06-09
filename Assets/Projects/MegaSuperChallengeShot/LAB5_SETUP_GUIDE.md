# Lab 5 — Setup Guide (UI in Unity)

This guide walks through every Editor-side step needed to complete Lab 5 on top of Lab 4. All C# scripts are already written — you only wire UI in the Unity Editor.

## Branch sanity check

```bash
git branch --show-current
# expected: labs/GD-5-domin-adding_ui_to_chicen_hunt
```

## What we are building (3 deliverables)

| # | Deliverable | Scripts |
|---|-------------|---------|
| 1 | Main Menu scene (first scene shown) | `MainMenuController.cs` |
| 2 | In-game pause menu (ESC toggles) | `PauseMenuController.cs` |
| 3 | HUD already exists from Lab 3 (Score / HP / Shots) | — |

`GameManager.cs` got new methods: `PauseGame()`, `ResumeGame()`, `TogglePause()`, `LoadMainMenu()`, `QuitGame()`. A new `Paused` state was added to the `GameState` enum.

---

## 1. Main Menu scene

### 1.1 Create the scene file

1. **File → New Scene**.
2. Pick **Basic 2D** template → Create.
3. **File → Save As…** → save into `Assets/Projects/MegaSuperChallengeShot/Scenes/` → name `MainMenu` → Save.

### 1.2 Build the UI

1. In the new scene's **Hierarchy**, right-click → **UI → Canvas**.
2. The Canvas auto-creates with an **EventSystem** (needed for buttons).
3. With Canvas selected, in Inspector → Canvas Scaler → set **UI Scale Mode = Scale With Screen Size** → Reference Resolution `1920 x 1080`.

### 1.3 Add background (optional but nice)

1. Right-click Canvas → **UI → Image** → name `Background`.
2. In RectTransform → set anchors to **Stretch all** (click anchor presets icon, hold Alt+Shift, click bottom-right stretch).
3. Set Left/Right/Top/Bottom all to `0` so it fills the screen.
4. Set Color to a dark gradient or solid color of your choice.

### 1.4 Add title text

1. Right-click Canvas → **UI → Text - TextMeshPro** (if prompted, click **Import TMP Essentials**).
2. Rename to `TitleText`.
3. RectTransform: position `(0, 250, 0)`, width `800`, height `120`.
4. TMP Inspector: Text = `CHICKEN HUNT`, Font Size = `90`, Alignment = Center, Color = bright (white or yellow).

### 1.5 Add Start button

1. Right-click Canvas → **UI → Button - TextMeshPro** → rename `StartButton`.
2. RectTransform: position `(0, 0, 0)`, width `300`, height `80`.
3. Expand StartButton → click the child **Text (TMP)** → change Text to `START`.

### 1.6 Add Quit button

1. Right-click Canvas → **UI → Button - TextMeshPro** → rename `QuitButton`.
2. RectTransform: position `(0, -120, 0)`, width `300`, height `80`.
3. Child Text → set to `QUIT`.

### 1.7 Attach the controller

1. Hierarchy → right-click empty area → **Create Empty** → name `MainMenuController`.
2. Inspector → **Add Component** → search "Main Menu Controller" → click.
3. Drag `StartButton` from Hierarchy into the script's `Start Button` field.
4. Drag `QuitButton` into `Quit Button` field.
5. `Game Scene Name` field should already say `lab3` (the existing game scene). If your game scene is named differently, change this to match — exact name, case-sensitive.

### 1.8 Save scene

**Cmd+S**.

---

## 2. Add MainMenu to Build Settings

This is required so the game scene's "Quit to Menu" button knows where to go.

1. **File → Build Settings…**
2. With the MainMenu scene currently open, click **Add Open Scenes**. It appears in the list.
3. Open the game scene (`lab3.unity`) and click **Add Open Scenes** again.
4. **Drag the `MainMenu` row to the top** (index 0). The top scene is what loads first when the game starts.
5. Close the Build Settings window.

---

## 3. In-game Pause Menu

### 3.1 Open the game scene

Open `Assets/Scenes/lab3.unity` (your main Lab 3/4 scene).

### 3.2 Build the pause panel

1. Find your existing **Canvas** in Hierarchy.
2. Right-click Canvas → **UI → Panel** → rename `PausePanel`.
3. The Panel auto-stretches to fill the canvas. In Inspector → Image → Color → set alpha to ~`180` (semi-transparent dark) so it dims the game behind.

### 3.3 Add the pause label

1. Right-click PausePanel → **UI → Text - TextMeshPro** → rename `PauseLabel`.
2. RectTransform: position `(0, 250, 0)`, width `600`, height `100`.
3. Text: `PAUSED`, Font Size `80`, Center alignment, white.

### 3.4 Add three buttons

For each button: right-click PausePanel → **UI → Button - TextMeshPro**, rename, position, set child Text.

| Button name | Position Y | Child Text |
|-------------|-----------|------------|
| `ResumeButton` | `100` | RESUME |
| `RestartButton` | `0` | RESTART |
| `QuitButton` | `-100` | QUIT TO MENU |

Each button: width `300`, height `80`, X position `0`.

### 3.5 Attach the pause controller

1. Hierarchy → right-click → **Create Empty** → name `PauseMenuController`.
2. **Add Component** → **Pause Menu Controller**.
3. Wire the fields by dragging from Hierarchy:
   - `Panel` ← drag `PausePanel`
   - `Resume Button` ← drag `ResumeButton`
   - `Restart Button` ← drag `RestartButton`
   - `Quit Button` ← drag `QuitButton`
   - `Toggle Key` leave as `Escape`

### 3.6 Wire GameManager's Main Menu scene name

1. Hierarchy → click `GameManager` GameObject.
2. Inspector → Game Manager component → find the new `Main Menu Scene Name` field.
3. Set value to `MainMenu` (exact name of the scene file you created in step 1.1, no .unity extension).

### 3.7 Save scene

**Cmd+S**.

---

## 4. Test plan (Play mode)

### From Main Menu

1. Open MainMenu scene → press **Play** (or run the build — main menu loads first because it's at index 0 in Build Settings).
2. Click **START** → game scene loads, you can play.
3. Press **Quit** in main menu → game stops (in editor) or app closes (in build).

### In-game pause

4. While playing → press **ESC** → game freezes (Time.timeScale = 0), pause panel appears.
5. Click **RESUME** → game continues from where it was.
6. Press **ESC** again → toggle pause off.
7. Press **ESC** to pause → click **RESTART** → scene reloads fresh.
8. Press **ESC** to pause → click **QUIT TO MENU** → MainMenu scene loads.

### Game over

9. Let HP drop to 0 → Game Over panel fades in (Lab 4 work).
10. Click anywhere on GameOver panel → restarts (Lab 3 behaviour, still works).

---

## 5. Final scene wiring checklist

### MainMenu scene
- [ ] Canvas + EventSystem present
- [ ] StartButton + QuitButton present
- [ ] MainMenuController GameObject has script attached
- [ ] StartButton + QuitButton dragged into script fields
- [ ] `Game Scene Name` = `lab3`
- [ ] Scene added to Build Settings at index 0

### lab3 scene (game scene)
- [ ] PausePanel created under Canvas, initially active (script hides it on Awake)
- [ ] ResumeButton, RestartButton, QuitButton all under PausePanel
- [ ] PauseMenuController GameObject has script attached
- [ ] All 4 references (Panel, 3 Buttons) wired in PauseMenuController
- [ ] GameManager's `Main Menu Scene Name` = `MainMenu`
- [ ] Scene added to Build Settings at index 1

---

## 6. Commit + push

```bash
git status
git add Assets/Projects/MegaSuperChallengeShot Assets/Scenes ProjectSettings/EditorBuildSettings.asset
git commit -m "GD-5: Add main menu, pause menu, and UI controllers"
git push -u origin labs/GD-5-domin-adding_ui_to_chicen_hunt
```

## 7. Create the pull request

After push, GitHub shows the branch with a "Compare & pull request" button:

1. **Base branch:** `labs/GD-4-domin-adding_animation_to_chicken_hunt` (branch you diverged from)
2. **Compare branch:** `labs/GD-5-domin-adding_ui_to_chicen_hunt`
3. Title: `GD-5: Add UI to Chicken Hunt`
4. Click **Create pull request**.

---

## What the lab graders look for

| Requirement | How it's met |
|-------------|--------------|
| HUD indicators (Score, HP, Shots) | Already in HUD from Lab 3 |
| In-game menu with Restart/Resume/Quit | PausePanel + PauseMenuController |
| Main menu scene with Start | MainMenu scene + MainMenuController |
| Code-driven UI control | `PauseMenuController` + `MainMenuController` + `GameManager` pause logic |
| Branch + PR to Lab 4 branch | Step 7 above |

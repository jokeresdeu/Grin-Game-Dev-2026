# Lab 6 — CubeHopper Editor Setup Guide (Friendly version)

This is a step-by-step guide for someone new to Unity. We'll build two scenes — `CubeHopper_MainMenu` and `CubeHopper_Game`. After every step there's a "**You should see:**" line so you can check you're on track.

> Unity version: **Unity 6 (6000.0.x)**
> All C# scripts are already written for you in `Assets/Projects/CubeHopper/Scripts/`. You just need to wire them up in the editor.

---

# Part 0 — One-time project setup

Do these once. They affect the whole project, not just one scene.

## 0.1 Open the project

1. Open Unity Hub.
2. Open the `Grin-Game-Dev-2026` project.
3. Wait for Unity to finish loading (you'll see a progress bar at the bottom).
4. Look at the bottom of the Unity window — there should be **no red errors** in the Console. Yellow warnings are OK.

**You should see:** Unity opened with the project, Console clean.

## 0.2 Add Tags

Tags help us identify "this is an obstacle" or "this is the player" in code.

1. Top menu: **Edit → Project Settings**.
2. In the left sidebar of the Project Settings window, click **Tags and Layers**.
3. Expand the **Tags** section (small triangle on the left).
4. Click the **`+`** button to add a new tag.
5. Type `Player` and press Enter.
6. Click `+` again, type `Obstacle`, press Enter.

**You should see:** Two new tags `Player` and `Obstacle` in the list.

> ⚠️ If the tag already exists, Unity will complain. That's fine — just skip it.

## 0.3 Add Layers

Layers tell the physics engine "this object is the ground; the player should collide with it".

1. Still in **Tags and Layers**, expand the **Layers** section.
2. Find the empty slot **`User Layer 8`** — click in the empty box on the right side and type `Ground`.
3. Find **`User Layer 9`** — type `Obstacle`.

**You should see:**
- `User Layer 8: Ground`
- `User Layer 9: Obstacle`

> ⚠️ **If you see "Ground already registered" error** — you typed the name twice. Scroll through all layers, find duplicate, clear it (delete the text from the box).

## 0.4 Make sure Layers can collide

1. Project Settings → **Physics 2D** (left sidebar).
2. Scroll down to **Layer Collision Matrix** (a grid of checkboxes).
3. Make sure the cell where `Default` row meets `Ground` column is **checked**.
4. Make sure `Default` row meets `Obstacle` column is **checked**.

**You should see:** Both cells checked.

> By default Unity has all cells checked, so most likely you don't need to change anything. Just verify.

## 0.5 Close Project Settings

Click the X on the Project Settings window. We're done with global setup.

---

# Part 1 — Prepare a Sprite

We need one simple white square sprite — we'll re-use it as the player cube, ground tiles, and life icons (by tinting).

## 1.1 Create the Square sprite

1. In the **Project** panel (bottom of Unity), navigate to `Assets/Projects/CubeHopper/Sprites/`.
2. Right-click in the empty area on the right.
3. Choose **Create → 2D → Sprites → Square**.
4. Unity will create a file called `Square.png` (or `Square` depending on version).

**You should see:** A small white square icon in your Sprites folder.

## 1.2 Configure the Square sprite

This is important — the import settings need to be right.

1. Click on the `Square` sprite once to select it.
2. Look at the **Inspector** on the right side.
3. Set **Sprite Mode** to `Single` (NOT `Multiple` — Multiple is for spritesheets).
4. Leave **Pixels Per Unit** at `256` (or whatever default — doesn't matter much for a flat square).
5. Click **Apply** at the bottom-right of the Inspector if it asks you to.

**You should see:** Sprite Mode = Single, Apply button is greyed out (because changes are saved).

> 💡 If your version of Unity doesn't have **Create → 2D → Sprites → Square**:
> - Alternative 1: Right-click → Create → Sprites → Square (some Unity versions)
> - Alternative 2: Download or create a tiny 32×32 white PNG in any image editor and drag it into the Sprites folder.

## 1.3 Triangle sprite for spikes

Same as Square but pick **Triangle**:

1. Right-click in `Assets/Projects/CubeHopper/Sprites/`.
2. **Create → 2D → Sprites → Triangle**.
3. Select it, set Sprite Mode = `Single`, Apply.

**You should see:** Both `Square` and `Triangle` in your Sprites folder.

---

# Part 2 — Build the MainMenu scene

This is the simpler scene. We'll do this first because you're already here.

## 2.1 Make sure you're in the right scene

1. Look at the top of the Unity window — the title bar should say something like **`CubeHopper_MainMenu - ...`**.
2. If not, in the Project panel go to `Assets/Projects/CubeHopper/Scenes/` and double-click `CubeHopper_MainMenu`.
3. If the scene doesn't exist yet:
   - File → New Scene → choose **2D** template → click Create.
   - File → Save As → navigate to `Assets/Projects/CubeHopper/Scenes/` → name it `CubeHopper_MainMenu` → Save.

**You should see:** Title bar shows `CubeHopper_MainMenu`. Hierarchy panel (left side) shows a `Main Camera` (and maybe nothing else, or a Directional Light).

## 2.2 Configure the Main Camera

1. In Hierarchy, click `Main Camera`.
2. Look at the Inspector on the right.
3. Find **Transform** at the top:
   - Position: `0, 0, -10`
4. Find **Camera** component below:
   - **Projection:** `Orthographic`
   - **Size:** `5`
   - **Background:** click the color box, pick a dark color. Use hex code `#0F1A2D` (paste it in the Hex field).

**You should see:** Game view (the "Game" tab) shows a dark blue/navy color.

## 2.3 Create the Canvas (UI container)

1. In Hierarchy, right-click in empty area.
2. **UI → Canvas**.
3. Unity creates a `Canvas` and an `EventSystem`.
4. Click on the new `Canvas` in Hierarchy.
5. In Inspector, find **Canvas Scaler** component:
   - **UI Scale Mode:** `Scale With Screen Size`
   - **Reference Resolution:** `1920` × `1080`
   - **Match:** drag the slider to `0.5` (middle)

**You should see:** A huge rectangle appears in the Scene view — that's the Canvas.

## 2.4 Add Title text

1. Right-click on `Canvas` in Hierarchy.
2. **UI → Text - TextMeshPro**.
3. If Unity asks to "Import TMP Essentials" — click **Import TMP Essentials**, wait, close the popup.
4. Unity creates a `Text (TMP)` child under Canvas.
5. Rename it: select it, press F2 (or Enter on Mac), type `Title`, press Enter.
6. In Inspector, with `Title` selected:
   - **Rect Transform** → anchor: click the anchor preset square, pick **middle-center** (the one in the middle of the grid). Hold Alt and click to also center the position.
   - **Pos X:** `0`, **Pos Y:** `200`, **Width:** `1200`, **Height:** `200`.
   - **TextMeshPro - Text (UI)** component:
     - **Text Input:** type `CUBEHOPPER`
     - **Font Size:** `144`
     - **Alignment:** Center, Middle
     - **Color:** White

**You should see:** Big white "CUBEHOPPER" text in the middle-top of the Game view.

## 2.5 Add BestScoreLabel

1. Right-click `Canvas` → **UI → Text - TextMeshPro**.
2. Rename to `BestScoreLabel`.
3. Inspector:
   - Anchor: middle-center
   - **Pos X:** `0`, **Pos Y:** `0`, **Width:** `800`, **Height:** `80`
   - **Text:** `Best: 0`
   - **Font Size:** `48`
   - **Alignment:** Center, Middle
   - **Color:** light gray (e.g., `#9CA3AF`)

**You should see:** "Best: 0" below the title.

## 2.6 Add PromptLabel

1. Right-click `Canvas` → **UI → Text - TextMeshPro**.
2. Rename to `PromptLabel`.
3. Inspector:
   - Anchor: middle-center
   - **Pos X:** `0`, **Pos Y:** `-150`, **Width:** `800`, **Height:** `100`
   - **Text:** `tap to start`
   - **Font Size:** `56`
   - **Alignment:** Center, Middle
   - **Color:** White

**You should see:** "tap to start" below the best score.

## 2.7 Add MainMenuController

1. In Hierarchy, right-click in empty area → **Create Empty**.
2. Rename it `MainMenuController`.
3. With it selected, in Inspector click **Add Component**.
4. Type `MainMenuController` in the search box.
5. Click on the `MainMenuController` script when it appears.

Now configure it:

6. In Inspector, find the script fields:
   - **Game Scene Name:** `CubeHopper_Game` (just type this string)
   - **Input Delay:** `0.25`
   - **Quit Key:** `Escape` (default)
   - **Best Score Label:** drag the `BestScoreLabel` from Hierarchy into this field
   - **Prompt Label:** drag the `PromptLabel` from Hierarchy into this field
   - **Prompt Pulse Speed:** `2.4`
   - **Prompt Alpha Min:** `0.45`
   - **Prompt Alpha Max:** `1`

**You should see:** All fields filled in the Inspector for `MainMenuController`.

## 2.8 Save the scene

**File → Save** (or `Cmd+S` on Mac).

**You should see:** Title bar no longer has `*` (asterisk means unsaved).

---

# Part 3 — Build the Game scene

This is the bigger scene. Take it step by step.

## 3.1 Create the scene

1. **File → New Scene**.
2. Pick **2D (Built-in Renderer)** or **2D (URP)** template.
3. Click **Create**.
4. **File → Save As** → navigate to `Assets/Projects/CubeHopper/Scenes/` → name `CubeHopper_Game` → Save.

**You should see:** Title bar = `CubeHopper_Game`.

## 3.2 Configure Main Camera

Same as before:

1. Click `Main Camera`.
2. Position: `0, 0.5, -10`
3. Projection: `Orthographic`
4. Size: `5`
5. Background: `#0F1A2D`

Add CameraShake component:

6. Click **Add Component** in Inspector.
7. Search for `CameraShake`, click it.
8. Set values: Duration `0.25`, Magnitude `0.18`, Frequency `28`.

Add BackgroundColorShifter:

9. Click **Add Component** → search `BackgroundColorShifter` → add.
10. **Camera:** drag `Main Camera` from Hierarchy into this field (or leave empty, the script will auto-find it).
11. **Gradient:** click on the gradient bar to open the gradient editor:
    - At position 0.0 (left edge), click and set color to `#0F1A2D`
    - Click in the middle to add a new key at position ~0.5, color `#1A2030`
    - At position 1.0 (right edge), set color to `#3D1A2D`
    - Close the gradient window.
12. **Smoothing:** `1.5`

**You should see:** Camera has 3 components: Camera, CameraShake, BackgroundColorShifter.

## 3.3 Create the Managers GameObject

1. Hierarchy → right-click → **Create Empty**.
2. Rename `Managers`.
3. Position: `0, 0, 0`.

Add three components by clicking **Add Component** for each:

4. **GameManager**:
   - Main Menu Scene Name: `CubeHopper_MainMenu`
   - Game Scene Name: `CubeHopper_Game`
   - Game Over Input Lock Seconds: `0.5`
5. **WorldSpeed** (leave defaults)
6. **ScoreManager** (leave defaults)

**You should see:** `Managers` GameObject with 3 script components.

> 💡 **About the scene name fields:** These are just **strings** (text). Type the exact scene filename (without `.unity` extension). The scenes must also be added to **Build Settings** later — we'll do that at the end.

## 3.4 Build the Ground

The ground is 4 tiles that recycle to look infinite.

### Create the parent

1. Hierarchy → right-click → **Create Empty**.
2. Rename `Ground`.
3. Position: `0, -3.5, 0`.

### Create the first tile

4. Right-click on `Ground` → **2D Object → Sprites → Square** (or just **Create Empty** and add components).

If you used "2D Object → Sprites → Square":
   - Unity creates a child with SpriteRenderer already configured.

If you used "Create Empty":
   - Click **Add Component** → search `Sprite Renderer` → add.
   - Drag the `Square` sprite from Sprites folder into the **Sprite** field.

5. Rename the child to `Tile_0`.
6. Set Transform:
   - Position: `-8, 0, 0`
   - Scale: `8, 1.5, 1`
7. In Sprite Renderer, set **Color** to `#3C4858` (dark blue-gray).
8. Add Component → **Box Collider 2D** (it auto-fits the sprite).
9. At the top of the Inspector, change **Layer** to `Ground` (the dropdown next to "Tag").

**You should see:** A wide dark gray bar in the Scene view at y=-3.5.

### Duplicate to make 3 more tiles

10. Right-click `Tile_0` in Hierarchy → **Duplicate** (or `Cmd+D`).
11. Rename to `Tile_1`. Position: `0, 0, 0`.
12. Duplicate again → `Tile_2`. Position: `8, 0, 0`.
13. Duplicate again → `Tile_3`. Position: `16, 0, 0`.

**You should see:** 4 tiles side by side forming a long ground.

### Add the GroundScroller script

14. Click `Ground` (the parent).
15. Add Component → `GroundScroller`.
16. **Tiles:** expand the list, set size to `4`, drag each `Tile_0/1/2/3` from Hierarchy into the slots **in order**.
17. **Tile Width:** `8`
18. **Recycle Threshold:** `-12`

**You should see:** Ground component is set up. When you press Play later, tiles will scroll left.

## 3.5 Create the Player

### Make the parent

1. Hierarchy → right-click → **Create Empty**.
2. Rename `Player`. Position: `-4, -1.5, 0`.
3. At the top of Inspector, set **Tag** to `Player`.

### Make the Visual child (the yellow square you see)

4. Right-click `Player` → **Create Empty**, rename to `Visual`.
5. Position (local): `0, 0, 0`. Scale: `0.7, 0.7, 1`.
6. Add Component → **Sprite Renderer**.
7. Drag `Square` sprite into the Sprite field.
8. Color: `#FFD24C` (yellow).

**You should see:** A yellow square in the scene where the player is.

### Add the physics components to Player (not Visual)

9. Click `Player` (the parent).
10. Add Component → **Rigidbody 2D**:
    - Body Type: `Dynamic`
    - Gravity Scale: `3`
    - Collision Detection: `Continuous`
    - Constraints: check **Freeze Rotation Z**.
11. Add Component → **Box Collider 2D**:
    - It will auto-fit. If the size looks wrong, click **Edit Collider** and adjust to match the yellow square (about 0.7 × 0.7).

### Add the GroundCheck child

12. Right-click `Player` → **Create Empty**, rename `GroundCheck`.
13. Position (local): `0, -0.4, 0`.

This is just a position marker — it's the point we check "is the ground below me?".

### Add the gameplay scripts

14. Click `Player` again.
15. Add Component → **PlayerHealth**:
    - Max Lives: `3`
    - Invincibility Seconds: `1.2`
    - **Renderer:** drag `Visual` from Hierarchy → look for the SpriteRenderer field (Unity will auto-pick the SpriteRenderer from Visual).
    - Flash Frequency: `12`
16. Add Component → **PlayerController**:
    - Jump Velocity: `11`
    - Fall Multiplier: `2.4`
    - Low Jump Multiplier: `1.6`
    - Coyote Seconds: `0.08`
    - Jump Buffer Seconds: `0.12`
    - **Ground Check:** drag `GroundCheck` from Hierarchy.
    - Ground Check Radius: `0.18`
    - **Ground Layers:** click the dropdown, **uncheck Everything**, then check only `Ground`.
    - Rotation Speed: `540`
    - **Health:** drag the `Player`'s PlayerHealth component (or it auto-finds).
17. Add Component → **PlayerCollision**:
    - **Health:** drag PlayerHealth (auto-finds).
    - Obstacle Tag: `Obstacle`
    - **Camera Shake:** drag `Main Camera` from Hierarchy.
    - Knockback Impulse: `6`
    - **Rigidbody:** drag the Rigidbody 2D from Player (auto-finds).

**You should see:** Player has 5 components: Rigidbody 2D, Box Collider 2D, PlayerHealth, PlayerController, PlayerCollision. Plus the Visual and GroundCheck children.

## 3.6 Make the Obstacle prefab

### Build the obstacle in the scene first

1. Hierarchy → right-click → **Create Empty**, rename `Obstacle_Spike`.
2. Position: `5, -2.8, 0` (we'll fine-tune later).
3. Add Component → **Sprite Renderer**:
   - Drag `Triangle` sprite into Sprite field.
   - Color: `#FF4D6A` (red-pink).
4. Set Transform Scale: `1, 1.2, 1`.
5. Add Component → **Polygon Collider 2D** (it auto-fits to the triangle).
6. **Tag:** `Obstacle`.
7. **Layer:** `Obstacle`.
8. Add Component → **ObstacleMover**:
   - Despawn X: `-15`

**You should see:** A red-pink triangle on the ground.

### Save as prefab

9. Drag the `Obstacle_Spike` from Hierarchy into the **Prefabs** folder in Project panel (`Assets/Projects/CubeHopper/Prefabs/`).
10. After Unity creates the prefab, **delete `Obstacle_Spike` from the Hierarchy** (right-click → Delete). We'll spawn instances at runtime.

### (Optional) Make a DoubleSpike variant

11. Drag the `Obstacle_Spike` prefab from Prefabs folder back into the scene.
12. Rename to `Obstacle_DoubleSpike`.
13. Inside it, duplicate the sprite or add a second triangle next to the first. Make it slightly taller.
14. Drag back to Prefabs folder. Delete from scene.

## 3.7 Obstacle Spawner

1. Hierarchy → right-click → **Create Empty**, rename `ObstacleSpawner`.
2. Position: `12, -2.6, 0`.
3. Right-click `ObstacleSpawner` → **Create Empty**, rename `SpawnPoint`. Local Position: `0, 0, 0`.
4. Click `ObstacleSpawner` again.
5. Add Component → **ObstacleSpawner**.
6. Configure:
   - **Entries:** expand, set Size to `1` (or `2` if you made DoubleSpike).
     - Element 0:
       - Prefab: drag `Obstacle_Spike` prefab from Prefabs folder.
       - Weight: `1.5`
       - Min Spacing: `3`
     - Element 1 (if you have it):
       - Prefab: drag `Obstacle_DoubleSpike`.
       - Weight: `0.6`
       - Min Spacing: `4`
   - **Spawn Point:** drag the `SpawnPoint` child.
   - Min Spawn Interval: `1.1`
   - Max Spawn Interval: `2.4`
   - Difficulty Curve Seconds: `45`
   - Min Interval At Max Difficulty: `0.55`
   - Max Interval At Max Difficulty: `1.1`
   - Max Concurrent Obstacles: `6`

**You should see:** ObstacleSpawner GameObject with all fields filled in.

## 3.8 HUD Canvas (the game's on-screen score/lives)

1. Hierarchy → right-click → **UI → Canvas**.
2. Rename to `HUD_Canvas`.
3. Canvas Scaler (already on it):
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920 × 1080
   - Match: 0.5

### Score label

4. Right-click `HUD_Canvas` → **UI → Text - TextMeshPro**. Rename `ScoreLabel`.
5. Inspector:
   - Anchor preset: top-center (top row, middle column)
   - Pos X: `0`, Pos Y: `-80`, Width: `800`, Height: `100`
   - Text: `000000`
   - Font Size: `64`
   - Alignment: Center
   - Color: White

### Best score label

6. Right-click `HUD_Canvas` → **UI → Text - TextMeshPro**. Rename `BestScoreLabel`.
7. Inspector:
   - Anchor preset: top-right
   - Pos X: `-200`, Pos Y: `-50`, Width: `400`, Height: `60`
   - Text: `Best 000000`
   - Font Size: `28`
   - Alignment: Right
   - Color: `#9CA3AF`

### Combo label

8. Right-click `HUD_Canvas` → **UI → Text - TextMeshPro**. Rename `ComboLabel`.
9. Inspector:
   - Anchor preset: top-center
   - Pos X: `0`, Pos Y: `-180`, Width: `400`, Height: `80`
   - Text: `x2`
   - Font Size: `44`
   - Alignment: Center
   - Color: `#FFD24C`
10. **Click the checkbox at the very top of Inspector** (next to the name `ComboLabel`) to **disable** the GameObject by default. The script will enable it when combo > 1.

### Lives icons

11. Right-click `HUD_Canvas` → **Create Empty**. Rename `Lives`.
12. Inspector:
    - Anchor: top-left
    - Pos X: `100`, Pos Y: `-60`, Width: `200`, Height: `64`
13. Add Component → **Horizontal Layout Group**:
    - Spacing: `8`
    - Child Alignment: Middle Left
    - Control Child Size Width/Height: Unchecked
    - Use Child Scale: Unchecked
    - Child Force Expand: Unchecked
14. Right-click `Lives` → **UI → Image**. Rename `Life_0`.
    - Source Image: drag the `Square` sprite.
    - Color: `#FF4D6A`
    - Width: `48`, Height: `48`
15. Duplicate (`Cmd+D`) twice → `Life_1`, `Life_2`.

**You should see:** 3 small red squares in the top-left of the Game view.

### HUDController script

16. Click `HUD_Canvas`.
17. Add Component → **HUDController**.
18. Fill in:
    - Score Label: drag `ScoreLabel`
    - Best Score Label: drag `BestScoreLabel`
    - Combo Label: drag `ComboLabel`
    - Life Icons: expand, size = 3, drag `Life_0`, `Life_1`, `Life_2` in order.
    - Life Active Color: White, alpha 1.
    - Life Lost Color: White, alpha 0.2.
    - Health: drag `Player` from Hierarchy (it'll auto-pick the PlayerHealth).

## 3.9 Pause Canvas

1. Hierarchy → right-click → **UI → Canvas**. Rename `Pause_Canvas`.
2. Inspector: **Sort Order:** `5`.

### Dim background panel

3. Right-click `Pause_Canvas` → **UI → Panel**. Rename `Panel`.
4. Image color: black, alpha 0.65 (`0, 0, 0, 165` in RGBA 0-255).
5. The Panel should automatically stretch to fill the screen.

### Title and buttons

6. Right-click `Panel` → **UI → Text - TextMeshPro**. Rename `Title`.
   - Anchor: top-center, Pos Y: `-150`, Width: `800`, Height: `200`
   - Text: `PAUSED`, Font Size: `96`, Color: White, Center alignment.
7. Right-click `Panel` → **UI → Button - TextMeshPro**. Rename `ResumeButton`.
   - Anchor: middle-center, Pos Y: `100`, Width: `300`, Height: `80`
   - On the button's child Text: change to `Resume`, Font Size `36`.
8. Duplicate 3 times:
   - `RestartButton` at Pos Y `0`, text `Restart`
   - `MainMenuButton` at Pos Y `-100`, text `Main Menu`
   - `QuitButton` at Pos Y `-200`, text `Quit`

### Wire up the script

9. Click `Pause_Canvas`.
10. Add Component → **PauseMenuController**:
    - **Panel:** drag the `Panel` child (the dim image, not the Canvas).
    - Toggle Key: `Escape`.
11. For each button, click it, scroll to **Button → On Click ()**, click `+`:
    - Drag `Pause_Canvas` into the object field.
    - In the function dropdown: `PauseMenuController` → `OnResumeClicked()` (for Resume button), or the matching method for others.

> 💡 The Pause_Canvas stays active in the scene. The `Panel` child gets enabled/disabled by the script based on game state.

12. **Important:** start with the `Panel` **disabled** — click `Panel`, uncheck the box at the top of the Inspector.

## 3.10 GameOver Canvas

1. Hierarchy → right-click → **UI → Canvas**. Rename `GameOver_Canvas`.
2. **Sort Order:** `10`.

### Panel

3. Right-click → **UI → Panel**. Rename `Panel`. Color: black, alpha 0.85.
4. **Disable the Panel** (uncheck top of Inspector).

### Labels

5. Right-click `Panel` → **UI → Text - TextMeshPro** × 4, all anchored middle-center:
   - `Title` — text `GAME OVER`, font 120, color `#FF4D6A`, Pos Y `200`.
   - `ScoreLabel` — text `Score: 0`, font 48, white, Pos Y `60`.
   - `BestScoreLabel` — text `Best: 0`, font 36, gray, Pos Y `0`.
   - `NewBestBadge` — text `★ NEW BEST! ★`, font 42, color `#FFD24C`, Pos Y `-60`. **Disable** this one (we'll enable from script).

### Buttons

6. Add 3 buttons under Panel (same way as Pause):
   - `RestartButton` Pos Y `-150`
   - `MainMenuButton` Pos Y `-220`
   - `QuitButton` Pos Y `-290`

### Wire up script

7. Click `GameOver_Canvas` → Add Component → **GameOverUI**:
   - Panel: drag `Panel` child.
   - Score Label: drag `ScoreLabel`.
   - Best Score Label: drag `BestScoreLabel`.
   - New Best Badge: drag `NewBestBadge`.
   - Input Delay: `0.5`.
   - Restart Key: `Space`.
   - Menu Key: `Escape`.
8. For each button → On Click → `GameOverUI` → matching method.

## 3.11 EventSystem

If your Hierarchy doesn't have an `EventSystem` already, right-click in Hierarchy → **UI → Event System**.

If there's an EventSystem and clicking buttons doesn't work later:
- Click EventSystem in Hierarchy.
- Look at the **Input Module** component in Inspector.
- If it says **Input System UI Input Module**, click the gear icon (3 dots) → Remove Component.
- Add Component → **Standalone Input Module**.

## 3.12 Save the scene

`Cmd+S` or **File → Save**.

---

# Part 4 — Build Settings

This tells Unity which scenes to include when building/playing.

1. **File → Build Settings**.
2. In the Project panel, navigate to `Assets/Projects/CubeHopper/Scenes/`.
3. Drag `CubeHopper_MainMenu` into the **Scenes In Build** list at the top.
4. Drag `CubeHopper_Game` into the list below it.
5. The order matters! Make sure `CubeHopper_MainMenu` is at index `0` (top) and `CubeHopper_Game` is at index `1`.
6. Close the Build Settings window.

---

# Part 5 — Try it!

1. Open `CubeHopper_MainMenu` scene (double-click in Project panel).
2. Press **▶ Play** at the top.
3. You should see the menu. Press Space or click anywhere — the game scene should load.
4. In game: press Space to jump. Avoid spikes.
5. Press Escape to pause.
6. Die 3 times → Game Over panel.
7. Click Restart → game restarts.

If something doesn't work, check the Console (bottom panel) for red errors. Most common issues:

| Error | Fix |
|-------|-----|
| `Scene 'CubeHopper_Game' couldn't be loaded` | Add it to Build Settings (Part 4). |
| `NullReferenceException` on `_health` or `_groundCheck` | You forgot to drag something into an Inspector field. Re-check the relevant component. |
| Player falls through the ground | Ground tiles don't have `Layer = Ground`, OR PlayerController's "Ground Layers" doesn't include Ground. |
| Player doesn't jump | Check the Input System: if your project uses the new Input System exclusively, you'll need to switch to "Both" in **Edit → Project Settings → Player → Active Input Handling**. |
| Buttons in Pause/GameOver don't click | EventSystem uses wrong Input Module — see step 3.11. |
| Combo never appears | Make sure `ComboLabel` GameObject is enabled in the prefab (HUDController will hide/show it). |

---

# Part 6 — Lab Checklist

When everything works:

- [ ] Both scenes saved in `Assets/Projects/CubeHopper/Scenes/`
- [ ] Scenes added to Build Settings (MainMenu first, Game second)
- [ ] Player jumps on Space / click / tap
- [ ] Obstacles spawn and scroll
- [ ] Player loses lives on hit (3 lives total)
- [ ] Game Over panel shows on death
- [ ] Restart button works
- [ ] Pause menu (Escape) works
- [ ] Best score persists when you restart (close and reopen Unity to test)
- [ ] Record `gameplay.mp4` with voice-over
- [ ] Commit and push to branch `labs/GD-6-karpenko-one-button-game`

---

# Tips for working in Unity

- **Cmd+S** saves the current scene often.
- **Cmd+Z** undoes the last action — works for almost everything.
- **F** with an object selected in Scene view focuses the camera on it.
- If you can't see the Inspector, top menu: **Window → General → Inspector**.
- If the Hierarchy is messed up, top menu: **Window → General → Hierarchy**.
- Red text in the Console = error, fix it. Yellow = warning, usually safe to ignore.
- If a script doesn't appear when you click "Add Component", make sure it compiled — check the bottom-right corner of Unity for a spinning icon (compiling).

Good luck! 🎮

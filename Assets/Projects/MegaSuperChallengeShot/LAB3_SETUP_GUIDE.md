# Lab 3 — Chicken Hunt: Interaction Mechanics

Guide for finishing the Unity Editor side of Lab 3 after the C# scripts are in place.

## What the scripts do

| Script | Role |
|---|---|
| `GameManager.cs` | Singleton state machine (`Playing` / `GameOver`). Triggers Game Over, restarts the scene via `SceneManager.LoadScene`. |
| `PlayerHealth.cs` | Tracks HP. Updates `Slider` + `TMP_Text`. Calls `GameManager.TriggerGameOver()` when HP hits 0. |
| `ScoreManager.cs` | Score singleton. Adds points per kill, ignores hits while in GameOver. |
| `BirdMover.cs` | Frame-rate independent linear movement (`Time.deltaTime`). |
| `BirdSpawner.cs` | Spawns birds on an interval at random Y. Stops spawning during GameOver. |
| `BirdEscapeTrigger.cs` | Trigger collider on the right edge. `OnTriggerEnter2D` damages the player and destroys the bird. |
| `CrosshairController.cs` | Mouse-follows. **LMB** = Raycast shot. **RMB** = Overlap shotgun. **R** = reload. |
| `GameOverUI.cs` | Shows panel with final score, hooks up the restart button. |

---

## Step 0 — Open the project

1. Open the project in Unity Hub.
2. Open scene `Assets/Projects/MegaSuperChallengeShot/Scenes/Main.unity`.
3. Wait for the Console — there must be **zero compile errors** before moving on.

---

## Step 1 — Tags and Layers

Open **Edit → Project Settings → Tags and Layers**:

- **Layers**: ensure layer **8** is named `Bird` (it might already be). The existing prefab uses Layer 8.
- **Tags**: no special tags needed for this lab.

---

## Step 2 — Bird prefab

Open `Assets/Projects/MegaSuperChallengeShot/Prefabs/Bird1_3_0.prefab`:

- **Layer**: `Bird` (layer 8).
- **CapsuleCollider2D**: `Is Trigger = ✔` (already set).
- **Add Component → Rigidbody2D**:
  - **Body Type**: `Kinematic` (critical — we move it via `transform`, not forces).
  - **Gravity Scale**: `0`.
  - **Collision Detection**: `Continuous` (so fast birds don't tunnel through the EscapeWall).
- **BirdMover**: `Speed = 2` (or whatever feels right — the new code is `Time.deltaTime`-scaled, so values are in units/sec).
- **Direction**: `(1, 0)` for left-to-right movement.

> ⚠️ **Why Rigidbody2D?** `OnTriggerEnter2D` only fires if at least one of the colliding objects has a `Rigidbody2D`. Without it the bird flies through `EscapeWall` and your HP never drops.

> ⚠️ The old prefab had `Speed = 0.005`. The new `BirdMover` multiplies by `Time.deltaTime`, so bump it to `1.5`–`3`.

---

## Step 3 — Scene hierarchy

Open `Main.unity`. Build this hierarchy:

```
Main Camera
Spawner            (empty GameObject — left edge of screen)
EscapeWall         (empty GameObject — right edge of screen)
Crosshair          (sprite + Crosshair script)
--- Managers ---
GameManager
ScoreManager
PlayerHealth       (or put the script on Canvas — your call)
--- UI ---
Canvas
  HUD
    ScoreText      (TextMeshPro)
    ShotsText      (TextMeshPro)
    HPSlider       (UI/Slider)
    HPText         (TextMeshPro)
  GameOverPanel
    Title          (TMP — "GAME OVER")
    FinalScoreText (TMP)
    RestartButton  (UI/Button)
EventSystem
```

### 3.1 Spawner

1. Create empty GameObject → name it `Spawner`.
2. Position: `X = -9, Y = 0, Z = 0` (a bit outside the left edge of a default camera).
3. Add component **BirdSpawner**.
4. Drag `Bird1_3_0.prefab` into `_birdPrefab`.
5. `Spawn Interval = 1.5`, `Min Y = -3`, `Max Y = 3` (tune to fit your camera frustum).

### 3.2 EscapeWall (the punishment zone)

1. Create empty GameObject → name it `EscapeWall`.
2. Position: `X = 10, Y = 0, Z = 0` (right of the camera frustum).
3. Add component **BoxCollider2D**:
   - **Is Trigger = ✔**
   - Size: `X = 1, Y = 20` (a tall thin strip).
4. Add component **BirdEscapeTrigger**:
   - `Damage Per Bird = 1`
   - `Bird Layer` = check **Bird** only.

### 3.3 Crosshair

1. Either keep the existing Crosshair GameObject, or create new empty → add `SpriteRenderer` with a crosshair sprite.
2. Add component **CrosshairController**.
3. Inspector:
   - `Max Shots = 10`
   - `Shots Text` → drag the `ShotsText` TMP from UI
   - `Target` → check **Bird** layer
   - `Shotgun Radius = 1.5`

### 3.4 GameManager

1. Create empty GameObject → name `GameManager`.
2. Add component **GameManager**.
3. `Game Over UI` field — leave empty for now; we wire it after the UI is built (Step 4).

### 3.5 ScoreManager + PlayerHealth

1. Create empty GameObject → name `ScoreManager`. Add `ScoreManager` component.
2. Create empty GameObject → name `PlayerHealth`. Add `PlayerHealth` component.
   - `Max Hp = 5`
   - Slider and Text references filled in Step 4.

---

## Step 4 — UI (Canvas)

### 4.1 Create the Canvas

`GameObject → UI → Canvas`. This auto-creates `Canvas` + `EventSystem`.

On the Canvas:
- **Render Mode**: `Screen Space - Overlay`.
- **Canvas Scaler → UI Scale Mode**: `Scale With Screen Size`.
- **Reference Resolution**: `1920 x 1080`.
- **Match**: `0.5`.
- ⚠️ Ensure the Canvas RectTransform `Scale = (1, 1, 1)`. If it's `(0,0,0)` the UI won't render — that bit me in Lab 4.

### 4.2 HUD container

Right-click Canvas → Create Empty → `HUD`. Stretch its RectTransform to fill the canvas.

Inside HUD:

**ScoreText** (top-left)
- `GameObject → UI → Text - TextMeshPro` (import TMP Essentials if prompted).
- Anchor: top-left. Position: `(20, -20)`.
- Font size 40, text "Score: 0".

**ShotsText** (top-right)
- TMP text. Anchor top-right. Position `(-20, -20)`.
- Font size 40, text "10/10".

**HPSlider** (top-center)
- `GameObject → UI → Slider`.
- Anchor top-center. Position `(0, -40)`. Width 400.
- Set Fill color to red/green for clarity.
- Min Value = 0, Max Value = 5, Value = 5.

**HPText** (centered on the slider, optional)
- TMP text. Child of the slider. Text "HP: 5/5".

### 4.3 Game Over Panel

Right-click Canvas → Create Empty → `GameOverPanel`. Stretch to fill. Add a UI Image as background (alpha ~0.7, black).

Inside the panel:
- **Title** — TMP, large, centered, text "GAME OVER".
- **FinalScoreText** — TMP, centered, text "Final Score: 0".
- **RestartButton** — `GameObject → UI → Button - TextMeshPro`. Centered. Button label "Restart".

Add the `GameOverUI` script to the **GameOverPanel** GameObject:
- `Panel` → drag the GameOverPanel itself (the script disables it on Awake).
- `Final Score Text` → drag FinalScoreText TMP.
- `Restart Button` → drag the RestartButton.

> The panel will start visible in the editor — that's fine. `GameOverUI.Awake()` calls `SetActive(false)` automatically.

### 4.4 Wire references

Go back to the manager GameObjects:

- **GameManager** → `Game Over UI` = drag the GameOverPanel GameObject (the one with `GameOverUI` script).
- **ScoreManager** → `Score Text` = drag `ScoreText`.
- **PlayerHealth** → `Hp Slider` = drag `HPSlider`. `Hp Text` = drag `HPText`.
- **CrosshairController** → `Shots Text` = drag `ShotsText`. `Target` = check `Bird` layer.

---

## Step 5 — Camera & background

- Main Camera position `(0, 0, -10)`, Projection `Orthographic`, Size `~5`.
- If you have a sky/background sprite, drop it as a child of the camera or at the world origin with a low sorting order.

---

## Step 6 — Test the core loop

Press Play. You should see:

- Birds spawning from the left and flying right.
- Mouse moves the crosshair.
- **LMB** kills a single bird the cursor is over (Raycast). Score goes up by 10.
- **RMB** kills every bird within `Shotgun Radius` (Overlap). Score goes up by 10 per kill.
- **R** reloads ammo back to max.
- A bird that crosses the right edge → HP drops by 1, bird is destroyed.
- HP hits 0 → Game Over panel appears with final score.
- Restart button reloads the scene.

If something doesn't work, check the Console first.

---

## Common pitfalls

| Symptom | Fix |
|---|---|
| Click does nothing | `Target` LayerMask on Crosshair is empty — set it to `Bird`. |
| Bird passes through escape wall | EscapeWall collider isn't a Trigger, or wrong layer mask. |
| UI is invisible | Canvas RectTransform `Scale` is `(0,0,0)`. Set to `(1,1,1)`. |
| Score doesn't update | ScoreManager has no `Score Text` reference. |
| Game Over never fires | PlayerHealth `Max Hp = 0`, or it's not in the scene. |
| `Time.deltaTime` warning / birds fly insanely fast | You raised `Speed` after the refactor — set it back to ~2. |

---

## Звіт (для здачі)

### 1. Які типи колізій використано?

- **Collider2D (CapsuleCollider2D)** на префабі птаха — задає геометрію для перевірки влучань та зіткнень.
- **Collider2D (BoxCollider2D, Trigger)** на `EscapeWall` — невидима «стіна-тригер» праворуч від камери: реєструє втечу птаха.
- **Trigger callback** — `OnTriggerEnter2D` у `BirdEscapeTrigger.cs`: коли птах входить у тригер, гравець отримує шкоду й птах знищується. Шари використано як фільтр (`LayerMask`).

### 2. Де застосовано Raycast та Overlap?

- **`Physics2D.GetRayIntersection`** (raycast у 2D через камеру) у `CrosshairController.FireSingle()` — основний постріл лівою кнопкою миші: пускаємо промінь з екрана у світ, і якщо влучили в колайдер шару `Bird`, нараховуємо очки.
- **`Physics2D.OverlapCircleAll`** у `CrosshairController.FireShotgun()` — «дробовик» по правій кнопці: знаходить усі колайдери у радіусі `Shotgun Radius` навколо прицілу й знищує їх одночасно.

### 3. Чому саме такий метод перевірки обрано?

- **Raycast для одиночного пострілу** — це точкове влучання вздовж променя, що ідеально моделює «постріл» з камери: він враховує всі колайдери на промені, і нам потрібен лише перший, найближчий. `GetRayIntersection` зручний у 2D, бо приймає `Ray` напряму з `Camera.ScreenPointToRay`.
- **OverlapCircleAll для дробовика** — постріл по площі, а не точці; нам потрібен список **усіх** колайдерів у радіусі, тому raycast не підходить (він повертає лише найближчий). `OverlapCircleAll` дає масив, який ми просто перебираємо.
- **Trigger (OnTriggerEnter2D) для втечі птаха** — це подія, керована фізикою: жодних щотактних `if (bird.x > limit)` перевірок, Unity сама викличе колбек, коли колайдер увійде в зону. Це чистіше й дешевше.

### 4. Як механіки впливають на core loop?

Core loop став таким:
1. **Спавн** → `BirdSpawner` створює птахів через інтервали, доки `GameManager.State == Playing`.
2. **Виклик до дії** → гравець бачить ціль і має обмежені постріли (`Max Shots`).
3. **Дія гравця** → клік мишею: одиночний raycast чи overlap-дробовик; за кожне влучання `ScoreManager` додає очки.
4. **Покарання за бездіяльність** → якщо птах долітає до правого краю, `BirdEscapeTrigger` забирає 1 HP.
5. **Програш** → коли HP падає до 0, `GameManager.TriggerGameOver()` зупиняє спавн і пострiли, показує панель з фінальним рахунком.
6. **Перезапуск** → кнопка Restart викликає `SceneManager.LoadScene` → петля стартує спочатку.

Тобто кожна механіка з лаби виконує конкретну роль у петлі: HP — стресор (тиск часу), рахунок — ціль (мотивація), умова програшу — точка завершення, перезапуск — повернення в гру. Без HP та програшу гра була б нескінченним «стрільбищем без наслідків»; з ними вона перетворюється на повноцінний game loop із зворотним зв'язком.

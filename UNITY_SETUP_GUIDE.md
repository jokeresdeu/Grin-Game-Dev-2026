# Lab 4 — Unity Setup Guide (Step-by-Step)

> **Student:** Ann Karpenko  
> **Branch:** `labs/GD-4-karpenko-adding_animation_to_chicen_hunt`  
> **Scene:** `Assets/Projects/MegaSuperChallengeShot/Scenes/Main.unity`

All C# scripts are already written and located in  
`Assets/Projects/MegaSuperChallengeShot/Scripts/`

This guide covers everything you need to do **inside Unity Editor**.

---

## PHASE 0 — Open the Project

1. Open Unity Hub → Open the `Grin-Game-Dev-2026` project
2. Wait for compilation. If there are errors about `TMPro.EditorUtilities`, that's expected — the old `CrosshairController` had that import, the new one doesn't. Just let Unity recompile.
3. Open the scene: **File → Open Scene → `Assets/Projects/MegaSuperChallengeShot/Scenes/Main.unity`**
4. In the Console window, clear all messages (click "Clear")

---

## PHASE 1 — Layers Setup

1. Go to **Edit → Project Settings → Tags and Layers**
2. Scroll down to **Layers** section
3. Find the first empty **User Layer** slot (probably slot 6)
4. Type: `Bird`
5. Close Project Settings

> **Why:** CrosshairController uses `LayerMask _target` set to "Bird" layer to detect what the crosshair can hit.

---

## PHASE 2 — Slice the Chest Spritesheet

1. In the **Project** window, navigate to:  
   `Assets/Sprites/Cainos/Pixel Art Platformer - Village Props/Texture/`
2. Click on **`TX Chest Animation.png`**
3. In the **Inspector**, set:
   - **Texture Type** = `Sprite (2D and UI)`
   - **Sprite Mode** = `Multiple`
   - **Pixels Per Unit** = `16` (these are pixel art sprites)
   - **Filter Mode** = `Point (no filter)`
   - **Compression** = `None`
4. Click **Apply**
5. Click **Sprite Editor** button
6. In Sprite Editor toolbar, click **Slice** dropdown
7. Set:
   - **Type** = `Grid By Cell Count`
   - **Column & Row** = `C: 7, R: 4`
8. Click **Slice** → you should see 28 cells highlighted
9. Click **Apply** (top-right of Sprite Editor)
10. Close Sprite Editor
11. Expand the `TX Chest Animation` asset in Project window — you should now see `TX Chest Animation_0` through `TX Chest Animation_27`

> **The first row (frames 0–6)** is the golden chest opening animation. Frame 0 = closed, frame 6 = fully open.

---

## PHASE 3 — Create Animator Controllers for Birds

You need ONE Animator Controller per bird type. I'll show B1 (Chicken) in detail — repeat for B2/B3/B4 with their own clips.

### 3.1 — B1 (Chicken) Animator Controller

1. In Project window, navigate to:  
   `Assets/Projects/MegaSuperChallengeShot/Graphics/Cute_Birds(4 in 1)Mini/B1/Animations/`
2. Right-click in that folder → **Create → Animator Controller**
3. Name it: `BirdFlyController_B1`
4. **Double-click** the new controller to open the **Animator** window
5. In the Animator window, go to the **Parameters** tab (left side):
   - Click **+** → **Float** → name it exactly: `FlySpeed`
   - Click **+** → **Trigger** → name it exactly: `IsHit`
   - Click **+** → **Bool** → name it exactly: `IsDead`

   > ⚠️ **Names must be EXACT** — the script uses `Animator.StringToHash("FlySpeed")`, `"IsHit"`, `"IsDead"`

6. From the same folder, **drag `Bird1_1.anim`** into the Animator window grid
   - It should become an **orange box** = default state
   - Right-click it → **Set as Layer Default State** if it's not orange
   - Rename it to `Fly` (right-click → "Rename" or click and edit name in Inspector)

7. Right-click empty space in Animator → **Create State → Empty** → name it `Hit`
8. Right-click empty space → **Create State → Empty** → name it `Death`

9. **Create transitions:**

   **Fly → Hit:**
   - Right-click `Fly` → **Make Transition** → click on `Hit`
   - Click the **arrow** between them
   - In Inspector:
     - **Has Exit Time** = `unchecked` ❌
     - **Conditions**: click **+** → select `IsHit`
     - **Transition Duration** = `0`

   **Hit → Fly:**
   - Right-click `Hit` → **Make Transition** → click on `Fly`
   - Click the arrow
   - In Inspector:
     - **Has Exit Time** = `checked` ✅
     - **Exit Time** = `1`
     - **Transition Duration** = `0.1`
     - **Conditions**: (none needed — exits by time)

   **Any State → Death:**
   - Right-click the green **Any State** box → **Make Transition** → click on `Death`
   - Click the arrow
   - In Inspector:
     - **Has Exit Time** = `unchecked` ❌
     - **Conditions**: click **+** → select `IsDead` → set to `true`
     - **Transition Duration** = `0`

10. **Save** (Ctrl+S)

### 3.2 — B2 (Duck) Animator Controller

1. Navigate to: `Assets/.../B2_Duck/Animations/`
2. Create → Animator Controller → name: `BirdFlyController_B2`
3. Open it, add the SAME 3 parameters: `FlySpeed` (float), `IsHit` (trigger), `IsDead` (bool)
4. Drag `Bird2_1.anim` as default state → rename to `Fly`
5. Create `Hit` and `Death` empty states
6. Create the SAME transitions as B1 (copy the exact settings above)

### 3.3 — B3 (Egret) Animator Controller

1. Navigate to: `Assets/.../B3_Egret/Animations/`
2. Create → Animator Controller → name: `BirdFlyController_B3`
3. Same 3 parameters, drag `BirdEgr_1.anim` as Fly default
4. Create Hit + Death states, same transitions

### 3.4 — B4 Animator Controller

1. Navigate to: `Assets/.../B4/Animations/`
2. Create → Animator Controller → name: `BirdFlyController_B4`
3. Same 3 parameters, drag `B4_s1.anim` as Fly default
4. Create Hit + Death states, same transitions

---

## PHASE 4 — Configure Bird Prefabs

### 4.1 — B1 Prefab (Bird1_3_0)

1. In Project window find: `Assets/.../B1/Prefabs/Bird1_3_0.prefab`
2. **Double-click** to enter Prefab Mode
3. Select the root GameObject in the Hierarchy

**Inspector settings:**

| Step | Action |
|------|--------|
| 4.1.1 | **Layer** dropdown (top of Inspector) → select `Bird` → "Yes, change children" |
| 4.1.2 | **Transform → Scale** = `(0.2, 0.2, 1)` (should already be set) |
| 4.1.3 | **SpriteRenderer → Material** = drag `Sprite-Lit-Default` from Project (search for it) |
| 4.1.4 | **SpriteRenderer → Sorting Layer** = `Default`, **Order in Layer** = `5` |
| 4.1.5 | **Animator → Controller** = drag `BirdFlyController_B1` from Project |
| 4.1.6 | Check if **CircleCollider2D** exists. If yes: **Radius** = `1`, **Is Trigger** = `unchecked`. If no: Add Component → CircleCollider2D, set those values |

Now add the new scripts:

| Step | Action |
|------|--------|
| 4.1.7 | **Add Component** → search `BirdMover` → add it |
| 4.1.8 | In BirdMover: **Speed** = `2`, **Move Right** = `checked ✅`, **Destroy Boundary X** = `18` |
| 4.1.9 | **Add Component** → search `BirdAnimationController` → add it |
| 4.1.10 | In BirdAnimationController: **Hit Flash Duration** = `0.15`, **Hit Color** = red `(255, 0, 0, 255)`, **Death Fall Speed** = `3`, **Death Fade Duration** = `0.8`, **Death Rotation Speed** = `200` |

4. Click the **← (back arrow)** to exit Prefab Mode
5. If asked "Save changes?" → **Save**

### 4.2 — B1 Second Variant (Bird1_1_0)

1. Open `Assets/.../B1/Prefabs/Bird1_1_0.prefab`
2. **Repeat ALL steps from 4.1.1 through 4.1.10** — identical settings
3. Save and exit

### 4.3 — B2 Duck Prefabs (Bird2 Duck_1_0 AND Bird2 Duck_3_0)

1. Open each prefab from `Assets/.../B2_Duck/Prefabs/`
2. Same steps as 4.1, **EXCEPT:**
   - Step 4.1.5: **Animator → Controller** = `BirdFlyController_B2`
   - Step 4.1.8: **Speed** = `2.5` (ducks are slightly faster)
3. Save each

### 4.4 — B3 Egret Prefabs (Bird3_Egret2_0 AND Bird3_Egret4_0)

1. Open each from `Assets/.../B3_Egret/Prefabs/`
2. Same steps, **EXCEPT:**
   - **Animator → Controller** = `BirdFlyController_B3`
   - **Speed** = `3` (egrets are fast)
3. Save each

### 4.5 — B4 Prefabs (B4_s3_0 AND B4_s5_0)

1. Open each from `Assets/.../B4/Prefabs/`
2. Same steps, **EXCEPT:**
   - **Animator → Controller** = `BirdFlyController_B4`
   - **Speed** = `1.5` (slow heavy birds)
3. Save each

---

## PHASE 5 — Scene Setup: Managers

Open the Main scene if not already open.

### 5.1 — GameAnimationManager

1. In Hierarchy, right-click → **Create Empty**
2. Rename to `GameAnimationManager`
3. Position: `(0, 0, 0)` (doesn't matter, it's a manager)
4. **Add Component** → search `GameAnimationManager` → add
5. Inspector should show:
   - **Global Animation Speed** = `1`
   - **Animations Paused** = `unchecked`

### 5.2 — Fix ScoreManager

The existing ScoreManager in the scene may have the old DontDestroyOnLoad version. Let's make sure it's using the new script:

1. Find `ScoreManager` in the Hierarchy (it may be on a Canvas or its own GameObject)
2. Select it → check Inspector
3. The script should show `ScoreManager (Script)` with field: **Score Text** (TMP_Text)
4. Make sure **Score Text** field has the TMP text element dragged in
5. If `ScoreManager` is on a separate GameObject (not on Canvas), that's fine — just make sure it exists in the scene

---

## PHASE 6 — Scene Setup: BirdSpawner

### 6.1 — Delete or Disable Old BirdSpawner

1. If there's already a `BirdSpawner` in the scene, select it
2. Check if it has the old `BirdSpawner` script (the one with only `_birdPrefab` single field)
3. The new script replaces it — Unity should auto-update since the class name is the same
4. If it shows missing script, remove the old component and add the new `BirdSpawner`

### 6.2 — Create Spawn Points

1. Select `BirdSpawner` in Hierarchy
2. Right-click it → **Create Empty** → rename to `SpawnPoint_Top`
   - **Transform → Position** = `(0, 3, 0)`
3. Right-click BirdSpawner again → **Create Empty** → rename to `SpawnPoint_Mid`
   - **Transform → Position** = `(0, 1, 0)`
4. Right-click BirdSpawner again → **Create Empty** → rename to `SpawnPoint_Bottom`
   - **Transform → Position** = `(0, -1, 0)`

### 6.3 — Configure BirdSpawner

Select the `BirdSpawner` GameObject. In Inspector:

| Field | Value | How to set |
|-------|-------|------------|
| **Bird Prefabs** | Size = `8` | Click the array, set size |
| **Bird Prefabs[0]** | `Bird1_1_0` | Drag from `B1/Prefabs/` in Project |
| **Bird Prefabs[1]** | `Bird1_3_0` | Drag from `B1/Prefabs/` |
| **Bird Prefabs[2]** | `Bird2 Duck_1_0` | Drag from `B2_Duck/Prefabs/` |
| **Bird Prefabs[3]** | `Bird2 Duck_3_0` | Drag from `B2_Duck/Prefabs/` |
| **Bird Prefabs[4]** | `Bird3_Egret2_0` | Drag from `B3_Egret/Prefabs/` |
| **Bird Prefabs[5]** | `Bird3_Egret4_0` | Drag from `B3_Egret/Prefabs/` |
| **Bird Prefabs[6]** | `B4_s3_0` | Drag from `B4/Prefabs/` |
| **Bird Prefabs[7]** | `B4_s5_0` | Drag from `B4/Prefabs/` |
| **Spawn Points** | Size = `3` | |
| **Spawn Points[0]** | `SpawnPoint_Top` | Drag from Hierarchy (child of BirdSpawner) |
| **Spawn Points[1]** | `SpawnPoint_Mid` | Drag from Hierarchy |
| **Spawn Points[2]** | `SpawnPoint_Bottom` | Drag from Hierarchy |
| **Spawn Interval** | `2` | |
| **Initial Delay** | `1` | |
| **Min Spawn Interval** | `0.8` | |
| **Difficulty Ramp Rate** | `0.02` | |
| **Min Speed** | `1.5` | |
| **Max Speed** | `4` | |
| **Alternate Directions** | `checked ✅` | |
| **Spawn Offset X** | `12` | |
| **Max Active Birds** | `10` | |

---

## PHASE 7 — Scene Setup: Crosshair

1. Find the **Crosshair** GameObject in the Hierarchy (should already exist from Lab 3)
2. Select it

### 7.1 — Add CrosshairAnimator

| Step | Action |
|------|--------|
| 7.1.1 | **Add Component** → search `CrosshairAnimator` → add |
| 7.1.2 | Idle Pulse Speed = `2` |
| 7.1.3 | Idle Pulse Amount = `0.05` |
| 7.1.4 | Shoot Punch Scale = `1.4` |
| 7.1.5 | Shoot Punch Duration = `0.1` |
| 7.1.6 | Shoot Flash Color = yellow `(255, 200, 0, 255)` |
| 7.1.7 | Reload Spin Duration = `0.3` |
| 7.1.8 | Reload Spin Degrees = `360` |
| 7.1.9 | Shake Intensity = `0.1` |
| 7.1.10 | Shake Duration = `0.2` |

### 7.2 — Verify CrosshairController

| Field | Value |
|-------|-------|
| **Max Shots** | `6` |
| **Text** | Should already have the TMP shots text dragged in |
| **Target** | Click the dropdown → check ONLY `Bird` layer |
| **Line Renderer** | Leave empty (optional) |

### 7.3 — SpriteRenderer check

| Field | Value |
|-------|-------|
| **Sorting Order** | `100` (so it renders on top of everything) |
| **Material** | `Sprite-Lit-Default` |

---

## PHASE 8 — Scene Setup: Chest

### 8.1 — Create Chest GameObject

1. In Hierarchy, right-click → **Create Empty** → rename to `Chest_1`
2. **Transform:**
   - **Position** = `(3, -3, 0)`
   - **Scale** = `(2, 2, 1)`

### 8.2 — Add Components

| Step | Action |
|------|--------|
| 8.2.1 | **Add Component** → `Sprite Renderer` |
| 8.2.2 | **Sprite** = drag `TX Chest Animation_0` from Project (the first frame, closed chest) |
| 8.2.3 | **Material** = `Sprite-Lit-Default` |
| 8.2.4 | **Sorting Order** = `3` |
| 8.2.5 | **Layer** (top of Inspector) = `Bird` |
| 8.2.6 | **Add Component** → `Box Collider 2D` |
| 8.2.7 | **Size** = `(0.5, 0.5)` |
| 8.2.8 | **Is Trigger** = `unchecked` ❌ |
| 8.2.9 | **Add Component** → search `ChestAnimationController` → add |

### 8.3 — Configure ChestAnimationController

| Field | How to set |
|-------|------------|
| **Closed Sprite** | Drag `TX Chest Animation_0` from Project |
| **Open Frames** → Size | `7` |
| **Open Frames[0]** | Drag `TX Chest Animation_0` |
| **Open Frames[1]** | Drag `TX Chest Animation_1` |
| **Open Frames[2]** | Drag `TX Chest Animation_2` |
| **Open Frames[3]** | Drag `TX Chest Animation_3` |
| **Open Frames[4]** | Drag `TX Chest Animation_4` |
| **Open Frames[5]** | Drag `TX Chest Animation_5` |
| **Open Frames[6]** | Drag `TX Chest Animation_6` |
| **Frame Rate** | `8` |
| **Auto Close Delay** | `3` |
| **Auto Close Enabled** | `checked ✅` |
| **Idle Bounce Amount** | `0.05` |
| **Idle Bounce Speed** | `2` |
| **Score Value** | `5` |

> **Tip:** You can multi-select frames 0–6 in the Project window and drag them all at once into the Open Frames array.

---

## PHASE 9 — Scene Setup: Clouds (Background)

You'll create 3 cloud objects. Use any white/light sprite (or use the Props sprite).

### 9.1 — Create a Cloud Sprite

If you don't have a cloud sprite:
1. Right-click in `Assets/Projects/MegaSuperChallengeShot/Graphics/` → **Create → Sprites → Square** (or use any white sprite)
2. Or use `Props_Bird(Mini).png` from the Props folder as a placeholder

### 9.2 — Cloud_1

1. Hierarchy → right-click → **Create Empty** → rename to `Cloud_1`
2. **Transform → Position** = `(-5, 4, 0)`, **Scale** = `(2, 1, 1)`
3. **Add Component** → `Sprite Renderer`
   - **Sprite** = your cloud sprite
   - **Color** = white with alpha ~0.5: `(255, 255, 255, 128)`
   - **Sorting Order** = `-5` (behind birds)
   - **Material** = `Sprite-Lit-Default`
4. **Add Component** → `CloudMover`
   - **Drift Speed** = `0.5`
   - **Reset X Left** = `-18`
   - **Reset X Right** = `18`
   - **Move Right** = `checked ✅`
   - **Randomize On Start** = `checked ✅`
5. **Add Component** → `PropFloatAnimator`
   - **Bob Amplitude** = `0.1`
   - **Bob Speed** = `1`
   - **Enable Sway** = `unchecked` ❌ (clouds don't sway)
   - **Randomize Phase** = `checked ✅`

### 9.3 — Cloud_2

1. **Duplicate** Cloud_1 (select it, Ctrl+D)
2. Rename to `Cloud_2`
3. **Position** = `(3, 3.5, 0)`
4. **CloudMover → Drift Speed** = `0.3` (slower = further away feeling)
5. **Scale** = `(3, 1.2, 1)` (bigger cloud)

### 9.4 — Cloud_3

1. **Duplicate** Cloud_1 again (Ctrl+D)
2. Rename to `Cloud_3`
3. **Position** = `(8, 3, 0)`
4. **CloudMover → Drift Speed** = `0.7` (faster)
5. **CloudMover → Move Right** = `unchecked ❌` (moves left for variety)
6. **Scale** = `(1.5, 0.8, 1)`

---

## PHASE 10 — Scene Setup: Background Props

### 10.1 — Prop_Tree

1. Hierarchy → right-click → **Create Empty** → rename to `Prop_Tree`
2. **Position** = `(-4, -3, 0)`
3. **Add Component** → `Sprite Renderer`
   - Use any prop sprite from `Assets/Sprites/Cainos/Pixel Art Platformer - Village Props/Texture/TX Village Props.png` (slice it first if needed, or use any available sprite)
   - **Sorting Order** = `-3`
4. **Add Component** → `PropFloatAnimator`
   - **Bob Amplitude** = `0.03` (subtle)
   - **Bob Speed** = `1.5`
   - **Enable Sway** = `checked ✅`
   - **Sway Angle** = `2` (gentle sway)
   - **Sway Speed** = `0.8`
   - **Enable Breathe** = `unchecked` ❌
   - **Randomize Phase** = `checked ✅`

### 10.2 — Prop_Bush

1. Duplicate Prop_Tree (Ctrl+D) → rename to `Prop_Bush`
2. **Position** = `(5, -3.5, 0)`
3. Change sprite if you want
4. **PropFloatAnimator:**
   - **Bob Amplitude** = `0.02`
   - **Enable Sway** = `checked ✅`
   - **Sway Angle** = `3`

---

## PHASE 11 — [BONUS 2] Knight Character

### 11.1 — KnightAnimationController (sprite-frame approach)

1. Hierarchy → right-click → **Create Empty** → rename to `Knight`
2. **Position** = `(-3, -4, 0)` (bottom of screen)
3. **Add Component** → `Sprite Renderer`
   - **Sprite** = `Knight_idle_01` from `Assets/Sprites/Knight Files/Knight PNG/`
   - **Sorting Order** = `10`
4. **Add Component** → `Rigidbody 2D`
   - **Body Type** = `Dynamic`
   - **Gravity Scale** = `1`
   - **Freeze Rotation Z** = `checked ✅` (under Constraints)
5. **Add Component** → `Box Collider 2D`
   - **Size** = adjust to fit the knight sprite (roughly `(0.5, 0.8)`)
6. **Add Component** → `KnightAnimationController`

**Now assign the sprite arrays:**

| Field | Sprites to drag (from Knight PNG folder) |
|-------|------------------------------------------|
| **Idle Frames** → Size = `6` | `Knight_idle_01`, `Knight_idle_02`, `Knight_idle_03`, `Knight_idle_04`, `Knight_idle_05`, `Knight_idle_06` |
| **Walk Frames** → Size = `6` | `Knight_walk_01`, `Knight_walk_02`, `Knight_walk_03`, `Knight_walk_04`, `Knight_walk_05`, `Knight_walk_06` |
| **Jump Frames** → Size = `2` | `Knight_jump_01`, `Knight_jump_02` |
| **Crouch Frames** → Size = `1` | `Knight_crouch_0` |

> **Tip:** In the Project window, navigate to `Knight PNG/`, sort by name, then multi-select sprites and drag them into the array.

| Field | Value |
|-------|-------|
| **Idle Frame Rate** | `6` |
| **Walk Frame Rate** | `10` |
| **Jump Frame Rate** | `4` |
| **Crouch Frame Rate** | `4` |
| **Move Speed** | `3` |
| **Jump Force** | `5` |
| **Ground Layer** | Check `Default` (or whatever layer your ground is) |

### 11.2 — Ground Check child

1. Select `Knight` → right-click → **Create Empty** → rename to `GroundCheck`
2. **Position** = `(0, -0.45, 0)` (bottom of the knight sprite)
3. Go back to `Knight` → **KnightAnimationController**:
   - **Ground Check** = drag `GroundCheck` from Hierarchy
   - **Ground Check Radius** = `0.2`

### 11.3 — Add a Ground platform (if not present)

1. Hierarchy → right-click → **2D Object → Sprites → Square**
2. Rename to `Ground`
3. **Position** = `(0, -5, 0)`, **Scale** = `(30, 1, 1)`
4. **Add Component** → `Box Collider 2D` (auto-sized)
5. Layer = `Default`

---

## PHASE 12 — [BONUS 2] Human Composite Character

### 12.1 — Create Parent

1. Hierarchy → right-click → **Create Empty** → rename to `HumanComposite`
2. **Position** = `(3, -4, 0)`
3. **Add Component** → `HumanCompositeController`

### 12.2 — Create Child Body Parts

For EACH row in this table, do: right-click `HumanComposite` → **Create Empty**, rename, add SpriteRenderer, set sprite and position.

| Child Name | Sprite (from `Assets/Sprites/Knight Files/Body Parts/`) | Local Position | Sorting Order |
|------------|-------|----------------|---------------|
| `Head` | `Head.png` | `(0, 0.7, 0)` | `5` |
| `BodyUpper` | `body upper.png` | `(0, 0.3, 0)` | `3` |
| `BodyLower` | `Body lower.png` | `(0, 0, 0)` | `2` |
| `LeftArm` | `Left Arm.png` | `(-0.3, 0.3, 0)` | `4` |
| `RightArm` | `Right Arm.png` | `(0.3, 0.3, 0)` | `4` |
| `Legs` | `Legs.png` | `(0, -0.3, 0)` | `1` |
| `Sword` | `sword.png` | `(0.4, 0.2, 0)` | `6` |

For each child:
1. Create Empty child under HumanComposite
2. Rename (see table)
3. **Add Component** → `Sprite Renderer`
4. **Sprite** = drag the corresponding `.png` from Project
5. **Sorting Order** = see table
6. **Local Position** = see table

### 12.3 — Wire References

1. Select `HumanComposite` in Hierarchy
2. In `HumanCompositeController`:

| Field | Drag from Hierarchy |
|-------|---------------------|
| **Head** | drag `Head` child |
| **Body Upper** | drag `BodyUpper` child |
| **Body Lower** | drag `BodyLower` child |
| **Left Arm** | drag `LeftArm` child |
| **Right Arm** | drag `RightArm` child |
| **Legs** | drag `Legs` child |
| **Sword** | drag `Sword` child |

| Field | Value |
|-------|-------|
| **Move Speed** | `2` |
| **Walk Bob Amount** | `0.05` |
| **Walk Bob Speed** | `8` |
| **Walk Arm Swing** | `15` |
| **Walk Leg Swing** | `20` |

---

## PHASE 13 — Final Scene Save

1. **Ctrl+S** to save the scene
2. **File → Save Project** to save all asset changes

---

## PHASE 14 — Test (Play Mode)

Press **Play** ▶ and verify each item:

| # | What to Check | Expected Result |
|---|---------------|-----------------|
| 1 | Birds spawn | Every ~2 seconds a bird appears from left or right edge |
| 2 | Birds animate | Wings flapping as they fly |
| 3 | Left-click on bird | Red flash → bird falls + rotates + fades out → destroyed |
| 4 | Crosshair follows mouse | Smooth tracking, gentle scale pulse |
| 5 | Left-click shoots | Crosshair enlarges briefly + yellow flash |
| 6 | Right-click reloads | Crosshair spins 360° |
| 7 | Shoot with 0 ammo | Crosshair shakes |
| 8 | Shot counter | Decreases on each shot, resets on right-click |
| 9 | Score | Increases when hitting a bird |
| 10 | Click on chest | Chest opens frame-by-frame, then auto-closes after 3s |
| 11 | Chest idle | Chest gently bounces up/down when not animating |
| 12 | Clouds | Drift across screen, wrap around, vary in opacity/size |
| 13 | Background props | Subtle bob and sway animation |
| 14 | Console logs | `[BirdMover]`, `[BirdAnimation]`, `[Crosshair]`, `[Chest]`, `[ScoreManager]` messages |
| 15 | [Bonus] Knight | Arrow keys to walk (sprite animation changes), Space to jump, S to crouch |
| 16 | [Bonus] HumanComposite | Arrow keys move body, arms swing when walking, body compresses when crouching |

---

## PHASE 15 — Git Commands

Open a terminal in the project root folder:

```bash
# Make sure you're on the right branch
git checkout labs/GD-4-karpenko-adding_animation_to_chicen_hunt

# Check status
git status

# Add all new and modified scripts
git add Assets/Projects/MegaSuperChallengeShot/Scripts/BirdAnimationController.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/ChestAnimationController.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/CrosshairAnimator.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/PropFloatAnimator.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/CloudMover.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/SpriteAnimator.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/GameAnimationManager.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/KnightAnimationController.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/HumanCompositeController.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/BirdMover.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/BirdSpawner.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/CrosshairController.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/ScoreManager.cs

# Add the report
git add Assets/Projects/MegaSuperChallengeShot/Lab4_Report_Karpenko.docx

# Add scene and any new assets (animator controllers, prefab changes)
git add Assets/Projects/MegaSuperChallengeShot/Scenes/
git add Assets/Projects/MegaSuperChallengeShot/Prefabs/
git add "Assets/Projects/MegaSuperChallengeShot/Graphics/Cute_Birds(4 in 1)Mini/"

# Add meta files
git add Assets/Projects/MegaSuperChallengeShot/Scripts/*.cs.meta
git add Assets/Projects/MegaSuperChallengeShot/Lab4_Report_Karpenko.docx.meta

# Commit
git commit -m "Lab 4: Add animations to Chicken Hunt

- 8 animated objects: 4 bird types, chest, crosshair, clouds, props
- BirdAnimationController: fly/hit/death via Animator + coroutine
- ChestAnimationController: frame-by-frame open/close + idle bounce
- CrosshairAnimator: pulse/shoot/reload/empty visual feedback
- CloudMover + PropFloatAnimator: background idle animations
- KnightAnimationController: [Bonus] walk/jump/crouch sprite-frame
- HumanCompositeController: [Bonus] body parts composition + 3 anims
- Updated BirdMover, BirdSpawner, CrosshairController, ScoreManager
- Fixed ScoreManager: removed DontDestroyOnLoad (scene-scoped)"

# Push
git push origin labs/GD-4-karpenko-adding_animation_to_chicen_hunt
```

---

## Troubleshooting

| Problem | Solution |
|---------|----------|
| Birds don't animate | Check Animator component has the correct Controller assigned. Check parameter names are EXACTLY: `FlySpeed`, `IsHit`, `IsDead` |
| Birds don't die on click | Check bird prefab Layer = `Bird`. Check CrosshairController Target LayerMask = `Bird` |
| Chest doesn't open | Check Layer = `Bird` on chest. Check Open Frames array is filled (not empty). Check BoxCollider2D exists |
| Crosshair doesn't pulse | Check CrosshairAnimator is on the SAME GameObject as CrosshairController |
| Score doesn't update | Check ScoreManager exists in scene with Score Text assigned |
| Knight falls through ground | Check Ground has BoxCollider2D. Check Knight has Rigidbody2D. Check Ground Layer matches Knight's Ground Layer mask |
| Compiler errors about `linearVelocity` | You're on Unity < 6. Change `linearVelocity` to `velocity` in KnightAnimationController.cs (lines 85 and 101) |
| `TMPro.EditorUtilities` error | This was in the old CrosshairController — the new one doesn't have it. If error persists, reimport the script |

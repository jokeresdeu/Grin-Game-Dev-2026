# Lab 4 — Setup Guide (Animations in Unity)

This guide walks through every Editor-side step needed to complete Lab 4 on top of the Lab 3 Chicken Hunt project. All C# scripts are already written and committed — you only need to wire the Animators in the Unity Editor.

## Branch sanity check

```bash
git branch --show-current
# expected: labs/GD-4-domin-adding_animation_to_chicken_hunt
```

If the branch is wrong, run:

```bash
git checkout labs/GD-3-domin-adding_mechanics_to_chicken_hunt
git checkout -b labs/GD-4-domin-adding_animation_to_chicken_hunt
```

## What we are animating (5+ objects)

| # | GameObject | Type of animation | Controller |
|---|------------|-------------------|------------|
| 1 | Bird (`Bird1_3_0` prefab) | Idle flap (movement) | `Bird1_3_0.controller` (already in asset pack — re-use) |
| 2 | Crosshair | State change (Shoot pulse, Reload spin) | `CrosshairAnimator.controller` (create) |
| 3 | Player Coop (the house/coop sprite) | Damage + Death + Idle breathing | `PlayerCoopAnimator.controller` (create) |
| 4 | Score Popup | Pop + fade-up | `ScorePopupAnimator.controller` (create) |
| 5 | Game Over Panel | Fade-in + scale-in | `GameOverPanelAnimator.controller` (create) |
| 6 | Title / Banner (bonus) | Idle bob | code-only (`IdleBobAnimator.cs`) |

All Animators are driven from code via `Animator.SetTrigger`/`SetFloat`/`SetBool`. The C# side is already done — you only need the Animator Controllers and Animation Clips.

---

## 1. Bird animations (Bird1_3_0 prefab) — re-use existing

**Goal:** wings flap continuously while the bird flies. The asset pack already provides this — we only need to wire it onto the prefab.

### Steps

1. Open `Assets/Projects/MegaSuperChallengeShot/Prefabs/Bird1_3_0.prefab` in Prefab Mode.
2. Select the root → **Add Component → Animator**.
3. In the Animator component's `Controller` field, drag in:
   `Assets/Projects/MegaSuperChallengeShot/Graphics/Cute_Birds(4 in 1)Mini/B1/Animations/Bird1_3_0.controller`
4. Save the prefab (`Ctrl/Cmd+S`).

That's it. The existing controller has a single state that loops `Bird1_3.anim` (the wing-flap sprite-swap), so every spawned bird flaps its wings forever — which satisfies the "movement animation" requirement.

> **No new files, no new clips, no parameters to set.** The bird's death is handled by `Destroy(gameObject)` in `CrosshairController.KillBird` exactly like Lab 3. Damage/Death animations live on the Player Coop instead (see § 3).

---

## 2. Crosshair animations

**Goal:** crosshair pulses on shoot, spins on reload.

### 2.1 Folder + clips

1. Create folder `Assets/Projects/MegaSuperChallengeShot/Animations/Crosshair/`.
2. Select the Crosshair GameObject in the Main scene. Open the Animation window → **Create** → save as `Crosshair_Idle.anim` inside that folder.

#### Crosshair_Idle (loop, 1s)
- Transform → Scale: 0s → 1; 0.5s → 1.05; 1s → 1.
- Tick Loop Time.

#### Crosshair_Shoot (one-shot, 0.2s)
- Transform → Scale: 0s → 1; 0.05s → 1.4; 0.2s → 1.
- Untick Loop Time.

#### Crosshair_Reload (one-shot, 0.5s)
- Transform → Rotation Z: 0s → 0°; 0.5s → 360°.
- Untick Loop Time.

### 2.2 CrosshairAnimator controller

Parameters:
- `Shoot` — Trigger
- `Reload` — Trigger

States: `Idle` (default, Motion = Crosshair_Idle), `Shoot`, `Reload`.

Transitions:

| From | To | Condition | Has Exit Time |
|------|----|-----------|---------------|
| Idle | Shoot | Trigger `Shoot` | ❌ |
| Shoot | Idle | exit time = 1 | ✅ |
| Idle | Reload | Trigger `Reload` | ❌ |
| Reload | Idle | exit time = 1 | ✅ |

### 2.3 Wire in scene

1. Select Crosshair in scene → **Add Component → Animator** if missing, set Controller = `CrosshairAnimator`.
2. **Add Component → Crosshair Animator** (the new script).
3. On the existing `Crosshair Controller` component, drag the same GameObject into the new `Animator` field (or leave empty — `GetComponent` will find it).
4. Save the scene.

---

## 3. Player Coop (house) animations

**Goal:** the coop sprite shakes when birds escape past it (damage), shrinks on death.

If you don't have a coop sprite in the scene yet, create one:
1. In the Main scene hierarchy, right-click → **Create Empty** → rename `PlayerCoop`.
2. Add child `SpriteRenderer` → use any sprite from `Graphics/Cute_Birds(4 in 1)Mini/Props/` (or a placeholder square sprite).
3. Position it bottom-center of the screen.

### 3.1 Animation clips

Create folder `Animations/PlayerCoop/`.

#### PlayerCoop_Idle (loop, 2s)
- Transform → Scale Y: 0s → 1; 1s → 1.03; 2s → 1 (subtle breathing).
- Loop Time on.

#### PlayerCoop_Damage (0.3s, one-shot)
- SpriteRenderer → Color: 0s → white; 0.1s → red; 0.3s → white.
- Note: the shake is *also* done in code (in `PlayerCoopAnimator.ShakeRoutine`) — the clip is just the color flash.

#### PlayerCoop_Death (0.5s, one-shot)
- Transform → Scale: 0s → (1,1,1); 0.5s → (1,0,1) (squish).
- SpriteRenderer → Color.a: 0s → 1; 0.5s → 0.

### 3.2 Controller `PlayerCoopAnimator.controller`

Parameters:
- `Damage` — Trigger
- `Die` — Trigger
- `IsAlive` — Bool (default = true)

States: `Idle` (default), `Damage`, `Death`.

Transitions:

| From | To | Condition | Has Exit Time |
|------|----|-----------|---------------|
| Idle | Damage | Trigger `Damage` | ❌ |
| Damage | Idle | exit time = 1, AND `IsAlive == true` | ✅ |
| Any State | Death | Trigger `Die` AND `IsAlive == false` | ❌ |

### 3.3 Wire

1. PlayerCoop GameObject → Add `Animator` (Controller = `PlayerCoopAnimator`).
2. Add Component → `Player Coop Animator`.
3. Select the GameObject holding `PlayerHealth` (likely the same Player GameObject) → drag the PlayerCoop GameObject into the new `Coop Animator` field on `PlayerHealth`.

---

## 4. Score Popup prefab

**Goal:** when a bird is killed, a `+10` text floats up from the bird's position, scales in, fades out.

### 4.1 Prefab

1. Hierarchy → Create → UI → Canvas (if no world-space Canvas exists yet) OR use the existing main Canvas in **Screen Space – Overlay** mode and convert positions in code (easier: add an empty GameObject parent named `PopupRoot` and use a world-space approach below).
2. Easiest path: Create Empty → name `ScorePopup`.
3. Add child → Create → 3D Object → Text - TextMeshPro (or 2D TextMeshPro with a SpriteRenderer-friendly setup). Set RectTransform width/height ~2x1 world units, Font Size ~6, color yellow.
4. On the root, **Add Component → Animator** (Controller will be assigned in step 4.3).
5. **Add Component → Score Popup** (the new script). Drag the child TMP into the `Label` field.
6. Drag the GameObject into `Assets/Projects/MegaSuperChallengeShot/Prefabs/` → name `ScorePopup.prefab`. Delete the instance from the scene.

### 4.2 Animation clip

Create folder `Animations/ScorePopup/`. Create clip `ScorePopup_Pop.anim` (one-shot, 0.8s):

- Transform → Scale: 0s → (0,0,1); 0.1s → (1.4,1.4,1); 0.25s → (1,1,1); 0.8s → (1,1,1).
- Transform → Position.y (local): 0s → 0; 0.8s → 1.2.
- TextMeshPro → Color.a: 0s → 1; 0.6s → 1; 0.8s → 0.

### 4.3 Controller `ScorePopupAnimator.controller`

- No parameters.
- One state `Pop` (default), Motion = `ScorePopup_Pop.anim`, **not looping**.
- The script destroys the GameObject after `_lifetime` (0.8s) so the clip plays exactly once.

Assign the controller to the prefab's Animator.

### 4.4 Wire into ScoreManager

1. In the Main scene, find the GameObject with `ScoreManager` (usually `GameManager` GameObject or `UI` GameObject).
2. Drag the `ScorePopup.prefab` into `Popup Prefab` field.
3. Optionally drag a parent transform into `Popup Parent` to keep hierarchy clean (otherwise it nests under `ScoreManager`).

---

## 5. Game Over Panel fade-in

**Goal:** when player dies, the Game Over panel fades + scales in instead of popping.

### 5.1 Animation clip

Create folder `Animations/GameOver/`.

#### GameOver_FadeIn (one-shot, 0.4s)
- The Panel needs a `CanvasGroup` component first: select the Panel → **Add Component → Canvas Group**. Default alpha = 1.
- In Animation window with Panel selected, create `GameOver_FadeIn.anim`:
  - CanvasGroup → Alpha: 0s → 0; 0.4s → 1.
  - Transform → Scale: 0s → (0.6,0.6,1); 0.4s → (1,1,1).
- Loop Time **off**.

### 5.2 Controller `GameOverPanelAnimator.controller`

- Parameter: `Show` (Trigger).
- States: `Hidden` (default — empty, no motion) + `Showing` (Motion = `GameOver_FadeIn`).
- Transition: `Hidden → Showing` on Trigger `Show`, Has Exit Time = ❌, Transition Duration = 0.

### 5.3 Wire

1. Panel GameObject → Add Animator (Controller = `GameOverPanelAnimator`).
2. **Add Component → Game Over Panel Animator**.
3. On `GameOverUI` script, drag the Panel GameObject into the new `Panel Animator` field.

> **Gotcha:** the panel starts inactive (it's `SetActive(false)` in `GameOverUI.Awake`). When `Show(...)` runs it calls `SetActive(true)` *then* fires the Trigger — that order matters because Animators only respond when the GameObject is active.

---

## 6. Title / Banner idle bob (bonus)

If you have a logo, title text, or a flag in the scene that should sway gently:

1. Select the object.
2. **Add Component → Idle Bob Animator** (the new code-only script).
3. Inspector fields:
   - `Bob Axis`: (0, 1, 0) for vertical sway, (1, 0, 0) for horizontal, etc.
   - `Amplitude`: 0.15
   - `Frequency`: 1.5
   - `Rotation Amplitude`: 5 (optional — degrees of wiggle)
   - `Phase Offset`: 0 (set different values on different objects to desync them).

This needs **no** Animation Clip and **no** Animator Controller — it's a pure-code oscillator. Good for backgrounds, props, banners.

---

## 7. Final scene wiring checklist

Open `Assets/Projects/MegaSuperChallengeShot/Scenes/Main.unity` and verify:

- [ ] Bird prefab has `Animator` (Controller = `Bird1_3_0.controller` from the asset pack) + `BirdMover` (existing).
- [ ] Crosshair has `Animator` (CrosshairAnimator) + `CrosshairAnimator` script + `CrosshairController` (existing).
- [ ] `CrosshairController._animator` field points to the new `CrosshairAnimator` component.
- [ ] PlayerCoop has `Animator` (PlayerCoopAnimator) + `PlayerCoopAnimator` script.
- [ ] `PlayerHealth._coopAnimator` field points to the PlayerCoop GameObject.
- [ ] `ScoreManager._popupPrefab` is set to `ScorePopup.prefab`.
- [ ] Game Over Panel has `CanvasGroup` + `Animator` (GameOverPanelAnimator) + `GameOverPanelAnimator` script.
- [ ] `GameOverUI._panelAnimator` field references the Panel's `GameOverPanelAnimator`.
- [ ] At least one object (Title / Logo / Banner) has `IdleBobAnimator`.

---

## 8. Test plan (Play mode)

1. **Idle frame** — Birds flap their wings, title gently bobs, coop breathes.
2. **Movement** — Birds fly across the screen (Lab 3 `BirdMover` translation) and their wings flap continuously (`Bird1_3.anim` loop).
3. **Hit** — Left-click a bird: bird is destroyed instantly (Lab 3 behaviour); `+10` score popup spawns at the kill location.
4. **Shotgun** — Right-click: crosshair pulse, every bird in radius dies.
5. **Reload** — Press `R`: crosshair spins, shots reset to 10/10.
6. **Damage** — Let a bird reach the right edge: coop flashes red, shakes via code, HP decreases.
7. **Game Over** — Drain HP: coop shrinks + fades, Game Over panel fades + scales in.
8. **Score popup** — every kill spawns a `+10` that floats up and fades.

---

## 9. Bonus tasks

### Бонус 1 — Spine animation (+5)
1. Window → Package Manager → "+" → Add package from git URL: `https://github.com/EsotericSoftware/spine-unity.git`.
2. Find a free Spine sample (e.g. *Spineboy* from `esotericsoftware.com/spine-demos`). Drop the `.json` + `.atlas.txt` + `.png` into `Assets/Imported/Spine/`.
3. Right-click the imported `_SkeletonData` asset → `Spine → Instantiate (Skeleton Animation)`. The Spine GameObject lands in the scene.
4. Place it as decoration (a tree, a hunter NPC, an animated logo) and pick an animation in `Skeleton Animation → Animation Name` dropdown.

### Бонус 2 — Built-up character with 3 animations (+5)
1. Find a "character parts" file (HUMAN or similar from the Asset Store, free packs work). Drop into `Assets/Imported/Human/`.
2. Create empty GameObject `Hero` → attach child sprites: Body, Head, ArmL, ArmR, LegL, LegR. Parent the limbs to the body.
3. Create 3 clips (`Hero_Walk`, `Hero_Jump`, `Hero_Crouch`) using the Animation window — animate the limbs' local rotations/positions on the timeline.
4. Build a small `HeroAnimator.controller` with Parameters `Speed` (Float), `IsCrouching` (Bool), `Jump` (Trigger). Idle ↔ Walk transitions on `Speed > 0.1`, Crouch on `IsCrouching`, Jump on trigger.
5. Drive it from a simple `HeroController.cs` reading WASD/Space.

---

## 10. Commit + push

```bash
git status                          # confirm new files
git add Assets/Projects/MegaSuperChallengeShot
git commit -m "GD-4: Add animations to Chicken Hunt"
git push -u origin labs/GD-4-domin-adding_animation_to_chicken_hunt
```

## 11. What's in the lab report

- Branch URL.
- Screenshot of the Animator graph for the Bird (showing Idle/Hit/Death + transitions).
- Screenshot of two clips in the Animation window timeline (e.g. `PlayerCoop_Damage` + `GameOver_FadeIn`).
- 3–5 lines on each animated object: what state changes drive it, what parameter the code sets.
- A short table mapping the 5 required animation categories → which clips fulfill each:

| Required category | Implementation |
|-------------------|----------------|
| Movement | `Bird1_3.anim` (wing flap loop from asset pack) + `BirdMover` translation |
| Damage/Death | `PlayerCoop_Damage`, `PlayerCoop_Death` |
| State change | `Crosshair_Shoot`/`Crosshair_Reload`, `GameOver_FadeIn` |
| Idle | `Bird1_3.anim` (flap), `Crosshair_Idle`, `PlayerCoop_Idle`, `IdleBobAnimator` |
| Other | `ScorePopup_Pop` floating text |

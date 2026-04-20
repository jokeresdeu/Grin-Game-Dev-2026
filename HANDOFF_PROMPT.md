# Lab 4 — Continuation Prompt

## WHO
Student: **Ann Karpenko** (work done by Dmytro, dmytro.domin@skelar.tech)
Branch: `GD-4-karpenko-adding_animation_to_chicen_hunt`
Project: Unity 6.3 LTS, URP, 2D "Chicken Hunt" (MegaSuperChallengeShot)

## WHAT THE LAB REQUIRES
Add animations to at least 5 game objects, control them from code based on state, optional Spine + composite character. Deliver: all scripts, prefabs, setup guide, Ukrainian-language report (.docx), git commit & push.

## WHAT IS ALREADY DONE

### All 13 C# scripts are written and in the repo:
Path: `Assets/Projects/MegaSuperChallengeShot/Scripts/`

1. **BirdAnimationController.cs** — Animator-optional controller. Death = coroutine (red flash → fall+rotate+fade → destroy). Uses Animator params FlySpeed(float), IsHit(trigger), IsDead(bool) if Animator present, gracefully handles null Animator.
2. **BirdMover.cs** — Horizontal movement, `SetDirection(bool)` for spawner, sprite flip (faces LEFT by default, negative scale.x = faces right), calls `OnShot()` for animated death.
3. **BirdSpawner.cs** — Timed multi-prefab spawner, 3 spawn points, difficulty ramp, forces Z=0 on spawn, calls `mover.SetDirection()`.
4. **ChestAnimationController.cs** — Frame-by-frame open/close via sprite array, idle bounce coroutine, OnMouseDown toggle, auto-close after delay, awards score.
5. **CloudMover.cs** — Horizontal drift with wrap-around, randomizes alpha+scale on wrap for depth.
6. **CrosshairAnimator.cs** — Coroutine animations: idle pulse, shoot punch+flash, reload spin, empty shake.
7. **CrosshairController.cs** — Mouse follow, Physics2D.OverlapPointAll on "Bird" LayerMask, integrates with CrosshairAnimator and BirdMover.OnShot().
8. **GameAnimationManager.cs** — Scene-scoped singleton (no DontDestroyOnLoad), tracks bird count + death count + global speed.
9. **HumanCompositeController.cs** — BONUS: 7 body part children (Head, BodyUpper, BodyLower, LeftArm, RightArm, Legs, Sword). Procedural Walk(bob+arm/leg swing), Jump(squash-stretch), Crouch(compress). Arrow keys + Space + S.
10. **KnightAnimationController.cs** — BONUS: State machine (Idle/Walk/Jump/Crouch), sprite-frame animation from arrays. Uses Rigidbody2D.linearVelocity (Unity 6). Ground check via OverlapCircle.
11. **PropFloatAnimator.cs** — Idle animation: vertical bob, rotation sway, scale breathe. Randomized phase.
12. **CloudMover.cs** + **PropFloatAnimator.cs** used together on clouds.
13. **SpriteAnimator.cs** — Generic code-driven sprite animator, Loop/PingPong/PlayOnce modes.
14. **ScoreManager.cs** — Scene-scoped singleton, AddScore(), AddScore(int), ResetScore().

### Additional deliverables:
- **UNITY_SETUP_GUIDE.md** — 15-phase setup guide (Phases 0-15)
- **Lab4_Report_Karpenko.docx** — Ukrainian-language report

### Git status:
- 42 files staged, 11 files with additional unstaged changes
- NOT YET COMMITTED — needs `git add` + `git commit` + `git push`

## WHAT IS BROKEN / NEEDS FIXING

### CRITICAL: Scene objects invisible in Game view (visible in Scene view)
**Root cause:** URP Renderer2D + Sprite-Lit-Default material requires a 2D Light to render in Game view. Scene view bypasses this.

**Fix needed — choose ONE:**
- **Option A (recommended):** Change SpriteRenderer Material on ALL manually-placed scene objects from `Sprite-Lit-Default` to `Sprite-Unlit-Default`. Affects: Knight, Ground, Chest_1, Cloud_1/2/3, Prop_Tree, Prop_Bush, HumanComposite children (Head, BodyUpper, BodyLower, LeftArm, RightArm, Legs, Sword).
- **Option B:** Add a `Global Light 2D` (GameObject → Light → Global Light 2D) AND ensure it stays in the scene. Previous attempts may have failed due to it being accidentally deleted or not saved.

### Bird prefabs — some had broken Animator clips:
- **Bird1 (B1):** WORKS — original asset pack .anim clips properly bound
- **Bird2 (B2 Duck), Bird3 (B3 Egret), Bird4 (B4):** Had broken Fly state animations (Clip Count: 0 or wrong bindings making birds invisible). User fixed some "by trial and error" selecting different .anim files. If still broken: remove Animator component from prefab (BirdAnimationController.cs now handles null Animator gracefully), OR re-drag correct .anim into Fly state Motion field.

### Knight not visible:
- Script works perfectly (logs show state changes: Idle→Walk→Jump→Crouch)
- Has Rigidbody2D, GroundCheck child, Ground Layer properly set
- Knight does NOT use Animator Controller — pure code-driven sprite swap
- Invisible because of Sprite-Lit-Default material (see fix above)
- Knight has NO Animator component (unlike birds), so removing Animator won't help — must change material

### Main Camera settings (MUST be correct):
- Position: **(0, 0, -10)**
- Scale: **(1, 1, 1)**
- Projection: Orthographic, Size: **5**
- Background Type: **Solid Color** (NOT Skybox)
- Renderer: **Default Renderer (Renderer2D)**
- Culling Mask: **Everything**
These got accidentally changed during debugging — verify they're correct.

### Canvas / UI:
- Canvas Scale gets auto-managed by Unity (Screen Space - Overlay)
- ScoreText and ShotsText overlap ("6/Score: 0") — may need repositioning
- Canvas Scaler: Reference Resolution 800x600, Scale With Screen Size

## SCENE HIERARCHY (expected)
```
asd (scene root)
├── Main Camera
│   ├── GameAnimationManager
│   ├── ScoreManager
│   └── GameAnimationManager (duplicate — can remove one)
├── Canvas
│   ├── ScoreText
│   └── ShotsText
├── BirdSpawner
├── Crosshair
├── Chest_1
├── Cloud_1
├── Cloud_2
├── Cloud_3
├── Prop_Tree
├── Prop_Bush
├── Knight
│   └── GroundCheck
├── Ground
├── HumanComposite
│   ├── Head
│   ├── BodyUpper
│   ├── BodyLower
│   ├── LeftArm
│   ├── RightArm
│   ├── Legs
│   └── Sword
├── Global Light 2D (if using Option B)
├── Bird1_1_0 (test bird placed manually — can remove)
└── EventSystem
```

## RECOMMENDED POSITIONS (for properly visible scene)
- **Main Camera:** (0, 0, -10)
- **Knight:** (-3, -3, 0), Scale (1, 1, 1)
- **Ground:** (0, -4.5, 0), Scale (30, 1, 1)
- **Chest_1:** (5, -3, 0), Scale (1, 1, 1)
- **Prop_Tree:** (-7, -2, 0), Scale (1, 1, 1)
- **Prop_Bush:** (7, -2, 0), Scale (1, 1, 1)
- **Cloud_1:** (-5, 4, 0), Scale (2, 2, 1)
- **Cloud_2:** (0, 3.5, 0), Scale (1.5, 1.5, 1)
- **Cloud_3:** (5, 4.5, 0), Scale (1.8, 1.8, 1)
- **HumanComposite:** (3, -3, 0), Scale (1, 1, 1)
- **BirdSpawner spawn points:** Y positions around -1, 1, 3 (X=0, Z=0)

## REMAINING TASKS

1. **Fix visibility** — Change all scene object materials to Sprite-Unlit-Default OR add working Global Light 2D
2. **Verify all objects are visible in Game view** — Knight, Chest, Clouds, Trees, Ground, HumanComposite, Birds
3. **Test gameplay** — Birds spawn, fly, can be shot (death animation plays), score increments, crosshair animations work, Knight moves with arrow keys, chest opens on click
4. **Remove test Bird1_1_0** from hierarchy if present
5. **Remove duplicate GameAnimationManager** if present
6. **Git commit & push:**
```bash
cd /path/to/Grin-Game-Dev-2026
git add -A
git commit -m "Lab 4: Add animations to Chicken Hunt — 5+ animated objects, code-driven animation, bonus knight & composite character"
git push origin GD-4-karpenko-adding_animation_to_chicen_hunt
```
7. **Verify report** — Lab4_Report_Karpenko.docx exists and is complete (Ukrainian language)

## KEY DEBUGGING LESSONS (for reference)
- Birds at Z=-10 → invisible. Fix: force Z=0 in BirdSpawner
- Camera must stay at Z=-10 for 2D, NOT Z=0
- Bird sprites face LEFT by default → moving right needs negative scale.x
- Dynamic Rigidbody2D breaks Physics2D.OverlapPointAll → removed from bird prefabs
- Animator clips with wrong bindings override sprite to null → remove Animator or recreate clips on the prefab
- Sprite-Lit-Default needs 2D Light in URP Renderer2D → use Sprite-Unlit-Default instead
- Scene view shows objects regardless of lighting; Game view requires proper lighting setup

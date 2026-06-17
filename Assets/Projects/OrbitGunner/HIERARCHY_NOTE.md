# OrbitGunner — natural scene hierarchy (experimental branch)

This branch (`tmp/GD-6-domin-natural-hierarchy`) rebuilds the project with a
**conventional, Inspector-wired scene hierarchy** instead of constructing everything
in code at runtime.

## What changed vs. the main lab branch
- Runtime scripts are now plain components with `[SerializeField]` references that you
  wire in the Inspector (e.g. `HUDController` has fields for each text/bar; the menu
  controllers reference their buttons). Buttons are hooked up in `Awake` via
  `onClick.AddListener`.
- Enemies and bullets are real **prefabs** (`Prefabs/Enemy.prefab`, `Prefabs/Bullet.prefab`)
  that the spawner / pool `Instantiate` — not GameObjects built from scratch in code.
- The runtime sprite generator (`SpriteFactory`) and the code-UI builder (`UiFactory`)
  and the runtime bootstrappers (`GameBootstrap`/`MenuBootstrap`) are **removed**.
- Sprites use Unity's built-in UI sprites (circle = Knob, square = Background), so there
  are no image assets to manage.

## How to generate the scenes (one time, in the Editor)
The scene hierarchy is created by an **edit-time** tool (not at runtime):

1. Open the project in Unity 6 and let it compile.
2. Menu bar → **OrbitGunner ▸ Build Scenes (Menu + Game)**.
3. This builds a full, normal hierarchy into both scenes, creates the two prefabs,
   wires every reference, adds the scenes to Build Settings, and saves. Open
   `Scenes/OrbitGunner_Game.unity` and you'll see a conventional hierarchy:

```
OrbitGunner_Game
├── Main Camera            (Camera + CameraShake)
├── EventSystem
├── Systems                (GameManager, ScoreManager, OverdriveMeter,
│                           DifficultyDirector, NovaBurst, BulletPool, EnemySpawner)
├── Core                   (SpriteRenderer + CoreHealth)
│   └── Nucleus
├── Turret                 (Weapon + TurretController)
│   └── Barrel
├── Enemies                (spawn container)
├── Bullets                (pool container)
└── HUDCanvas              (Canvas + HUDController + PauseMenuController + GameOverUI)
    ├── Score / Best / Wave / Combo
    ├── HpBar / OverdriveBar
    ├── PausePanel  (Resume / Restart / Menu / Quit)
    └── GameOverPanel (Score / Best / NewBest / Restart / Menu)
```

After running it once you can edit everything in the Inspector like a normal Unity scene;
the builder script (`Editor/OrbitGunnerSceneBuilder.cs`) can be kept as tooling or deleted.

> Why a builder instead of pre-saved scenes? The scenes were authored in an environment
> that can't run Unity; an edit-time builder produces a guaranteed-valid hierarchy with
> correctly-wired references, which hand-written scene YAML cannot guarantee.

Note: enemies use built-in sprites (circle / rounded square), so the "Runner" type is a
square rather than a triangle — purely cosmetic.

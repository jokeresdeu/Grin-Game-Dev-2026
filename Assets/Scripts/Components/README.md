# Game Components Documentation

A collection of ready-to-use MonoBehaviour components for 2D games. Add these to GameObjects to instantly get game functionality without writing code.

---

## Table of Contents

- [Quick Start](#quick-start)
- [Movement Components](#movement-components)
- [Spawn Components](#spawn-components)
- [Interaction Components](#interaction-components)
- [Utility Components](#utility-components)
- [Recipes](#recipes)

---

## Quick Start

1. Select a GameObject in your scene
2. Click **Add Component** in the Inspector
3. Search for the component name (e.g., "HorizontalMover")
4. Configure the exposed fields in the Inspector
5. Press Play!

### Tips

- Most components work independently - mix and match as needed
- Components with `[RequireComponent]` will auto-add dependencies (like Rigidbody2D)
- Use **UnityEvents** (onDamage, onCollect, etc.) to connect components without code
- Gizmos help visualize settings in Scene view (select the object to see them)

---

## Movement Components

Located in: `Components/Movement/`

### HorizontalMover

Moves object left/right using A/D or Arrow keys.

| Field | Description |
|-------|-------------|
| Speed | Movement speed in units per second |
| Flip Sprite | Automatically flip sprite based on direction |

**How to use:**
1. Add to any GameObject with a SpriteRenderer
2. Set speed
3. Play and use A/D or Left/Right arrows

---

### VerticalMover

Moves object up/down using W/S or Arrow keys.

| Field | Description |
|-------|-------------|
| Speed | Movement speed in units per second |

**How to use:**
1. Add to any GameObject
2. Set speed
3. Play and use W/S or Up/Down arrows

---

### TopDownMover

Moves object in all directions (top-down games like Zelda).

| Field | Description |
|-------|-------------|
| Speed | Movement speed in units per second |
| Normalize Movement | Prevent faster diagonal movement |

**How to use:**
1. Add to player GameObject
2. Set speed
3. Use WASD or Arrow keys to move in any direction

---

### RigidbodyHorizontalMover

Physics-based horizontal movement. Better for platformers (respects collisions).

| Field | Description |
|-------|-------------|
| Speed | Movement speed |
| Flip Sprite | Flip sprite based on direction |

**How to use:**
1. Add to GameObject (auto-adds Rigidbody2D)
2. Add a Collider2D to your object
3. Set speed and play

**Note:** This sets velocity directly, which works well with RigidbodyJumper.

---

### RigidbodyTopDownMover

Physics-based movement for top-down games.

| Field | Description |
|-------|-------------|
| Speed | Movement speed |
| Normalize Movement | Prevent faster diagonal movement |

**How to use:**
1. Add to player (auto-adds Rigidbody2D)
2. Gravity is automatically set to 0
3. Use WASD to move

---

### RigidbodyJumper

Makes object jump with ground detection. Essential for platformers.

| Field | Description |
|-------|-------------|
| Jump Force | How high the jump goes |
| Jump Key | Key to press for jumping (default: Space) |
| Ground Check | Transform position to check for ground |
| Ground Check Radius | Size of ground detection circle |
| Ground Layer | Which layers count as "ground" |

**How to use:**
1. Add to player (auto-adds Rigidbody2D)
2. Create an empty child GameObject, position it at the player's feet
3. Assign it to "Ground Check"
4. Set "Ground Layer" to match your ground objects' layer
5. Press Space to jump!

**Important:** Your ground/platform objects must be on the layer you specify in Ground Layer.

---

### Rotator

Continuously spins the object.

| Field | Description |
|-------|-------------|
| Rotation Speed | Degrees per second |
| Clockwise | Rotation direction |

**How to use:**
1. Add to any GameObject
2. Set rotation speed
3. Great for spinning coins, hazards, or decorations

---

### FollowTarget

Makes object follow another object (cameras, enemies, pets).

| Field | Description |
|-------|-------------|
| Target | The Transform to follow |
| Follow Speed | How fast to catch up (higher = tighter follow) |
| Smooth Follow | Enable smooth interpolation |
| Offset | Distance offset from target |
| Follow X/Y | Which axes to follow |

**How to use (Camera):**
1. Add to Main Camera
2. Drag player into "Target" field
3. Set Offset Z to -10 (camera needs to be behind objects)
4. Adjust Follow Speed for feel

---

### LookAtMouse

Rotates object to face the mouse cursor. Perfect for aiming.

| Field | Description |
|-------|-------------|
| Rotation Offset | Adjust if sprite doesn't face right by default |
| Smooth Rotation | Enable smooth turning |
| Rotation Speed | How fast to turn (when smooth) |

**How to use:**
1. Add to player or weapon
2. If sprite faces up instead of right, set Rotation Offset to -90
3. Play and aim with mouse

---

### MoveToClick

Point-and-click movement (click somewhere, object moves there).

| Field | Description |
|-------|-------------|
| Speed | Movement speed |
| Stopping Distance | How close to get before stopping |
| Mouse Button | 0 = left click, 1 = right click |
| Flip Sprite | Flip based on movement direction |
| Show Target Indicator | Show a marker where clicked |
| Target Indicator Prefab | Prefab to spawn at click location |

**How to use:**
1. Add to player
2. Set speed
3. Click anywhere to move there

---

### Draggable

Allows dragging object with mouse. Requires a Collider2D.

| Field | Description |
|-------|-------------|
| Smooth Drag | Smooth movement while dragging |
| Smooth Speed | How smooth the drag feels |
| Return To Start | Object returns when released |
| Constrain X/Y | Lock movement on an axis |

**How to use:**
1. Add to any GameObject
2. Add a Collider2D (BoxCollider2D, CircleCollider2D, etc.)
3. Click and drag the object!

**Tip:** Enable "Return To Start" for puzzle games where pieces snap back.

---

### Oscillator

Moves object back and forth smoothly (sine wave motion).

| Field | Description |
|-------|-------------|
| Movement Distance | How far to move on X and Y |
| Speed | Oscillation speed |
| Start Offset | Phase offset (randomize multiple platforms) |

**How to use:**
1. Add to platform or obstacle
2. Set Movement Distance (e.g., X=3, Y=0 for horizontal)
3. Adjust speed

**Example:** Moving platform: X=5, Y=0, Speed=2

---

### FloatingMotion

Gentle bobbing motion for collectibles and items.

| Field | Description |
|-------|-------------|
| Float Height | How high/low it bobs |
| Float Speed | Speed of bobbing |
| Rotate | Also rotate while floating |
| Rotation Speed | How fast to rotate |
| Randomize Start | Different items bob out of sync |

**How to use:**
1. Add to coins, power-ups, or pickups
2. Default settings work well
3. Enable "Rotate" for extra polish

---

### PatrolPath

Move along a series of waypoints. Great for enemies and platforms.

| Field | Description |
|-------|-------------|
| Waypoints | Array of Transform positions |
| Speed | Movement speed |
| Wait Time | Pause duration at each waypoint |
| Loop | Return to start after last waypoint |
| Ping Pong | Go back and forth |
| Flip Sprite | Face movement direction |

**How to use:**
1. Create empty GameObjects as waypoints, position them in scene
2. Add PatrolPath to your enemy/platform
3. Drag waypoints into the Waypoints array
4. Choose Loop or Ping Pong behavior
5. Yellow gizmo lines show the path in Scene view

---

### AutoMover

Automatically moves in a direction. Perfect for projectiles.

| Field | Description |
|-------|-------------|
| Direction | Movement direction (X, Y) |
| Speed | Movement speed |
| Use Local Direction | Use object's rotation for direction |

**How to use:**
1. Add to projectile prefab
2. Set Direction (e.g., X=1, Y=0 for right)
3. Enable "Use Local Direction" if projectile should follow its rotation

---

## Spawn Components

Located in: `Components/Spawn/`

### Spawner

Spawn a prefab when pressing a key.

| Field | Description |
|-------|-------------|
| Prefab | What to spawn |
| Spawn Point | Where to spawn (default: this object) |
| Spawn Key | Key to press |
| Inherit Rotation | Spawned object matches rotation |
| Spawn Offset | Position offset |
| Cooldown | Minimum time between spawns |
| On Spawn | Event fired when spawning |

**How to use:**
1. Create a prefab you want to spawn
2. Add Spawner to player or spawner object
3. Drag prefab into Prefab field
4. Set the spawn key
5. Press key to spawn!

---

### TimedSpawner

Automatically spawns prefabs at intervals.

| Field | Description |
|-------|-------------|
| Prefab | What to spawn |
| Spawn Point | Where to spawn |
| Spawn Interval | Seconds between spawns |
| Initial Delay | Wait before first spawn |
| Spawn On Start | Spawn immediately when game starts |
| Max Spawns | Limit total spawns (-1 = unlimited) |
| Max Active Objects | Limit concurrent objects (-1 = unlimited) |

**How to use:**
1. Add to empty GameObject in scene
2. Assign prefab
3. Set interval
4. Use Max Active Objects to prevent too many enemies

**Note:** Max Active Objects uses tags - make sure your prefab has a tag set.

---

### RandomSpawner

Spawns prefabs at random positions within an area.

| Field | Description |
|-------|-------------|
| Prefabs | Array of prefabs to randomly choose from |
| Spawn Area Size | Width and height of spawn area |
| Spawn Area Offset | Offset from spawner position |
| Spawn Interval | Base time between spawns |
| Random Interval Variance | Randomize timing |
| Max Active Objects | Limit concurrent objects |

**How to use:**
1. Add to empty GameObject
2. Add prefabs to array
3. Set spawn area size (cyan rectangle in Scene view)
4. Position the spawner where you want the area

---

### ProjectileShooter

Shoot projectiles in a direction or toward mouse.

| Field | Description |
|-------|-------------|
| Projectile Prefab | The projectile to shoot |
| Fire Point | Where projectiles spawn |
| Projectile Speed | How fast projectiles move |
| Fire Key | Key to shoot (or use mouse) |
| Use Mouse Button | Use mouse instead of keyboard |
| Mouse Button | Which mouse button (0=left, 1=right) |
| Shoot Direction | FacingDirection, TowardMouse, or fixed direction |
| Fire Rate | Seconds between shots |
| Auto Fire | Hold button for continuous fire |

**How to use:**
1. Create a projectile prefab with:
   - Sprite
   - Collider2D (set as Trigger)
   - Rigidbody2D (Gravity Scale = 0)
   - DamageDealer component
   - Lifetime component
2. Add ProjectileShooter to player
3. Create empty child "FirePoint" positioned at gun barrel
4. Assign prefab and fire point
5. Choose direction mode

**Tip:** Use "TowardMouse" with "LookAtMouse" for twin-stick shooter feel.

---

### ObjectPool

Reuse objects instead of creating/destroying (better performance).

| Field | Description |
|-------|-------------|
| Prefab | Object to pool |
| Initial Pool Size | Pre-create this many objects |
| Expandable | Create more if pool is empty |
| Create Container | Organize in hierarchy |

**How to use (from code):**
```csharp
// Get object from pool
GameObject obj = ObjectPool.Instance.GetObject(position, rotation);

// Return object to pool (instead of Destroy)
ObjectPool.Instance.ReturnObject(obj);
```

---

## Interaction Components

Located in: `Components/Interaction/`

### Health

Tracks hit points. The foundation for anything that can be damaged.

| Field | Description |
|-------|-------------|
| Max Health | Starting/maximum HP |
| Current Health | (Runtime) Current HP |
| Invincible On Start | Start invincible |
| Invincibility Duration | Brief invincibility after taking damage |
| On Damage | Event when damaged |
| On Heal | Event when healed |
| On Death | Event when HP reaches 0 |

**How to use:**
1. Add to player, enemy, or destructible object
2. Set max health
3. Connect events to trigger behaviors:
   - On Damage → FlashOnDamage.Flash()
   - On Death → Destroy this GameObject, or GameManager.GameOver()

**Public methods (call from other scripts or UnityEvents):**
- `TakeDamage(float damage)`
- `Heal(float amount)`
- `SetInvincible(float duration)`
- `ResetHealth()`

---

### DamageDealer

Deals damage to objects with Health component.

| Field | Description |
|-------|-------------|
| Damage | Amount of damage to deal |
| Destroy On Hit | Destroy self after dealing damage |
| Target Layers | Which layers can be damaged |
| Use Trigger | Damage on trigger enter |
| Use Collision | Damage on collision |
| Damage Cooldown | Time before can damage same target again |
| On Deal Damage | Event when damage is dealt |

**How to use:**
1. Add to projectile, enemy, or hazard
2. Set damage amount
3. Choose trigger and/or collision
4. Set Target Layers to "Player" layer for enemies, "Enemy" layer for player attacks

**Important:** The target must have a Health component to take damage.

---

### Collectible

Makes object collectible (coins, power-ups, keys).

| Field | Description |
|-------|-------------|
| Collectible Type | Category name (e.g., "Coin", "Health", "Key") |
| Value | How much it's worth |
| Destroy On Collect | Remove after collection |
| Collect Delay | Delay before destroying |
| Collect Effect Prefab | Spawn effect when collected |
| On Collected | Event when collected |

**How to use:**
1. Add to pickup object
2. Add a Collider2D (set as Trigger)
3. Set type and value
4. Optionally assign a particle effect prefab

---

### Collector

Picks up Collectible objects.

| Field | Description |
|-------|-------------|
| Collectible Types | Array of types to collect (empty = all) |
| Collect All Types | Ignore type filter, collect everything |
| On Collect | Event with Collectible reference |
| On Value Collected | Event with value (int) |

**How to use:**
1. Add to player
2. Player needs a Collider2D
3. Connect On Value Collected to ScoreManager.AddScore()

**Example event setup:**
- On Value Collected → ScoreManager.AddScore (Dynamic int)

---

### Knockback

Pushes objects away on contact.

| Field | Description |
|-------|-------------|
| Knockback Force | Push strength |
| Target Layers | Which layers to affect |
| Direction | FromCenter, FixedDirection, or MovementDirection |
| Fixed Direction | Direction if using FixedDirection |
| Use Trigger/Collision | When to apply knockback |

**How to use:**
1. Add to enemy or hazard
2. Set force amount
3. Target must have Rigidbody2D

---

### Interactable

Object that can be interacted with using a key press.

| Field | Description |
|-------|-------------|
| Interact Key | Key to press (default: E) |
| One Time Interaction | Can only interact once |
| Interaction Prompt | UI element to show when nearby |
| On Interact | Event when interacted |
| On Player Enter/Exit | Events for proximity |

**How to use:**
1. Add to NPC, chest, door, or switch
2. Add Collider2D (set as Trigger)
3. Create UI prompt as child, assign to Interaction Prompt
4. Connect On Interact to your action (open door, show dialogue, etc.)

**Important:** Player must have tag "Player" for this to work.

---

### TriggerZone

Area that fires events when entered.

| Field | Description |
|-------|-------------|
| Filter By Tag | Only trigger for specific tag |
| Target Tag | Tag to filter (default: "Player") |
| Target Layers | Layer filter |
| One Time Trigger | Only trigger once ever |
| On Enter | Event when object enters |
| On Stay | Event while object is inside |
| On Exit | Event when object leaves |

**How to use:**
1. Add to empty GameObject
2. Add Collider2D (set as Trigger)
3. Size the collider to match your zone
4. Connect events

**Use cases:**
- Checkpoint: On Enter → Respawner.SetCheckpoint()
- Kill zone: On Enter → Health.TakeDamage(9999)
- Cutscene trigger: On Enter → Start cutscene

---

### Healer

Heals objects with Health component.

| Field | Description |
|-------|-------------|
| Heal Amount | HP to restore |
| Destroy On Heal | Remove after healing |
| Target Layers | Which layers to heal |
| Continuous Heal | Keep healing while in contact |
| Heal Interval | Time between continuous heals |

**How to use:**
1. Add to health pickup or healing zone
2. Add Collider2D (set as Trigger)
3. Set heal amount

---

## Utility Components

Located in: `Components/Utility/`

### ScreenWrapper

Wrap around screen edges (Asteroids-style).

| Field | Description |
|-------|-------------|
| Wrap Horizontal | Wrap left/right edges |
| Wrap Vertical | Wrap top/bottom edges |
| Buffer | Extra distance before wrapping |

**How to use:**
1. Add to player or any object that should wrap
2. Enable desired axes
3. Object exiting right appears on left

---

### ScreenClamper

Keep object within screen bounds.

| Field | Description |
|-------|-------------|
| Clamp Horizontal | Restrict left/right |
| Clamp Vertical | Restrict top/bottom |
| Padding | Distance from screen edge |

**How to use:**
1. Add to player
2. Object cannot leave visible area

---

### Lifetime

Auto-destroy after time. Essential for projectiles.

| Field | Description |
|-------|-------------|
| Lifetime | Seconds before destruction |
| Use Unscaled Time | Ignore time scale (pause) |
| On Lifetime End | Event before destroying |

**How to use:**
1. Add to projectile, effect, or temporary object
2. Set lifetime (e.g., 5 seconds)

---

### DestroyOnCollision

Destroy when hitting specified layers.

| Field | Description |
|-------|-------------|
| Destroy On Layers | Which layers trigger destruction |
| Use Trigger | Check trigger collisions |
| Use Collision | Check physical collisions |
| Destroy Effect Prefab | Spawn effect on destruction |

**How to use:**
1. Add to projectile
2. Set layers (e.g., "Walls" layer)
3. Projectile destroys when hitting walls

---

### DestroyOnInvisible

Destroy when object leaves camera view.

| Field | Description |
|-------|-------------|
| Delay | Wait before destroying |
| Only After Visible | Must be seen first before this activates |

**How to use:**
1. Add to projectiles or spawned enemies
2. Cleans up objects that leave play area

**Note:** Requires a Renderer component (SpriteRenderer).

---

### CameraFollow2D

Camera follows a target with smooth movement.

| Field | Description |
|-------|-------------|
| Target | Transform to follow |
| Smooth Speed | Follow smoothness |
| Offset | Position offset (Z should be -10) |
| Follow X/Y | Which axes to follow |
| Use Bounds | Limit camera movement |
| Min/Max X/Y | Camera boundaries |
| Look Ahead | Camera leads in movement direction |
| Look Ahead Distance | How far ahead to look |

**How to use:**
1. Add to Main Camera
2. Drag player to Target
3. Set Offset to (0, 0, -10)
4. Enable Use Bounds for level boundaries (yellow rectangle in Scene)

---

### ScoreManager

Global score tracking with high score persistence.

| Field | Description |
|-------|-------------|
| Starting Score | Initial score value |
| On Score Changed | Event with current score |
| On High Score Beaten | Event when new high score |

**How to use:**
1. Add to empty GameObject (creates itself as singleton)
2. Access from anywhere: `ScoreManager.Instance.AddScore(10)`
3. Connect On Score Changed to UI text update

**Properties:**
- `ScoreManager.Instance.CurrentScore`
- `ScoreManager.Instance.HighScore`

**Methods:**
- `AddScore(int points)`
- `ResetScore()`
- `ResetHighScore()`

---

### GameManager

Handles pause, restart, and game state.

| Field | Description |
|-------|-------------|
| Pause Key | Key to pause (default: Escape) |
| Restart Key | Key to restart (default: R) |
| On Game Start | Event when game begins |
| On Game Pause | Event when paused |
| On Game Resume | Event when resumed |
| On Game Over | Event when game ends |
| On Game Restart | Event when restarting |

**How to use:**
1. Add to empty GameObject (singleton)
2. Connect events to show/hide pause menu
3. Call `GameManager.Instance.GameOver()` when player dies

**Methods:**
- `PauseGame()` / `ResumeGame()` / `TogglePause()`
- `GameOver()`
- `RestartGame()`
- `LoadScene(string name)` or `LoadScene(int index)`
- `QuitGame()`

---

### Respawner

Respawn at checkpoint or start position.

| Field | Description |
|-------|-------------|
| Spawn Point | Current checkpoint transform |
| Use Start Position | Use initial position instead |
| Respawn Delay | Wait before respawning |
| Reset Velocity | Zero out velocity on respawn |
| Reset Health | Restore health on respawn |

**How to use:**
1. Add to player
2. Call `Respawn()` from Health.OnDeath event
3. Use `SetCheckpoint()` from checkpoint TriggerZone

---

### Timer

General purpose countdown/countup timer.

| Field | Description |
|-------|-------------|
| Duration | Timer length in seconds |
| Count Down | Count down (true) or up (false) |
| Start On Awake | Begin automatically |
| Loop | Restart when complete |
| On Timer Start | Event when timer starts |
| On Timer Complete | Event when timer finishes |
| On Timer Update | Event every frame with current time |

**How to use:**
1. Add to empty GameObject
2. Set duration
3. Connect On Timer Complete to your action
4. Call `StartTimer()` to begin

**Properties:**
- `CurrentTime`
- `TimeRemaining`
- `TimePercent` (0-1)

---

### SoundPlayer

Play sound effects easily from events.

| Field | Description |
|-------|-------------|
| Sounds | Array of AudioClips |
| Randomize Pitch | Slight pitch variation |
| Min/Max Pitch | Pitch range |

**How to use:**
1. Add to any object that needs sounds
2. Add AudioClips to Sounds array
3. Call `PlaySound()` from events

**Connect to events:**
- Spawner.OnSpawn → SoundPlayer.PlaySound
- Health.OnDamage → SoundPlayer.PlaySound
- Collectible.OnCollected → SoundPlayer.PlaySound

---

### FlashOnDamage

Visual feedback when damaged.

| Field | Description |
|-------|-------------|
| Flash Color | Color to flash (default: red) |
| Flash Duration | Length of each flash |
| Flash Count | Number of flashes |

**How to use:**
1. Add to object with SpriteRenderer
2. Connect Health.OnDamage → FlashOnDamage.Flash

---

## Recipes

Common combinations for building game objects:

### Platformer Player
```
Components:
- RigidbodyHorizontalMover (speed: 7)
- RigidbodyJumper (jumpForce: 12)
- Health (maxHealth: 3)
- Collector
- Respawner
- FlashOnDamage

Setup:
- Add BoxCollider2D
- Create GroundCheck child at feet
- Set Ground Layer
- Connect Health.OnDamage → FlashOnDamage.Flash
- Connect Health.OnDeath → Respawner.Respawn
```

### Top-Down Player
```
Components:
- RigidbodyTopDownMover (speed: 5)
- LookAtMouse
- ProjectileShooter (shootDirection: TowardMouse)
- Health
- ScreenClamper

Setup:
- Add CircleCollider2D
- Create FirePoint child
- Set up projectile prefab
```

### Enemy (Patrolling)
```
Components:
- PatrolPath
- DamageDealer (damage: 1, useTrigger: true)
- Health
- Knockback

Setup:
- Create waypoint objects
- Assign waypoints to PatrolPath
- Set DamageDealer targetLayers to "Player"
- Connect Health.OnDeath → Destroy
```

### Projectile
```
Components:
- AutoMover (or use Rigidbody2D velocity from ProjectileShooter)
- DamageDealer (destroyOnHit: true)
- Lifetime (lifetime: 3)
- DestroyOnCollision (walls layer)

Setup:
- Add CircleCollider2D (isTrigger: true)
- Add Rigidbody2D (gravityScale: 0)
```

### Coin/Collectible
```
Components:
- FloatingMotion (rotate: true)
- Collectible (type: "Coin", value: 1)

Setup:
- Add CircleCollider2D (isTrigger: true)
- Create particle effect for On Collected
```

### Moving Platform
```
Components:
- Oscillator (movementDistance: X=5)
OR
- PatrolPath (with waypoints)

Setup:
- Add BoxCollider2D
- Set layer to "Ground" (for player's RigidbodyJumper)
```

### Health Pickup
```
Components:
- FloatingMotion
- Healer (healAmount: 25, destroyOnHeal: true)

Setup:
- Add CircleCollider2D (isTrigger: true)
```

### Kill Zone
```
Components:
- TriggerZone (targetTag: "Player")

Setup:
- Add BoxCollider2D (isTrigger: true)
- Size to cover danger area
- Connect OnEnter → Player's Health.TakeDamage (set to 9999)
```

### Checkpoint
```
Components:
- TriggerZone (oneTimeTrigger: true)

Setup:
- Add BoxCollider2D (isTrigger: true)
- Connect OnEnter → Player's Respawner.SetCheckpoint (drag this checkpoint)
```

---

## Layer Setup Recommendations

For components to work properly, set up these layers:

| Layer | Used By |
|-------|---------|
| Player | Player object |
| Enemy | Enemy objects |
| Ground | Platforms, floors |
| Projectile | Player/enemy projectiles |
| Pickup | Collectibles, health |

Configure in **Edit → Project Settings → Tags and Layers**

---

## Common Issues

**"Jumping doesn't work"**
- Check Ground Layer is set correctly on RigidbodyJumper
- Ensure ground objects are on that layer
- Verify GroundCheck position is at player's feet

**"Damage doesn't work"**
- Target must have Health component
- Check Target Layers on DamageDealer
- Ensure colliders are set up (trigger vs collision)

**"Collectibles don't work"**
- Collector needs a Collider2D
- Collectible needs a Collider2D with isTrigger = true
- Check that objects can physically overlap

**"Events don't fire"**
- Make sure UnityEvent is connected in Inspector
- For dynamic values, choose the method with parameter type
- Check that the target object is assigned

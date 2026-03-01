# Лабораторна робота №3 — Додавання механік до прототипу Chicken Hunt

**Студент:** Анна Карпенко
**Гілка:** `labs/GD-3-karpenko-adding_mechanics_to_chicken_hunt`
**Прототип:** Chicken Hunt (MegaSuperChallengeShot)
**Дата:** 2026

---

## 1. Розбір вимог

Лабораторна робота вимагає розширити існуючий прототип 2D-гри, додавши:

| Вимога | Реалізація |
|--------|------------|
| Collider2D | BoxCollider2D на Bird, BirdEscapeZone, PowerUp |
| Trigger | BirdEscapeZone — OnTriggerEnter2D забирає життя при втечі птаха |
| Physics2D.Raycast | ShootingController — вертикальний промінь з нижнього краю екрану для сканування птахів |
| Physics2D.Overlap* | ShootingController — OverlapCircleAll для влучання; PowerUpCollector — OverlapBoxAll для збору бонусів |
| HP / Lives | GameManager — 3 життя, зменшуються при втечі птаха |
| Score | GameManager — очки за знищення птахів (1/2/3 залежно від типу) |
| Lose condition | Lives <= 0 → GameOver |
| Scene restart | SceneManager.LoadScene після GameOver або по кнопці Restart |
| UI: Image | Slider fill image — колір змінюється залежно від HP |
| UI: Slider | Slider для відображення кількості життів |
| UI: TMP | TextMeshPro для Score, Shots, HP-тексту, Game Over |
| Game state | Enum GameState (Playing, Paused, GameOver) + GameManager як state machine |

---

## 2. Аналіз існуючого прототипу

Початковий прототип містить 4 скрипти:

- **CrosshairController.cs** — курсор-приціл, стрільба через `Physics2D.OverlapPointAll`, обмежена кількість пострілів, перезарядка ПКМ.
- **BirdMover.cs** — рух птаха вправо з постійною швидкістю.
- **BirdSpawner.cs** — створює одного птаха при Start.
- **ScoreManager.cs** — Singleton з DontDestroyOnLoad, лічильник очок.

**Обмеження прототипу:**
- Тільки 1 птах спавниться одноразово.
- Немає життів та умови програшу.
- Немає Game Over, перезапуску.
- UI показує лише score та shots як TMP-текст.
- Немає Trigger-взаємодій.
- Немає Raycast-використання.

---

## 3. Запропоновані механіки

### 3.1 Хвилі птахів
Птахи з'являються хвилями з обох сторін екрану. Три типи:
- **Normal** (білий) — повільний, 1 очко
- **Fast** (жовтий) — швидший, 2 очки
- **Bonus** (червоний) — найшвидший, 3 очки

### 3.2 Зони втечі (Trigger)
По краях екрану — невидимі BoxCollider2D (isTrigger). Коли птах пролітає через них — гравець втрачає життя.

### 3.3 Raycast-сканер
Вертикальний промінь з нижнього краю екрану постійно сканує поле. Якщо птах перетинає промінь — показується лазерна лінія (LineRenderer). Натискання Space стріляє по Raycast-цілі.

### 3.4 Power-Up (OverlapBox)
Періодично з'являються бонуси перезарядки. Збір через `Physics2D.OverlapBoxAll` навколо курсора.

### 3.5 Game Over + Restart
При 0 життів — Game Over панель з фінальним рахунком. Автоматичний перезапуск через 2 секунди або по кнопці Restart.

---

## 4. Дизайн колізій та детектування

```
┌─────────────────────────────────────────────┐
│              ІГРОВЕ ПОЛЕ                     │
│                                              │
│  [EscapeZone]                [EscapeZone]    │
│  BoxCollider2D               BoxCollider2D   │
│  isTrigger=true              isTrigger=true  │
│       │                           │          │
│       │    Bird ──►               │          │
│       │   CircleCollider2D        │          │
│       │         │                 │          │
│       │         ▼                 │          │
│       │   OnTriggerEnter2D        │          │
│       │   → LoseLife()            │          │
│       │                           │          │
│       │                           │          │
│  ─────┼───── Raycast (↑) ────────┼─────     │
│       │      Physics2D.Raycast    │          │
│       │                           │          │
│       │   OverlapCircleAll        │          │
│       │   (при кліку миші)        │          │
│       │                           │          │
│       │   OverlapBoxAll           │          │
│       │   (збір PowerUp)          │          │
│                                              │
│   [Crosshair] ← Mouse position              │
│   + ShootingController                       │
│   + PowerUpCollector                         │
└─────────────────────────────────────────────┘
```

### Layers:
- **Bird** — шар для птахів
- **PowerUp** — шар для бонусів

### Методи детектування:

| Метод | Де використано | Навіщо |
|-------|---------------|--------|
| `Physics2D.OverlapCircleAll` | ShootingController.HandleShooting() | Перевірка влучання — область навколо курсора при пострілі |
| `Physics2D.OverlapBoxAll` | PowerUpCollector.Update() | Збір бонусів навколо курсора (прямокутна область) |
| `Physics2D.Raycast` | ShootingController.HandleRaycastScan() | Сканування — вертикальний промінь знаходить птахів |
| `OnTriggerEnter2D` | BirdEscapeZone | Детектування втечі птаха за межі поля |

---

## 5. Дизайн станів гри

```
    ┌──────────┐
    │          │
    │ Playing  │ ←── StartNewGame()
    │          │
    └────┬─────┘
         │ Lives <= 0
         ▼
    ┌──────────┐
    │          │
    │ GameOver │ ──► Time.timeScale = 0
    │          │     Show GameOver UI
    └────┬─────┘
         │ RestartDelay / Button
         ▼
    SceneManager.LoadScene()
    (повертається до Playing)
```

Стани реалізовано через `enum GameState` та `GameManager.SetState()`.

---

## 6. Ієрархія сцени

```
Main (Scene)
├── Main Camera
├── Background (SpriteRenderer)
│
├── --- MANAGERS ---
├── GameManager              [GameManager.cs]
├── UIManager                [UIManager.cs]
├── BirdWaveSpawner          [BirdWaveSpawner.cs]
├── PowerUpSpawner           [PowerUpSpawner.cs]
│
├── --- GAMEPLAY ---
├── Crosshair                [ShootingController.cs, PowerUpCollector.cs]
│   └── (Sprite: crosshair image)
│
├── RaycastOrigin (Empty)    (позиція: X=0, Y=-5.5)
│
├── --- ESCAPE ZONES (Triggers) ---
├── EscapeZoneLeft           [BirdEscapeZone.cs]
│   └── BoxCollider2D (isTrigger, розмір: 2x12, позиція: X=-11)
├── EscapeZoneRight          [BirdEscapeZone.cs]
│   └── BoxCollider2D (isTrigger, розмір: 2x12, позиція: X=+11)
│
├── --- UI ---
├── Canvas (Screen Space - Overlay)
│   ├── TopPanel
│   │   ├── ScoreText (TMP)
│   │   ├── ShotsText (TMP)
│   │   └── LivesPanel
│   │       ├── LivesText (TMP)
│   │       └── LivesSlider (UI Slider)
│   │           └── Fill (Image — колір змінюється)
│   └── GameOverPanel (inactive by default)
│       ├── GameOverTitle (TMP: "GAME OVER")
│       ├── FinalScoreText (TMP)
│       └── RestartButton (UI Button + TMP: "Restart")
└── EventSystem
```

---

## 7. Скрипти C#

Усі скрипти знаходяться у:
`Assets/Projects/MegaSuperChallengeShot/Scripts/`

### 7.1 GameState.cs
Перелік станів гри: `Playing`, `Paused`, `GameOver`.

### 7.2 GameManager.cs
Singleton. Зберігає score, lives, shots. Обробляє переходи між станами.
- `AddScore(int)` — додає очки.
- `LoseLife()` — забирає життя, перевіряє програш.
- `TryShoot()` — зменшує патрони, повертає `true` якщо можна стріляти.
- `Reload()` — перезарядка.
- `RestartScene()` — `SceneManager.LoadScene(currentScene)`.
- Events: `OnScoreChanged`, `OnLivesChanged`, `OnShotsChanged`, `OnStateChanged`.

### 7.3 UIManager.cs
Підписується на події GameManager. Оновлює:
- `TMP_Text` — score, shots, lives, game over score.
- `Slider` — lives (maxValue = maxLives, value = currentLives).
- `Image` — fill колір slider (зелений → жовтий → червоний).
- `GameObject` — GameOverPanel (активується при GameOver).
- `Button` — RestartButton (onClick → restart).

### 7.4 ShootingController.cs (замінює CrosshairController)
- Crosshair слідує за мишею.
- **OverlapCircleAll** — ЛКМ стріляє, перевіряє радіус навколо курсора.
- **Raycast** — постійний промінь знизу вгору, Space стріляє по цілі.
- LineRenderer — візуалізація лазера.

### 7.5 Bird.cs (замінює BirdMover)
- Три типи: Normal, Fast, Bonus.
- `Die()` — додає очки через GameManager, знищує об'єкт.
- `SetDirection(Vector2)` — задає напрямок і розгортає спрайт.
- Rigidbody2D (Kinematic) + Collider2D.

### 7.6 BirdWaveSpawner.cs (замінює BirdSpawner)
- Хвилі з обох сторін екрану.
- Випадковий Y, випадковий тип.
- Інтервал зменшується з часом (прогресія складності).

### 7.7 BirdEscapeZone.cs
- BoxCollider2D (isTrigger = true).
- **OnTriggerEnter2D** — якщо Bird потрапив → `GameManager.LoseLife()`.

### 7.8 PowerUp.cs
- Бонусний об'єкт з анімацією покачування.
- `Collect()` — виконує дію (перезарядка).

### 7.9 PowerUpCollector.cs
- На Crosshair.
- **Physics2D.OverlapBoxAll** — перевіряє область навколо курсора кожен кадр.

### 7.10 PowerUpSpawner.cs
- Періодично створює PowerUp у випадковій позиції.

---

## 8. Кроки налаштування в Unity

### 8.1 Створення Layers
1. Edit → Project Settings → Tags and Layers
2. Додати Layer: **Bird** (наприклад, User Layer 8)
3. Додати Layer: **PowerUp** (наприклад, User Layer 9)

### 8.2 Налаштування Bird Prefab
1. Відкрити існуючий `Bird1_3_0.prefab` в `Assets/Projects/MegaSuperChallengeShot/Prefabs/`
2. Видалити компонент `BirdMover`
3. Додати компонент `Bird`
4. Додати `Rigidbody2D` (Gravity Scale = 0, Body Type = Kinematic)
5. Додати `CircleCollider2D`
6. Встановити Layer = **Bird**
7. Налаштувати Bird: Base Speed = 3, Bird Type = Normal
8. Зберегти як `BirdNormal.prefab`
9. Зробити дублікати для Fast (Speed=4.8, жовтий тінт) та Bonus (Speed=6, червоний тінт)

### 8.3 Створення PowerUp Prefab
1. Створити порожній GameObject
2. Додати SpriteRenderer (будь-який спрайт з проєкту, наприклад зірка)
3. Додати `CircleCollider2D`
4. Додати `PowerUp` компонент
5. Layer = **PowerUp**
6. Зберегти як `PowerUpReload.prefab`

### 8.4 Сцена: GameManager
1. Створити порожній GameObject "GameManager"
2. Додати компонент `GameManager`
3. Налаштувати: Max Lives = 3, Max Shots = 6, Restart Delay = 2

### 8.5 Сцена: Crosshair
1. Вибрати існуючий Crosshair об'єкт
2. Видалити `CrosshairController`
3. Додати `ShootingController`:
   - Shot Radius = 0.5
   - Target Layer = Bird
   - Raycast Layer = Bird
   - Raycast Origin = (посилання на RaycastOrigin об'єкт)
4. Додати `PowerUpCollector`:
   - Collection Size = (1, 1)
   - PowerUp Layer = PowerUp

### 8.6 Сцена: RaycastOrigin
1. Створити порожній GameObject "RaycastOrigin"
2. Position: (0, -5.5, 0) — нижній край екрану
3. (Опціонально) Додати LineRenderer для лазера:
   - Width: 0.05
   - Material: Default-Line
   - Color: Red
   - Привласнити в ShootingController → Laser Line

### 8.7 Сцена: Escape Zones
1. Створити "EscapeZoneLeft" (позиція: -11, 0, 0)
   - Додати BoxCollider2D: Size (2, 12), isTrigger = true
   - Додати `BirdEscapeZone`
2. Створити "EscapeZoneRight" (позиція: 11, 0, 0)
   - Аналогічно

### 8.8 Сцена: BirdWaveSpawner
1. Створити порожній "BirdWaveSpawner"
2. Додати `BirdWaveSpawner`
3. Призначити 3 префаби (Normal, Fast, Bonus)
4. Налаштувати: Spawn Interval = 1.5, Min Y = -3, Max Y = 3

### 8.9 Сцена: PowerUpSpawner
1. Створити порожній "PowerUpSpawner"
2. Додати `PowerUpSpawner`
3. Призначити PowerUp prefab
4. Spawn Interval = 12

### 8.10 Сцена: UI Canvas
1. Створити Canvas (Screen Space - Overlay)
2. Додати дітей:
   - **ScoreText** (TextMeshPro): верхній лівий кут, "Score: 0"
   - **ShotsText** (TextMeshPro): верхній правий кут, "Shots: 6/6"
   - **LivesPanel** (Horizontal Layout Group):
     - **LivesText** (TMP): "HP: 3/3"
     - **LivesSlider** (UI → Slider):
       - Max Value: 3, Value: 3, Whole Numbers: true
       - Fill Image: зелений колір
   - **GameOverPanel** (Panel, inactive):
     - **GameOverTitle** (TMP): "GAME OVER", центр, великий шрифт
     - **FinalScoreText** (TMP): "Final Score: 0"
     - **RestartButton** (Button): "Restart"
3. Створити порожній "UIManager"
4. Додати `UIManager` та призначити всі UI-посилання

### 8.11 Видалення старих скриптів
1. Видалити зі сцени об'єкти з `CrosshairController`, `BirdSpawner`, старий `ScoreManager`
2. Файли `CrosshairController.cs`, `BirdMover.cs`, `BirdSpawner.cs`, `ScoreManager.cs` можна залишити в проєкті (не зламають білд) або видалити

### 8.12 Build Settings
1. File → Build Settings → Add Open Scenes (додати Main сцену)

---

## 9. Git: створення гілки та push

```bash
# Перейти в директорію проєкту
cd /path/to/Grin-Game-Dev-2026

# Переконатися що ми на правильній гілці
git checkout GD-3-Interaction

# Створити нову гілку для лаби
git checkout -b labs/GD-3-karpenko-adding_mechanics_to_chicken_hunt

# Перевірити статус
git status

# Додати нові файли
git add Assets/Projects/MegaSuperChallengeShot/Scripts/GameState.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/GameManager.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/UIManager.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/ShootingController.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/Bird.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/BirdWaveSpawner.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/BirdEscapeZone.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/PowerUp.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/PowerUpCollector.cs
git add Assets/Projects/MegaSuperChallengeShot/Scripts/PowerUpSpawner.cs
git add Assets/Projects/MegaSuperChallengeShot/Lab3_Karpenko_ChickenHunt.md

# Комміт
git commit -m "Lab 3: Add interaction mechanics to Chicken Hunt

- Add GameManager with state system (Playing/GameOver)
- Add ShootingController with OverlapCircle + Raycast
- Add Bird with types (Normal/Fast/Bonus)
- Add BirdWaveSpawner with difficulty progression
- Add BirdEscapeZone (Trigger — lose life on escape)
- Add PowerUp + OverlapBox collection
- Add UIManager (TMP, Slider, Image, GameOver panel)
- Add scene restart via SceneManager.LoadScene

Student: Ann Karpenko"

# Push на remote
git push -u origin labs/GD-3-karpenko-adding_mechanics_to_chicken_hunt
```

---

## 10. Письмова відповідь (українською)

### 1. Які типи колізій було використано?

У проєкті використано два основні типи колізій:

**Trigger-колізії (OnTriggerEnter2D):** У скрипті `BirdEscapeZone` використовується BoxCollider2D з увімкненим `isTrigger`. Коли птах (з CircleCollider2D) пролітає крізь цю зону — спрацьовує `OnTriggerEnter2D`, що сигналізує про втечу птаха за межі ігрового поля. Тригер-колізія обрана тому, що нам не потрібна фізична зупинка об'єкта — лише факт перетину зони.

**Overlap-перевірки (Physics2D.OverlapCircleAll та Physics2D.OverlapBoxAll):** Це не «класичні» колізії Unity, а ручні перевірки перетину. `OverlapCircleAll` використовується при пострілі для визначення, чи є птахи в радіусі навколо курсора. `OverlapBoxAll` — для збору бонусів (PowerUp) навколо прицілу.

Фізичні колізії (OnCollisionEnter2D) не використовуються, оскільки гра є shooter-типу без необхідності фізичного зіткнення тіл.

### 2. Де було використано Raycast або Overlap?

**Physics2D.Raycast** — використовується в `ShootingController.HandleRaycastScan()`. Від об'єкта `RaycastOrigin` (нижній край екрану) постійно направляється вертикальний промінь вгору. Якщо промінь перетинає колайдер птаха — малюється лазерна лінія (LineRenderer). При натисканні Space — відбувається постріл по виявленій Raycast-цілі.

**Physics2D.OverlapCircleAll** — використовується в `ShootingController.HandleShooting()`. При кліку лівою кнопкою миші перевіряється кругова область навколо курсора: всі птахи, що потрапили в радіус `shotRadius`, знищуються.

**Physics2D.OverlapBoxAll** — використовується в `PowerUpCollector.Update()`. Кожен кадр перевіряється прямокутна область навколо прицілу на наявність бонусних об'єктів (PowerUp). Якщо знайдено — бонус збирається автоматично.

### 3. Чому було обрано саме такий метод перевірки?

- **OverlapCircleAll для стрільби** — обрано тому, що реальний постріл дробовика має кругову область ураження. Це природний вибір для shooter-гри, де «влучання» — це потрапляння в радіус навколо точки прицілу. OverlapCircle ефективніший за кілька Raycast, коли потрібно перевірити область, а не лінію.

- **Raycast для сканування** — обрано для демонстрації лінійної перевірки. Вертикальний промінь імітує «лазерний сканер», який знаходить першого птаха на своєму шляху. Raycast ідеально підходить для лінійних перевірок, де важлива точка зіткнення та напрямок.

- **OverlapBoxAll для збору бонусів** — обрано для демонстрації іншого типу Overlap-перевірки. Прямокутна область краще відповідає формі курсора, ніж коло. Також це дозволяє окремо налаштовувати розмір збору бонусів від розміру стрільби.

- **Trigger для зон втечі** — OnTriggerEnter2D обрано тому, що зони втечі — це стаціонарні області, які не повинні фізично зупиняти птахів. Trigger-підхід використовує вбудовану систему колізій Unity і не потребує ручних перевірок кожен кадр, що ефективніше.

### 4. Як механіки впливають на core loop?

Основний ігровий цикл (core loop) Chicken Hunt після доданих механік:

**Спостереження → Прицілювання → Стрільба → Результат → Повторення**

1. **Птахи з'являються хвилями** (BirdWaveSpawner) — створюють постійний потік цілей, які потрібно знищувати. Зростаюча частота спавну підвищує складність.

2. **Гравець прицілюється та стріляє** (ShootingController, OverlapCircle) — основна дія гравця. Обмежена кількість патронів додає стратегічний елемент.

3. **Raycast-сканер** надає альтернативний спосіб стрільби (Space), що розширює набір дій гравця та додає тактичний вибір.

4. **Trigger-зони втечі** (BirdEscapeZone) — створюють negative feedback loop: якщо гравець не встигає стріляти — птахи тікають → життя зменшуються → наближається програш. Це мотивує гравця діяти швидше.

5. **Бонуси перезарядки** (PowerUp + OverlapBox) — positive feedback loop: уважний гравець збирає бонуси → отримує більше патронів → може знищити більше птахів.

6. **Система життів та Game Over** (GameManager) — створює tension та кінцеву мету: протриматися якомога довше та набрати максимум очок.

7. **Перезапуск сцени** — забезпечує повторюваність (replayability), дозволяючи миттєво почати нову гру.

Загалом, додані механіки перетворюють простий прототип «клацни по птаху» на повноцінний arcade shooter з прогресією складності, ресурс-менеджментом (патрони), та чітким циклом зворотного зв'язку.

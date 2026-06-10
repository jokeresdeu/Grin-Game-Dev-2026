# CubeHopper — Лабораторна робота №6

**Студент:** Карпенко Анна
**Група:** Інб22340д
**Лабораторна робота:** GD-6 — Створення "однокнопкової гри"
**Гілка:** `labs/GD-6-karpenko-one-button-game`

Unity version: **Unity 6 (6000.0.x)**

---

## Що це

**CubeHopper** — 2D-раннер у стилі Geometry Dash з однією дією: натиснути будь-яку клавішу / ЛКМ / тап, щоб куб стрибнув і уникнув шипа.

> Натисни **будь-яку кнопку**, щоб куб стрибнув над шипом — без натискання він біжить далі, з трьома хітами і прогресуючою швидкістю.

Повний опис механіки, core loop, унікальних фішок і архітектури — у [`Assets/Projects/CubeHopper/README.md`](Assets/Projects/CubeHopper/README.md).

Інструкція з налаштування сцен у Unity Editor — у [`Assets/Projects/CubeHopper/LAB6_SETUP_GUIDE.md`](Assets/Projects/CubeHopper/LAB6_SETUP_GUIDE.md).

---

## Як запустити

1. Відкрити проект у Unity 6.
2. Відкрити сцену `Assets/Projects/CubeHopper/Scenes/CubeHopper_MainMenu.unity`.
3. Натиснути ▶ Play.
4. Будь-яке натискання у головному меню → почнеться гра.
5. У грі — стрибати, уникати шипів, виживати.

## Build Settings

Порядок сцен у Build Settings:

- `0` → `CubeHopper_MainMenu.unity` (стартова сцена)
- `1` → `CubeHopper_Game.unity` (геймплей)

---

## Структура коду

19 скриптів у `Assets/Projects/CubeHopper/Scripts/`, ~1290 рядків. Кожен у власному файлі з єдиною відповідальністю — GameManager (state machine), WorldSpeed (швидкість світу), ScoreManager (очки + комбо + PlayerPrefs), PlayerController (введення + фізика), PlayerHealth (життя + інвулнерабіліті), ObstacleSpawner (зважений процедурний спавн), GroundScroller + ParallaxLayer (нескінченний фон), CameraShake (perlin noise), HUDController + GameOverUI + PauseMenuController + MainMenuController (UI).

---

## Умова програшу та перезапуск

- 3 хіти по шипах → Game Over → панель з фінальним рахунком і Best Score (з PlayerPrefs).
- Будь-яке натискання на екрані Game Over (після 0.5с lock) перезапускає сцену.
- Esc → повернення у головне меню.

---

## Геймплейне відео

`gameplay.mp4` (буде додано окремо) — запис проходження гри з голосовими коментарями про механіку, core loop, програш і перезапуск.

---

## Попередні лабораторні в цій репозиторії

Гілка `labs/GD-6-karpenko-one-button-game` починається з фінального стану Lab 5. Попередні лабораторні (Chicken Hunt) залишаються в `Assets/Projects/MegaSuperChallengeShot/` і не використовуються Lab 6. CubeHopper — окремий незалежний проєкт у `Assets/Projects/CubeHopper/`.

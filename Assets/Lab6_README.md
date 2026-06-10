# Lab 6: Pulse Climber

Pulse Climber is a complete one-button Unity prototype.

## Core Loop

- Start from the main menu.
- Press one action button to jump upward and reverse horizontal direction.
- Land on green platforms to keep climbing.
- Avoid red platforms.
- Lose if the player falls below the camera or touches a red platform.
- Restart from the game over screen or pause menu.

## Controls

- Space
- Left mouse click
- Touch / tap

All three inputs represent the same single gameplay action.

## UI

- Main menu: Start, Quit.
- HUD: score, best score, pause/menu button.
- Pause menu: Resume, Restart, Quit.
- Game over menu: score, restart, main menu.

## Progress Tracking

The score is based on the highest height reached. The best score is saved with `PlayerPrefs`.

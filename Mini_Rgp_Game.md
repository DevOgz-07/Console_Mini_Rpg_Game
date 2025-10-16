# 🎮 Console Mini RPG Game

A simple yet engaging console-based RPG game developed in **C#**. Players can create their own character, battle various monsters, gain experience, level up, manage inventory, and try to survive in an increasingly dangerous world.

---

## 🧠 About

This project is a small but functional RPG system designed for developers who want to learn and implement core game mechanics. It features fundamental RPG elements such as character creation, monster combat, leveling system, and inventory management.

---

## 🧙 Characters

| Attribute        | Value                       |
|------------------|--------------------------   |
| **Health (HP)**  | 100                         |
| **Attack Power** | 50                          |
| **Level**        | 1                           |
| **Experience**   | 0                           |
| **Inventory**    | One Sword and Health Potion |
| **Equipped Item**| Null                        |

> Players create their character by entering a name at the beginning. No class selection is available yet. As they defeat enemies, they gain XP and level up, which increases both health and attack power.

---

## 👹 Monsters

| Monster        | Type   | HP   | Attack Power | XP Reward | Possible Loot                                                                                        |
|----------------|--------|------|---------------|------------|----------------------------------------------------------------------------------------------------|
| **Goblin**     | Normal | 20   | 5             | 10         | Rusty Sword *(Common)*,Worn Cloak *(Common)*,Small Health Potion *(Common)*                        |
| **Skeleton**   | Normal | 25   | 7             | 15         | Bone Dagger *(Uncommon)*,Skeleton Armor *(Uncommon)*,Medium Health Potion *(Uncommon)*             |
| **Orc**        | Normal | 35   | 10            | 20         | Iron Axe *(Rare)*,Leather Armor *(Rare)*,Large Health Potion *(Rare)*                              |
| **Orc Chief**  | Boss   | 250  | 30            | 150        | Chief's Axe *(Legendary)*,Chief's Armor *(Legendary)*,Orc Heart *(Mythic)*                         |
| **Dragon**     | Boss   | 400  | 50            | 200        | Dragon Staff *(Legendary)*,Dragon Scale Armor *(Legendary)*,Ancient Heart *(Mythic)*               |

> Each monster may drop different items upon defeat. Stronger enemies drop more powerful and rarer loot.
> characters and monsters can be updated later !!

---

## 🕹️ Gameplay

1. The player creates a character (by entering a name).
2. Random monster encounters occur.
3. During combat, the player can:
   - Attack
   - To Escape
   - Use an item from the inventory
4. After battles, experience points are earned and the player levels up.

---

## 🧪 Features

- Console-based interface (CLI)
- Basic AI-controlled enemies
- Leveling and XP system
- Inventory with usable and equippable items
- Weapon management
- Loot drops with rarity types

---

## ⚙️ Setup & Run

> This project requires **.NET 6.0 SDK** or higher to run.

### Steps:

```bash
# Clone the repository
git clone https://github.com/DevOgz-07/Console_Mini_Rpg_Game.git

# Navigate into the project folder
cd Console_Mini_Rpg_Game

# Run the project
dotnet run

cd Console_Mini_Rpg_Game

# Uygulamayı çalıştır
dotnet run


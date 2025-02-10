# 🎯 2D Balloon Shooter Game 🎈

This is a simple 2D balloon shooter game built in Unity, where balloons float around and arrows are shot to pop them. The game includes collision detection between balloons, arrows, and walls. When an arrow hits a balloon, the balloon pops and a new one spawns. The game also includes various boundary checks to ensure that objects do not go out of the scene.

## 🚀 Features
- 🎈 Balloon movement and physics-based collision detection
- 🏹 Arrow projectile system
- 🔄 Bounce mechanics for balloons when colliding with walls
- 🎯 Balloon spawning when out of bounds or popped
- ⚡ Detection of direct hits and glancing blows for arrows

## 🕹️ How to Play
- Shoot arrows at the floating balloons to pop them.
- If an arrow hits a balloon directly, both the arrow and balloon are destroyed, and a new balloon will spawn.
- If an arrow glances off the balloon, the balloon is pushed slightly in the direction of the collision.

## ⚙️ Collision Detection and Resolution

In this game, collisions are detected and resolved in the following ways:

### 🎈 Balloon-to-Balloon Collisions
- A collision is detected by calculating the distance between the centers of two balloons and comparing it to the sum of their radii.
- If the distance is less than the combined radii, a collision has occurred, and we apply a physics-based response using an impulse to adjust the velocities of both balloons. The balloons are also separated to avoid overlap.

### 🧱 Balloon-to-Wall Collisions
- Balloons check for collisions with the scene's walls (left, right, top, and bottom).
- When a balloon hits a wall, the velocity is adjusted to simulate bouncing off the wall, and the balloon is repositioned slightly to avoid sticking to the wall.

### 🏹 Arrow-to-Balloon Collisions
- The collision between an arrow and a balloon is checked by finding the closest point on the arrow to the balloon and calculating the distance between the two.
- If the distance is smaller than the balloon's radius, the game checks if it's a direct hit or a glancing blow.
  - A **direct hit** destroys both the balloon and the arrow.
  - A **glancing blow** causes the balloon to be pushed in the direction of the collision, and the arrow's velocity is slightly altered.

### 🚪 Out-of-Bounds Detection
- Balloons and arrows that move out of the scene bounds (i.e., past the screen edges or the top) are destroyed and removed.
- New balloons are spawned when this occurs.

## 💾 Installation
1. Clone or download the repository.
2. Open the project in Unity.
3. Play the game within Unity or build it to your desired platform.

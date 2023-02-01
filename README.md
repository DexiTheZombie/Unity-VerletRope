# Unity-VerletRope

## Description
A very small implementation of Verlet Integration with rope dynamics.

This isnt designed to be a full project, however i might continue to work on this to make a simple "Cut the Rope" clone. using Verlet Integration for the rope dynamics.

## Usage

### RopeCutter.cs
Currently not used, its planned usage is towards a simple clone of the mobile game "Cut the Rope", when the mouse collides with a rope while holding the left mouse button, split into 2 smaller verlet ropes that disappear after x seconds.

### VerletRope.cs
The base script that controls the generation and simulation of the rope, also controls a line renderer component to create simplistic visuals.

```
Rope Resolution
    The number of points along rope + 2

Rope Segment Length
    The length between points.

Sub Step Count
    How many times it applies costraints to the points.
    aka Simulation Resolution

Rope Anchor Start/End
    The start and end transforms that the rope will anchor too.

LineRenderer and EdgeCollider
    Standard unity components used for visuals and collision detection.
```

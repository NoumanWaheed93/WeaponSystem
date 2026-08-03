# Weapon System

[![upm](https://img.shields.io/badge/upm-1.0.0-blue)](https://github.com/NoumanWaheed93/WeaponSystem)
[![license](https://img.shields.io/badge/license-MIT-green)](LICENSE.md)
[![unity](https://img.shields.io/badge/unity-6000.0%2B-black)](https://unity.com)

Ammo- and fire-rate-driven weapons for Unity.

`Weapon` is a plain C# base class that owns the two rules every weapon shares: **do I still have ammo**,
and **has enough time passed since the last shot**. Concrete weapons override `Fire()` to add what
actually happens when the trigger is pulled — a raycast, a projectile, a homing projectile.

Time comes in through `ITimeProvider` rather than `UnityEngine.Time`, so the firing logic is unit
testable without entering play mode. The `MonoBehaviour` types in the package are the scene-facing
wrappers, not the logic.

## Contents

- [Install](#install)
- [Quick start](#quick-start)
- [Weapons](#weapons)
- [API](#api)
- [Projectiles](#projectiles)
- [Running the tests](#running-the-tests)
- [License](#license)

## Install

Add the package via **Window ▸ Package Manager ▸ + ▸ Add package from git URL…**:

```
https://github.com/NoumanWaheed93/WeaponSystem.git
```

Or add it to `Packages/manifest.json` directly:

```json
{
  "dependencies": {
    "com.artisangames.weaponsystem": "https://github.com/NoumanWaheed93/WeaponSystem.git"
  }
}
```

**Requires Unity 6000.0 or newer.** `Projectile` uses `Rigidbody.linearVelocity`, which replaced
`Rigidbody.velocity` in Unity 6.

### Additional dependencies

Two dependencies are distributed as git URLs rather than through a registry, so UPM cannot resolve them
automatically. Add them to `Packages/manifest.json` yourself:

```json
{
  "dependencies": {
    "com.artisangames.healthsystem": "https://github.com/NoumanWaheed93/HealthSystem.git",
    "net.tnrd.nsubstitute": "https://github.com/Thundernerd/Unity3D-NSubstitute.git"
  }
}
```

| Package                        | Needed by       | Why                                                     |
| ------------------------------ | --------------- | ------------------------------------------------------- |
| `com.artisangames.healthsystem`| Runtime         | `GunRaycastBased` applies damage through `IDamageable`.  |
| `net.tnrd.nsubstitute`         | Tests only      | The edit-mode tests mock `ITimeProvider` and the factories. |

## Quick start

Give the weapon a barrel `Transform` and a clock, then call `Fire()`:

```csharp
using UnityEngine;
using WeaponSystem;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform barrel;

    private Weapon weapon;

    private void Awake()
    {
        weapon = new GunRaycastBased(
            barrel,
            new GameTimeProvider(),   // reads UnityEngine.Time.time
            maximumAmmo: 100,
            bulletsPerSecond: 8f,
            range: 250f,
            damageAmount: 10f);
    }

    // Raycast weapons should be fired from FixedUpdate.
    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
            weapon.Fire();        // returns false if out of ammo or still cooling down
    }
}
```

`Fire()` returns `bool`, and that return value is the whole feedback channel — `true` means a shot
actually left the barrel, so it is what you hook muzzle flashes, recoil and audio to:

```csharp
if (weapon.Fire())
{
    muzzleFlash.Play();
    audioSource.PlayOneShot(gunshot);
}
```

Reloading refills to `MaximumAmmo`:

```csharp
if (weapon.RemainingAmmo == 0)
    weapon.Reload();
```

## Weapons

All three take the same first four constructor parameters —
`(Transform barrel, ITimeProvider timeProvider, int maximumAmmo, float bulletsPerSecond)` — and differ
only in what they add.

### `GunRaycastBased`

Hitscan. Raycasts forward from the barrel and damages the first `IDamageable` it hits.

```csharp
new GunRaycastBased(barrel, time, 100, 8f, range: 250f, damageAmount: 10f);
```

Because it calls `Physics.Raycast`, **fire it from `FixedUpdate`**.

Nothing happens on a miss, and nothing happens when the collider has no `IDamageable` — the shot is
still consumed either way.

### `ProjectileLauncher`

Spawns a projectile through an `IProjectileFactory` and launches it from the barrel.

```csharp
new ProjectileLauncher(barrel, time, 20, 2f, projectileFactory);
```

Calls `factory.GetProjectile()`, then `projectile.Launch(barrel)`, which snaps the projectile's
position and rotation to the barrel and zeroes its velocity.

### `GuidedProjectileLauncher`

Same as above, but pulls a homing projectile and assigns it a target.

```csharp
var launcher = new GuidedProjectileLauncher(barrel, time, 4, 0.5f, projectileFactory);
launcher.Target = enemy.transform;
```

Calls `factory.GetHomingProjectile()`, launches it, then sets `Target` on the new projectile. `Target`
is read at fire time, so re-assigning it between shots sends each missile somewhere different.

> `Target` is not validated. A `null` target produces a projectile whose `Target` is `null`, and
> `GuidedProjectile.FixedUpdate` will throw when it dereferences it.

## API

### `Weapon`

```csharp
public abstract class Weapon
```

| Member                                   | Notes                                                                  |
| ---------------------------------------- | ---------------------------------------------------------------------- |
| `int MaximumAmmo { get; }`               | Set once in the constructor.                                            |
| `int RemainingAmmo { get; }`             | Starts at `MaximumAmmo`, decremented per successful shot.               |
| `float ShotInterval { get; }`            | `1f / bulletsPerSecond`, computed in the constructor.                   |
| `Transform Barrel { get; }`              | `protected`. Fire origin and direction (`Barrel.forward`).              |
| `virtual bool Fire()`                    | `true` if a shot was taken. `false` when out of ammo or cooling down.   |
| `virtual void Reload()`                  | Resets `RemainingAmmo` to `MaximumAmmo`. Instant — no reload duration.  |
| `protected bool HasShotIntervalPassed()` | `true` for a weapon that has never fired, whatever its interval.        |
| `protected bool HasAmmoRanOut()`         | `RemainingAmmo <= 0`.                                                   |

A weapon that has never fired can always fire immediately, no matter how long `ShotInterval` is — the
cooldown is measured from the previous shot, not from construction.

Ammo is checked **before** the interval, so an empty weapon returns `false` rather than waiting.

### `ITimeProvider`

```csharp
public interface ITimeProvider
{
    float GetTime();
}
```

The weapon's clock, in seconds. `GameTimeProvider` is the shipped implementation and returns
`UnityEngine.Time.time`. Substitute your own to drive weapons off unscaled, networked or paused time —
or, in tests, off a value you control.

### `IProjectileFactory`

```csharp
public interface IProjectileFactory
{
    IProjectile GetProjectile();
    IGuidedProjectile GetHomingProjectile();
}
```

Where projectiles come from. The launchers never call `Instantiate` themselves, so a pool is a drop-in
replacement for the `Instantiate`-per-shot `ProjectileFactoryDemo` included in the package.

### `IProjectile` / `IGuidedProjectile`

```csharp
public interface IProjectile
{
    void Launch(Transform barrel);
}

public interface IGuidedProjectile : IProjectile
{
    Transform Target { get; set; }
}
```

## Projectiles

### `Projectile`

`MonoBehaviour`, `[RequireComponent(typeof(Rigidbody))]`. Flies straight forward at a serialized
`speed` by writing `Rigidbody.linearVelocity` every `FixedUpdate`.

Assign the `Rigidbody` in the inspector — the `m_rigidbody` field is serialized and is **not** fetched
via `GetComponent` at runtime.

`Launch(barrel)` teleports the projectile to the barrel and clears both linear and angular velocity, so
a pooled projectile does not carry momentum from its previous life.

### `GuidedProjectile`

Extends `Projectile`. Each `FixedUpdate` it slerps its rotation toward `Target`, at a serialized
`turningSpeed` in degrees-ish per second, then flies forward as normal. Lower `turningSpeed` gives a
missile that can be out-turned.

### Scene-facing helpers

| Type                         | Notes                                                                  |
| ---------------------------- | ---------------------------------------------------------------------- |
| `WeaponMonoBehaviour`        | Abstract base exposing `maxAmmo`, `bulletsPerSecond` and `barrelGO` in the inspector, plus a `protected Weapon weapon` field for subclasses to construct. |
| `RaycastGunMonobehaviourDemo`| Sample `WeaponMonoBehaviour` wiring up a `GunRaycastBased` and flashing a bullet-line object for 35 ms per shot. |
| `ProjectileFactoryDemo`      | Sample `IProjectileFactory` that `Instantiate`s two prefabs. Replace with a pool for production use. |

The `Demo` types are reference implementations kept in `Runtime` so scenes can use them directly. They
are not required — implement `IProjectileFactory` and derive from `WeaponMonoBehaviour` yourself.

## Running the tests

The package ships edit-mode tests covering ammo depletion, the shot interval and the first-shot rule.
Because tests in a package are hidden by default, add the package to your project's `testables` in
`Packages/manifest.json`:

```json
{
  "testables": [
    "com.artisangames.weaponsystem"
  ]
}
```

The tests then appear under **Window ▸ General ▸ Test Runner ▸ EditMode**.

`WeaponTests` is an abstract base run once per concrete weapon, with `ITimeProvider` substituted so
tests set the clock directly:

```csharp
weapon.Fire();                                  // shot at t = 0
time.GetTime().Returns(weapon.ShotInterval / 2f);
Assert.IsFalse(weapon.Fire());                  // still cooling down
```

Note that `GetTime()` returns **absolute** time, so each step is measured from the start of the test,
not from the previous shot.

The tests require `net.tnrd.nsubstitute` — see [Additional dependencies](#additional-dependencies).

## License

[MIT](LICENSE.md) © Nouman Waheed

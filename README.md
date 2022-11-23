# GOAPDemo - Design doc(temp)

## Soldier Behaviors

### Goal

    Self-protection, 
        Current State: IS_IN_DANGER - 1
        Target State: IS_IN_DANGER - 0
    Kill target, 
        Current State: HAS_TARGET - 1, TARGET_IN_SIGHT – 1, IS_TARGET_IN_RANGE – 1
        Target State: HAS_TARGET - 0
    Get near to target, 
        Current State: HAS_TARGET – 1, IS_TARGET_IN_RANGE – 0
        Target State: IS_TARGET_IN_RANGE - 1, TARGET_IN_SIGHT - 1
    Guard.
        Current State: HAS_TARGET – 0
        Target State: HAS_TARGET - 1

## Action

### Move to target

    Precondition: HAS_TARGET
    Effect: IS_TARGET_IN_RANGE – 1, TARGET_IN_SIGHT – 1
    Cost: 1.0

### Frightened

    Precondition: IS_IN_DANGER
    Effect: HAS_TARGET – 0
    Cost: (distance – 2.0) / 2.0

### Attack

    Precondition: IS_TARGET_IN_RANGE, TARGET_IN_SIGHT
    Effect: HAS_TARGET – 0
    Cost: 1.0

### Patrol

    Precondition: not HAS_TARGET
    Effect: HAS_TARGET – 1
    Cost: 1.0

### Jump

    Precondition: TARGET_IN_SIGHT.
    Effect: HAS_TARGET - 0
    Cost: (Jump_position_distance - distance)

## Basic functions

    Face To.
    Move To.
    Fire.
    Jump To.

## World States

    HAS_TARGET
    IS_TARGET_NEAR
    IS_TARGET_IN_RANGE
    TARGET_IN_SIGHT
    IS_IN_DANGER


# GOAPDemo

一个简单的基于GOAP（Goal-Oriented Action Planning）的AI示例。

## 示例视频

https://user-images.githubusercontent.com/18349598/204325962-1f60f5a5-2c4a-4892-ac4b-33fde52ef95f.mp4

https://user-images.githubusercontent.com/18349598/204325972-e3a1fbc5-7b5b-4409-a936-07e61327fb51.mp4

## 下面为Demo中的AI行为设计

## Goals

    Self-protection, 
        Current State: IS_LOW_HP - 1, IS_TARGET_NEAR - 1
        Target State: IS_LOW_HP - 0
    Kill target, 
        Current State: HAS_TARGET - 1, TARGET_IN_SIGHT – 1, IS_TARGET_IN_RANGE – 1
        Target State: HAS_TARGET - 0
    Get near to target, 
        Current State: HAS_TARGET – 1, IS_TARGET_IN_RANGE – 0
        Target State: IS_TARGET_IN_RANGE - 1, TARGET_IN_SIGHT - 1
    Guard.
        Current State: HAS_TARGET – 0
        Target State: HAS_TARGET - 1

## Actions

### Move to target

    Precondition: HAS_TARGET - 1
    Effect: IS_TARGET_IN_RANGE – 1, TARGET_IN_SIGHT – 1
    Cost: 1.0

### Frightened

    Precondition: IS_LOW_HP - 1
    Effect: IS_LOW_HP – 0
    Cost: (distance – 2.0) / 2.0

### Attack

    Precondition: IS_TARGET_IN_RANGE - 1, TARGET_IN_SIGHT - 1
    Effect: HAS_TARGET – 0
    Cost: 1.0

### Patrol

    Precondition: HAS_TARGET - 0
    Effect: HAS_TARGET – 1
    Cost: 1.0

### Jump

    Precondition: TARGET_IN_SIGHT - 1.
    Effect: HAS_TARGET - 0
    Cost: (Jump_position_distance - distance)

### Hide

    Precondition: IS_LOW_HP - 1.
    Effect: IS_LOW_HP - 0
    Cost: 1.0

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
    IS_LOW_HP



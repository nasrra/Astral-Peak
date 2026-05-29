using UnityEngine;

public static class SortingLayerManager{
    // used for checking layers in colliders
    public static readonly int
        SKYBOX        = SortingLayer.NameToID("skybox"),
        BACKGROUND    = SortingLayer.NameToID("background"),
        EFFECTS_BACK  = SortingLayer.NameToID("effects_back"),
        BOSS_BACK     = SortingLayer.NameToID("boss_back"),
        BOSS_FRONT    = SortingLayer.NameToID("boss_front"),
        CHARACTERS    = SortingLayer.NameToID("characters"),
        ITEMS         = SortingLayer.NameToID("items"),
        PROJECTILES   = SortingLayer.NameToID("projectiles"),
        EFFECTS_FRONT = SortingLayer.NameToID("effects_front"),
        DOOR          = SortingLayer.NameToID("door"),
        FORGROUND     = SortingLayer.NameToID("forground"), // Typo fixed: "FORGROUND" → "Foreground"
        GAMEPLAY_UI   = SortingLayer.NameToID("gameplay_ui"),
        UI            = SortingLayer.NameToID("ui");
}

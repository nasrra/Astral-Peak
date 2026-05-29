// using Sounds;
// using System.Collections.Generic;

// public interface SoundSet{
//     public List<Sound> get_sounds();
// }

// public struct MeleeSoundSet : SoundSet {
//     private List<Sound> sounds;
//     public List<Sound> get_sounds() {
//         if (sounds == null) {
//             sounds = new List<Sound>() {
//                 new MeleeHit(),
//                 new MeleeSwing1(),
//                 new MeleeSwing2(),
//                 new MeleeSwing3(),
//             };
//         }
//         return sounds;
//     }
// }

// public struct SnowSoundSet : SoundSet {
//     private List<Sound> sounds;
//     public List<Sound> get_sounds() =>
//         sounds == null 
//             ?new List<Sound>(){
//                 new SnowFootstep1(),
//                 new SnowFootstep2(),
//                 new SnowFootstep3(),
//                 new SnowFootstep4(),
//                 new SnowImpactHeavy(),
//                 new SnowImpactLight(),
//                 new SnowRolling(),}
//             :sounds;
// }

// public struct MagicSoundSet : SoundSet {
//     private List<Sound> sounds;
//     public List<Sound> get_sounds() {
//         if (sounds == null) {
//             sounds = new List<Sound>() {
//                 new Magic1(),
//                 new MagicExplosion(),
//                 new Whoosh1(),
//                 new MagicFootstep1(),
//                 new MagicFootstep2(),
//                 new MagicFootstep3(),
//                 new MagicSnowCast(),
//             };
//         }
//         return sounds;
//     }
// }

// public struct ElectricitySoundSet : SoundSet{
//     private List<Sound> sounds;
//     public List<Sound> get_sounds()
//         =>sounds==null
//         ? new List<Sound>(){
//             new Electricity1(),
//             new Electricity2(),
//             new Electricity3(),
//             new ElectricityLoop(),
//             new ElectricBurst()
//             }
//         : sounds;
// }

// public struct UiSoundSet : SoundSet {
//     private List<Sound> sounds;
//     public List<Sound> get_sounds() {
//         if (sounds == null) {
//             sounds = new List<Sound>() {
//                 new DeepBoom(),
//                 new HeartThump(),
//                 new WoodenPing(),
//             };
//         }
//         return sounds;
//     }
// }

// public struct WolfSoundSet : SoundSet {
//     private List<Sound> sounds;
//     public List<Sound> get_sounds() {
//         if (sounds == null) {
//             sounds = new List<Sound>() {
//                 new DogBark1(),
//                 new WolfHowl(),
//             };
//         }
//         return sounds;
//     }
// }

// public struct OutdoorAmbienceSoundSet : SoundSet {
//     private List<Sound> sounds;
//     public List<Sound> get_sounds() {
//         if (sounds == null) {
//             sounds = new List<Sound>() {
//                 new SoftWind(),
//                 new HeavyWind(),
//             };
//         }
//         return sounds;
//     }
// }

// public struct ThunderSoundSet : SoundSet{
//     private List<Sound> sounds;
//     public List<Sound> get_sounds() 
//         => sounds == null
//         ? new List<Sound>(){
//             new Thunder1(),
//             new Thunder2(),
//         }
//         : sounds;
// }

// public struct AltarCutsceneSoundSet : SoundSet {
//     private List<Sound> sounds;
//     public List<Sound> get_sounds() {
//         if (sounds == null) {
//             sounds = new List<Sound>() {
//                 new AltarMusic(),
//                 new DeepThumping(),
//             };
//         }
//         return sounds;
//     }
// }

// public struct CavalrySoundSet : SoundSet {
//     private List<Sound> sounds;
//     public List<Sound> get_sounds() {
//         if (sounds == null) {
//             sounds = new List<Sound>() {
//                 new WolfBossMusic1(),
//                 new WolfBossMusic2(),
//                 new Ping(),
//             };
//         }
//         return sounds;
//     }
// }

// public struct FireSoundSet : SoundSet {
//     private List<Sound> sounds;
//     public List<Sound> get_sounds() {
//         if (sounds == null) {
//             sounds = new List<Sound>() {
//                 new Steam(),
//                 new SmallFire(),
//             };
//         }
//         return sounds;
//     }
// }

// public struct DomineSoundSet : SoundSet {
//     private List<Sound> sounds;
//     public List<Sound> get_sounds() {
//         if (sounds == null) {
//             sounds = new List<Sound>() {
//                 new DomineMusic(),
//                 // Opening Cutscene
//                 new DomineAnOffering(),
//                 new DomineBurnedWoman(),
//                 new DomineCursedOne(),
//                 new DomineDeadWoman(),
//                 new DomineEnterRoom(),
//                 new DomineFixWoman(),
//                 new DomineFoolOrBrave(),
//                 new DomineItsExpensive(),
//                 new DomineLostSoul(),
//                 new DomineNoHope(),
//                 new DomineOldDoor(),
//                 new DomineReviveWoman(),
//                 new DomineSacrificeRitual(),
//                 new DomineStrongWill(),
//                 new DomineUnderArches(),
//                 new DomineUnless(),
//                 new DomineWaitingMortal(),
//                 new DomineWalkAether(),
//                 new DomineWarning(),
//                 new DomineYoureMortals(),
//                 new DomineAshesToWind(),
//                 new DomineWhyHere(),
//                 new DomineNo(),
//                 new DomineMountainSummit(),
//             };
//         }
//         return sounds;
//     }
// }

// public struct RangedSoundSet : SoundSet {
//     private List<Sound> sounds;
//     public List<Sound> get_sounds() {
//         if (sounds == null) {
//             sounds = new List<Sound>() {
//                 new BowShot(),
//                 new CoinToss(),
//             };
//         }
//         return sounds;
//     }
// }

// public struct StoneSoundSet : SoundSet {
//     private List<Sound> sounds;
//     public List<Sound> get_sounds() {
//         if (sounds == null) {
//             sounds = new List<Sound>() {
//                 new StoneFootstep1(),
//                 new StoneFootstep2(),
//                 new StoneFootstep3(),
//                 new StoneFootstep4(),
//                 new StoneHeavyFootstep1(),
//                 new StoneHeavyFootstep2(),
//                 new StoneHeavyFootstep3(),
//                 new StoneHeavyFootstep4(),
//                 new StoneImpactLight(),
//                 new StoneDoor(),
//                 new StoneShiftFast(),
//             };
//         }
//         return sounds;
//     }
// }

// public struct RiderSoundSet : SoundSet {
//     private List<Sound> sounds;
//     public List<Sound> get_sounds() {
//         if (sounds == null) {
//             sounds = new List<Sound>() {
//                 new BossYell(),
//                 new WhistleLong(),
//             };
//         }
//         return sounds;
//     }
// }

// public struct HollowSoundSet : SoundSet {
//     private List<Sound> sounds;
//     public List<Sound> get_sounds() {
//         if (sounds == null) {
//             sounds = new List<Sound>() {
//                 new BossYell(),
//                 new WoodenRattle1(),
//                 new WoodenRattle2(),
//                 new WoodenRattle4(),
//             };
//         }
//         return sounds;
//     }
// }

// public struct MageSoundSet : SoundSet{
//     private List<Sound> sounds;
//     public List<Sound> get_sounds(){
//         sounds = sounds == null
//         ?new List<Sound>(){
//             new BellChimes1(),
//             new Ping(),        
//             new MageBossMusic1(),
//             new MageBossMusic2(),
//         }
//         :sounds;
//         return sounds;
//     }
// }

// public struct GiantSoundSet : SoundSet{
//     private List<Sound> sounds;
//     public List<Sound> get_sounds(){
//         sounds = sounds == null
//         ?new List<Sound>(){
//             new Ping(),
//             new HammerDown1(),
//             new Whoosh2(),
//             new BossYell(),
//             new WaterBubble(),
//             new WaterGurgleLoop(),
//             new WaterSplash(),
//             new WaterSplashDeep1(),
//             new WaterSplashDeep2(),
//             new WoodenRattleImpact(),
//             new WaterRushHeavy(),
//             new HandClapReverb(),
//         }
//         :sounds;
//         return sounds;
//     }
// }

// public struct IntroductionSoundSet : SoundSet{
//     private List<Sound> sounds;
//     public List<Sound> get_sounds(){
//         sounds = sounds == null
//         ?new List<Sound>(){
//             new SmallFire(),
//             new Steam(),
//             new Sounds.Introduction(),        
//         }
//         :sounds;
//         return sounds;
//     }    
// }

// public struct BeatriceSoundSet : SoundSet{
//     private List <Sound> sounds;
//     public List<Sound> get_sounds(){
//         sounds = sounds == null
//         ?new List<Sound>(){
//             new WomanGaspReverb(),
//         }
//         :sounds;
//         return sounds;
//     }
// }
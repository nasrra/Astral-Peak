using AYellowpaper.SerializedCollections;
using UnityEngine;

public class LineParticleEmittersHandler : MonoBehaviour{
    [SerializeField] SerializedDictionary<string, LineParticleEmitter> emitters = new SerializedDictionary<string, LineParticleEmitter>();
    public void emit_once(string emitter_id, Vector3 start_pos, Vector3 end_pos) => emitters[emitter_id].emit_once(start_pos,end_pos);
    public void emit_once(string emitter_id) => emitters[emitter_id].emit_once();
    public void start_emitting(string emitter_id) => emitters[emitter_id].start_emitting();
    public void stop_emitting(string emitter_id) => emitters[emitter_id].stop_emitting();
}

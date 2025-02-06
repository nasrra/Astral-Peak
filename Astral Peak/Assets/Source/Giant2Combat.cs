using System;
using System.Collections.Generic;

public class Giant2Combat : BossCombat{
    protected override void create_movesets(){
        movesets = new Dictionary<string, Action>(){
            {"phase_1",()=>{}}
        };
    }
}

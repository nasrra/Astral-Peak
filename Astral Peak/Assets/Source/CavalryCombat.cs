using System;
using System.Collections.Generic;
using UnityEngine;

enum F{
    FRONT_STRIKE,
    FETCH,
    BITE,
}

enum B{
    BACK_STRIKE,
}

public class CavalryCombat : BossCombat{
    [SerializeField] BossAttack 
        front_strike = new BossAttack(CavalryAnimator.FRONT_STRIKE), 
        back_strike = new BossAttack(CavalryAnimator.BACK_STRIKE),
        bite = new BossAttack(CavalryAnimator.BITE_1),
        fetch = new BossAttack(CavalryAnimator.FETCH_1);

    void Start() => set_movesets();

    void set_movesets(){
        front_strike.set_transform(transform);
        back_strike.set_transform(transform);
        bite.set_transform(transform);
        fetch.set_transform(transform);
        //front_moveset[(int)F.FRONT_STRIKE] = front_strike;
        //front_moveset[(int)F.BITE] = bite;
        front_moveset[(int)F.FETCH] = fetch;
        back_moveset[(int)B.BACK_STRIKE] = back_strike;
    }
}


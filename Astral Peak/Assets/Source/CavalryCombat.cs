using System;
using System.Collections.Generic;
using UnityEngine;

enum F{
    FRONT_STRIKE,
    BITE,
}

enum B{
    BACK_STRIKE,
}

public class CavalryCombat : BossCombat{
    [SerializeField] BossAttack front_strike = new BossAttack(CavalryAnimator.FRONT_STRIKE);
    [SerializeField] BossAttack back_strike = new BossAttack(CavalryAnimator.BACK_STRIKE);
    [SerializeField] BossAttack bite = new BossAttack(CavalryAnimator.BITE_1);

    void Start() => set_movesets();

    public void set_movesets(){
        front_strike.set_transform(transform);
        back_strike.set_transform(transform);
        bite.set_transform(transform);
        front_moveset[(int)F.FRONT_STRIKE] = front_strike;
        front_moveset[(int)F.BITE] = bite;
        back_moveset[(int)B.BACK_STRIKE] = back_strike;
    }
}


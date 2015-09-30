using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public enum CatapultState
    {
        Idle,
        UserPulling,
        ProjectileFlying
    }

    public enum GameState
    {
        Idle,
        Start,
        ProjectileMovingToCatapult,
        Won,
        Lost,
        Playing,
        levelEnd
    }


    public enum ProjectileState
    {
        BeforeThrown,
        Thrown
    }

}


using System;
using System.Collections.Generic;
using System.Text;

namespace Jester
{
    enum RPC
    {
        CompleteTask = 1
    }
    enum CustomRPC
    {
        SetJester = 40,
        JesterWin = 50,
        SetLocalPlayersJester = 41,
        SyncCustomSettingsJester = 42,
        SetLastPlayerTask = 43
    }
}

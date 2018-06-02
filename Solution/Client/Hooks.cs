using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UMS;
using UnityEngine;

namespace DAClient
{
    /// <summary>
    /// Hooks that allows us to call editor code from the client
    /// These will obviously not work in the built game
    /// </summary>
    public static class Hooks
    {
        public static Func<List<ModFile>> GetModsInProject;
    }
}

using System;
using UnityEngine;

namespace TandC.GeometryAstro.Services
{
    public interface IPauseService
    {
        public void Init();
        public void Dispose();

        public bool IsPaused { get; }
    }
}
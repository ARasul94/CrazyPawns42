using Factories.PawnFactory;
using UnityEngine;

namespace Services
{
    public class PawnSpawnService
    {
        private readonly IPawnFactory m_pawnFactory;
        private readonly CrazyPawn.CrazyPawnSettings m_settings;

        public PawnSpawnService(
            IPawnFactory _pawnFactory,
            CrazyPawn.CrazyPawnSettings _settings)
        {
            m_pawnFactory = _pawnFactory;
            m_settings = _settings;
        }

        public void SpawnInitialPawns()
        {
            for (var i = 0; i < m_settings.InitialPawnCount; i++)
            {
                var position = GetRandomPositionInsideInitialZone();
                m_pawnFactory.Create(position);
            }
        }

        private Vector3 GetRandomPositionInsideInitialZone()
        {
            var point = Random.insideUnitCircle * m_settings.InitialZoneRadius;
            return new Vector3(point.x, 0f, point.y);
        }
    }
}
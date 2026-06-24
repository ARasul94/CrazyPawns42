using System;
using Models;
using UniRx;
using UnityEngine;

namespace ViewModels
{
    public class PawnViewModel : IDisposable
    {
        private readonly PawnModel m_model;

        public int id => m_model.id;

        public ReactiveProperty<Vector3> position { get; }
        public ReactiveProperty<bool> isOutsideBoard { get; } = new(false);

        public PawnViewModel(PawnModel _model)
        {
            m_model = _model;
            position = new ReactiveProperty<Vector3>(_model.position);
            position.Subscribe(_position => m_model.position = _position);
        }

        public void Dispose()
        {
            position?.Dispose();
            isOutsideBoard?.Dispose();
        }
    }
}
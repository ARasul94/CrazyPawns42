using System;
using Models;
using UniRx;
using UnityEngine;

namespace ViewModels
{
    public class PawnViewModel : IDisposable
    {
        public int id => m_model.id;

        public ReadOnlyReactiveProperty<Vector3> position { get; }
        public ReadOnlyReactiveProperty<bool> isOutsideBoard { get; }

        private readonly ReactiveProperty<Vector3> m_position;
        private readonly ReactiveProperty<bool> m_isOutsideBoard;
        
        private readonly PawnModel m_model;

        public PawnViewModel(PawnModel _model)
        {
            m_model = _model;
            
            m_position = new ReactiveProperty<Vector3>(_model.position);
            position = new ReadOnlyReactiveProperty<Vector3>(m_position);
            
            m_isOutsideBoard = new ReactiveProperty<bool>(false);
            isOutsideBoard = new ReadOnlyReactiveProperty<bool>(m_isOutsideBoard);
            
            m_position.Subscribe(_position => m_model.position = _position);
        }

        public void SetPosition(Vector3 _position)
        {
            m_position.Value = _position;
        }

        public void SetIsOutsideBoard(bool _isOutsideBoard)
        {
            m_isOutsideBoard.Value = _isOutsideBoard;
        }

        public void Dispose()
        {
            position?.Dispose();
            isOutsideBoard?.Dispose();
        }
    }
}
using UnityEngine;

namespace Models
{
    public class PawnModel
    {
        public int id { get; }
        public Vector3 position { get; set; }

        public PawnModel(int _id, Vector3 _position)
        {
            id = _id;
            position = _position;
        }
    }
}
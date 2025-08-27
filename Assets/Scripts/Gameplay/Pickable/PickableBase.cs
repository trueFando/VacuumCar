using UnityEngine;

namespace Gameplay.Pickable
{
    public abstract class PickableBase : MonoBehaviour
    {
        public Rigidbody2D Rigidbody => _rigidbody;
        public int Mass => _mass;

        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private int _mass;
    }
}
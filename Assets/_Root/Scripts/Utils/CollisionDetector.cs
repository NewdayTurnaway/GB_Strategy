using System;
using UniRx;
using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(Rigidbody))]
    public class CollisionDetector : MonoBehaviour
    {
        public IObservable<Collision> Collisions => _collisions;
        private readonly Subject<Collision> _collisions = new();

        private void OnCollisionStay(Collision collision) => 
            _collisions.OnNext(collision);
    }
}
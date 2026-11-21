using Input;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(AnimationController))]
    public sealed class PlayerController : MonoBehaviour
    {
        [field: Header("Self Components")]
        [field: SerializeField] private Rigidbody2D rig;
                                public Rigidbody2D Rig => rig;
        [field: SerializeField] private Animator anima;
                                public Animator Anima => anima;
        
        [field: Header("Other Components")]
        [field: SerializeField] private InputHandler input;
                                public InputHandler Input => input;
        
        [field: Header("Attributes")]
        [field: SerializeField] private float moveSpeed = 5;

        private void FixedUpdate()
        {
            rig.linearVelocityX = Input.WalkDir * moveSpeed;
        }
    }
}
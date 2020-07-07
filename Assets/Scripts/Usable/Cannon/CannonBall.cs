﻿using Assets.Scripts.Characters.Titan;
using Logging;
using UnityEngine;
using Zenject;

namespace Cannon
{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody), typeof(PhotonView))]
    internal sealed class CannonBall : Photon.MonoBehaviour
    {
        private BoomFactory boomFactory;
        private new Collider collider;
        private int heroViewId;
        private ILogger logger;
        private new Transform transform;

        private void Start()
        {
            transform = base.transform;
            collider = GetComponent<SphereCollider>();
            collider.isTrigger = true;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!photonView.isMine) return;

            var titan = other.gameObject.GetComponent<MindlessTitan>();
            if (titan)
                titan.photonView.RPC(nameof(titan.OnCannonHitRpc), titan.photonView.owner, heroViewId, other.collider.name);
            
            Explode();
            
            logger.Log($"{nameof(CannonBall)} entered {other.gameObject.name}", this);
        }

        private void OnTriggerExit(Collider other)
        {
            collider.isTrigger = false;
        }

        private void Explode()
        {
            boomFactory.Create(4, transform.position, transform.rotation);
            PhotonNetwork.Destroy(gameObject);
        }

        [Inject]
        private void Construct(
            LoggerFactory loggerFactory,
            BoomFactory boomFactory,
            Vector3 velocity,
            int heroViewId)
        {
            logger = loggerFactory.Create(this);
            this.boomFactory = boomFactory;
            this.heroViewId = heroViewId;
            
            GetComponent<Rigidbody>().velocity = velocity;
        }

        public sealed class Factory : PlaceholderFactory<int, Vector3, Vector3, Quaternion, byte, CannonBall> {}
    }
}
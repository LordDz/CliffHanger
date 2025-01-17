﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamekit3D
{
    public class GrenadierShield : MonoBehaviour
    {
        private static Collider[] sOverlapCache = new Collider[16];

        public new ParticleSystem particleSystem;

        protected SphereCollider m_Collider;
        protected int m_PlayerMask;

        private void OnEnable()
        {
            particleSystem.gameObject.SetActive(true);
            particleSystem.time = 0;
            particleSystem.Play(true);

            m_Collider = GetComponent<SphereCollider>();

            m_PlayerMask = 1 << LayerMask.NameToLayer("Player");
        }

        private void Update()
        {
            Damageable.DamageMessage dmgData = new Damageable.DamageMessage()
            {
                amount = 1,
                damager = this,
                stopCamera = false,
                forceMultiplier = 1.0f
            };


            int count = Physics.OverlapSphereNonAlloc(transform.position, m_Collider.radius * transform.localScale.x,
                sOverlapCache, m_PlayerMask);

            for (int i = 0; i < count; ++i)
            {
                Damageable d = sOverlapCache[i].GetComponent<Damageable>();

                if (d != null)
                {
                    dmgData.direction = d.transform.position - transform.position;
                    d.ApplyDamage(dmgData);
                }
            }
        }
    }
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader;

namespace FKsCRE.Content.Ammunition.CPreMoodLord.PerennialBullet
{
    public class PerennialBulletPlayer : ModPlayer
    {
        //private int hitCounter = 0; // �ӵ����м���
        //private int decayTimer = 600; // ������������ʱ����10�룬600֡��
        //private int cooldownTimer = 0; // ��ȴ��ʱ��
        //private int defenseBoost = 0; // Ĭ�ϵķ�������

        //public override void ResetEffects()
        //{
        //    // �����ȴ��ʱ������ 0��������ȴʱ��
        //    if (cooldownTimer > 0)
        //    {
        //        cooldownTimer--;
        //        return; // ��ȴ�в��������� Buff
        //    }

        //    // ������� 10 ��δ���е��ˣ��Ƴ� Buff
        //    if (decayTimer > 0)
        //    {
        //        decayTimer--;
        //    }
        //    else
        //    {
        //        Player.ClearBuff(ModContent.BuffType<PerennialBulletPBuff>()); // �Ƴ��������� Buff
        //        hitCounter = 0; // ��ռ���
        //    }
        //}

        //public void OnBulletHit()
        //{
        //    if (cooldownTimer > 0)
        //        return; // ��ȴ�ڼ䲻���������¼�

        //    hitCounter++;

        //    if (hitCounter >= 20) // ÿ���� 20 �θ��� Buff
        //    {
        //        int defenseBoostValue = 5; // ���η�������ֵ
        //        Player.AddBuff(ModContent.BuffType<PerennialBulletPBuff>(), 300); // ���� 5 �� Buff

        //        // ����������ֵ���ݸ� Buff
        //        if (Player.TryGetModPlayer<PerennialBulletPlayer>(out var modPlayer))
        //        {
        //            modPlayer.AssignBuffDefense(defenseBoostValue); // ֪ͨ Buff ��������ֵ
        //        }

        //        // �������ɹ�������������ɫ������Ч
        //        for (int i = 0; i < 8; i++) // ���� 8 ������
        //        {
        //            Vector2 particleVelocity = Main.rand.NextVector2CircularEdge(2f, 2f) * 1.5f; // �Ͽ��ٶȣ�Բ�α߽�
        //            Dust dust = Dust.NewDustPerfect(
        //                Player.Center,              // ��������λ��
        //                DustID.GreenFairy,          // ��ɫ����
        //                particleVelocity,           // �����ٶ�
        //                150,                        // ͸����
        //                Color.LightGreen,           // ������ɫ
        //                Main.rand.NextFloat(1.2f, 1.6f) // ���Ӵ�С
        //            );
        //            dust.noGravity = true; // ���Ӳ�������Ӱ��
        //        }

        //        hitCounter -= 20; // ���ü��������ಿ��
        //    }

        //    // ÿ���������÷���˥����ʱ��
        //    decayTimer = 600; // 10 ��
        //}

        //// ���������������������ݸ� Buff
        //public void AssignBuffDefense(int defenseBoostValue)
        //{
        //    if (Player.HasBuff(ModContent.BuffType<PerennialBulletPBuff>()))
        //    {
        //        PerennialBulletPBuff buff = Player.GetModBuff<PerennialBulletPBuff>();
        //        if (buff != null)
        //        {
        //            buff.SetDefenseBoost(defenseBoostValue); // ���� Buff ���÷�����
        //        }
        //    }
        //}





        //public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        //{
        //    ClearDefenseBuff();
        //}

        //public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        //{
        //    ClearDefenseBuff();
        //}

        //private void ClearDefenseBuff()
        //{
        //    // ���Buff��������ȴ״̬
        //    Player.ClearBuff(ModContent.BuffType<PerennialBulletPBuff>());
        //    cooldownTimer = 300; // 5����ȴ
        //    hitCounter = 0; // ���ü�����
        //}



        public int StackCount; // �������ѵ�����

        public override void ResetEffects()
        {
            // �������Ƿ�װ���������������У��������Ƴ� Buff �Ͷѵ�
            //if (Player.HeldItem.type != ModContent.ItemType<PerennialBulletWeapon>())
            {
                //StackCount = 0;
                //Player.ClearBuff(ModContent.BuffType<PerennialBulletPBuff>());
            }
        }


        private int increaseStackCountCalls = 0; // ׷�ٵ��ô���

        public void IncreaseStackCount()
        {
            increaseStackCountCalls++; // ÿ�ε������Ӽ���

            if (increaseStackCountCalls >= 50) // �����ô����ﵽ 50 ʱ����Ч��
            {
                StackCount++; // ���Ӷѵ�����
                Player.AddBuff(ModContent.BuffType<PerennialBulletPBuff>(), 600); // ˢ�� 10 ��� Buff

                // �������ɹ�������������ɫ������Ч
                for (int i = 0; i < 8; i++) // ���� 8 ������
                {
                    Vector2 particleVelocity = Main.rand.NextVector2CircularEdge(2f, 2f) * 1.5f; // �Ͽ��ٶȣ�Բ�α߽�
                    Dust dust = Dust.NewDustPerfect(
                        Player.Center,              // ��������λ��
                        DustID.GreenFairy,          // ��ɫ����
                        particleVelocity,           // �����ٶ�
                        150,                        // ͸����
                        Color.LightGreen,           // ������ɫ
                        Main.rand.NextFloat(1.2f, 1.6f) // ���Ӵ�С
                    );
                    dust.noGravity = true; // ���Ӳ�������Ӱ��
                }

                // ���õ��ü�����
                increaseStackCountCalls = 0;
            }
        }



        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            ClearBuffOnHit();
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            ClearBuffOnHit();
        }

        private void ClearBuffOnHit()
        {
            // ��� Buff �Ͷѵ�����
            StackCount = 0;
            Player.ClearBuff(ModContent.BuffType<PerennialBulletPBuff>());
        }

    }
}

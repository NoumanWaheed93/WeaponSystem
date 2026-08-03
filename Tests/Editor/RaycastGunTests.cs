using NUnit.Framework;
using WeaponSystem;

public class RaycastGunTests : WeaponTests
{
    [SetUp]
    public override void SetUp()
    {
        base.SetUp();

        weapon = new GunRaycastBased(barrelTransform, time, 100, 1, 10, 10);
    }

    [Test]
    public void Gun_Instance_Can_Be_Created()
    {
        Assert.IsNotNull(weapon);
    }

    //A very slow weapon must still be allowed to fire its first shot
    //immediately, no matter how long its shot interval is.
    [Test]
    public void Slow_Weapon_Can_Fire_Its_First_Shot_Immediately()
    {
        Weapon slowGun = new GunRaycastBased(barrelTransform, time, 100, 1f / 500f, 10, 10);
        //Floats cannot hold 1/500 exactly, so allow a small difference.
        Assert.AreEqual(500f, slowGun.ShotInterval, 0.01f, "Shot interval should be about 500 seconds");
        Assert.IsTrue(slowGun.Fire(), "Could not fire the first shot of a slow weapon");
    }
}

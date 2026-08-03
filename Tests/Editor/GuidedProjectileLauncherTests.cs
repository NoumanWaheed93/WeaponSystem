using NUnit.Framework;
using WeaponSystem;
using NSubstitute;

public class GuidedProjectileLauncherTests : WeaponTests
{
    [SetUp]
    public override void SetUp()
    {
        base.SetUp();
        IProjectileFactory projectileFactory = Substitute.For<IProjectileFactory>();
        IGuidedProjectile projectile = Substitute.For<IGuidedProjectile>();
        projectileFactory.GetHomingProjectile().Returns(projectile);
        weapon = new GuidedProjectileLauncher(barrelTransform, time, 100, 1, projectileFactory);
    }

    [Test]
    public void Gun_Instance_Can_Be_Created()
    {
        Assert.IsNotNull(weapon);
    }

}

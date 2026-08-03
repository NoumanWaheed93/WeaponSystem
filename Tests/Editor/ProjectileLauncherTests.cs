using WeaponSystem;
using NSubstitute;
using NUnit.Framework;

public class ProjectileLauncherTests : WeaponTests
{
    [SetUp]
    public override void SetUp()
    {
        base.SetUp();
        IProjectileFactory projectileFactory = Substitute.For<IProjectileFactory>();
        IProjectile projectile = Substitute.For<IProjectile>();
        projectileFactory.GetProjectile().Returns(projectile);
        weapon = new ProjectileLauncher(barrelTransform, time, 100, 1, projectileFactory);
    }

    [Test]
    public void ProjectileLauncher_Instance_Can_Be_Created()
    {
        Assert.IsNotNull(weapon);
    }
}

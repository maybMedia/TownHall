using Moq;
using TownHall.Core;

namespace TownHall.Tests
{
	public class TestBase
	{
		public Mock<IUnitOfWork> mockUnitOfWork;

		[SetUp]
		public void Setup()
		{
		}

		[TearDown]
		public void TearDown()
		{
		}
	}
}
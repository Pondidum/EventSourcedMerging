using System;
using EventSourcedMerging.Domain;
using EventSourcedMerging.Events;
using EventSourcedMerging.Services;
using Ledger;
using Ledger.Stores;
using Shouldly;
using Xunit;

namespace EventSourcedMerging
{
	public class Tests
	{
		private readonly User _primary;
		private readonly User _secondary;
		private readonly AggregateStore<int> _store;

		public Tests()
		{
			_store = new AggregateStore<int>(new InMemoryEventStore());

			_primary = User.Create(1, "Andy");
			_primary.PushEvent(new MobilePhoneChangedEvent("0798 1234 123"));
			_primary.PushEvent(new HomePhoneChangedEvent("01412 123 123"));
			_primary.PushEvent(new DateOfBirthChangedEvent(new DateTime(1980, 7, 12)));

			_secondary = User.Create(2, "Andrew");
			_secondary.PushEvent(new MobilePhoneChangedEvent("0798 9876 987"));

			_store.Save("Users", _primary);
			_store.Save("Users", _secondary);
		}

		[Fact]
		public void When_merging()
		{
			var ms = new MergeService();
			ms.Merge(_primary, _secondary);

			_primary.PushEvent(new MobilePhoneChangedEvent("0744 4444 444"));
			_store.Save("Users", _primary);

			_primary.ShouldSatisfyAllConditions(
				() => _primary.Name.ShouldBe("Andrew"),
				() => _primary.Phones[PhoneType.Mobile].ShouldBe("0744 4444 444"),
				() => _primary.Phones[PhoneType.Home].ShouldBe("01412 123 123")
			);
		}
	}
}

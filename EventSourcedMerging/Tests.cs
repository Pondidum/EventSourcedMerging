using System;
using EventSourcedMerging.Domain;
using EventSourcedMerging.Events;
using Xunit;

namespace EventSourcedMerging
{
	public class Tests
	{

		[Fact]
		public void When_merging()
		{
			var user = User.Create(1234, "Andy");
			user.PushEvent(new MobilePhoneChangedEvent("0798 1234 123"));
			user.PushEvent(new HomePhoneChangedEvent("01412 123 123"));
			user.PushEvent(new DateOfBirthChangedEvent(new DateTime(1980, 7, 12)));
		}
	}
}

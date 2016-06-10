using Ledger;

namespace EventSourcedMerging.Events
{
	public class MobilePhoneChangedEvent : DomainEvent<int>
	{
		public string MobileNumber { get; }

		public MobilePhoneChangedEvent(string mobileNumber)
		{
			MobileNumber = mobileNumber;
		}
	}
}

using Ledger;

namespace EventSourcedMerging.Events
{
	public class HomePhoneChangedEvent : DomainEvent<int>
	{
		public string HomeNumber { get; }

		public HomePhoneChangedEvent(string homeNumber)
		{
			HomeNumber = homeNumber;
		}
	}
}
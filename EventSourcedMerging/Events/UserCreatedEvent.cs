using Ledger;

namespace EventSourcedMerging.Events
{
	public class UserCreatedEvent : DomainEvent<int>
	{
		public int ID { get; }
		public string Name { get; }

		public UserCreatedEvent(int id, string name)
		{
			ID = id;
			Name = name;
		}
	}
}
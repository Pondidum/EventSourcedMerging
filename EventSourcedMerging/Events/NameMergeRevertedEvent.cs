using Ledger;

namespace EventSourcedMerging.Events
{
	public class NameMergeRevertedEvent : DomainEvent<int>
	{
		public int MergeID { get; }
		public string Name { get; }

		public NameMergeRevertedEvent(int mergeID, string name)
		{
			MergeID = mergeID;
			Name = name;
		}
	}
}
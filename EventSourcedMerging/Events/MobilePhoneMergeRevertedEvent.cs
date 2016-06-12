using Ledger;

namespace EventSourcedMerging.Events
{
	public class MobilePhoneMergeRevertedEvent : DomainEvent<int>
	{
		public int MergeID { get; }
		public string MobileNumber { get; }

		public MobilePhoneMergeRevertedEvent(int mergeID, string mobileNumber)
		{
			MergeID = mergeID;
			MobileNumber = mobileNumber;
		}
	}
}
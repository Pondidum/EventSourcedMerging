using Ledger;

namespace EventSourcedMerging.Events
{
	public class MobilePhoneMergedEvent : DomainEvent<int>
	{
		public int MergeID { get; }
		public string MobileNumber { get; }

		public MobilePhoneMergedEvent(int mergeID, string mobileNumber)
		{
			MergeID = mergeID;
			MobileNumber = mobileNumber;
		}
	}
}
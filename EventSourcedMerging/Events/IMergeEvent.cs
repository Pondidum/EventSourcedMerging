using Ledger;

namespace EventSourcedMerging.Events
{
	public interface IMergeEvent
	{
		int MergeID { get; }
	}
}

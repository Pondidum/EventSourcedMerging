﻿using Ledger;

namespace EventSourcedMerging.Events
{
	public class NameMergedEvent : DomainEvent<int>, IMergeEvent
	{
		public int MergeID { get; }
		public string NewName { get; }

		public NameMergedEvent(int mergeID, string newName)
		{
			MergeID = mergeID;
			NewName = newName;
		}
	}
}

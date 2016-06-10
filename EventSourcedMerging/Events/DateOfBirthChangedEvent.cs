using System;
using Ledger;

namespace EventSourcedMerging.Events
{
	public class DateOfBirthChangedEvent : DomainEvent<int>
	{
		public DateTime DateOfBirth { get; }

		public DateOfBirthChangedEvent(DateTime dateOfBirth)
		{
			DateOfBirth = dateOfBirth;
		}
	}
}
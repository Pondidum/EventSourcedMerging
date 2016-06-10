using System;
using EventSourcedMerging.Domain;
using EventSourcedMerging.Events;

namespace EventSourcedMerging.Services
{
	public class MergeService
	{
		public void Merge(User primary, User secondary)
		{
			/*
			{ "type": "nameMergedEvent", "mergeID": "34385", "newName": "andrew" },
			{ "type": "mobilePhoneMergedEvent", "mergeID": "34385", "mobileNumber": "0798 9876 987" },
			{ "type": "mobileChangedEvent", "mobileNumber": "0744 4444 444" }
			*/

			//not implementing this properly yet...
			var mergeID = new Random().Next(0, 10000);

			primary.PushEvent(new NameMergedEvent(mergeID, secondary.Name));
			primary.PushEvent(new MobilePhoneMergedEvent(mergeID, secondary.Phones[PhoneType.Mobile]));
		}
	}
}

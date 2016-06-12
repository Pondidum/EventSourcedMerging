using System;
using System.Collections.Generic;
using System.Linq;
using EventSourcedMerging.Domain;
using EventSourcedMerging.Events;
using Ledger;
using Ledger.Infrastructure;

namespace EventSourcedMerging.Services
{
	public class MergeService
	{
		public int Merge(User primary, User secondary)
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

			return mergeID;
		}

		public User UndoMerge(AggregateStore<int> store, int userID, int mergeID)
		{
			var previousWasMerge = false;
			Func<DomainEvent<int>, EventTypes> segregator = e =>
			{
				var asMerge = e as IMergeEvent;
				var isMergeEvent = asMerge != null && asMerge.MergeID == mergeID;

				if (isMergeEvent == false)
					return previousWasMerge == false
					   ? EventTypes.Before
					   : EventTypes.After;

				previousWasMerge = true;
				return EventTypes.Merge;
			};

			var user = User.Blank();
			var events = store.Replay("Users", userID);
			var mergeTransformations = new List<Transformation>();

			Segreate(events, segregator,
				before => user.LoadFromEvents(new[] { before }), //ew not efficient
				merge => user.LoadFromEvents(new[] { MergeEvent(user, mergeTransformations, mergeID, merge) }),
				after => user.LoadFromEvents(new[] { after }) //ew not efficient{ }
			);

			mergeTransformations
				.Where(t => t.HasChangedSinceMerge() == false)
				.Select(t => t.UndoEvent)
				.ForEach(e => user.PushEvent(e));

			return user;
		}

		private static DomainEvent<int> MergeEvent(User user, List<Transformation> transformations, int mergeID, DomainEvent<int> e)
		{
			var map = new Dictionary<Type, Transformation>();

			map[typeof(NameMergedEvent)] = new Transformation
			{
				HasChangedSinceMerge = () => user.Name != ((NameMergedEvent)e).NewName,
				UndoEvent = new NameMergeRevertedEvent(mergeID, user.Name)
			};

			map[typeof(MobilePhoneMergedEvent)] = new Transformation
			{
				HasChangedSinceMerge = () => user.Phones[PhoneType.Mobile] != ((MobilePhoneMergedEvent)e).MobileNumber,
				UndoEvent = new MobilePhoneMergeRevertedEvent(mergeID, user.Phones[PhoneType.Mobile])
			};

			Transformation delta;
			if (map.TryGetValue(e.GetType(), out delta) == false)
				return e;

			transformations.Add(delta);

			return e;
		}

		private class Transformation
		{
			public Func<bool> HasChangedSinceMerge { get; set; }
			public DomainEvent<int> UndoEvent { get; set; }
		}

		private void Segreate(IEnumerable<DomainEvent<int>> events, Func<DomainEvent<int>, EventTypes> choice, params Action<DomainEvent<int>>[] collections)
		{
			foreach (var domainEvent in events)
			{
				var index = (int)choice(domainEvent);
				collections[index](domainEvent);
			}
		}

		private enum EventTypes
		{
			Before,
			Merge,
			After
		}

	}
}
